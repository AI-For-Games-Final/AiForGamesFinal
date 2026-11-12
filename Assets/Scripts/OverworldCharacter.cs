using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldCharacter : MonoBehaviour
{
    public int mID;
    [SerializeField] SpriteRenderer slimeMode;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        slimeMode.color = CharacterManager.FindSimCharacter(mID).m_IdColor;
    }

    private void OnMouseDown()
    {
        CharacterManager.interactionsLeft -= 1;
        CharacterManager.ClearIndex();
        if (CharacterManager.interactionsLeft < 0)
        {
            SceneManager.LoadScene("EndOfSim");
        }
        OverworldManager.currentTalkID = mID;
        GameObject.Find("Dialogue").GetComponent<DialogueManager>().StartDialogue();
    }
}
