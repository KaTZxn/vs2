using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialNotesJudgeScript : MonoBehaviour
{
    /// <summary>
    /// プレイヤーの状態等を管理してるスクリプト参照用
    /// </summary>
    [SerializeField]
    PlayerManagerScript playerManager;

    /// <summary>
    /// ViewNoteManagerに参照する用
    /// </summary>
    [SerializeField]
    ViewNotesManager viewNotesManager;

    /// <summary>
    /// スキルのチャージ率を表示のするオブジェクト
    /// </summary>
    [SerializeField]
    List<SkillChargeRate> skillChargeText;


    /// <summary>
    /// コンボ数を表記してるオブジェクトに参照する用
    /// </summary>
    [SerializeField]
    List<ComboScript> ComboScript;


    /// <summary>
    /// ホールドノーツのキラキラエフェクト
    /// </summary>
    [SerializeField]
    List<NotesEffect_LightScript> holdNotesEffects;

    List<Color> holdNotesEffects_Color;

    /// <summary>
    /// Lineを管理してるマネージャーに参照する用
    /// </summary>
    [SerializeField]
    LinesManager linesManager;


    /// <summary>
    /// playerInputに参照する用
    /// </summary>
    [SerializeField]
    Tutorial_PlayerInput playerInput;



    /// <summary>
    /// エフェクト生成用のエフェクトリスト
    /// </summary>
    [SerializeField]
    List<RawImage> effectList;


    /// <summary>
    /// 生成するエフェクトの一時保存先
    /// </summary>
    [SerializeField]
    RawImage[] effect;

    /// <summary>
    /// 生成するエフェクトの親
    /// </summary>
    [SerializeField]
    Canvas effectParent;


    /// <summary>
    /// エフェクト生成時の、Pos指定用
    /// </summary>
    [SerializeField]
    private RectTransform[] instancePos;

    /// <summary>
    /// 各レーンのHoldNotesの最初の判定時の渡されたtimeを保存しておく用
    /// </summary>
    [SerializeField]
    private long[] holdStartJudgeTime;

    /// <summary>
    /// リンクノーツの最初の判定時の渡されたtimeを記憶しておく用
    /// </summary>
    [SerializeField]
    private long linkFirstJudgeTime;

    /// <summary>
    /// 色変更時使用するカラーコードの、配列_1
    /// </summary>
    [SerializeField]
    private string[] charaColorCode_ver1;

    /// <summary>
    /// 色変更時使用するカラーコードの、配列_2
    /// </summary>
    [SerializeField]
    private string[] charaColorCode_ver2;

    /// <summary>
    ///string型のカラーコードをColor変換した時の受け取り先
    /// </summary>
    Color colourCode;



    /// <summary>
    /// NoteJudgeの種類
    /// </summary>
    enum JudgeType
    {
        Briliant = 0,
        Great,
        Good,
        Poor,
    }
    /*
    Briliant +- 30ms
    Great +- 60ms
    Good +- 120ms
    Poor
    */
    /// <summary>
    /// Briliantの判定用の値
    /// </summary>
    private long briliantJudge = 30;
    /// <summary>
    /// Greatの判定用の値
    /// </summary>
    private long greatJudge = 60;
    /// <summary>
    /// Goodの判定用の値
    /// </summary>
    private long goodJudge = 120;

    /// <summary>
    /// レベル別でプレイヤーの使用する変数を記憶しておく用（Void Start()のみでの使用）
    /// </summary>
    [SerializeField]
    private List<PlayerUseValueClass> playerUseValue_LevelIndex;

    /// <summary>
    /// スコア加算などに実際に使用する変数
    /// </summary>
    [SerializeField]
    private List<PlayerUseValueClass> playerUseValues;

    /// <summary>
    /// 相手の端のレーンまで取っていたら、カウントを許可しない用
    /// </summary>
    [SerializeField]
    private bool[] canAddScoreCount;

    /// <summary>
    /// スキルで判定を絞る為の値(割り算)
    /// </summary>
    [SerializeField]
    public long[] judgeLeverage;


    /// <summary>
    /// スコア用UI参照用
    /// </summary>
    [SerializeField]
    private List<ScoreScript> scoreValue;

    /// <summary>
    /// 判定時のエフェクトの周りのこ
    /// </summary>
    [SerializeField]
    List<NotesJudgeEffects> judgeEffects;

    /// <summary>
    /// プレイヤーがノーミスかどうか
    /// </summary>
    [SerializeField]
    private bool[] isPlayerZeroPoor;



    // ノーツの種類ごとの、いつ判定するかなどのロジック部分-----------------------------------------------------------------------------------------------------------------

    private void Start()
    {

        // プレイヤーの人数分 i == プレイヤーの番号
        // プレイヤーごとに、選択した難易度の、使用する数値を代入
        for (int i = 0; i < 2; i++)
        {
            //　スキルのチャージ率
            playerUseValues[i].skillValues =
                playerUseValue_LevelIndex[playerManager.nowMusicLevel[i]].skillValues;

            //　スコアの加算量
            playerUseValues[i].scoreValues =
                playerUseValue_LevelIndex[playerManager.nowMusicLevel[i]].scoreValues;

            //　レーン奪取に必要な総スコアの加算量
            playerUseValues[i].lineSteal_NeedScore =
                playerUseValue_LevelIndex[playerManager.nowMusicLevel[i]].lineSteal_NeedScore;

            //　レーン奪取時のスコアの加算量
            playerUseValues[i].lineSteal_AddScore =
                playerUseValue_LevelIndex[playerManager.nowMusicLevel[i]].lineSteal_AddScore;
        }

        // プレイヤーマネージャーに参照する用
        playerManager = GameObject.Find("SceneManager").GetComponent<PlayerManagerScript>();


        // ノーミスチェックの初期化
        isPlayerZeroPoor[(int)ConstRepo.Player.P1] = true;
        isPlayerZeroPoor[(int)ConstRepo.Player.P2] = true;

        // プレイヤーマネージャーのフルコンボ判定を初期化
        playerManager.isPlayerFullcombo[(int)ConstRepo.Player.P1] = true;
        playerManager.isPlayerFullcombo[(int)ConstRepo.Player.P2] = true;
    }


    private void Update()
    {
        //　デバック用
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("1Pがラインとられた");



            Debug.Log("2Pがラインとられた");
            //レーンを奪取して
            // もし、レーンの奪取が端まで行ったらfalseが帰ってくる
            canAddScoreCount[(int)ConstRepo.Player.P2]
                = linesManager.LineIsStolen_1p(
                playerUseValues[1].lineSteal_AddScore[0],
                playerUseValues[1].lineSteal_AddScore[1]);

            // レーンを奪ったら、相手の奪うレーンが増えるので、許可を再度だす
            if (!canAddScoreCount[(int)ConstRepo.Player.P1])
            {
                canAddScoreCount[(int)ConstRepo.Player.P1] = true;
            }
        }

        //　デバック用
        if (Input.GetKeyDown(KeyCode.H))
        {
            //レーンを奪取して
            // もし、レーンの奪取が端まで行ったらfalseが帰ってくる
            canAddScoreCount[(int)ConstRepo.Player.P1]
                = linesManager.LineIsStolen_2p(
                playerUseValues[0].lineSteal_AddScore[0],
                playerUseValues[0].lineSteal_AddScore[1]);


            // レーンを奪ったら、相手の奪うレーンが増えるので、許可を再度だす
            if (!canAddScoreCount[(int)ConstRepo.Player.P2])
            {
                canAddScoreCount[(int)ConstRepo.Player.P2] = true;
            }
        }
    }




    /// <summary>
    /// 渡された時間をもとに、今の状況で適切な判定を計算する
    /// </summary>
    /// <param name="time">判定するノーツの時間</param>
    /// <param name="line">判定するノーツが所属しているレーン</param>
    public void SimpleNotesJudgement(long time, int line, int usePlayer)
    {
        // 判定を取得
        Judge(time, line, usePlayer);


        // 判定を取得したNotesのフェードアウト
        viewNotesManager.NowSimpleViewNotesFadeOut(line);
    }




    /// <summary>
    /// HoldNotesの最初の判定
    /// </summary>
    /// <param name="time"></param>
    /// <param name="line"></param>
    /// <param name="usePlayer"></param>
    public bool HoldNotes_StartJudgement(long time, int line, int usePlayer)
    {
        //good以上の判定なら
        if (time <= goodJudge / judgeLeverage[usePlayer])
        {
            //現在のHoldNotesのレイヤーを変えて、判定バーより下を描画しないようにする
            viewNotesManager.NowHoldViewNotesChangeLayers(line);

            //good以上の判定なら次の判定用に、timeを記憶
            holdStartJudgeTime[line] = time;

            // レーンの奪取状況とキャラのカラーにあった番号を取得
            if (linesManager.haveLines_1p >= line)
            {
                if (ColorUtility.TryParseHtmlString(charaColorCode_ver2
                    [playerManager.playerCharaNum[(int)ConstRepo.Player.P1]], out colourCode))
                {
                    //キラキラエフェクトの色を指定
                    holdNotesEffects[line].SetParticleColor(colourCode);
                }

            }
            else
            {
                if (ColorUtility.TryParseHtmlString(charaColorCode_ver2
                    [playerManager.playerCharaNum[(int)ConstRepo.Player.P2]], out colourCode))
                {
                    //キラキラエフェクトの色を指定
                    holdNotesEffects[line].SetParticleColor(colourCode);
                }
            }



            //キラキラエフェクトを生成
            holdNotesEffects[line].ParticlePlay();

            //次の判定許可
            return true;

        }
        //poor判定なら
        else
        {
            // 記憶を初期化
            holdStartJudgeTime[line] = 0;
            // endの判定を呼び出し
            HoldNotes_EndJudgement(time, line, usePlayer);
            //次の判定を許可を出さない
            return false;
        }
    }

    /// <summary>
    /// HoldNotesの最終判定
    /// </summary>
    /// <param name="time"></param>
    /// <param name="line"></param>
    /// <param name="usePlayer"></param>
    public void HoldNotes_EndJudgement(long time, int line, int usePlayer)
    {
        //　値の大きいほうを使用
        if (holdStartJudgeTime[line] > time)
        {
            time = holdStartJudgeTime[line];
        }

        // キラキラエフェクトの停止
        holdNotesEffects[line].ParticleStop();

        // 判定を取得
        Judge(time, line, usePlayer);

        // 判定を取得したNotesのフェードアウト
        viewNotesManager.NowHoldViewNotesFadeOut(line);
    }

    /// <summary>
    /// リンクノーツの最初の判定
    /// </summary>
    /// <param name="time"></param>
    /// <param name="usePlayer"></param>
    /// <returns></returns>
    public bool LinkNotes_FirstJudgement(long time, int usePlayer)
    {
        //good以上の判定なら
        if (time <= goodJudge / judgeLeverage[usePlayer])
        {
            //good以上の判定なら次の判定用に、timeを記憶
            linkFirstJudgeTime = time;

        }
        //poor判定なら
        else
        {
            // Poorの値の代入(Poorの値のため、マイナス)
            linkFirstJudgeTime = ConstRepo.poorTiming;
        }
        //同じプレイヤーが、2回判定しないように、許可を出さない
        return false;
    }

    /// <summary>
    /// リンクノーツの最後の判定（2回目）
    /// </summary>
    /// <param name="time"></param>
    /// <param name="usePlayer"></param>
    /// <returns></returns>
    public void LinkNotes_LastJudgement(long time, int line, int usePlayer)
    {
        //good以上の判定なら
        if (time <= goodJudge / judgeLeverage[usePlayer])
        {
            //　値の小さいほうを使用
            if (linkFirstJudgeTime < time)
            {
                // 最初の判定でpoorならマイナスの値が入ってるため、そちらが優先される
                time = linkFirstJudgeTime;
            }
        }


        // 判定を取得（リンクノーツだけエフェクトの位置が特殊なため、引数を8に指定）
        LinkJudge(time, 8, usePlayer);

        Debug.Log("linkFirstJudgeTime" + linkFirstJudgeTime);
        //値の初期化
        linkFirstJudgeTime = 0;

        // 判定を取得したNotesのフェードアウト
        viewNotesManager.NowSimpleViewNotesFadeOut(line);
    }



    //各タイプに適した、判定の仕分け---------------------------------------------------------------------------------------------------------


    /// <summary>
    /// 判定を元に、スコアの加算やエフェクトなどの判定の処理を行う
    /// </summary>
    /// <param name="time"></param>
    /// <param name="line"></param>
    /// <param name="usePlayer"></param>
    /// <returns></returns>
    private void Judge(long time, int line, int usePlayer)
    {
        // Briliantの判定時間 / スキルによる倍率以下なら
        if (time <= briliantJudge / judgeLeverage[usePlayer])
        {
            OtherThanPoorJudge(line, usePlayer, (int)JudgeType.Briliant);

            NotesJudgeEffect((int)JudgeType.Briliant, line, usePlayer);
        }
        // Greatの判定時間 / スキルによる倍率以下なら
        else if (time <= greatJudge / judgeLeverage[usePlayer])
        {
            OtherThanPoorJudge(line, usePlayer, (int)JudgeType.Great);

            NotesJudgeEffect((int)JudgeType.Great, line, usePlayer);
        }
        // Goodの判定時間 / スキルによる倍率以下なら
        else if (time <= goodJudge / judgeLeverage[usePlayer])
        {
            OtherThanPoorJudge(line, usePlayer, (int)JudgeType.Good);

            NotesJudgeEffect((int)JudgeType.Good, line, usePlayer);
        }
        //poor判定
        else
        {
            PoorJudge(line, usePlayer);
        }


        // スコアUIの更新
        scoreValue[usePlayer].ScoreUpdate();



        //　スコア加算量のカウントが一定以上になったらレーン奪取される
        LineStolen_Check(usePlayer);
    }


    /// <summary>
    /// 渡された時間をもとに、毒ノーツに対して適切な判定を計算する
    /// </summary>
    /// <param name="time">判定するノーツの時間</param>
    /// <param name="line">判定するノーツが所属しているレーン</param>
    public void PoisonNotesJudgement(long time, int line, int usePlayer)
    {

        // Briliantの判定時間 / スキルによる倍率以下なら(poisonなため内部処理は、poor)
        if (time <= briliantJudge / judgeLeverage[usePlayer])
        {
            PoorJudge(line, usePlayer);
        }

        // Graetの判定時間 / スキルによる倍率以下なら(poisonなため内部処理は、Good)
        else if (time <= greatJudge / judgeLeverage[usePlayer])
        {
            OtherThanPoorJudge(line, usePlayer, (int)JudgeType.Good);

            NotesJudgeEffect((int)JudgeType.Good, line, usePlayer);
        }
        // Goodの判定時間 / スキルによる倍率以下なら(poisonなため内部処理は、Great)
        else if (time <= goodJudge / judgeLeverage[usePlayer])
        {
            OtherThanPoorJudge(line, usePlayer, (int)JudgeType.Great);

            NotesJudgeEffect((int)JudgeType.Great, line, usePlayer);
        }

        //poor判定(poisonなため内部処理は、Briliant)
        else
        {
            OtherThanPoorJudge(line, usePlayer, (int)JudgeType.Briliant);

            NotesJudgeEffect((int)JudgeType.Briliant, line, usePlayer);
        }


        // スコアUIの更新
        scoreValue[usePlayer].ScoreUpdate();


        // 判定を取得したNotesのフェードアウト
        viewNotesManager.NowSimpleViewNotesFadeOut(line);



        //　スコア加算量のカウントが一定以上になったらレーン奪取される
        LineStolen_Check(usePlayer);
    }



    /// <summary>
    /// LinkNotes専用判定を元に、スコアの加算やエフェクトなどの判定の処理を行う
    /// </summary>
    /// <param name="time"></param>
    /// <param name="line"></param>
    /// <param name="usePlayer"></param>
    /// <returns></returns>
    private void LinkJudge(long time, int line, int usePlayer)
    {
        // Briliantの判定時間 / スキルによる倍率以下なら
        //if (time <= briliantJudge / judgeLeverage[usePlayer])
        //{
        //    LinkOnly_OtherThanPoorJudge(line, usePlayer, (int)JudgeType.Briliant);

        //    NotesJudgeEffect((int)JudgeType.Briliant, line, usePlayer);
        //}
        //// Greatの判定時間 / スキルによる倍率以下なら
        //else if (time <= greatJudge / judgeLeverage[usePlayer])
        //{
        //    LinkOnly_OtherThanPoorJudge(line, usePlayer, (int)JudgeType.Great);

        //    NotesJudgeEffect((int)JudgeType.Great, line, usePlayer);
        //}
        //else


        //Good以上は、ブリリアント固定
        // Goodの判定時間 / スキルによる倍率以下なら
        if (time <= goodJudge / judgeLeverage[usePlayer])
        {
            LinkOnly_OtherThanPoorJudge(line, usePlayer, (int)JudgeType.Briliant);

            NotesJudgeEffect((int)JudgeType.Briliant, line, usePlayer);
        }
        //poor判定
        else
        {
            LinkOnly_PoorJudge(line, usePlayer);
        }

        //両プレイヤー分
        for (int i = 0; i < 2; i++)
        {
            // スコアUIの更新
            scoreValue[i].ScoreUpdate();
        }


        //　スコア加算量のカウントが一定以上になったらレーン奪取される
        LineStolen_Check(usePlayer);
    }


    /// <summary>
    /// レーン奪取に必要なスコア加算総量以上、スコアが加算されているかのチェックと,
    /// スコア加算総量が条件を満たしていれば、レーン奪取をする
    /// </summary>
    /// <param name="usePlayer"></param>
    void LineStolen_Check(int usePlayer)
    {
        //レーン奪取に必要なスコア量以上、スコアが加算されていたら
        if (playerUseValues[usePlayer].lineSteal_NeedScore <= playerUseValues[usePlayer].addScoreCount)
        {
            //プレイヤーに対応した、レーンの奪取
            if (usePlayer == (int)ConstRepo.Player.P1)
            {
                //レーンを奪取して
                // もし、レーンの奪取が端まで行ったらfalseが帰ってくる
                canAddScoreCount[(int)ConstRepo.Player.P1]
                    = linesManager.LineIsStolen_2p(
                    playerUseValues[usePlayer].lineSteal_AddScore[0],
                    playerUseValues[usePlayer].lineSteal_AddScore[1]);


                // レーンを奪ったら、相手の奪うレーンが増えるので、許可を再度だす
                if (!canAddScoreCount[(int)ConstRepo.Player.P2])
                {
                    canAddScoreCount[(int)ConstRepo.Player.P2] = true;
                }
            }
            else
            {
                //レーンを奪取して
                // もし、レーンの奪取が端まで行ったらfalseが帰ってくる
                canAddScoreCount[(int)ConstRepo.Player.P2]
                    = linesManager.LineIsStolen_1p(
                    playerUseValues[usePlayer].lineSteal_AddScore[0],
                    playerUseValues[usePlayer].lineSteal_AddScore[1]);

                // レーンを奪ったら、相手の奪うレーンが増えるので、許可を再度だす
                if (!canAddScoreCount[(int)ConstRepo.Player.P1])
                {
                    canAddScoreCount[(int)ConstRepo.Player.P1] = true;
                }
            }

            //値の初期化
            playerUseValues[usePlayer].addScoreCount = 0;
        }
    }







    //判定時の最終処理系---------------------------------------------------------------------------------------------------------


    /// <summary>
    /// Poor以外の判定時処理
    /// </summary>
    /// <param name="line"></param>
    /// <param name="usePlayer"></param>
    /// <param name="judgeType"></param>
    private void OtherThanPoorJudge(int line, int usePlayer, int judgeType)
    {
        //judgeTypeに適切な、エフェクトを一時格納オブジェクトに生成
        effect[line] = Instantiate(effectList[judgeType]);

        //生成したオブジェクトの親を設定
        effect[line].transform.parent = instancePos[line].transform;

        // エフェクトを引数のlineに対応する場所に移動
        effect[line].rectTransform.position = instancePos[line].position;


        // ブリリアント以外なら、カラー変更する
        if (judgeType != (int)JudgeType.Briliant)
        {
            // レーンの奪取状況とキャラのカラーにあった番号を取得
            if (linesManager.haveLines_1p >= line)
            {
                if (ColorUtility.TryParseHtmlString(charaColorCode_ver2
                    [playerManager.playerCharaNum[(int)ConstRepo.Player.P1]], out colourCode))
                {
                    //生成したオブジェクトの色を指定
                    effect[line].GetComponent<RawImage>().color = colourCode;
                }

            }
            else
            {
                if (ColorUtility.TryParseHtmlString(charaColorCode_ver2
                    [playerManager.playerCharaNum[(int)ConstRepo.Player.P2]], out colourCode))
                {
                    //生成したオブジェクトの色を指定
                    effect[line].GetComponent<RawImage>().color = colourCode;
                }
            }
        }



        //スコア総数に、judgeTypeに適切な、スコア加算
        scoreValue[usePlayer].scoreValue +=
            playerUseValues[usePlayer].scoreValues[judgeType];


        // スコアの加算量の記憶が可能なら
        if (canAddScoreCount[usePlayer])
        {
            //スコア加算量を記憶しておく用変数に、judgeTypeに適切な、スコア加算
            playerUseValues[usePlayer].addScoreCount +=
                playerUseValues[usePlayer].scoreValues[judgeType];
        }

        //総打数記憶用変数に加算
        scoreValue[usePlayer].totalHitsNum++;

        //スキルのチャージ率の増加(judgeTypeに適切な)
        skillChargeText[usePlayer].AddSkillValue(playerUseValues[usePlayer]
            .skillValues[judgeType]);

        //コンボ数の加算
        ComboScript[usePlayer].AddComboValue();



    }


    /// <summary>
    /// Poorの判定時処理
    /// </summary>
    /// <param name="line"></param>
    /// <param name="usePlayer"></param>
    private void PoorJudge(int line, int usePlayer)
    {
        // poorエフェクトを一時格納オブジェクトに生成
        effect[line] = Instantiate(effectList[(int)JudgeType.Poor]);

        // 生成したオブジェクトの親を設定
        effect[line].transform.parent = instancePos[line].transform;

        // エフェクトを引数のlineに対応する場所に移動
        effect[line].rectTransform.position = instancePos[line].position;

        // コンボ数のリセット
        ComboScript[usePlayer].ComboReset();

        // ノーミスだったら
        if (isPlayerZeroPoor[usePlayer])
        {
            // フルコンボ判定を消す
            playerManager.isPlayerFullcombo[usePlayer] = false;

        }
    }



    /// <summary>
    /// LinkNotes専用の！！！　Poor以外の判定時処理
    /// </summary>
    /// <param name="line"></param>
    /// <param name="usePlayer"></param>
    /// <param name="judgeType"></param>
    private void LinkOnly_OtherThanPoorJudge(int line, int usePlayer, int judgeType)
    {
        //Good以上は、ブリリアント判定に変更


        //judgeTypeに適切な、エフェクトを一時格納オブジェクトに生成
        effect[8] = Instantiate(effectList[judgeType]);

        //生成したオブジェクトの親を設定
        effect[8].transform.parent = instancePos[8].transform;

        // エフェクトを引数のlineに対応する場所に移動
        effect[8].rectTransform.position = instancePos[8].position;

        //// ブリリアント以外なら、カラー変更する
        //if(judgeType != (int)JudgeType.Briliant)
        //{
        //    // レーンの奪取状況とキャラのカラーにあった番号を取得
        //    if (linesManager.haveLines_1p >= line)
        //    {
        //        if (ColorUtility.TryParseHtmlString(charaColorCode_ver2
        //            [playerManager.playerCharaNum[(int)ConstRepo.Player.P1]], out colourCode))
        //        {
        //            //生成したオブジェクトの色を指定
        //            effect[line].GetComponent<RawImage>().color = colourCode;
        //        }

        //    }
        //    else
        //    {
        //        if (ColorUtility.TryParseHtmlString(charaColorCode_ver2
        //            [playerManager.playerCharaNum[(int)ConstRepo.Player.P2]], out colourCode))
        //        {
        //            //生成したオブジェクトの色を指定
        //            effect[line].GetComponent<RawImage>().color = colourCode;
        //        }
        //    }
        //}

        // 全レーンにブリリアントの、テキスト以外のエフェクトを再生
        for (int i = 0; i < 8; i++)
        {
            NotesJudgeEffect((int)JudgeType.Briliant, i, 0);
        }


        //両プレイヤー分
        for (int i = 0; i < 2; i++)
        {
            //スコア総数に、judgeTypeに適切な値をたす
            scoreValue[i].scoreValue += playerUseValues[i].scoreValues[judgeType];

            // スコアの加算量の記憶が可能なら
            if (canAddScoreCount[i])
            {
                //スコア加算量を記憶しておく用変数に、judgeTypeに適切な、スコア加算
                playerUseValues[i].addScoreCount +=
                    playerUseValues[i].scoreValues[judgeType];
            }

            //総打数記憶用変数に加算
            scoreValue[i].totalHitsNum++;

            //スキルのチャージ率の増加(judgeTypeに適切な)
            skillChargeText[i].AddSkillValue(playerUseValues[i]
                .skillValues[judgeType]);

            //コンボ数の加算
            ComboScript[i].AddComboValue();
        }
    }


    /// <summary>
    /// LinkNotes専用の！！！　Poorの判定時処理
    /// </summary>
    /// <param name="line"></param>
    /// <param name="usePlayer"></param>
    private void LinkOnly_PoorJudge(int line, int usePlayer)
    {
        //poorエフェクトを一時格納オブジェクトに生成
        effect[line] = Instantiate(effectList[(int)JudgeType.Poor]);

        //生成したオブジェクトの親を設定
        effect[line].transform.parent = instancePos[line].transform;

        // エフェクトを引数のlineに対応する場所に移動
        effect[line].rectTransform.position = instancePos[line].position;

        //両プレイヤー分
        for (int i = 0; i < 2; i++)
        {
            //コンボ数のリセット
            ComboScript[i].ComboReset();


            // ノーミスだったら
            if (isPlayerZeroPoor[i])
            {
                // フルコンボ判定を消す
                playerManager.isPlayerFullcombo[i] = false;
            }
        }
    }


    public void NotesJudgeEffect(int effectNum, int line, int colourNum)
    {
        if (linesManager.haveLines_1p >= line)
        {
            colourNum = playerManager.playerCharaNum[(int)ConstRepo.Player.P1];
        }
        else
        {
            colourNum = playerManager.playerCharaNum[(int)ConstRepo.Player.P2];
        }



        switch (effectNum)
        {
            case (int)JudgeType.Briliant:

                judgeEffects[line].BriliantEffect();
                break;

            case (int)JudgeType.Great:

                judgeEffects[line].GreatEffect(colourNum);
                break;

            case (int)JudgeType.Good:
                judgeEffects[line].GoodEffect(colourNum);
                break;

            default:
                break;

        }

    }
}
