using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SimCharacter
{
    //class that contains all the information on a specific simCharacter
    public int m_IndexID;
    public string m_name;
    public Color m_IdColor; // <- Using this to differentiate characters until we have character artwork
    public CharacterManager.characterRoles m_PositionName;
}

public class CharacterManager : MonoBehaviour
{
    public static List<SimCharacter> simCharIndex = new List<SimCharacter>();
    private static int currentIndexID;
    [SerializeField] GameObject characterDisplayerInput, characterDisplayPrefabInput;
    public static GameObject characterDisplayer, characterDisplayPrefab;

    public enum characterRoles
    {
        MARKETING,
        ACCOUNTING,
        RND,
        PRODUCTION,
        MANAGEMENT
    }

    void Start()
    {
        characterDisplayer = characterDisplayerInput;
        characterDisplayPrefab = characterDisplayPrefabInput;
        currentIndexID = 0;
        for(int i = 0; i < 8; i++)
        {
            CreateNewSimCharacter();
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public static void CreateNewSimCharacter() //Creates a new sim character, sets random values for the sim character, adds that sim character to the public character list, then creates the need display UI
    {
        //Setting up sim character
        SimCharacter tempSimCharacter = new SimCharacter();
        tempSimCharacter.m_name = "todd";
        tempSimCharacter.m_IdColor = new Color(Random.Range(0.5f,1.0f), Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f));
        tempSimCharacter.m_IndexID = currentIndexID;
        tempSimCharacter.m_PositionName = (characterRoles)Random.Range(0, 4);
        simCharIndex.Add(tempSimCharacter);
        //Setting up UI
        GameObject tempSimCharDisplay = Instantiate(characterDisplayPrefab, characterDisplayer.transform);
        tempSimCharDisplay.GetComponent<CharacterDisplay>().mIndex = currentIndexID;
        currentIndexID++;

    }

    public static SimCharacter FindSimCharacter(int inputIndex) //Inputs a sim character's IDnumber and returns the sim character
    {
        for (int i = 0; i < simCharIndex.Count; i++)
        {
            if (simCharIndex[i].m_IndexID == inputIndex)
            {
                return simCharIndex[i];
            }
        }
        return null;
    }

    public static int FindSimCharIndexPosition(int inputIndex) //Inputs a sim character's IDnumber and returns the sim character's location in the sim character index
    {
        for (int i = 0; i < simCharIndex.Count; i++)
        {
            if (simCharIndex[i].m_IndexID == inputIndex)
            {
                return i;
            }
        }
        return -1;
    }
}
