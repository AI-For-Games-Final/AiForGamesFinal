using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class DialogueUI : UIGroup
{
    [HideInInspector] public static DialogueUI instance;

    [Tooltip("The text that shows the character's line. Assign it so we don't accidentally grab the input field's text.")]
    [SerializeField] TextMeshProUGUI _textTF;
    [SerializeField] TMP_InputField _responseInput;
    [SerializeField] string _thinkingText = "...";
    //List<Button> _responseButtons = new List<Button>();

    private void Awake()
    {
        if (instance == null)
            instance = this;

        //_responseButtons = GetComponentsInChildren<Button>().ToList();
        if (_responseInput == null)
            _responseInput = GetComponentInChildren<TMP_InputField>();
        if (_textTF == null)
            _textTF = GetComponentInChildren<TextMeshProUGUI>();

        // Pressing Enter in the input field sends the message.
        _responseInput.onSubmit.AddListener(_ => DialogueManager.instance.ReadResponse(_responseInput));
    }

    public override void Start()
    {
        base.Start();
    }

    public override void DisplayDialogue(Component sender, object data)
    {
        SetCanvasGroup(true);

        _textTF.text = (data as Dialogue).GetText();

        /*for (int i = 0; i < _responseButtons.Count; i++)
		{
			_responseButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
			_responseButtons[i].onClick.RemoveAllListeners();
		}

		if ((data as Dialogue).GetChoices().Count > 0)
		{
			List<DialogueChoice> choices = (data as Dialogue).GetChoices();

			for (int i = 0; i < choices.Count; i++)
			{
				if (i >= _responseButtons.Count)
					break;
				var responseButton = _responseButtons[i];
				var response = choices[i]._response;
				_responseButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = choices[i]._choiceText;
				responseButton.onClick.RemoveAllListeners();
				responseButton.onClick.AddListener(() => DialogueManager.instance.ReadDialogue(response));
				Debug.Log($"Index is {i}, Will display {choices[i]._response}");
			}
		}*/
    }

    public void DisplayText(string text)
    {
        SetCanvasGroup(true);
        _textTF.text = text;
    }

    public void ShowThinking()
    {
        _textTF.text = _thinkingText;
        _responseInput.text = "";
        _responseInput.interactable = false;
    }

    public void DisplayResponse()
    {
        _responseInput.gameObject.SetActive(true);
        _responseInput.interactable = true;
        _responseInput.text = "";
        _responseInput.ActivateInputField();
    }
}