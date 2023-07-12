using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicLevelUIScript : MonoBehaviour
{
    /// <summary>
    /// プレイヤーのマネージャー参照用
    /// </summary>
    PlayerManagerScript _playerManager;

    /// <summary>
    /// 読み込む用の譜面データ（JsonReader）
    /// </summary>
    private JsonReader _json;


    /// <summary>
    /// 難易度の、テキスト配列
    /// </summary>
    [SerializeField]
    private string[] _levelText;

    /// <summary>
    /// 楽曲の難易度取得
    /// </summary>
    private int _levelNum;

    /// <summary>
    /// 使用しているプレイヤーの番号
    /// </summary>
    [SerializeField]
    private int usePlayer;

    /// <summary>
    /// レベルの種類を表示してるテキスト
    /// </summary>
    [SerializeField]
    Text text;

    [SerializeField]
    Text text2;

    /// <summary>
    /// レベルの数値を表示してるテキスト
    /// </summary>
    [SerializeField]
    Text value;

    // Start is called before the first frame update
    void Start()
    {
        _playerManager = GameObject.Find("SceneManager").GetComponent<PlayerManagerScript>();

        _json = GameObject.Find("Json").GetComponent<JsonReader>();

        //使用してるプレイヤーがどの難易度を選択したか、代入
        _levelNum = _playerManager.nowMusicLevel[usePlayer];

        //　楽曲の選択された難易度の、レベルの数値を表示
        switch (_levelNum)
        { 
            case 0:
                value.text = _json._songList.difflist.natural.level.ToString(("F1"));
                break;
            case 1:
                value.text = _json._songList.difflist.highSense.level.ToString(("F1"));
                break;
            case 2:
                value.text = _json._songList.difflist.sixthSense.level.ToString(("F1"));
                break;
        }


        // 楽曲の選択された難易度の種類を表示
        text.text = _levelText[_levelNum];
    }


    public void HideLevelText()
    {
        text.color -= new Color(0, 0, 0, 1f);
        text2.color -= new Color(0, 0, 0, 1f);
        value.color -= new Color(0, 0, 0, 1f);
    }
}
