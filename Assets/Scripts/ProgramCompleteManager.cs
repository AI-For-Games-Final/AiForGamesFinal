using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgramCompleteManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartScene()
    {
        CharacterManager.interactionsLeft = 10;
        SceneManager.LoadScene("MainScene");
    }
}
