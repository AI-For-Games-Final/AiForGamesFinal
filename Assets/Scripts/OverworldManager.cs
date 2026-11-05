using UnityEngine;

public class OverworldManager : MonoBehaviour
{
    [SerializeField] GameObject overworldSimChar;
    void Start()
    {
        for(int i = 0; i < CharacterManager.simCharIndex.Count; i++)
        {
            GameObject tempOverworldSimChar = Instantiate(overworldSimChar,transform);
            tempOverworldSimChar.transform.position += new Vector3(Random.Range(-4.0f, 5.0f), 0.0f, Random.Range(-6.0f, 7.0f));
            tempOverworldSimChar.GetComponent<OverworldCharacter>().mID = i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
