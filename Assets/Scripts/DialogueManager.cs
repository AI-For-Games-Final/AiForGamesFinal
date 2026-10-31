using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [HideInInspector] public static DialogueManager instance;
    [SerializeField] Dialogue _startDialogue;

    [SerializeField, TextArea(3, 10)]
    string _systemPrompt = "You are Splosh, a slime pirate aboard the captain's ship. Your job is Cook.\nYour personality: Easily Aggrivated. What you secretly want: A lover.\nYour current loyalty to the captain is 4 out of 10.\n\nThe player is your captain. Reply in character in one to three short sentences,\ngooey and piratey but understandable. React honestly to how the captain treats you:\nrespect, fairness, and keeping promises raise loyalty; insults, broken promises,\nand ignoring your needs lower it. If loyalty is low, be sullen or hint at mutiny.\n\nEnd every reply with a tag on its own line: [LOYALTY:X] where X is a whole number\nfrom -3 to 3 showing how this message changed your loyalty.";
    [Tooltip("How many user/assistant messages to keep (the system prompt is always kept).")]
    [SerializeField] int _maxHistoryMessages = 20;

    readonly List<ChatMessage> _history = new List<ChatMessage>();
    bool _waitingForReply;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void StartDialogue()
    {
        ResetConversation(_systemPrompt);
        ReadDialogue(_startDialogue);

        // Let the model know what "it" already said so the conversation stays consistent.
        if (!string.IsNullOrEmpty(_startDialogue.GetText()))
            _history.Add(new ChatMessage("assistant", _startDialogue.GetText()));
    }

    // Optional: talk to one of your sim characters, using their name and role.
    public void StartDialogueWith(SimCharacter character)
    {
        string prompt = _systemPrompt +
            $"\nYour name is {character.m_name} and you work in {character.m_PositionName}.";
        ResetConversation(prompt);
        ReadDialogue(_startDialogue);
        if (!string.IsNullOrEmpty(_startDialogue.GetText()))
            _history.Add(new ChatMessage("assistant", _startDialogue.GetText()));
    }

    public void ResetConversation(string systemPrompt)
    {
        _history.Clear();
        _history.Add(new ChatMessage("system", systemPrompt));
        _waitingForReply = false;
    }

    public void SkipDialogue()
    {

    }

    public void ReadNextDialogue()
    {

    }

    public void ReadDialogue(Dialogue dialogue)
    {
        Debug.Log("Reading dialogue");
        DialogueUI.instance.DisplayDialogue(this, dialogue);
        DialogueUI.instance.DisplayResponse();
    }

    // Called by the input field's onSubmit (hooked up in DialogueUI) or a "Send" button.
    public void ReadResponse(TMP_InputField inputField)
    {
        if (_waitingForReply) return;

        string playerText = inputField.text.Trim();
        if (playerText.Length == 0) return;

        _history.Add(new ChatMessage("user", playerText));
        TrimHistory();

        _waitingForReply = true;
        DialogueUI.instance.ShowThinking();
        StartCoroutine(LLMClient.instance.SendChat(_history, OnReply, OnError));
    }

    void OnReply(string reply)
    {
        _waitingForReply = false;
        _history.Add(new ChatMessage("assistant", reply));
        DialogueUI.instance.DisplayText(reply);
        DialogueUI.instance.DisplayResponse();
    }

    void OnError(string error)
    {
        _waitingForReply = false;
        Debug.LogError("[DialogueManager] LLM error: " + error);

        // Drop the unanswered player line so they can retry cleanly.
        if (_history.Count > 1 && _history[_history.Count - 1].role == "user")
            _history.RemoveAt(_history.Count - 1);

        DialogueUI.instance.DisplayText("(They didn't seem to hear you. Try again.)");
        DialogueUI.instance.DisplayResponse();
    }

    void TrimHistory()
    {
        // Index 0 is the system prompt; remove the oldest messages after it.
        while (_history.Count > _maxHistoryMessages + 1)
            _history.RemoveAt(1);
    }
}