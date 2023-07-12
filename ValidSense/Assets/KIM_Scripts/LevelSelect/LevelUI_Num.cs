using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelUI_Num : MonoBehaviour
{
    [SerializeField]
    JsonReader json;

    [SerializeField]
    private Text NormalLevelNum;

    public int NaturalNum;

    [SerializeField]
    private Text HighLevelNum;

    public int HighSenseNum;

    [SerializeField]
    private Text SixthLevelNum;

    public int SixthSenseNum;

    [SerializeField]
    private int LevelUI_Pos;

    [SerializeField]
    private GameObject NormalLevelUI;

    [SerializeField]
    private GameObject HighLevelUI;

    [SerializeField]
    private GameObject SixthLevelUI;

    // Start is called before the first frame update
    void Start()
    {

        json = GameObject.Find("Json").GetComponent<JsonReader>();

        NaturalNum = json._songList.difflist.natural.level;
        HighSenseNum = json._songList.difflist.highSense.level;
        SixthSenseNum = json._songList.difflist.sixthSense.level;
        
        NormalLevelNum.text = NaturalNum.ToString();
        HighLevelNum.text = HighSenseNum.ToString();
        SixthLevelNum.text = SixthSenseNum.ToString();

        if(NaturalNum < 10)
        {
            NormalLevelUI.transform.position = new Vector3(NormalLevelNum.transform.position.x + LevelUI_Pos, NormalLevelUI.transform.position.y, NormalLevelNum.transform.position.z);
        }
        else if(NaturalNum >= 10)
        {
            NormalLevelUI.transform.position = new Vector3(NormalLevelNum.transform.position.x + (LevelUI_Pos + 7), NormalLevelUI.transform.position.y, NormalLevelNum.transform.position.z);
        }

        if (HighSenseNum < 10)
        {
            HighLevelUI.transform.position = new Vector3(HighLevelNum.transform.position.x + LevelUI_Pos, HighLevelUI.transform.position.y, HighLevelNum.transform.position.z);
        }
        else if (HighSenseNum >= 10)
        {
            HighLevelUI.transform.position = new Vector3(HighLevelNum.transform.position.x + (LevelUI_Pos + 7), HighLevelUI.transform.position.y, HighLevelNum.transform.position.z);
        }

        if (SixthSenseNum < 10)
        {
            SixthLevelUI.transform.position = new Vector3(SixthLevelNum.transform.position.x + LevelUI_Pos, SixthLevelUI.transform.position.y, SixthLevelNum.transform.position.z);
        }
        else if (SixthSenseNum >= 10)
        {
            SixthLevelUI.transform.position = new Vector3(SixthLevelNum.transform.position.x + (LevelUI_Pos + 7), SixthLevelUI.transform.position.y, SixthLevelNum.transform.position.z);
        }
    }

}
