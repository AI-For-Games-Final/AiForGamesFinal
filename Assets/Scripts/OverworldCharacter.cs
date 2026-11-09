using UnityEngine;

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
        print("You just clicked on " + CharacterManager.FindSimCharacter(mID).m_name);
    }
}
