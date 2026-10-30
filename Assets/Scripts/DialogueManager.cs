using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [HideInInspector] public static DialogueManager instance;
    [SerializeField] Dialogue _startDialogue;

	private void Start()
	{
        if (instance == null)
            instance = this;
	}

    public void StartDialogue()
    {
		ReadDialogue(_startDialogue);
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

    public void ReadResponse(InputField inputField)
    {
        
    }
}
