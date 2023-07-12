using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_ViewNotesManager : MonoBehaviour
{
    /// <summary>
    /// キャラをいじってる子に参照用
    /// </summary>
    [SerializeField]
    PlayerManagerScript playerManager;

    /// <summary>
    /// プレイヤーの入力を管理している子に参照用
    /// </summary>
    [SerializeField]
    PlayerInput playerInput;

    /// <summary>
    /// Notesの見た目を生成しているオブジェクト参照用
    /// </summary>
    [SerializeField]
    ViewNotesCreator viewNotesCreator;

    /// <summary>
    /// Notesの色変更時使用するカラーコードの、配列
    /// </summary>
    public string[] charaNotesColorCode;

    /// <summary>
    /// PoisonNotesの色変更時使用するカラーコードの、配列
    /// </summary>
    public string[] poisonNotesColorCode;

    /// <summary>
    ///string型のカラーコードをColor変換した時の受け取り先
    /// </summary>
    Color colorCode;

    /// <summary>
    /// 表示用ノーマルノーツ、スライドノーツをlineごとに格納し、生成したViewノーツを管理する用
    /// </summary>
    [SerializeField]
    public List<ViewNotesListClass> simpleViewNotesLists;


    /// <summary>
    /// 各ラインが何番目の表示用ノーマルノーツ、スライドノーツに干渉するか、管理用
    /// </summary>
    [SerializeField]
    public int[] simpleViewNotesLineNum;


    /// <summary>
    /// 各ラインが何番目の表示用ノーマルノーツ、スライドノーツを消すか、管理用
    /// </summary>
    [SerializeField]
    public int[] simpleViewNotesDereatNum_Line;


    /// <summary>
    /// 表示用ホールドノーツをlineごとに格納し、生成したViewノーツを管理する用
    /// </summary>
    [SerializeField]
    public List<ViewNotesListClass> holdViewNotesLists;


    /// <summary>
    /// 各ラインが何番目の表示用ホールドノーツに干渉するか、管理用
    /// </summary>
    [SerializeField]
    public int[] holdViewNotesLineNum;


    /// <summary>
    /// 各ラインが何番目の表示用ホールドノーツを消すか、管理用
    /// </summary>
    [SerializeField]
    public int[] holdViewNotesDereatNum_Line;


    /// <summary>
    /// Viewノーツをどれくらいの速さで流すかの基準になる、値
    /// 主に難易度に対応
    /// </summary>
    public float[] viewNotesSpeed;

    /// <summary>
    /// Viewノーツをどれくらいの速さで流すかの基準になる、値
    /// 主に難易度に対応
    /// </summary>
    public float[] viewNotesSpeed_Index;


    /// <summary>
    /// スキルによるViewノーツSpeedの変化用の値(倍率)
    /// </summary>
    [Range(0.5f, 2.5f)]
    public float[] NotesSpeedLeverage;

    /// <summary>
    /// 現在干渉するNotesの各ラインごとの一時保存先
    /// </summary>
    [SerializeField]
    List<SpriteRenderer> nowSimpleNotes_Sprite;

    /// <summary>
    /// ポイズンノーツか、判別用に使用しているポイズンノーツのスプライトの名前を保存しておく用
    /// </summary>
    private string _poisonNotesName;

    private void Start()
    {
        playerManager = GameObject.Find("SceneManager").GetComponent<PlayerManagerScript>();

        //　難易度に合わせてNotesの基本速度の設定
        for (int i = 0; i < 2; i++)
        {
            viewNotesSpeed[i] = viewNotesSpeed_Index[playerManager.nowMusicLevel[i]];
        }

        //使用している、ポイズンNotesのスプライトの名前を取得
        _poisonNotesName = viewNotesCreator.poisonNotesSprite.name;
    }




    /// <summary>
    /// // 引数番のラインの、viewNotesDereatNum_Line番のViewNotesのフェードアウト
    /// </summary>
    /// <param name="line"> 干渉するラインの指定 </param>
    public void NowSimpleViewNotesFadeOut(int line)
    {
        //viewNotesDereatNum_Line番のViewNotesのフェードアウト
        simpleViewNotesLists[line].viewNotesList[simpleViewNotesDereatNum_Line[line]]
            .GetComponent<ViewNotesScript>().NotesFadeOut();


        simpleViewNotesDereatNum_Line[line]++;
    }


    /// <summary>
    /// // 引数番のラインの、viewNotesDereatNum_Line番のViewNotesのフェードアウト
    /// </summary>
    /// <param name="line"> 干渉するラインの指定 </param>
    public void NowHoldViewNotesFadeOut(int line)
    {
        //HoldNotesDereatNum_Line番のHoldNotesのフェードアウト
        holdViewNotesLists[line].viewNotesList[holdViewNotesDereatNum_Line[line]]
            .GetComponent<HoldNotesScript>().AllFadeOut();

        holdViewNotesDereatNum_Line[line]++;
    }

    /// <summary>
    /// HoldNotesのレイヤーを変更して、描画範囲の調整されたカメラのみに描画させる
    /// </summary>
    /// <param name="line"></param>
    public void NowHoldViewNotesChangeLayers(int line)
    {
        //HoldNotesDereatNum_Line番のHoldNotesのレイヤーを変更して、描画範囲の違うカメラのみに描画される
        holdViewNotesLists[line].viewNotesList[holdViewNotesDereatNum_Line[line]]
            .GetComponent<HoldNotesScript>().ChangeLayers();
    }


    /// <summary>
    /// 引数のラインの流れているノーツの色引数のキャラの色にすべて変える
    /// </summary>
    /// <param name="line">ノーツの色を変えるライン</param>
    /// <param name="playerNum">どっちのプレイヤーか判定用</param>
    public void ActiveHoldViewNotesColourChange(int line, int playerNum)
    {


        for (int i = 0; i < holdViewNotesLists[line].viewNotesList.Count - holdViewNotesDereatNum_Line[line];)
        {
            //　最初だけHoldNotesが今入力されているかで、飛ばすか飛ばさないか参照
            if (playerInput.canHoldEndJudges[line] && i == 0)
            {

            }
            else
            {
                if (playerNum == (int)ConstRepo.Player.P1)
                {
                    holdViewNotesLists[line].viewNotesList[holdViewNotesDereatNum_Line[line] + i].
                   GetComponent<HoldNotesScript>().NotesColorCange(charaNotesColorCode
                   [playerManager.playerCharaNum[(int)ConstRepo.Player.P1]]);
                }
                else
                {
                    holdViewNotesLists[line].viewNotesList[holdViewNotesDereatNum_Line[line] + i].
                   GetComponent<HoldNotesScript>().NotesColorCange(charaNotesColorCode
                   [playerManager.playerCharaNum[(int)ConstRepo.Player.P2]]);
                }
            }
            // 次にする
            i++;
        }
    }

    /// <summary>
    /// 引数のラインの流れているノーツの色引数のキャラの色にすべて変える
    /// </summary>
    /// <param name="line">ノーツの色を変えるライン</param>
    /// <param name="playerNum">どっちのプレイヤーか判定用</param>
    public void ActiveSimpleViewNotesColourChange(int line, int playerNum)
    {
        for (int i = 0; i < simpleViewNotesLists[line].viewNotesList.Count - simpleViewNotesDereatNum_Line[line];)
        {
            //スプライトの一時保存
            nowSimpleNotes_Sprite[line] = simpleViewNotesLists[line].viewNotesList[simpleViewNotesDereatNum_Line[line] + i].
                   GetComponent<SpriteRenderer>();

            //使用してしているポイズンノーツのスプライトと同じスプライトの名前なら
            if (nowSimpleNotes_Sprite[line].sprite.name == _poisonNotesName)
            {
                //　ポイズンノーツ用のカラーコードで、染色
                if (playerNum == (int)ConstRepo.Player.P1)
                {
                    if (ColorUtility.TryParseHtmlString(poisonNotesColorCode
                        [playerManager.playerCharaNum[(int)ConstRepo.Player.P1]], out colorCode))
                    {
                        nowSimpleNotes_Sprite[line].color = colorCode;
                    }
                }
                else
                {
                    if (ColorUtility.TryParseHtmlString(poisonNotesColorCode
                        [playerManager.playerCharaNum[(int)ConstRepo.Player.P2]], out colorCode))
                    {
                        nowSimpleNotes_Sprite[line].color = colorCode;
                    }
                }
            }
            // 通常ノーツ
            else
            {
                //　通常用のカラーコードで、染色
                if (playerNum == (int)ConstRepo.Player.P1)
                {
                    if (ColorUtility.TryParseHtmlString(charaNotesColorCode
                        [playerManager.playerCharaNum[(int)ConstRepo.Player.P1]], out colorCode))
                    {
                        nowSimpleNotes_Sprite[line].color = colorCode;
                    }
                }
                else
                {
                    if (ColorUtility.TryParseHtmlString(charaNotesColorCode
                        [playerManager.playerCharaNum[(int)ConstRepo.Player.P2]], out colorCode))
                    {
                        nowSimpleNotes_Sprite[line].color = colorCode;
                    }
                }
            }

            // 次にする
            i++;
        }
    }
}
