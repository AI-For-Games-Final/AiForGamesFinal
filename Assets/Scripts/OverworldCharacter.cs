using UnityEngine;

public class OverworldCharacter : MonoBehaviour
{
    public int mID;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        print("You just clicked on " + CharacterManager.FindSimCharacter(mID).m_name);
    }
}
