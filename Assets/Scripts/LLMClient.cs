using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

// llm.gameguild.gg is a LiteLLM proxy, which speaks the OpenAI chat-completions format.

[Serializable]
public class ChatMessage
{
    public string role;     // "system", "user", or "assistant"
    public string content;

    public ChatMessage() { }
    public ChatMessage(string role, string content)
    {
        this.role = role;
        this.content = content;
    }
}

[Serializable]
class ChatRequest
{
    public string model;
    public List<ChatMessage> messages;
    public float temperature;
}

[Serializable] class ChatChoice { public ChatMessage message; }
[Serializable] class ChatResponse { public List<ChatChoice> choices; }
[Serializable] class ErrorInfo { public string message; }
[Serializable] class ErrorBody { public ErrorInfo error; }
[Serializable] class ModelInfo { public string id; }
[Serializable] class ModelList { public List<ModelInfo> data; }

public class LLMClient : MonoBehaviour
{
    [HideInInspector] public static LLMClient instance;

    [SerializeField] string _baseUrl = "https://llm.gameguild.gg";
    [SerializeField] string _apiKey = "";
    [Tooltip("Must match a model name the proxy exposes. Tick 'Log Models On Start' to see the list.")]
    [SerializeField] string _model = "";
    [SerializeField, Range(0f, 2f)] float _temperature = 0.8f;
    [SerializeField] int _timeoutSeconds = 30;
    [Tooltip("How many times to try when the server is busy or times out.")]
    [SerializeField] int _maxAttempts = 3;
    [Tooltip("Wait before retrying; grows each attempt (2s, 4s, ...).")]
    [SerializeField] float _retryDelaySeconds = 2f;
    [SerializeField] bool _logModelsOnStart = false;

    string Url(string path) => _baseUrl.TrimEnd('/') + path;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        if (_logModelsOnStart)
            StartCoroutine(LogAvailableModels());
    }

    public IEnumerator SendChat(List<ChatMessage> messages, Action<string> onSuccess, Action<string> onError)
    {
        if (string.IsNullOrEmpty(_apiKey) || string.IsNullOrEmpty(_model))
        {
            onError?.Invoke("LLMClient needs an API key and a model name set in the Inspector.");
            yield break;
        }

        var body = new ChatRequest
        {
            model = _model,
            messages = messages,
            temperature = _temperature,
        };
        byte[] json = Encoding.UTF8.GetBytes(JsonUtility.ToJson(body));

        for (int attempt = 1; attempt <= _maxAttempts; attempt++)
        {
            string responseText = null;
            string error = null;
            bool retryable = false;

            using (var req = new UnityWebRequest(Url("/v1/chat/completions"), "POST"))
            {
                req.uploadHandler = new UploadHandlerRaw(json);
                req.downloadHandler = new DownloadHandlerBuffer();
                req.SetRequestHeader("Content-Type", "application/json");
                req.SetRequestHeader("Authorization", "Bearer " + _apiKey);
                req.timeout = _timeoutSeconds;

                yield return req.SendWebRequest();

                if (req.result == UnityWebRequest.Result.Success)
                {
                    responseText = req.downloadHandler.text;
                }
                else
                {
                    error = ExtractError(req);
                    // Busy server (429/503) or a timeout is worth retrying
                    retryable = req.responseCode == 429 || req.responseCode == 503 || req.responseCode == 0
                        || error.Contains("busy") || error.Contains("\"429\"");
                    error = $"HTTP {req.responseCode}: {error}";
                }
            }

            if (responseText != null)
            {
                ChatResponse resp = null;
                try { resp = JsonUtility.FromJson<ChatResponse>(responseText); }
                catch (Exception e) { onError?.Invoke("Couldn't parse response: " + e.Message); yield break; }

                if (resp?.choices == null || resp.choices.Count == 0 || resp.choices[0].message == null)
                {
                    onError?.Invoke("Response had no choices: " + responseText);
                    yield break;
                }

                onSuccess?.Invoke(resp.choices[0].message.content.Trim());
                yield break;
            }

            if (!retryable || attempt == _maxAttempts)
            {
                onError?.Invoke(error);
                yield break;
            }

            float wait = _retryDelaySeconds * attempt;
            Debug.LogWarning($"[LLMClient] Attempt {attempt} failed ({error}). Retrying in {wait}s...");
            yield return new WaitForSeconds(wait);
        }
    }

    public IEnumerator LogAvailableModels()
    {
        using (var req = UnityWebRequest.Get(Url("/v1/models")))
        {
            req.SetRequestHeader("Authorization", "Bearer " + _apiKey);
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[LLMClient] Model list failed, HTTP {req.responseCode}: {ExtractError(req)}");
                yield break;
            }

            var list = JsonUtility.FromJson<ModelList>(req.downloadHandler.text);
            var names = new List<string>();
            if (list?.data != null)
                foreach (var m in list.data) names.Add(m.id);
            Debug.Log("[LLMClient] Available models: " + string.Join(", ", names));
        }
    }

    static string ExtractError(UnityWebRequest req)
    {
        string text = req.downloadHandler?.text;
        if (!string.IsNullOrEmpty(text))
        {
            try
            {
                var err = JsonUtility.FromJson<ErrorBody>(text);
                if (!string.IsNullOrEmpty(err?.error?.message)) return err.error.message;
            }
            catch { }
            return text;
        }
        return req.error;
    }
}