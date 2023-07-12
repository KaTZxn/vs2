using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinNotesCreator : MonoBehaviour
{
    /// <summary>
    /// プレイヤーマネージャー参照用
    /// </summary>
    PlayerManagerScript playerManager;

    
    /// <summary>
    /// Viewノーツをどの高さから生成するかの、基準として使う数値
    /// </summary>
    [SerializeField]
    private float _instantiatePosY;


    /// <summary>
    /// Viewノーツを各ラインに生成する時用の配列
    /// </summary>
    [SerializeField]
    private float[] _instantiatePosX = {0,0 };

    /// <summary>
    /// Viewノーツをどの奥行で生成する時用の数値
    /// </summary>
    [SerializeField]
    private float _instantiatePosZ;

    /// <summary>
    /// winNotes格納用
    /// </summary>
    [SerializeField]
    GameObject winNotes;

    /// <summary>
    /// 生成した、winNotes干渉用
    /// </summary>
    GameObject generatWinNotes;


    /// <summary>
    ///  判定バーに到達予定時間
    /// </summary>
    [SerializeField]
    private float _winNotesTime;

    /// <summary>
    /// Notesのスピード
    /// </summary>
    [SerializeField]
    private float _winNotesSpeed;

    /// <summary>
    /// WinNotesの色変更時使用するカラーコードの、配列
    /// </summary>
    [SerializeField]
    public string[] WinNotesColorCode;

    /// <summary>
    ///string型のカラーコードをColor変換した時の受け取り先
    /// </summary>
    Color colorCode;

    private void Start()
    {
        playerManager = GameObject.Find("SceneManager").GetComponent<PlayerManagerScript>();
    }

    /// <summary>
    /// WinNotesの生成
    /// </summary>
    /// <param name="usePlayer"></param>
    public void generationWinNotes(int usePlayer)
    {
        // WinNotesを生成して、干渉用に、格納
        generatWinNotes = Instantiate(winNotes);

        // winNotesの位置を調整
        generatWinNotes.transform.position = new Vector3(
            _instantiatePosX[usePlayer], _instantiatePosY, _instantiatePosZ);

        // Notesの速さ(60fps) 判定基準時間 使用者を設定
        generatWinNotes.GetComponent<WinNotesScript>().NotesSpeed_TIme_UsePlayer_Setter
            // 距離(Y * 速さ) / 時間(60fps用に設定) = 速さ
            (((_instantiatePosY * _winNotesSpeed) / _winNotesTime )/ 60
            // 判定基準時間
            , _winNotesTime
            // 使用者
            , usePlayer) ;

        // 使用者のキャラに合わせたカラーコードで、染色
        if (ColorUtility.TryParseHtmlString(
            WinNotesColorCode[playerManager.playerCharaNum[usePlayer]],out colorCode))
        {
            generatWinNotes.GetComponent<SpriteRenderer>().color = colorCode;
        }
    }

}
