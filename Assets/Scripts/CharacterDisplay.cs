using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDisplay : MonoBehaviour
{
    public int mIndex;
    [SerializeField] Image mPortrait;
    [SerializeField] TMP_Text mNameText, mRoleText;
    void Start()
    {
        transform.localPosition = new Vector3(0.0f,360.0f - (CharacterManager.FindSimCharIndexPosition(mIndex) * 100.0f),0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        mPortrait.color = CharacterManager.FindSimCharacter(mIndex).m_IdColor;
        mRoleText.text = CharacterManager.FindSimCharacter(mIndex).m_PositionName.ToString();
        mNameText.text = CharacterManager.FindSimCharacter(mIndex).m_name;
    }
}
