using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_PlayerInput : MonoBehaviour
{
    /// <summary>
    /// キャラの番号を記憶しているオブジェクトに参照する用
    /// </summary>
    [SerializeField]
    TestChara charaObj;


    [SerializeField]
    MusicPlayer musicPlayer;


    /// <summary>
    /// 判定バー到達のどれくらい前から判定可するかの値
    /// </summary>
    [SerializeField]
    private double judgeTime;


    /// <summary>
    /// ラインマネージャーに参照する用
    /// </summary>
    [SerializeField]
    LinesManager linesManager;


    /// <summary>
    /// ノーツのデータスクリプトを参照する用
    /// </summary>
    [SerializeField]
    NotesDataList notesDataList;


    /// <summary>
    /// ノーツの判定用スクリプトを参照する用
    /// </summary>
    [SerializeField]
    TutorialNotesJudgeScript notesJudge;


    /// <summary>
    /// スキル使用用スクリプトを参照する用
    /// </summary>
    [SerializeField]
    SkillManager skillManager;


    /// <summary>
    /// スキルのUIスクリプトを参照する用
    /// </summary>
    [SerializeField]
    List<SkillChargeRate> skillChargeRate;


    /// <summary>
    /// 何番目のノーツに干渉するかの値の配列
    /// </summary>
    [SerializeField]
    public int[] lineNotesNum;


    /// <summary>
    /// スキルの発動時に連続でのキー入力に対する猶予時間
    /// </summary>
    [SerializeField]
    private float _inputStandbyTime;


    /// <summary>
    /// スキルの発動時同時押しで誤作動しないようにする為の入力無効時間
    /// </summary>
    [SerializeField]
    private float _inputInvalidTime;


    /// <summary>
    /// スキルの発動時にキー入力してからどれくらい時間が経過してるか記憶用(右からの入力)
    /// </summary>
    [SerializeField]
    private float[] _inputWaitTime_R_1P;

    /// <summary>
    /// スキルの発動時にキー入力してからどれくらい時間が経過してるか記憶用(左からの入力)
    /// </summary>
    [SerializeField]
    private float[] _inputWaitTime_L_1P;


    /// <summary>
    /// スキルの発動時にキー入力してからどれくらい時間が経過してるか記憶用(右からの入力)
    /// </summary>
    [SerializeField]
    private float[] _inputWaitTime_R_2P;

    /// <summary>
    /// スキルの発動時にキー入力してからどれくらい時間が経過してるか記憶用(左からの入力)
    /// </summary>
    [SerializeField]
    private float[] _inputWaitTime_L_2P;

    /// <summary>
    /// レーンごとに入力にに応じて、白くなり、押しているレーンが分かるようにするオブジェクト
    /// </summary>
    [SerializeField]
    LineOverTexture[] line_OverTextures;

    /// <summary>
    /// レーンごとに次のノーツデータを格納するリスト
    /// </summary>
    [SerializeField]
    List<JsonReader.NoteList> nextJudgeNotes;


    /// <summary>
    /// 判定をレーンごとに許可する変数
    /// </summary>
    [SerializeField]
    private bool[] canNotesJudges;

    /// <summary>
    /// holdのendTimeの判定をレーンごとに許可する変数
    /// </summary>
    [SerializeField]
    public bool[] canHoldEndJudges;

    /// <summary>
    /// holdのendTimeのキーをレーンごとに許可する変数
    /// </summary>
    [SerializeField]
    private bool[] isHoldEndJudge_KeyUp;

    /// <summary>
    /// LinkNotesに対して入力をプレイヤーごとに、制御する変数
    /// </summary>
    [SerializeField]
    private bool[] canLinkNotesInput;

    /// <summary>
    /// 現在のリンクノーツの判定が最初かどうかの変数
    /// </summary>
    [SerializeField]
    private bool isLinkNotes_FirstJudge;

    /// <summary>
    /// Notes判定を何回したかカウントする用
    /// </summary>
    [SerializeField]
    private int jugeCount;

    /// <summary>
    /// 手動と、オートを、何回切り替えたか、カウントする用
    /// </summary>
    private int myselfAutoSwitchCount;

    /// <summary>
    /// 手動と、オートを何回判定したら、切り替えるか設定用
    /// </summary>
    [SerializeField]
    private int[] myselfAuto_SwitchNum;

    /// <summary>
    /// オートで判定する
    /// </summary>
    [SerializeField]
    bool autoJudge;

    /// <summary>
    /// 譜面データダウンロードを待機(プレイヤーの操作と、オート機能も)
    /// </summary>
    [SerializeField]
    private float standbyTime;

    /// <summary>
    /// プレイヤーの入力を許可する変数
    /// </summary>
    private bool canInput;

    /// <summary>
    /// ラインのオーバーテクスチャが可視化されていたら
    /// </summary>
    [SerializeField]
    private bool[] line_OnFadeIn = { false, false, false, false, false, false, false, false };


    /// <summary>
    /// ホールドノーツのSEを再生用
    /// </summary>
    [SerializeField]
    List<HoldNotesSE> holdNotesSEPlayers;

    /// <summary>
    /// スキル発動を許可する変数
    /// </summary>
    private bool tutorial_CanSkill = true;


    private void Start()
    {
        // プレイヤーの入力を拒否
        canInput = false;

        // 譜面データダウンロードを引数分待機して、読み込む()
        Invoke("SetData", standbyTime);

        //プレイヤーの入力を許可
        Invoke("SetOn_CanInput", standbyTime);
    }

    /// <summary>
    /// プレイヤーの入力を許可
    /// </summary>
    void SetOn_CanInput()
    {
        canInput = true;
    }


    void SetData()
    {
        // 最初のノーツデータを各レーンに格納
        for (int i = 0; i < 8;)
        {
            // 指定のレーンのデータを該当レーン格納
            SetNextNotesData(i);
            i++;
        }

        // 値の初期化
        autoJudge = true;
        jugeCount = 0;
        myselfAutoSwitchCount = 0;
    }

    void Update()
    {
        //        //判定バーデバック用(判定バー到達予定時間に任意エラーにて強制停止)
        //        if (MusicData.Timer >= nextJudgeNotes[0].time)
        //        {
        //            Debug.Log("Timer" + MusicData.Timer);
        //            Debug.Log("Judge" + nextJudgeNotes[0].time);

        //            Debug.Log("差" + (MusicData.Timer - nextJudgeNotes[0].time));

        //#if UNITY_EDITOR
        //            UnityEditor.EditorApplication.isPaused = true;
        //#endif
        //        }

        //        //Hold end 判定バーデバック用(判定バー到達予定時間に任意エラーにて強制停止)
        //        if (MusicData.Timer >= nextJudgeNotes[1].endtime)
        //        {
        //            Debug.Log("Timer" + MusicData.Timer);
        //            Debug.Log("Judge" + nextJudgeNotes[1].endtime);

        //            Debug.Log("差" + (MusicData.Timer - nextJudgeNotes[1].endtime));

        //#if UNITY_EDITOR
        //            UnityEditor.EditorApplication.isPaused = true;
        //#endif
        //        }






        #region // 1p 入力中ラインを白くして、わかりやすくする

        if (Input.GetKeyDown(KeyCode.Q))
        {
            // ノーツが無くなった時用の空打ちSE
            if (!canNotesJudges[0])
            {
                // 空打ちSE
                SEPlayer.instance.SEOneShot(0);
            }

            // ラインのオーバーテクスチャをフェードインさせる
            line_OverTextures[0].fadeIn();


        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            // ラインのオーバーテクスチャが、表示されていたら
            // フェードアウト
            line_OverTextures[0].fadeOut();
        }


        // wに対応するラインが奪われていたら、Sキーで判定
        if (linesManager.Get_1pHaveLines() <= 0)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                //　ノーツが無くなった時用の空打ちSE
                if (!canNotesJudges[1])
                {
                    // 空打ちSE
                    SEPlayer.instance.SEOneShot(0);
                }

                // ラインのオーバーテクスチャをフェードインさせる
                line_OverTextures[1].fadeIn();
                line_OnFadeIn[1] = true;

            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                line_OverTextures[1].fadeOut();
                line_OnFadeIn[1] = false;
            }


            // もし、Sキーが入力されていないかつ、レーンが表示されている
            if (!Input.GetKey(KeyCode.S) && line_OnFadeIn[1])
            {
                // キーを離した時に、フェードアウト
                if (Input.GetKeyUp(KeyCode.W))
                {
                    line_OverTextures[1].fadeOut();
                    line_OnFadeIn[1] = false;
                }
            }

        }
        // wに対応するラインが奪われていなかったら、Wキーで判定
        else
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                //　ノーツが無くなった時用の空打ちSE
                if (!canNotesJudges[1])
                {
                    // 空打ちSE
                    SEPlayer.instance.SEOneShot(0);
                }

                line_OverTextures[1].fadeIn();
                line_OnFadeIn[1] = true;
            }
            else if (Input.GetKeyUp(KeyCode.W))
            {
                line_OverTextures[1].fadeOut();
                line_OnFadeIn[1] = false;
            }

            // もし、Wキーが入力されていないかつ、レーンが表示されている
            if (!Input.GetKey(KeyCode.W) && line_OnFadeIn[1])
            {
                // キーを離した時に、フェードアウト
                if (Input.GetKeyUp(KeyCode.S))
                {
                    line_OverTextures[1].fadeOut();
                    line_OnFadeIn[1] = false;
                }
            }
        }


        // Eに対応するラインが奪われていたら、Dキーで判定
        if (linesManager.Get_1pHaveLines() <= 1)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                //　ノーツが無くなった時用の空打ちSE
                if (!canNotesJudges[2])
                {
                    // 空打ちSE
                    SEPlayer.instance.SEOneShot(0);
                }

                line_OverTextures[2].fadeIn();
                line_OnFadeIn[2] = true;
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                line_OverTextures[2].fadeOut();
                line_OnFadeIn[2] = false;
            }

            // もし、Dキーが入力されていないかつ、レーンが表示されている
            if (!Input.GetKey(KeyCode.D) && line_OnFadeIn[2])
            {
                // キーを離した時に、フェードアウト
                if (Input.GetKeyUp(KeyCode.E))
                {
                    line_OverTextures[2].fadeOut();
                    line_OnFadeIn[2] = false;
                }
            }
        }
        // Eに対応するラインが奪われていなかったら、Eキーで判定
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //　ノーツが無くなった時用の空打ちSE
                if (!canNotesJudges[2])
                {
                    // 空打ちSE
                    SEPlayer.instance.SEOneShot(0);
                }
                line_OverTextures[2].fadeIn();
                line_OnFadeIn[2] = true;
            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
                line_OverTextures[2].fadeOut();
                line_OnFadeIn[2] = false;
            }

            // もし、Eキーが入力されていないかつ、レーンが表示されている
            if (!Input.GetKey(KeyCode.E) && line_OnFadeIn[2])
            {
                // キーを離した時に、フェードアウト
                if (Input.GetKeyUp(KeyCode.D))
                {
                    line_OverTextures[2].fadeOut();
                    line_OnFadeIn[2] = false;
                }
            }
        }


        // Rに対応するラインが奪われていたらFキーで判定
        if (linesManager.Get_1pHaveLines() <= 2)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                //　ノーツが無くなった時用の空打ちSE
                if (!canNotesJudges[3])
                {
                    // 空打ちSE
                    SEPlayer.instance.SEOneShot(0);
                }
                line_OverTextures[3].fadeIn();
                line_OnFadeIn[3] = true;
            }
            else if (Input.GetKeyUp(KeyCode.F))
            {
                line_OverTextures[3].fadeOut();
                line_OnFadeIn[3] = false;
            }

            // もし、Fキーが入力されていないかつ、レーンが表示されている
            if (!Input.GetKey(KeyCode.F) && line_OnFadeIn[3])
            {
                // キーを離した時に、フェードアウトS
                if (Input.GetKeyUp(KeyCode.R))
                {
                    line_OverTextures[3].fadeOut();
                    line_OnFadeIn[3] = false;
                }
            }
        }
        // Rに対応するラインが奪われていなかったらRキーで判定
        else
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (!canNotesJudges[3])
                {
                    SEPlayer.instance.SEOneShot(0);
                }

                line_OverTextures[3].fadeIn();
                line_OnFadeIn[3] = true;
            }
            else if (Input.GetKeyUp(KeyCode.R))
            {
                line_OverTextures[3].fadeOut();
                line_OnFadeIn[3] = false;
            }

            // もし、Rキーが入力されていないかつ、レーンが表示されている
            if (!Input.GetKey(KeyCode.R) && line_OnFadeIn[3])
            {
                // キーを離した時に、フェードアウト
                if (Input.GetKeyUp(KeyCode.F))
                {
                    line_OverTextures[3].fadeOut();
                    line_OnFadeIn[3] = false;
                }
            }
        }
        #endregion


        #region // 2p入力中ラインを白くして、わかりやすくする
        // Pキーを押したとき
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!canNotesJudges[4])
            {
                SEPlayer.instance.SEOneShot(0);
            }
            line_OverTextures[4].fadeIn();
        }
        else if (Input.GetKeyUp(KeyCode.P))
        {
            line_OverTextures[4].fadeOut();
        }


        // Lに対応するラインが奪われてなかったらLキーで判定
        if (linesManager.Get_1pHaveLines() >= 6)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                if (!canNotesJudges[5])
                {
                    SEPlayer.instance.SEOneShot(0);
                }
                line_OverTextures[5].fadeIn();
                line_OnFadeIn[5] = true;
            }
            else if (Input.GetKeyUp(KeyCode.L))
            {
                line_OverTextures[5].fadeOut();
                line_OnFadeIn[5] = false;
            }

            // もし、Lキーが入力されていないかつ、レーンが表示されている
            if (!Input.GetKey(KeyCode.L) && line_OnFadeIn[5])
            {
                // キーを離した時に、フェードアウト
                if (Input.GetKeyUp(KeyCode.O))
                {
                    line_OverTextures[5].fadeOut();
                    line_OnFadeIn[5] = false;
                }
            }
        }
        // Lに対応するラインが奪われていたらOキーで判定
        else
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                if (!canNotesJudges[5])
                {
                    SEPlayer.instance.SEOneShot(0);
                }
                line_OverTextures[5].fadeIn();
                line_OnFadeIn[5] = true;
            }
            else if (Input.GetKeyUp(KeyCode.O))
            {
                line_OverTextures[5].fadeOut();
                line_OnFadeIn[5] = false;
            }

            // もし、Oキーが入力されていないかつ、レーンが表示されている
            if (!Input.GetKey(KeyCode.O) && line_OnFadeIn[5])
            {
                // キーを離した時に、フェードアウト
                if (Input.GetKeyUp(KeyCode.L))
                {
                    line_OverTextures[5].fadeOut();
                    line_OnFadeIn[5] = false;
                }
            }
        }



        // Kに対応するラインが奪われてなかったらKキーで判定
        if (linesManager.Get_1pHaveLines() >= 5)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                if (!canNotesJudges[6])
                {
                    SEPlayer.instance.SEOneShot(0);
                }
                line_OverTextures[6].fadeIn();
                line_OnFadeIn[6] = true;

            }
            else if (Input.GetKeyUp(KeyCode.K))
            {
                line_OverTextures[6].fadeOut();
                line_OnFadeIn[6] = false;
            }

            // もし、Kキーが入力されていないかつ、レーンが表示されている
            if (!Input.GetKey(KeyCode.K) && line_OnFadeIn[6])
            {
                // キーを離した時に、フェードアウト
                if (Input.GetKeyUp(KeyCode.I))
                {
                    line_OverTextures[6].fadeOut();
                    line_OnFadeIn[6] = false;
                }
            }
        }
        // Kに対応するラインが奪われていたらIキーで判定
        else
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (!canNotesJudges[6])
                {
                    SEPlayer.instance.SEOneShot(0);
                }
                line_OverTextures[6].fadeIn();
                line_OnFadeIn[6] = true;
            }
            else if (Input.GetKeyUp(KeyCode.I))
            {
                line_OverTextures[6].fadeOut();
                line_OnFadeIn[6] = false;
            }

            // もし、Iキーが入力されていないかつ、レーンが表示されている
            if (!Input.GetKey(KeyCode.I) && line_OnFadeIn[6])
            {
                // キーを離した時に、フェードアウト
                if (Input.GetKeyUp(KeyCode.K))
                {
                    line_OverTextures[6].fadeOut();
                    line_OnFadeIn[6] = false;
                }
            }
        }


        // Jに対応するラインが奪われてなかったらJキーで判定
        if (linesManager.Get_1pHaveLines() >= 4)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                if (!canNotesJudges[7])
                {
                    SEPlayer.instance.SEOneShot(0);
                }
                line_OverTextures[7].fadeIn();
                line_OnFadeIn[7] = true;
            }
            else if (Input.GetKeyUp(KeyCode.J))
            {
                line_OverTextures[7].fadeOut();
                line_OnFadeIn[7] = false;
            }


            // もし、Jキーが入力されていないかつ、レーンが表示されている
            if (!Input.GetKey(KeyCode.J) && line_OnFadeIn[7])
            {
                // キーを離した時に、フェードアウト
                if (Input.GetKeyUp(KeyCode.U))
                {
                    line_OverTextures[7].fadeOut();
                    line_OnFadeIn[7] = false;
                }
            }
        }
        // Jに対応するラインが奪われていたらUキーで判定
        else
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                if (!canNotesJudges[7])
                {
                    SEPlayer.instance.SEOneShot(0);
                }
                line_OverTextures[7].fadeIn();
                line_OnFadeIn[7] = true;
            }
            else if (Input.GetKeyUp(KeyCode.U))
            {
                line_OverTextures[7].fadeOut();
                line_OnFadeIn[7] = false;
            }

            // もし、Uキーが入力されていないかつ、レーンが表示されている
            if (!Input.GetKey(KeyCode.U) && line_OnFadeIn[7])
            {
                // キーを離した時に、フェードアウト
                if (Input.GetKeyUp(KeyCode.J))
                {
                    line_OverTextures[7].fadeOut();
                    line_OnFadeIn[7] = false;
                }
            }
        }

        #endregion





        if (!autoJudge && canInput)
        {
            //LinkNotes-------------------------------------------------------------------------------------------

            #region // 1Pがリンクノーツだった時の入力　(Qキー以外)
            //1Pのリンクノーツ固定レーンの0のノーツがリンクノーツ
            if (nextJudgeNotes[0].type == (int)ConstRepo.NotesType.Link)
            {
                //2Pのリンクノーツ固定レーンの7のノーツがリンクノーツではない
                //か、
                //レーン7がリンクノーツかつ、
                //ノーツ7より、レーン0の到達時間のほうが早ければ
                // レーン7が初期化されていたら許可
                if (nextJudgeNotes[7].type != (int)ConstRepo.NotesType.Link
                    ||
                    (nextJudgeNotes[7].type == (int)ConstRepo.NotesType.Link
                    &&
                    nextJudgeNotes[7].time > nextJudgeNotes[0].time)
                    || nextJudgeNotes[7].time == 0)
                {
                    //1P--------------------------------------------------------
                    // Sに対応するラインが奪われてなかったらSキーで判定
                    if (linesManager.Get_1pHaveLines() <= 0)
                    {
                        if (Input.GetKeyDown(KeyCode.S))
                        {
                            // 前のNotesの入力に干渉しないようにする
                            if (nextJudgeNotes[0].time < nextJudgeNotes[1].time
                                || nextJudgeNotes[1].time == 0)
                            {
                                // 引数のラインのノーツチェック
                                NotesTypeCheckAnd_Judge(0, (int)ConstRepo.Player.P1, false);
                            }
                        }
                    }
                    // Sに対応するラインが奪われていたらWキーで判定
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.W))
                        {
                            // 前のNotesの入力に干渉しないようにする
                            if (nextJudgeNotes[0].time <= nextJudgeNotes[1].time
                                || nextJudgeNotes[1].time == 0)
                            {
                                // 引数のラインのノーツチェック
                                NotesTypeCheckAnd_Judge(0, (int)ConstRepo.Player.P1, false);
                            }
                        }
                    }

                    // Dに対応するラインが奪われてなかったらDキーで判定
                    if (linesManager.Get_1pHaveLines() <= 1)
                    {
                        if (Input.GetKeyDown(KeyCode.D))
                        {
                            // 前のNotesの入力に干渉しないようにする
                            if (nextJudgeNotes[0].time <= nextJudgeNotes[2].time
                                || nextJudgeNotes[2].time == 0)
                            {
                                // 引数のラインのノーツチェック
                                NotesTypeCheckAnd_Judge(0, (int)ConstRepo.Player.P1, false);
                            }
                        }
                    }
                    // Dに対応するラインが奪われていたらEキーで判定
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            // 前のNotesの入力に干渉しないようにする
                            if (nextJudgeNotes[0].time <= nextJudgeNotes[2].time
                                || nextJudgeNotes[2].time == 0)
                            {
                                // 引数のラインのノーツチェック
                                NotesTypeCheckAnd_Judge(0, (int)ConstRepo.Player.P1, false);
                            }
                        }
                    }

                    // Fに対応するラインが奪われてなかったらFキーで判定
                    if (linesManager.Get_1pHaveLines() <= 2)
                    {
                        if (Input.GetKeyDown(KeyCode.F))
                        {
                            // 前のNotesの入力に干渉しないようにする
                            if (nextJudgeNotes[0].time <= nextJudgeNotes[3].time
                                || nextJudgeNotes[3].time == 0)
                            {
                                // 引数のラインのノーツチェック
                                NotesTypeCheckAnd_Judge(0, (int)ConstRepo.Player.P1, false);
                            }
                        }
                    }
                    // Fに対応するラインが奪われていたらRキーで判定
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.R))
                        {
                            // 前のNotesの入力に干渉しないようにする
                            if (nextJudgeNotes[0].time <= nextJudgeNotes[3].time
                                || nextJudgeNotes[3].time == 0)
                            {
                                // 引数のラインのノーツチェック
                                NotesTypeCheckAnd_Judge(0, (int)ConstRepo.Player.P1, false);
                            }
                        }
                    }



                    //2P--------------------------------------------------------

                    // Pキーを押したとき
                    if (Input.GetKeyDown(KeyCode.P))
                    {
                        // 前のNotesの入力に干渉しないようにする
                        if (nextJudgeNotes[0].time <= nextJudgeNotes[7].time
                                || nextJudgeNotes[7].time == 0)
                        {
                            // 引数のラインのノーツチェック
                            NotesTypeCheckAnd_Judge(0, (int)ConstRepo.Player.P2, false);
                        }
                    }

                    // Lに対応するラインが奪われてなかったらLキーで判定
                    if (linesManager.Get_1pHaveLines() >= 6)
                    {
                        if (Input.GetKeyDown(KeyCode.L))
                        {
                            // 前のNotesの入力に干渉しないようにする
                            if (nextJudgeNotes[0].time <= nextJudgeNotes[6].time
                                || nextJudgeNotes[6].time == 0)
                            {
                                // 引数のラインのノーツチェック
                                NotesTypeCheckAnd_Judge(0, (int)ConstRepo.Player.P2, false);
                            }
                        }
                    }
                    // Lに対応するラインが奪われていたらOキーで判定
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.O))
                        {
                            // 前のNotesの入力に干渉しないようにする
                            if (nextJudgeNotes[0].time <= nextJudgeNotes[6].time
                                || nextJudgeNotes[6].time == 0)
                            {
                                // 引数のラインのノーツチェック
                                NotesTypeCheckAnd_Judge(0, (int)ConstRepo.Player.P2, false);
                            }
                        }
                    }

                    // Kに対応するラインが奪われてなかったらKキーで判定
                    if (linesManager.Get_1pHaveLines() >= 5)
                    {
                        if (Input.GetKeyDown(KeyCode.K))
                        {
                            // 前のNotesの入力に干渉しないようにする
                            if (nextJudgeNotes[0].time <= nextJudgeNotes[5].time
                                || nextJudgeNotes[5].time == 0)
                            {
                                // 引数のラインのノーツチェック
                                NotesTypeCheckAnd_Judge(0, (int)ConstRepo.Player.P2, false);
                            }
                        }
                    }
                    // Kに対応するラインが奪われていたらIキーで判定
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.I))
                        {
                            // 前のNotesの入力に干渉しないようにする
                            if (nextJudgeNotes[0].time <= nextJudgeNotes[5].time
                                || nextJudgeNotes[5].time == 0)
                            {
                                // 引数のラインのノーツチェック
                                NotesTypeCheckAnd_Judge(0, (int)ConstRepo.Player.P2, false);
                            }
                        }
                    }

                    // Jに対応するラインが奪われてなかったらJキーで判定
                    if (linesManager.Get_1pHaveLines() >= 4)
                    {
                        if (Input.GetKeyDown(KeyCode.J))
                        {
                            // 前のNotesの入力に干渉しないようにする
                            if (nextJudgeNotes[0].time <= nextJudgeNotes[4].time
                                || nextJudgeNotes[4].time == 0)
                            {
                                // 引数のラインのノーツチェック
                                NotesTypeCheckAnd_Judge(0, (int)ConstRepo.Player.P2, false);
                            }

                        }
                    }
                    // Jに対応するラインが奪われていたらUキーで判定
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.U))
                        {
                            // 前のNotesの入力に干渉しないようにする
                            if (nextJudgeNotes[0].time <= nextJudgeNotes[4].time
                                || nextJudgeNotes[4].time == 0)
                            {
                                // 引数のラインのノーツチェック
                                NotesTypeCheckAnd_Judge(0, (int)ConstRepo.Player.P2, false);
                            }
                        }
                    }
                }
            }

            #endregion Singleton   
            //1Pがリンクノーツだった時の入力終わり

            #region // 2Pがリンクノーツだった時の入力　(Pキー以外)
            //2Pのリンクノーツ固定レーンの7のノーツがリンクノーツ
            if (nextJudgeNotes[7].type == (int)ConstRepo.NotesType.Link)
            {
                //1Pのリンクノーツ固定レーンの0のノーツがリンクノーツではない
                //か、
                //レーン0がリンクノーツかつ、
                //ノーツ0より、レーン7の到達時間のほうが早ければ
                // レーン0が初期化されていたら許可
                if (nextJudgeNotes[0].type != (int)ConstRepo.NotesType.Link
                    ||
                    (nextJudgeNotes[0].type == (int)ConstRepo.NotesType.Link
                    &&
                    nextJudgeNotes[0].time > nextJudgeNotes[7].time)
                    || nextJudgeNotes[0].time == 0)
                {
                    //1P--------------------------------------------------------

                    // Qキーを押したとき
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        // 前のNotesの入力に干渉しないようにする
                        if (nextJudgeNotes[7].time <= nextJudgeNotes[0].time)
                        {
                            // 引数のラインのノーツチェック
                            NotesTypeCheckAnd_Judge(7, (int)ConstRepo.Player.P1, false);
                        }
                    }

                    // Sに対応するラインが奪われてなかったらSキーで判定
                    if (linesManager.Get_1pHaveLines() <= 0)
                    {
                        if (Input.GetKeyDown(KeyCode.S))
                        {
                            // 前のNotesの入力に干渉しないようにする
                            // 初期化されていたら、OK
                            if (nextJudgeNotes[7].time < nextJudgeNotes[1].time
                                || nextJudgeNotes[1].time == 0)
                            {
                                // 引数のラインのノーツチェック
                                NotesTypeCheckAnd_Judge(7, (int)ConstRepo.Player.P1, false);
                            }
                        }
                    }
                    // Sに対応するラインが奪われていたらWキーで判定
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.W))
                        {
                            // 前のNotesの入力に干渉しないようにする
                            // 初期化されていたら、OK
                            if (nextJudgeNotes[7].time <= nextJudgeNotes[1].time
                                || nextJudgeNotes[1].time == 0)
                            {
                                // 引数のラインのノーツチェック
                                NotesTypeCheckAnd_Judge(7, (int)ConstRepo.Player.P1, false);
                            }
                        }
                    }

                    // Dに対応するラインが奪われてなかったらDキーで判定
                    if (linesManager.Get_1pHaveLines() <= 1)
                    {
                        if (Input.GetKeyDown(KeyCode.D))
                        {
                            // 前のNotesの入力に干渉しないようにする
                            // 初期化されていたら、OK
                            if (nextJudgeNotes[7].time <= nextJudgeNotes[2].time
                                || nextJudgeNotes[2].time == 0)
                            {
                                // 引数のラインのノーツチェック
                                NotesTypeCheckAnd_Judge(7, (int)ConstRepo.Player.P1, false);
                            }
                        }
                    }
                    // Dに対応するラインが奪われていたらEキーで判定
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            // 前のNotesの入力に干渉しないようにする
                            // 初期化されていたら、OK
                            if (nextJudgeNotes[7].time <= nextJudgeNotes[2].time
                                || nextJudgeNotes[2].time == 0)
                            {
                                // 引数のラインのノーツチェック
                                NotesTypeCheckAnd_Judge(7, (int)ConstRepo.Player.P1, false);
                            }
                        }
                    }

                    // Fに対応するラインが奪われてなかったらFキーで判定
                    if (linesManager.Get_1pHaveLines() <= 2)
                    {
                        if (Input.GetKeyDown(KeyCode.F))
                        {
                            // 前のNotesの入力に干渉しないようにする
                            // 初期化されていたら、OK
                            if (nextJudgeNotes[7].time <= nextJudgeNotes[3].time
                                || nextJudgeNotes[3].time == 0)
                            {
                                // 引数のラインのノーツチェック
                                NotesTypeCheckAnd_Judge(7, (int)ConstRepo.Player.P1, false);
                            }
                        }
                    }
                    // Fに対応するラインが奪われていたらRキーで判定
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.R))
                        {
                            // 前のNotesの入力に干渉しないようにする
                            // 初期化されていたら、OK
                            if (nextJudgeNotes[7].time <= nextJudgeNotes[3].time
                                || nextJudgeNotes[3].time == 0)
                            {
                                // 引数のラインのノーツチェック
                                NotesTypeCheckAnd_Judge(7, (int)ConstRepo.Player.P1, false);
                            }
                        }
                    }



                    //2P--------------------------------------------------------


                    // Lに対応するラインが奪われてなかったらLキーで判定
                    if (linesManager.Get_1pHaveLines() >= 6)
                    {
                        if (Input.GetKeyDown(KeyCode.L))
                        {
                            // 前のNotesの入力に干渉しないようにする
                            // 初期化されていたら、OK
                            if (nextJudgeNotes[7].time <= nextJudgeNotes[6].time
                                || nextJudgeNotes[6].time == 0)
                            {
                                // 引数のラインのノーツチェック
                                NotesTypeCheckAnd_Judge(7, (int)ConstRepo.Player.P2, false);
                            }
                        }
                    }
                    // Lに対応するラインが奪われていたらOキーで判定
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.O))
                        {
                            // 前のNotesの入力に干渉しないようにする
                            // 初期化されていたら、OK
                            if (nextJudgeNotes[7].time <= nextJudgeNotes[6].time
                                || nextJudgeNotes[6].time == 0)
                            {
                                // 引数のラインのノーツチェック
                                NotesTypeCheckAnd_Judge(7, (int)ConstRepo.Player.P2, false);
                            }
                        }
                    }

                    // Kに対応するラインが奪われてなかったらKキーで判定
                    if (linesManager.Get_1pHaveLines() >= 5)
                    {
                        if (Input.GetKeyDown(KeyCode.K))
                        {
                            // 前のNotesの入力に干渉しないようにする
                            // 初期化されていたら、OK
                            if (nextJudgeNotes[7].time <= nextJudgeNotes[5].time
                                || nextJudgeNotes[5].time == 0)
                            {
                                // 引数のラインのノーツチェック
                                NotesTypeCheckAnd_Judge(7, (int)ConstRepo.Player.P2, false);
                            }
                        }
                    }
                    // Kに対応するラインが奪われていたらIキーで判定
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.I))
                        {
                            // 前のNotesの入力に干渉しないようにする
                            // 初期化されていたら、OK
                            if (nextJudgeNotes[7].time <= nextJudgeNotes[5].time
                                || nextJudgeNotes[5].time == 0)
                            {
                                // 引数のラインのノーツチェック
                                NotesTypeCheckAnd_Judge(7, (int)ConstRepo.Player.P2, false);
                            }
                        }
                    }

                    // Jに対応するラインが奪われてなかったらJキーで判定
                    if (linesManager.Get_1pHaveLines() >= 4)
                    {
                        if (Input.GetKeyDown(KeyCode.J))
                        {
                            // 前のNotesの入力に干渉しないようにする
                            // 初期化されていたら、OK
                            if (nextJudgeNotes[7].time <= nextJudgeNotes[4].time
                                || nextJudgeNotes[4].time == 0)
                            {
                                // 引数のラインのノーツチェック
                                NotesTypeCheckAnd_Judge(7, (int)ConstRepo.Player.P2, false);
                            }

                        }
                    }
                    // Jに対応するラインが奪われていたらUキーで判定
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.U))
                        {
                            // 前のNotesの入力に干渉しないようにする
                            // 初期化されていたら、OK
                            if (nextJudgeNotes[7].time <= nextJudgeNotes[4].time
                                || nextJudgeNotes[4].time == 0)
                            {
                                // 引数のラインのノーツチェック
                                NotesTypeCheckAnd_Judge(7, (int)ConstRepo.Player.P2, false);
                            }
                        }
                    }
                }
            }
            #endregion
            //2Pがリンクノーツだった時の入力終わり

            //LinkNotes終わり-------------------------------------------------------------------------------------------





            //simpleNotes-------------------------------------------------------------------------------------------

            #region // 1p 入力に対応してノーツを判定する----------------------------------------------------------------------------------
            // 配列の総数を超えて配列参照しないようにする制御
            if (canNotesJudges[0])
            {
                // Qキーを押したとき
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    // 引数のラインのノーツチェック
                    NotesTypeCheckAnd_Judge(0, 0, true);
                }
                // poor判定の自動化
                NotesTypeCheckAnd_JudgePoor(0, 0);

            }

            // 上のラインが違う番-------------------------------------------------------------------------
            if (canNotesJudges[1])
            {
                // Sに対応するラインが奪われてなかったらSキーで判定
                if (linesManager.Get_1pHaveLines() <= 0)
                {
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        // 引数のラインのノーツチェック
                        NotesTypeCheckAnd_Judge(1, 0, false);
                    }
                }
                // Sに対応するラインが奪われていたらWキーで判定
                else
                {
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        // 引数のラインのノーツチェック
                        NotesTypeCheckAnd_Judge(1, 0, true);
                    }
                }

                // poor判定の自動化
                NotesTypeCheckAnd_JudgePoor(1, 0);
            }

            // 上のラインが違う番--------------------------------------------------------------------------
            if (canNotesJudges[2])
            {
                // Dに対応するラインが奪われてなかったらDキーで判定
                if (linesManager.Get_1pHaveLines() <= 1)
                {
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        // 引数のラインのノーツチェック
                        NotesTypeCheckAnd_Judge(2, 0, false);
                    }
                }
                // Dに対応するラインが奪われていたらEキーで判定
                else
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {

                        // 引数のラインのノーツチェック
                        NotesTypeCheckAnd_Judge(2, 0, true);
                    }
                }
                // poor判定の自動化
                NotesTypeCheckAnd_JudgePoor(2, 0);
            }

            // 上のラインが違う番-------------------------------------------------------------------
            if (canNotesJudges[3])
            {
                // Fに対応するラインが奪われてなかったらFキーで判定
                if (linesManager.Get_1pHaveLines() <= 2)
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        // 引数のラインのノーツチェック
                        NotesTypeCheckAnd_Judge(3, 0, false);

                    }
                }
                // Fに対応するラインが奪われていたらRキーで判定
                else
                {
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        // 引数のラインのノーツチェック
                        NotesTypeCheckAnd_Judge(3, 0, true);
                    }
                }


                // poor判定の自動化
                NotesTypeCheckAnd_JudgePoor(3, 0);
            }
            #endregion
            //入力終わり

            #region // 2p 入力に対応してノーツを判定する----------------------------------------------------------------------------------
            // 配列の総数を超えて配列参照しないようにする制御
            if (canNotesJudges[7])
            {
                // Pキーを押したとき
                if (Input.GetKeyDown(KeyCode.P))
                {
                    // 引数のラインのノーツチェック
                    NotesTypeCheckAnd_Judge(7, 1, true);
                }
                // poor判定の自動化
                NotesTypeCheckAnd_JudgePoor(7, 1);
            }

            // 上のラインが違う番-------------------------------------------------------------------------
            if (canNotesJudges[6])
            {
                // Lに対応するラインが奪われてなかったらLキーで判定
                if (linesManager.Get_1pHaveLines() >= 6)
                {
                    if (Input.GetKeyDown(KeyCode.L))
                    {
                        // 引数のラインのノーツチェック
                        NotesTypeCheckAnd_Judge(6, 1, false);
                    }
                }
                // Lに対応するラインが奪われていたらOキーで判定
                else
                {
                    if (Input.GetKeyDown(KeyCode.O))
                    {
                        // 引数のラインのノーツチェック
                        NotesTypeCheckAnd_Judge(6, 1, true);
                    }
                }

                // poor判定の自動化
                NotesTypeCheckAnd_JudgePoor(6, 1);
            }

            // 上のラインが違う番--------------------------------------------------------------------------
            if (canNotesJudges[5])
            {

                // Kに対応するラインが奪われてなかったらKキーで判定
                if (linesManager.Get_1pHaveLines() >= 5)
                {
                    if (Input.GetKeyDown(KeyCode.K))
                    {
                        // 引数のラインのノーツチェック
                        NotesTypeCheckAnd_Judge(5, 1, false);
                    }
                }
                // Kに対応するラインが奪われていたらIキーで判定
                else
                {
                    if (Input.GetKeyDown(KeyCode.I))
                    {

                        // 引数のラインのノーツチェック
                        NotesTypeCheckAnd_Judge(5, 1, true);
                    }
                }
                // poor判定の自動化
                NotesTypeCheckAnd_JudgePoor(5, 1);
            }

            // 上のラインが違う番-------------------------------------------------------------------
            if (notesDataList.listNumMax[4] > lineNotesNum[4])
            {
                // Jに対応するラインが奪われてなかったらJキーで判定
                if (linesManager.Get_1pHaveLines() >= 4)
                {
                    if (Input.GetKeyDown(KeyCode.J))
                    {
                        // 引数のラインのノーツチェック
                        NotesTypeCheckAnd_Judge(4, 1, false);

                    }
                }
                // Jに対応するラインが奪われていたらUキーで判定
                else
                {
                    if (Input.GetKeyDown(KeyCode.U))
                    {
                        // 引数のラインのノーツチェック
                        NotesTypeCheckAnd_Judge(4, 1, true);
                    }
                }


                // poor判定の自動化
                NotesTypeCheckAnd_JudgePoor(4, 1);
            }
            #endregion
            //入力終わり

            //simpleNotes終わり-------------------------------------------------------------------------------------





            //HoldNotesのEnd判定用--------------------------------------------------------------------------------
            #region // 1PのHoldNotesに対応したKeyUp

            // EndTimeの判定をしてもよいかの許可用
            if (canHoldEndJudges[0])
            {
                if (Input.GetKeyUp(KeyCode.Q))
                {
                    HoldNotes_EndJudge(0, (int)ConstRepo.Player.P1);
                }

                //　end部分のpoor自動化
                HoldNotes_EndJudgePoor(0, (int)ConstRepo.Player.P1);
            }

            // EndTimeの判定をしてもよいかの許可用
            if (canHoldEndJudges[1])
            {
                // 上段キーか、下段のキーを使用するかの制御
                if (isHoldEndJudge_KeyUp[1])
                {
                    if (Input.GetKeyUp(KeyCode.W))
                    {
                        HoldNotes_EndJudge(1, (int)ConstRepo.Player.P1);
                    }
                }
                else
                {
                    if (Input.GetKeyUp(KeyCode.S))
                    {
                        HoldNotes_EndJudge(1, (int)ConstRepo.Player.P1);
                    }
                }

                //　end部分のpoor自動化
                HoldNotes_EndJudgePoor(1, (int)ConstRepo.Player.P1);
            }

            // EndTimeの判定をしてもよいかの許可用
            if (canHoldEndJudges[2])
            {
                // 上段キーか、下段のキーを使用するかの制御
                if (isHoldEndJudge_KeyUp[2])
                {
                    if (Input.GetKeyUp(KeyCode.E))
                    {
                        HoldNotes_EndJudge(2, (int)ConstRepo.Player.P1);
                    }
                }
                else
                {
                    if (Input.GetKeyUp(KeyCode.D))
                    {
                        HoldNotes_EndJudge(2, (int)ConstRepo.Player.P1);
                    }
                }

                //　end部分のpoor自動化
                HoldNotes_EndJudgePoor(2, (int)ConstRepo.Player.P1);
            }

            // EndTimeの判定をしてもよいかの許可用
            if (canHoldEndJudges[3])
            {
                // 上段キーか、下段のキーを使用するかの制御
                if (isHoldEndJudge_KeyUp[3])
                {
                    if (Input.GetKeyUp(KeyCode.R))
                    {
                        HoldNotes_EndJudge(3, (int)ConstRepo.Player.P1);
                    }
                }
                else
                {
                    if (Input.GetKeyUp(KeyCode.F))
                    {
                        HoldNotes_EndJudge(3, (int)ConstRepo.Player.P1);
                    }
                }

                //　end部分のpoor自動化
                HoldNotes_EndJudgePoor(3, (int)ConstRepo.Player.P1);
            }
            #endregion
            //入力終わり

            #region // 2PのHoldNotesに対応したKeyUp

            // EndTimeの判定をしてもよいかの許可用
            if (canHoldEndJudges[7])
            {
                if (Input.GetKeyUp(KeyCode.P))
                {
                    HoldNotes_EndJudge(7, (int)ConstRepo.Player.P2);
                }

                //　end部分のpoor自動化
                HoldNotes_EndJudgePoor(7, (int)ConstRepo.Player.P2);
            }

            // EndTimeの判定をしてもよいかの許可用
            if (canHoldEndJudges[6])
            {
                // 上段キーか、下段のキーを使用するかの制御
                if (isHoldEndJudge_KeyUp[6])
                {
                    if (Input.GetKeyUp(KeyCode.O))
                    {
                        HoldNotes_EndJudge(6, (int)ConstRepo.Player.P2);
                    }
                }
                else
                {
                    if (Input.GetKeyUp(KeyCode.L))
                    {
                        HoldNotes_EndJudge(6, (int)ConstRepo.Player.P2);
                    }
                }

                //　end部分のpoor自動化
                HoldNotes_EndJudgePoor(6, (int)ConstRepo.Player.P2);
            }

            // EndTimeの判定をしてもよいかの許可用
            if (canHoldEndJudges[5])
            {
                // 上段キーか、下段のキーを使用するかの制御
                if (isHoldEndJudge_KeyUp[5])
                {
                    if (Input.GetKeyUp(KeyCode.I))
                    {
                        HoldNotes_EndJudge(5, (int)ConstRepo.Player.P2);
                    }
                }
                else
                {
                    if (Input.GetKeyUp(KeyCode.K))
                    {
                        HoldNotes_EndJudge(5, (int)ConstRepo.Player.P2);
                    }
                }

                //　end部分のpoor自動化
                HoldNotes_EndJudgePoor(5, (int)ConstRepo.Player.P2);
            }

            // EndTimeの判定をしてもよいかの許可用
            if (canHoldEndJudges[4])
            {
                // 上段キーか、下段のキーを使用するかの制御
                if (isHoldEndJudge_KeyUp[4])
                {
                    if (Input.GetKeyUp(KeyCode.U))
                    {
                        HoldNotes_EndJudge(4, (int)ConstRepo.Player.P2);
                    }
                }
                else
                {
                    if (Input.GetKeyUp(KeyCode.J))
                    {
                        HoldNotes_EndJudge(4, (int)ConstRepo.Player.P2);
                    }
                }

                //　end部分のpoor自動化
                HoldNotes_EndJudgePoor(4, (int)ConstRepo.Player.P2);
            }

            #endregion
            //入力終わり

            //HoldNotesのEnd判定終わり-----------------------------------------------------------------------------------

        }
        // Notes判定Auto機能(入力を許可されていたら)
        else if (canInput)
        { 

            // 1p用オート
            for(int i = 0; i < 4; i++ )
            {
                // 譜面終了後判定検索しないように制御
                if (nextJudgeNotes[i].time == 0)
                {

                }
                else if (nextJudgeNotes[i].time - MusicData.Timer < 30)
                {
                    if (!canHoldEndJudges[i])
                    {
                        NotesTypeCheckAnd_Judge(i, (int)ConstRepo.Player.P1, true);
                    }
                }
            }
            // 2p用オート
            for (int i = 4; i < 8; i++)
            {
                // 譜面終了後判定検索しないように制御
                if (nextJudgeNotes[i].time == 0)
                {
                    
                }
                else if (nextJudgeNotes[i].time - MusicData.Timer < 30)
                {
                    if (!canHoldEndJudges[i])
                    {
                        NotesTypeCheckAnd_Judge(i, (int)ConstRepo.Player.P2, true);
                    }
                }
            }

            // 1p用ホールドの最後の判定オート
            for (int i = 0; i < 4; i++)
            {
                if (nextJudgeNotes[i].type == (int)ConstRepo.NotesType.Hold)
                {
                    // 譜面終了後判定検索しないように制御
                    if (nextJudgeNotes[i].endtime == 0)
                    {

                    }
                    else if (nextJudgeNotes[i].endtime - MusicData.Timer < 30)
                    {
                        HoldNotes_EndJudge(i, (int)ConstRepo.Player.P1);
                    }
                }
            }
            // 2p用ホールドの最後の判定オート
            for (int i = 4; i < 8; i++)
            {

                if (nextJudgeNotes[i].type == (int)ConstRepo.NotesType.Hold)
                {
                    // 譜面終了後判定検索しないように制御
                    if (nextJudgeNotes[i].endtime == 0)
                    {

                    }
                    else if (nextJudgeNotes[i].endtime - MusicData.Timer < 30)
                    {
                        HoldNotes_EndJudge(i, (int)ConstRepo.Player.P2);
                    }
                }
            }
        }





        //スキル発動系----------------------------------------------------------------------------------
        #region // 1Pスキルの発動処理---------------------------------------------------------------------------
        if (skillChargeRate[0].skillValue >= 100 && skillManager.IsCanUseSkill(0)
            && musicPlayer.notMusicEnd)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                StartCoroutine(L_SkillUseInput_Key_D());
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                StartCoroutine(R_SkillUseInput_Key_D());
            }
        }
        #endregion

        #region // 2Pスキルの発動処理---------------------------------------------------------------------------
        if (skillChargeRate[1].skillValue >= 100 && skillManager.IsCanUseSkill(1)
            && musicPlayer.notMusicEnd)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                StartCoroutine(L_SkillUseInput_Key_K());
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                StartCoroutine(R_SkillUseInput_Key_K());
            }
        }
        #endregion
        //スキル発動系終わり----------------------------------------------------------------------------
    }



    // NotesJudge--------------------------------------------------------------------------------------------

    /// <summary>
    /// 判定可能なら、ノーツタイプに応じて、ジャッジ処理をスイッチする関数
    /// </summary>
    /// <param name="line"></param>
    /// <param name="usePlayer"></param>
    void NotesTypeCheckAnd_Judge(int line, int usePlayer, bool isKeyUp)
    {
        // criwearの時間 - 判定バー到達予定時間 < judgeTimeなら実行
        if (nextJudgeNotes[line].time - MusicData.Timer < judgeTime 
            && nextJudgeNotes[line].time != 0)
        {
            //　場にリンクノーツがなかったら、通常判定
            if (nextJudgeNotes[0].type != (int)ConstRepo.NotesType.Link
                && nextJudgeNotes[7].type != (int)ConstRepo.NotesType.Link)

            {
                // Notesのタイプで処理をスイッチ
                switch (nextJudgeNotes[line].type)
                {
                    // リンクノーツの判定処理
                    case 3:

                        if (canLinkNotesInput[usePlayer])
                        {

                            canLinkNotesInput[usePlayer] = false;
                            LinkNotes_Judge(line, usePlayer);
                        }

                        break;
                    // ホールドノーツの判定処理
                    case 2:
                        isHoldEndJudge_KeyUp[line] = isKeyUp;
                        HoldNotes_StartJudge(line, usePlayer);

                        break;
                    // その他（ノーマルノーツ、スライドノーツ）の判定処理
                    default:
                        SimpleNotesJudge(line, usePlayer);
                        break;
                }
            }
        }


        // リンクノーツ入力時に他のノーツと同時に判定されないようにする
        if (nextJudgeNotes[0].type == (int)ConstRepo.NotesType.Link)
        {
            if (nextJudgeNotes[line].time - MusicData.Timer < judgeTime
                && nextJudgeNotes[line].time != 0)
            {

                // Notesのタイプで処理をスイッチ
                switch (nextJudgeNotes[line].type)
                {
                    // リンクノーツの判定処理
                    case 3:
                        // リンクノーツ入力時に同じレーンのノーツより後ろなら反応しないようにする
                        if (nextJudgeNotes[0].time <= nextJudgeNotes[line].time)
                        {
                            if (canLinkNotesInput[usePlayer])
                            {

                                canLinkNotesInput[usePlayer] = false;
                                Debug.Log("type" + nextJudgeNotes[0].type + "time" + nextJudgeNotes[0].time + "type" + nextJudgeNotes[0].type + "time" + nextJudgeNotes[line].time);
                                LinkNotes_Judge(line, usePlayer);
                            }
                        }
                        break;

                    // ホールドノーツの判定処理
                    case 2:
                        // リンクノーツ入力時に自分が後ろなら反応しないようにする
                        if (nextJudgeNotes[0].time > nextJudgeNotes[line].time)
                        {
                            isHoldEndJudge_KeyUp[line] = isKeyUp;
                            HoldNotes_StartJudge(line, usePlayer);
                        }
                        break;

                    // その他（ノーマルノーツ、スライドノーツ）の判定処理
                    default:
                        // リンクノーツ入力時に自分が後ろなら反応しないようにする
                        if (nextJudgeNotes[0].time > nextJudgeNotes[line].time)
                        {
                            Debug.Log("Simple" + nextJudgeNotes[0].time + nextJudgeNotes[line].time);
                            SimpleNotesJudge(line, usePlayer);
                        }
                        break;
                }
            }
        }



        // リンクノーツ入力時に他のノーツと同時に判定されないようにする
        if (nextJudgeNotes[7].type == (int)ConstRepo.NotesType.Link)
        {
            if (nextJudgeNotes[line].time - MusicData.Timer < judgeTime
                && nextJudgeNotes[line].time != 0)
            {
                // Notesのタイプで処理をスイッチ
                switch (nextJudgeNotes[line].type)
                {
                    // リンクノーツの判定処理
                    case 3:
                        // リンクノーツ入力時に同じレーンのノーツより後ろなら反応しないようにする
                        if (nextJudgeNotes[7].time <= nextJudgeNotes[line].time)
                        {
                            if (canLinkNotesInput[usePlayer])
                            {

                                canLinkNotesInput[usePlayer] = false;
                                LinkNotes_Judge(line, usePlayer);
                            }
                        }
                        break;

                    // ホールドノーツの判定処理
                    case 2:
                        // リンクノーツ入力時に自分が後ろなら反応しないようにする
                        if (nextJudgeNotes[7].time > nextJudgeNotes[line].time)
                        {
                            isHoldEndJudge_KeyUp[line] = isKeyUp;
                            HoldNotes_StartJudge(line, usePlayer);
                        }
                        break;

                    // その他（ノーマルノーツ、スライドノーツ）の判定処理
                    default:
                        // リンクノーツ入力時に自分が後ろなら反応しないようにする
                        if (nextJudgeNotes[7].time > nextJudgeNotes[line].time)
                        {
                            SimpleNotesJudge(line, usePlayer);
                        }
                        break;
                }
            }


        }

        //それ以外なら、空打ち判定として、空打ちSEのみ
        else
        {
            // 空打ちSE
            SEPlayer.instance.SEOneShot(0);
        }

    }


    /// <summary>
    /// ノーマルノーツ、スライドノーツの判定
    /// </summary>
    /// <param name="line">どのラインのノーツを判定するか指定用</param>
    void SimpleNotesJudge(int line, int usePlayer)
    {
        // タンバリン
        SEPlayer.instance.SEOneShot(1);

        // ノーツがポイズンノーツかそうじゃないかで、判定を変える
        if (nextJudgeNotes[line].isPoison)
        {
            // 判定用関数で、ポイズンノーツの判定をする
            notesJudge.PoisonNotesJudgement(
                // criwearの時間 - 判定バー到達予定時間 を絶対値化して引数に設定
                (long)Mathf.Abs(nextJudgeNotes[line].time - MusicData.Timer), line, usePlayer);
        }
        else
        {
            // 判定用関数で、ノーマルノーツの判定をする
            notesJudge.SimpleNotesJudgement(
                // criwearの時間 - 判定バー到達予定時間 を絶対値化して引数に設定
                (long)Mathf.Abs(nextJudgeNotes[line].time - MusicData.Timer), line, usePlayer);
        }


        //干渉するノーツを次にする
        lineNotesNum[line]++;
        //次のノーツデータを格納
        SetNextNotesData(line);
    }


    /// <summary>
    /// ホールドノーツの最初の判定
    /// </summary>
    void HoldNotes_StartJudge(int line, int usePlayer)
    {
        Debug.Log("手動Start");



        // 判定用関数で、ホールドノーツの判定し、戻り値でpoorならfalse　good以上ならtrue
        canHoldEndJudges[line] =
            notesJudge.HoldNotes_StartJudgement(
                // criwearの時間 - 判定バー到達予定時間 を絶対値化して引数に設定
                (long)Mathf.Abs(nextJudgeNotes[line].time - MusicData.Timer), line, usePlayer);


        // 次の判定の許可が出なかったら、次にする
        if (!canHoldEndJudges[line])
        {
            // 入力失敗SE
            SEPlayer.instance.SEOneShot(0);

            //干渉するノーツを次にする
            lineNotesNum[line]++;
            //次のノーツデータを格納
            SetNextNotesData(line);
        }
        else
        {
            // ホールドノーツSE再生（ループ）
            holdNotesSEPlayers[line].holdNotesSEPlayer.HoldNotesSEPlay();
        }
    }


    /// <summary>
    /// ホールドノーツの最後の判定
    /// </summary>
    void HoldNotes_EndJudge(int line, int usePlayer)
    {
        Debug.Log("手動END");
        //判定許可を閉じる


        // ホールドノーツSE再生（ループ）を止める
        holdNotesSEPlayers[line].holdNotesSEPlayer.HoldNotesSEStop();


        // 判定用関数で、ホールドノーツの最終判定をする
        notesJudge.HoldNotes_EndJudgement(
                // criwearの時間 - 判定バー到達予定時間 を絶対値化して引数に設定
                (long)Mathf.Abs(nextJudgeNotes[line].endtime - MusicData.Timer), line, usePlayer);


        //干渉するノーツを次にする
        lineNotesNum[line]++;
        //次のノーツデータを格納
        SetNextNotesData(line);


        canHoldEndJudges[line] = false;
    }


    /// <summary>
    /// リンクノーツの判定(最大で計2回判定される)
    /// </summary>
    /// <param name="line"></param>
    /// <param name="usePlayer"></param>
    void LinkNotes_Judge(int line, int usePlayer)
    {

        //　流れているLinkNotesの初めての判定なら、First
        if (isLinkNotes_FirstJudge)
        {
            Debug.Log("LinkNotes_FirstJudge");

            //　戻り値は、固定でfalse;
            isLinkNotes_FirstJudge = notesJudge.LinkNotes_FirstJudgement(
                // criwearの時間 - 判定バー到達予定時間 を絶対値化して引数に設定
                (long)Mathf.Abs(nextJudgeNotes[line].time - MusicData.Timer), usePlayer);

            // オートなら最後の判定を実行
            if (autoJudge)
            {
                if(usePlayer == (int)ConstRepo.Player.P1)
                {
                    LinkNotes_Judge(0, (int)ConstRepo.Player.P2);
                }
                else
                {
                    LinkNotes_Judge(7, (int)ConstRepo.Player.P1);
                }
            }

        }
        //　それ以外なら、Last
        else
        {

            // リンクノーツSE
            SEPlayer.instance.SEOneShot(3);

            Debug.Log("LinkNotes_LastJudge");

            notesJudge.LinkNotes_LastJudgement(
                // criwearの時間 - 判定バー到達予定時間 を絶対値化して引数に設定
                (long)Mathf.Abs(nextJudgeNotes[line].time - MusicData.Timer), line, usePlayer);


            //リンクノーツの制御を解除
            canLinkNotesInput[(int)ConstRepo.Player.P1] = true;
            canLinkNotesInput[(int)ConstRepo.Player.P2] = true;

            //値の初期化
            isLinkNotes_FirstJudge = true;

            //干渉するノーツを次にする
            lineNotesNum[line]++;
            //次のノーツデータを格納
            SetNextNotesData(line);
        }
    }







    // PoorJudge----------------------------------------------------------------------------------------------

    /// <summary>
    /// Poor判定の時間になったら、ノーツタイプに応じて、ジャッジ処理をスイッチする関数
    /// </summary>
    /// <param name="line"></param>
    /// <param name="usePlayer"></param>
    void NotesTypeCheckAnd_JudgePoor(int line, int usePlayer)
    {
        // 判定バー到達予定時間 - criwearの時間 < poorTimingなら実行
        if (nextJudgeNotes[line].time - MusicData.Timer < (long)ConstRepo.poorTiming
                && nextJudgeNotes[line].time != 0)
        {
            // Notesのタイプで処理をスイッチ
            switch (nextJudgeNotes[line].type)
            {
                // リンクノーツの判定処理
                case 3:
                    LinkNotesJudgePoor(line, usePlayer);

                    break;
                // ホールドノーツの判定処理
                case 2:

                    // 非入力中かを判定
                    if (!canHoldEndJudges[line])
                    {
                        HoldNotes_StartJudgePoor(line, usePlayer);
                    }
                    break;

                // その他（ノーマルノーツ、スライドノーツ）の判定処理
                default:
                    SimpleNotesJudgePoor(line, usePlayer);
                    break;
            }
        }
    }




    /// <summary>
    /// ノーマルノーツ、スライドノーツのpoor判定
    /// </summary>
    /// <param name="line">どのラインのノーツを判定するか指定用</param>
    void SimpleNotesJudgePoor(int line, int usePlayer)
    {
        // ノーツがポイズンノーツかそうじゃないかで、判定を変える
        if (nextJudgeNotes[line].isPoison)
        {
            // 判定用関数で、毒ノーツの判定をする
            notesJudge.PoisonNotesJudgement(
                // criwearの時間 - 判定バー到達予定時間 を絶対値化して引数に設定
                (long)Mathf.Abs(nextJudgeNotes[line].time - MusicData.Timer), line, usePlayer);
        }
        else
        {
            // 判定用関数で、ノーマルノーツの判定をする
            notesJudge.SimpleNotesJudgement(
                // criwearの時間 - 判定バー到達予定時間 を絶対値化して引数に設定
                (long)Mathf.Abs(nextJudgeNotes[line].time - MusicData.Timer), line, usePlayer);
        }


        //干渉するノーツを次にする
        lineNotesNum[line]++;
        //次のノーツデータを格納
        SetNextNotesData(line);
    }


    /// <summary>
    /// ホールドノーツの最初の判定 Poorなので音無し
    /// </summary>
    void HoldNotes_StartJudgePoor(int line, int usePlayer)
    {

        Debug.Log("自動Start_Poor");

        notesJudge.HoldNotes_StartJudgement(
            // criwearの時間 - 判定バー到達予定時間 を絶対値化して引数に設定
            (long)Mathf.Abs(nextJudgeNotes[line].time - MusicData.Timer), line, usePlayer);


        // 次の判定の許可が出なかったら、次にする
        if (!canHoldEndJudges[line])
        {
            //干渉するノーツを次にする
            lineNotesNum[line]++;
            //次のノーツデータを格納
            SetNextNotesData(line);
        }
    }

    /// <summary>
    /// ホールドノーツの最後の判定　 Poorなので音無し
    /// </summary>
    void HoldNotes_EndJudgePoor(int line, int usePlayer)
    {


        // 判定バー到達予定時間 - criwearの時間 < poorTimingなら実行
        if (nextJudgeNotes[line].endtime
                - MusicData.Timer < (long)ConstRepo.poorTiming
                &&
                nextJudgeNotes[line].endtime != 0)
        {

            Debug.Log("自動End_Poor");
            //判定許可を閉じる
            canHoldEndJudges[line] = false;


            // ホールドノーツSE再生（ループ）を止める
            holdNotesSEPlayers[line].holdNotesSEPlayer.HoldNotesSEStop();

            // 判定用関数で、ホールドノーツの最終判定をする
            notesJudge.HoldNotes_EndJudgement(
                // criwearの時間 - 判定バー到達予定時間 を絶対値化して引数に設定
                (long)Mathf.Abs(nextJudgeNotes[line].endtime - MusicData.Timer), line, usePlayer);


            //干渉するノーツを次にする
            lineNotesNum[line]++;
            //次のノーツデータを格納
            SetNextNotesData(line);


        }
    }


    /// <summary>
    /// リンクノーツのpoor判定
    /// </summary>
    /// <param name="line">どのラインのノーツを判定するか指定用</param>
    void LinkNotesJudgePoor(int line, int usePlayer)
    {
        Debug.Log("LinkNotes_LastJudge(自動)");


        notesJudge.LinkNotes_LastJudgement(
            // criwearの時間 - 判定バー到達予定時間 を絶対値化して引数に設定
            (long)Mathf.Abs(nextJudgeNotes[line].time - MusicData.Timer), line, usePlayer);


        //リンクノーツの制御を解除
        canLinkNotesInput[(int)ConstRepo.Player.P1] = true;
        canLinkNotesInput[(int)ConstRepo.Player.P2] = true;

        //値の初期化
        isLinkNotes_FirstJudge = true;

        //干渉するノーツを次にする
        lineNotesNum[line]++;
        //次のノーツデータを格納
        SetNextNotesData(line);
    }


    /// <summary>
    /// NotesDataを参照する用のオブジェクトの中身を更新
    /// </summary>
    /// <param name="line"></param>
    private void SetNextNotesData(int line)
    {
        // 判定回数をカウント
        jugeCount++;


        if (notesDataList.listNumMax[line] > lineNotesNum[line])
        {
            nextJudgeNotes[line] = notesDataList.notesLists[line].notesList[lineNotesNum[line]];
        }
        else
        {
            nextJudgeNotes[line].type = 0;
            nextJudgeNotes[line].time = 0;
            nextJudgeNotes[line].endtime = 0;
            canNotesJudges[line] = false;
        }
        // 判定回数が、設定した値以上になったら
        if (jugeCount >= myselfAuto_SwitchNum[myselfAutoSwitchCount])
        {
            // カウントのリセット
            jugeCount = 0;

            // オートと、手動の切り替えをカウント
            myselfAutoSwitchCount++;

            // オートだったら手動にする
            if (autoJudge)
            {
                autoJudge = false;
            }
            // オートじゃなかったら、オートにする
            else
            {
                autoJudge = true;
            }
        }
    }















    // Skill周りの処理-------------------------------------------------------------------------------------------


    /// <summary>
    /// スキル発動を不可にする
    /// </summary>
    public void SetFalse_Tutorial_CanSkill()
    {
        tutorial_CanSkill = false;
    }




    /// <summary>
    /// 左からのスキル入力の発動関数
    /// </summary>
    /// <returns></returns>
    IEnumerator L_SkillUseInput_Key_D()
    {
        // 同時押しでの誤作動をしないようにする時間
        yield return new WaitForSeconds(_inputInvalidTime);


        // 値の初期化
        _inputWaitTime_L_1P[0] = 0;


        while (_inputStandbyTime >= _inputWaitTime_L_1P[0])
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.D))
            {
                StartCoroutine(L_SkillUseInput_Key_F());
                _inputWaitTime_L_1P[0] = _inputStandbyTime;
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.F))
            {
                _inputWaitTime_L_1P[0] = _inputStandbyTime;
            }

            _inputWaitTime_L_1P[0] += Time.deltaTime;
        }
    }
    IEnumerator L_SkillUseInput_Key_F()
    {
        // 同時押しでの誤作動をしないようにする時間
        yield return new WaitForSeconds(_inputInvalidTime);

        // 値の初期化
        _inputWaitTime_L_1P[1] = 0;


        while (_inputStandbyTime >= _inputWaitTime_L_1P[1])
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.F) && skillManager.IsCanUseSkill(0))
            {


                // スキルの使用が許可されていたら、スキル発動
                if (tutorial_CanSkill)
                {

                    // スキルSE
                    SEPlayer.instance.SEOneShot(9);

                    // スキルアニメーション
                    charaObj.Chara_Anim_Att((int)ConstRepo.Player.P1);
                    charaObj.Chara_Anim_Hit((int)ConstRepo.Player.P2);

                    // スキルの使用
                    skillChargeRate[0].UseSkill();
                    skillManager.UseSkill_Logic((int)ConstRepo.Player.P1);
                }

                _inputWaitTime_L_1P[1] = _inputStandbyTime;
            }
            else if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.E)
                || Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                _inputWaitTime_L_1P[1] = _inputStandbyTime;
            }
            _inputWaitTime_L_1P[1] += Time.deltaTime;
        }
    }


    /// <summary>
    /// 右からのスキル入力の発動関数
    /// </summary>
    /// <returns></returns>
    IEnumerator R_SkillUseInput_Key_D()
    {
        // 同時押しでの誤作動をしないようにする時間
        yield return new WaitForSeconds(_inputInvalidTime);

        // 値の初期化
        _inputWaitTime_R_1P[0] = 0;


        while (_inputStandbyTime >= _inputWaitTime_R_1P[0])
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.D))
            {
                StartCoroutine(R_SkillUseInput_Key_S());
                _inputWaitTime_R_1P[0] = _inputStandbyTime;
            }
            else if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.S))
            {
                _inputWaitTime_R_1P[0] = _inputStandbyTime;
            }

            _inputWaitTime_R_1P[0] += Time.deltaTime;
        }
    }
    IEnumerator R_SkillUseInput_Key_S()
    {
        // 同時押しでの誤作動をしないようにする時間
        yield return new WaitForSeconds(_inputInvalidTime);

        // 値の初期化
        _inputWaitTime_R_1P[1] = 0;


        while (_inputStandbyTime >= _inputWaitTime_R_1P[1])
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.S) && skillManager.IsCanUseSkill(0))
            {


                // スキルの使用が許可されていたら、スキル発動
                if (tutorial_CanSkill)
                {
                    // スキルSE
                    SEPlayer.instance.SEOneShot(9);

                    // スキルアニメーション
                    charaObj.Chara_Anim_Att((int)ConstRepo.Player.P1);
                    charaObj.Chara_Anim_Hit((int)ConstRepo.Player.P2);

                    // スキルの使用
                    skillChargeRate[0].UseSkill();
                    skillManager.UseSkill_Logic((int)ConstRepo.Player.P1);
                }


                _inputWaitTime_R_1P[1] = _inputStandbyTime;
            }
            else if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.E)
                || Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.D))
            {
                _inputWaitTime_R_1P[1] = _inputStandbyTime;
            }
            _inputWaitTime_R_1P[1] += Time.deltaTime;
        }
    }




    //2p-----------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 左からのスキル入力の発動関数
    /// </summary>
    /// <returns></returns>
    IEnumerator L_SkillUseInput_Key_K()
    {
        // 同時押しでの誤作動をしないようにする時間
        yield return new WaitForSeconds(_inputInvalidTime);


        // 値の初期化
        _inputWaitTime_L_2P[0] = 0;


        while (_inputStandbyTime >= _inputWaitTime_L_2P[0])
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.K))
            {
                StartCoroutine(L_SkillUseInput_Key_L());
                _inputWaitTime_L_2P[0] = _inputStandbyTime;
            }
            else if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.L))
            {
                _inputWaitTime_L_2P[0] = _inputStandbyTime;
            }

            _inputWaitTime_L_2P[0] += Time.deltaTime;
        }
    }
    IEnumerator L_SkillUseInput_Key_L()
    {
        // 同時押しでの誤作動をしないようにする時間
        yield return new WaitForSeconds(_inputInvalidTime);

        // 値の初期化
        _inputWaitTime_L_2P[1] = 0;


        while (_inputStandbyTime >= _inputWaitTime_L_2P[1])
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.L) && skillManager.IsCanUseSkill(1))
            {

                // スキルの使用が許可されていたら、スキル発動
                if (tutorial_CanSkill)
                {
                    // スキルSE
                    SEPlayer.instance.SEOneShot(9);

                    // スキルアニメーション
                    charaObj.Chara_Anim_Att((int)ConstRepo.Player.P2);
                    charaObj.Chara_Anim_Hit((int)ConstRepo.Player.P1);

                    skillChargeRate[1].UseSkill();
                    skillManager.UseSkill_Logic((int)ConstRepo.Player.P2);
                }

                _inputWaitTime_L_2P[1] = _inputStandbyTime;
            }
            else if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.I)
                 || Input.GetKeyDown(KeyCode.U) || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K))
            {
                _inputWaitTime_L_2P[1] = _inputStandbyTime;
            }
            _inputWaitTime_L_2P[1] += Time.deltaTime;
        }
    }


    /// <summary>
    /// 右からのスキル入力の発動関数
    /// </summary>
    /// <returns></returns>
    IEnumerator R_SkillUseInput_Key_K()
    {
        // 同時押しでの誤作動をしないようにする時間
        yield return new WaitForSeconds(_inputInvalidTime);

        // 値の初期化
        _inputWaitTime_R_2P[0] = 0;


        while (_inputStandbyTime >= _inputWaitTime_R_2P[0])
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.K))
            {
                StartCoroutine(R_SkillUseInput_Key_J());
                _inputWaitTime_R_2P[0] = _inputStandbyTime;
            }
            else if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.L))
            {
                _inputWaitTime_R_2P[0] = _inputStandbyTime;
            }

            _inputWaitTime_R_2P[0] += Time.deltaTime;
        }
    }
    IEnumerator R_SkillUseInput_Key_J()
    {
        // 同時押しでの誤作動をしないようにする時間
        yield return new WaitForSeconds(_inputInvalidTime);

        // 値の初期化
        _inputWaitTime_R_2P[1] = 0;


        while (_inputStandbyTime >= _inputWaitTime_R_2P[1])
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.J) && skillManager.IsCanUseSkill(1))
            {

                // スキルの使用が許可されていたら、スキル発動
                if (tutorial_CanSkill)
                {

                    // スキルSE
                    SEPlayer.instance.SEOneShot(9);

                    // スキルアニメーション
                    charaObj.Chara_Anim_Att((int)ConstRepo.Player.P2);
                    charaObj.Chara_Anim_Hit((int)ConstRepo.Player.P1);

                    // スキルの使用
                    skillChargeRate[1].UseSkill();
                    skillManager.UseSkill_Logic((int)ConstRepo.Player.P2);
                }

                _inputWaitTime_R_2P[1] = _inputStandbyTime;
            }
            else if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.I)
                || Input.GetKeyDown(KeyCode.U) || Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.K))
            {
                _inputWaitTime_R_2P[1] = _inputStandbyTime;
            }
            _inputWaitTime_R_2P[1] += Time.deltaTime;
        }
    }
    //-----------------------------------------------------------------------------------------------------------

}
