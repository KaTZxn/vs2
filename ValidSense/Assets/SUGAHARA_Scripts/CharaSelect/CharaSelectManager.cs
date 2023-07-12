using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaSelectManager : MonoBehaviour
{
    /// <summary>
    /// シーンマネージャー参照する用
    /// </summary>
    [SerializeField]
    GameObject sceneManager;

    /// <summary>
    /// キャラのUIなど一括でいじる用
    /// </summary>
    [SerializeField]
    List<CharaSelect_CahraClass> charaClassList;


    /// <summary>
    /// キャラのアイコンを参照する用
    /// </summary>
    [SerializeField]
    List<RawImage> charaIcon;

    /// <summary>
    /// キャラのアイコンのキャラ選択後の画像データ
    /// </summary>
    [SerializeField]
    List<Texture> charaIcon_GreySprite;

    [SerializeField]
    List<Texture> charaIcon_ColorSprite;

    /// <summary>
    /// カーソル
    /// </summary>
    [SerializeField]
    List<GameObject> cursor;

    /// <summary>
    /// カーソル移動用のpos
    /// </summary>
    [SerializeField]
    private float[] cursorPosY;

    /// <summary>
    /// プレイヤーの入力に対応して増減する値
    /// </summary>
    [SerializeField]
    private int[] cursorNum;

    /// <summary>
    /// カーソルが移動可能か
    /// </summary>
    [SerializeField]
    private bool[] canMoveCursor;

    /// <summary>
    /// 次のシーンに遷移するのにかかる時間
    /// </summary>
    [SerializeField]
    private float toNextSceneTiem;

    /// <summary>
    /// キャラ選択が終わっていないことを表すbool
    /// </summary>
    [SerializeField]
    bool[] notEndSelect;

    /// <summary>
    /// タイマーを止めるために取得
    /// </summary>
    [SerializeField]
    SWTimer timer;

    /// <summary>
    /// プレイヤーの入力を許可するbool
    /// </summary>
    [SerializeField]
    private bool _canInput;

    /// <summary>
    /// スキルの説明を切り替えるように参照
    /// </summary>
    [SerializeField]
    List<CharaSkillAttendUIManager>  skillAttendUIManager;

    /// <summary>
    /// シーンチェンジ用のUIを出す用
    /// </summary>
    [SerializeField]
    SceneChangeUI sceneChangeUI;

    void Start()
    {
        // シーンマネージャーに参照用
        sceneManager = GameObject.Find("SceneManager");

        //入力を拒否する
        _canInput = false;


        // カーソルの初期値を、キャラ依存に設定
        cursorNum = sceneManager.GetComponent<PlayerManagerScript>().playerCharaNum;


        //初期表示更新 表示キャラクター
        charaClassList[(int)ConstRepo.Player.P1]
            .CharaUI_1P_DisplaySwitching(cursorNum[(int)ConstRepo.Player.P1]);
        charaClassList[(int)ConstRepo.Player.P2]
            .CharaUI_1P_DisplaySwitching(cursorNum[(int)ConstRepo.Player.P2]);

        //初期表示更新 カーソル位置
        MoveCursor((int)ConstRepo.Player.P1);
        MoveCursor((int)ConstRepo.Player.P2);

        //初期表示更新 スキルの名前と、説明文の変更
        skillAttendUIManager[(int)ConstRepo.Player.P1].
            SkillName_Attend_SetSprite(cursorNum[(int)ConstRepo.Player.P1]);
        skillAttendUIManager[(int)ConstRepo.Player.P2].
            SkillName_Attend_SetSprite(cursorNum[(int)ConstRepo.Player.P2]);


        Invoke("CanInputSet_True", 0.7f);

        Invoke("CanInputSet_False", timer.timeMaxValue );
    }


    void Update()
    {

        // プレイヤーの入力が許可されていたら
        if(_canInput)
        {
            // キャラ決定してなかったら、1pのカーソル移動
            if (canMoveCursor[(int)ConstRepo.Player.P1])
            {
                if (//Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.E) ||
                    Input.GetKeyDown(KeyCode.R))
                {
                    SEPlayer.instance.SEOneShot(5);

                    ChangeCharacter((int)ConstRepo.Player.P1, -1);
                }
                else if (//Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) ||
                    Input.GetKeyDown(KeyCode.F))
                {
                    SEPlayer.instance.SEOneShot(5);

                    ChangeCharacter((int)ConstRepo.Player.P1, 1);
                }
            }

            // キャラ決定してなかったら、2pのカーソル移動
            if (canMoveCursor[(int)ConstRepo.Player.P2])
            {
                if (//Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.I) ||
                    Input.GetKeyDown(KeyCode.U))
                {
                    SEPlayer.instance.SEOneShot(5);

                    ChangeCharacter((int)ConstRepo.Player.P2, -1);
                }
                else if (//Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.K) ||
                    Input.GetKeyDown(KeyCode.J))
                {
                    SEPlayer.instance.SEOneShot(5);

                    ChangeCharacter((int)ConstRepo.Player.P2, 1);
                }
            }


            //2Pがキャラを決定していて、その上キャラが被りしていたらキャラ決定不可
            if (!canMoveCursor[(int)ConstRepo.Player.P2] &&
                cursorNum[(int)ConstRepo.Player.P1] == cursorNum[(int)ConstRepo.Player.P2])
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    SEPlayer.instance.SEOneShot(4);
                }

            }
            //1Pキャラの決定
            else if (Input.GetKeyDown(KeyCode.Q) && notEndSelect[(int)ConstRepo.Player.P1])
            {
                CharaSelect((int)ConstRepo.Player.P1);
            }
            // キャラ決定解除
            else if (Input.GetKeyDown(KeyCode.S) && !notEndSelect[(int)ConstRepo.Player.P1])
            {
                Debug.Log("リセット");
                CharaSelect_Reset((int)ConstRepo.Player.P1);
            }


            //1Pがキャラを決定していて、その上キャラが被りしていたらキャラ決定不可
            if (!canMoveCursor[(int)ConstRepo.Player.P1] &&
                cursorNum[(int)ConstRepo.Player.P1] == cursorNum[(int)ConstRepo.Player.P2])
            {
                if (Input.GetKeyDown(KeyCode.P))
                {
                    SEPlayer.instance.SEOneShot(4);
                }
            }
            //2Pキャラの決定
            else if (Input.GetKeyDown(KeyCode.P) && notEndSelect[(int)ConstRepo.Player.P2])
            {
               
                CharaSelect((int)ConstRepo.Player.P2);
            }
            // キャラ決定解除
            else if (Input.GetKeyDown(KeyCode.L) && !notEndSelect[(int)ConstRepo.Player.P2])
            {
                Debug.Log("リセット");
                CharaSelect_Reset((int)ConstRepo.Player.P2);
            }




            //1p2pがキャラを選択したらMusicSelectに遷移
            if (!canMoveCursor[(int)ConstRepo.Player.P1] && !canMoveCursor[(int)ConstRepo.Player.P2])
            {
                // 入力を許可しを閉じる
                _canInput = false;

                // タイマーの停止
                timer.CountDownStop();

                // 曲選択画面に遷移
                StartCoroutine(ToMusicSelectScene());
            }
        }
    }

    /// <summary>
    /// カーソルの移動
    /// </summary>
    /// <param name="player"></param>
    void MoveCursor(int player)
    {
        cursor[player].transform.position = new Vector3
            (cursor[player].transform.position.x,cursorPosY[cursorNum[player]], cursor[player].transform.position.z);
   
    }


    /// <summary>
    /// キャラクターの変更
    /// </summary>
    /// <param name="player"></param>
    /// <param name="cursorNumFluctuation"></param>
    void ChangeCharacter(int player, int cursorNumFluctuation)
    {
        //カーソルの位置を決定させる
        if (0 > cursorNumFluctuation)
        {
            //　現在、一番上のカーソルのなら
            if (cursorNum[player] == (int)ConstRepo.Chara.Sight)
            {
                // 一番下のカーソルの番号にする
                cursorNum[player] = (int)ConstRepo.Chara.Hear;
            }
            else
            {
                // カーソルの番号減算
                cursorNum[player]--;
            }
        }
        else if (0 < cursorNumFluctuation)
        {
            //　現在、一番下のカーソルのなら
            if (cursorNum[player] == (int)ConstRepo.Chara.Hear)
            {
                // 一番下上のカーソルの番号にする
                cursorNum[player] = (int)ConstRepo.Chara.Sight;
            }
            else
            {
                // カーソルの番号加算
                cursorNum[player]++;
            }
        }

        // キャラ表示スイッチ
        charaClassList[player].
            CharaUI_1P_DisplaySwitching(cursorNum[player]);

        // カーソルの移動
        MoveCursor(player);

        // スキルの名前と、説明文の変更
        skillAttendUIManager[player].SkillName_Attend_SetSprite(cursorNum[player]);

    }

    /// <summary>
    /// キャラクター決定
    /// </summary>
    /// <param name="playerNum"></param>
    void CharaSelect(int playerNum)
    {
        //キャラ選択したことを、記憶する
        notEndSelect[playerNum] = false;

        //決定時のSEを再生する
        SEPlayer.instance.SEOneShot(6);

        //キャラ選択時のアニメーションを再生する
        charaClassList[playerNum]
            .SelectChara_AimChoice(cursorNum[playerNum]);

        //カーソルをの移動許可を閉じる
        canMoveCursor[playerNum] = false;

        //選択したキャラに合った、キャラアイコンの画像を灰色のものにする
        charaIcon[cursorNum[playerNum]].texture
            = charaIcon_GreySprite[cursorNum[playerNum]];

        // キャラの表示順を前にする
        charaClassList[playerNum].SetCharaOrderInLayer_Max();
    }

    /// <summary>
    /// キャラ選択のリセット
    /// </summary>
    /// <param name="playerNum"></param>
    void CharaSelect_Reset(int playerNum)
    {
        // キャラ選択し終わった事をなかったことにする
        notEndSelect[playerNum] = true;

        //決定時のSEを再生する
        SEPlayer.instance.SEOneShot(6);

        //カーソルをの移動許可を再度だす
        canMoveCursor[playerNum] = true;

        //選択したキャラに合った、キャラアイコンの画像をカラーに戻す
        charaIcon[cursorNum[playerNum]].texture
            = charaIcon_ColorSprite[cursorNum[playerNum]];

        // キャラの表示順を元に戻す（後ろにする）
        charaClassList[playerNum].SetCharaOrderInLayer_Min();
    }



    /// <summary>
    /// シーン遷移
    /// </summary>
    /// <returns></returns>
    IEnumerator ToMusicSelectScene()
    {
        yield return new WaitForSeconds(toNextSceneTiem - 1.5f);

        // シーンチェンジUIのアニメーションを再生
        sceneChangeUI.SceneEndAnim();

        yield return new WaitForSeconds(1.5f);

        // 曲を止める
        MainBGMPlayer.instance.StopPlayer();

        // 選択したキャラを、プレイヤーマネージャーに登録する
        sceneManager.GetComponent<PlayerManagerScript>().playerCharaNum = cursorNum;

        // シーンを遷移する
        sceneManager.GetComponent<Test>().ToMusicSelectScene();
    }

    /// <summary>
    /// プレイヤーの入力を許可する
    /// </summary>
    void CanInputSet_True()
    {
        _canInput = true;
    }

    /// <summary>
    /// プレイヤーの入力を許可しない
    /// </summary>
    void CanInputSet_False()
    {
        _canInput = false;
    }
}
