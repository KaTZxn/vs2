using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TitleMp4BG : MonoBehaviour
{
    [SerializeField]
    PlayerManagerScript playerManagerScript;


    [SerializeField]
    string[] charaColorCode;

    Color colorCode;


    // Start is called before the first frame update
    void Start()
    {
        playerManagerScript = GameObject.Find("SceneManager").GetComponent<PlayerManagerScript>();


        // 前回の勝利キャラの色にタイトルのmp4を変える
        if (ColorUtility.TryParseHtmlString(charaColorCode[playerManagerScript.winCharaNum],
            out colorCode))
        {

            // 所持ラインの一番2p側を相手の色に変更する
            gameObject.GetComponent<RawImage>().color = colorCode;
        }
    }


}
