using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
public class DialogueUI : UIGroup
{
    [HideInInspector] public static DialogueUI instance;
	TextMeshProUGUI _textTF;
	List<Button> _responseButtons = new List<Button>();

	private void Awake()
	{
		_responseButtons = GetComponentsInChildren<Button>().ToList();
		_textTF = GetComponentInChildren<TextMeshProUGUI>();
	}

	public virtual void Start()
	{
		base.Start();

		if (instance == null)
			instance = this;
	}
	public override void Display(Component sender, object data)
	{
		SetCanvasGroup(true);

		_textTF.text = (data as Dialogue).GetText();



		for (int i = 0; i < _responseButtons.Count; i++)
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
		}
	}
}
