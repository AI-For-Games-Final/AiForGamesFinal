using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

[Serializable]
public class Dialogue
{
	[SerializeField] string _text;
	public string GetText(){ return _text; }
	[SerializeField] List<DialogueChoice> _choices = new List<DialogueChoice>();
	public List<DialogueChoice> GetChoices(){ return _choices; }

}

[Serializable]
public struct DialogueChoice
{
	[SerializeField] public string _choiceText;
	[SerializeField] public Dialogue _response;
}
