using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinesManager : MonoBehaviour
{
    /// <summary>
    /// キャラをいじってるObjに参照用
    /// </summary>
    [SerializeField]
    TestChara testChara;

    /// <summary>
    /// live2DをいじってるObjに参照する用
    /// </summary>
    [SerializeField]
    GameObject live2D_Canvas;

    /// <summary>
    /// ノーツの見た目などを管理してる子に参照する用
    /// </summary>
    [SerializeField]
    ViewNotesManager viewNotesManager;

    /// <summary>
    /// 1pのスコアの数値を参照する用
    /// </summary>
    [SerializeField]
    List <ScoreScript> scoreScript;

    /// <summary>
    /// クラウンUIを参照する用
    /// </summary>
    [SerializeField]
    CrownUIManager crownUIManager;

    /// <summary>
    /// スキル周りのUIを参照する用
    /// </summary>
    [SerializeField]
    List<SkillChargeRate> skillUIs;

    /// <summary>
    /// コンボ表記系
    /// </summary>
    [SerializeField]
    List<ComboScript> comboScripts;

    /// <summary>
    /// 曲のレベル表記UI
    /// </summary>
    [SerializeField]
    List<MusicLevelUIScript> musicLevelUIs;


    /// <summary>
    /// Line参照用
    /// </summary>
    [SerializeField]
    List<GameObject> lines;


    /// <summary>
    /// Lineの色変更時使用するカラーコードの、配列
    /// </summary>
    public string[] charaColorCode;


    /// <summary>
    ///string型のカラーコードをColor変換した時の受け取り先
    /// </summary>
    Color colorCode;


    /// <summary>
    /// 現在所持しているラインの本数(配列用に0から計算)
    /// </summary>
    [Range(0, 6)]
    [SerializeField]
    public int haveLines_1p;


    /// <summary>
    /// lineの透明度を指定する
    /// </summary>
    [Range(0f, 1.0f)]
    [SerializeField]
    private float _lineAlpha;

    /// <summary>
    /// ジャッチ演出で使う、ジャッチ開始までの時間
    /// </summary>
    [SerializeField]
    private float judgeStandbyTime;


    /// <summary>
    /// ジャッチ演出で使う、レーンの色を何秒おきに変えるか設定用
    /// </summary>
    [SerializeField]
    private float judgeColourChangeStandbyTime;


    /// <summary>
    /// 勝った方のプレイヤー番号
    /// </summary>
    [SerializeField]
    private int winPlayerNum;

    /// <summary>
    /// プレイヤーの所持レーン数の変動を許可する変数(変数のスイッチ用)
    /// </summary>
    private bool canFluctuation_1pHaveLines;

    /// <summary>
    /// 曲のデータ参照用
    /// </summary>
    JsonReader songDataJson;

    /// <summary>
    /// ジャッジの準備を始める時間
    /// </summary>
    long judgeSetupTime;

    /// <summary>
    /// ジャッジ演出のセットアップで使うUIのアニメーター
    /// </summary>
    [SerializeField]
    Animator judgeUIAnimator;

    /// <summary>
    /// 勝利ノーツを生成するクリエイター
    /// </summary>
    [SerializeField]
    WinNotesCreator winNotesCreator;

    /// <summary>
    /// シークバー周りに干渉する用
    /// </summary>
    [SerializeField]
    SeekBarManager seekBarManager;

    /// <summary>
    /// スキルに干渉する用
    /// </summary>
    [SerializeField]
    SkillManager skillManager;

    /// <summary>
    /// プレイヤーマネージャー参照する用
    /// </summary>
    PlayerManagerScript playerManagerScript;

    private void Start()
    {
        // プレイヤーがレーンを奪った時に、見た目も変るようにtrueにする
        canFluctuation_1pHaveLines = true;

        // 1p2pのキャラに合わせてLineの色を変える
        SetLineColor_1p2p();


        //プレイヤーマネージャーを取得
        playerManagerScript = GameObject.Find("SceneManager").GetComponent<PlayerManagerScript>();


        // キャラ周りのデータ参照用
        testChara = GameObject.Find("CharaControl").GetComponent<TestChara>();

        // 曲データ参照用
        songDataJson = GameObject.Find("Json").GetComponent<JsonReader>();

        // ジャッジ演出準備を始める時間を設定
        judgeSetupTime = songDataJson._songList.songdata.kiaitime;
    }




    private void Update()
    {



        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("全ラインを灰色化");

            LineJudgmentSetup();
        }

        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    Debug.Log("ラインをリセット");
        //    SetLineColor_1p2p();

        //    haveLines_1p = 3;
        //}


        // セットアップで閉じるboolを利用して一回のみ読み込む
        // CriWareの時間と、judgeSetupTimeを比べて、CriWareの時間の方が大きくなったら
        if (canFluctuation_1pHaveLines
            && MusicData.Timer > judgeSetupTime)
        {
            // ジャッジ演出のセットアップを実行
            LineJudgmentSetup();
        }

    }






    /// <summary>
    /// 1pHaveLineのゲッター
    /// </summary>
    /// <returns></returns>
    public int Get_1pHaveLines()
    {
        return haveLines_1p;
    }



    /// <summary>
    /// 1p所持ラインの一番2p側を相手の色に変更する
    /// </summary>
    public bool LineIsStolen_1p(int lineStealScore_Max , int lineStealScore_Min)
    {



        // レーン所持数の変動が許可されていたら
        if (canFluctuation_1pHaveLines)
        {

            //配列参照を防ぐ
            if (haveLines_1p > 0)
            {

                // レーン奪取SE
                SEPlayer.instance.SEOneShot(8);


                // Color型への変換成功するとcolorにColor型の2pキャラのカラーコードが代入される）outキーワードで参照渡しにする
                if (ColorUtility.TryParseHtmlString(charaColorCode[testChara.count[1]],
                    out colorCode))
                {
                    // 透明度の設定
                    colorCode.a = _lineAlpha;

                    // 所持ラインの一番2p側を相手の色に変更する
                    lines[haveLines_1p].GetComponent<SpriteRenderer>().color = colorCode;

                    // 1p所持ラインを減算
                    haveLines_1p--;
                }


                //1pのラインを奪取されたので２Pのスコアを加算
                //1pの所持ラインが3以下なら元々2pのラインなので加算直が少ないほうを適応
                if (haveLines_1p < 3)
                {
                    scoreScript[1].scoreValue += lineStealScore_Max;
                }
                else
                {
                    scoreScript[1].scoreValue += lineStealScore_Min;
                }
                //スコア表記の更新
                scoreScript[1].ScoreUpdate();


                //奪われたラインに流れてるノーツの色を2pの色に変更
                viewNotesManager.ActiveSimpleViewNotesColourChange(haveLines_1p + 1, 1);
                viewNotesManager.ActiveHoldViewNotesColourChange(haveLines_1p + 1, 1);


                // 2Pのキャラの攻撃アニメーションを再生
                testChara.Chara_Anim_Att((int)ConstRepo.Player.P2);

                // 1Pのキャラの被ダメージアニメーションを再生
                testChara.Chara_Anim_Hit((int)ConstRepo.Player.P1);


                //奪ったレーンカウントを加算
                scoreScript[1].stolenLaneCount++;

            }

        }
        // レーン所持数の変動が許可されていなかったら
        else
        {
            //奪ったレーンカウントを加算
            scoreScript[1].stolenLaneCount++;
        }




        Debug.Log(haveLines_1p);
        //　端のレーンまで取得したら
        if (haveLines_1p <= 0)
        {
            Debug.Log(false);
            return false;
        }
        //　端のレーンまで取得してなかったら
        else
        {
            Debug.Log(true);
            return true;
        }
    }


    /// <summary>
    /// 2pの所持ラインの一番1p側を自分の色に変更する
    /// </summary>
    public bool LineIsStolen_2p(int lineStealScore_Max, int lineStealScore_Min)
    {

        // レーン所持数の変動が許可されていたら
        if (canFluctuation_1pHaveLines)
        {
            //配列参照を防ぐ
            if (haveLines_1p < 6)
            {
                // レーン奪取SE
                SEPlayer.instance.SEOneShot(8);

                // Color型への変換成功すると（colorにColor型の赤色が代入される）outキーワードで参照渡しにする
                if (ColorUtility.TryParseHtmlString(charaColorCode[testChara.count[0]],
                out colorCode))
                {

                    // 1p所持ラインを加算
                    haveLines_1p++;

                    // 透明度の設定
                    colorCode.a = _lineAlpha;

                    // 2pの所持ラインの一番1p側を1pの色に変更する
                    lines[haveLines_1p].GetComponent<SpriteRenderer>().color = colorCode;

                }

                //2pのラインを奪取されたので1pのスコアを加算
                //1pの所持ラインが3以下なら元々2pのラインなので加算直が少ないほうを適応
                if (haveLines_1p > 3)
                {
                    scoreScript[0].scoreValue += lineStealScore_Max;
                }
                else
                {
                    scoreScript[0].scoreValue += lineStealScore_Min;
                }

                //スコア表記の更新
                scoreScript[0].ScoreUpdate();


                //奪われたラインに流れてるノーツの色を1pの色に変更
                viewNotesManager.ActiveSimpleViewNotesColourChange(haveLines_1p, 0);
                viewNotesManager.ActiveHoldViewNotesColourChange(haveLines_1p, 0);


                // 1Pのキャラの攻撃アニメーションを再生
                testChara.Chara_Anim_Att((int)ConstRepo.Player.P1);

                // 2Pのキャラの被ダメージアニメーションを再生
                testChara.Chara_Anim_Hit((int)ConstRepo.Player.P2);

                //奪ったレーンカウントを加算
                scoreScript[0].stolenLaneCount++;
            }
        }
        // レーン所持数の変動が許可されていなかったら
        else
        {
            //奪ったレーンカウントを加算
            scoreScript[0].stolenLaneCount++;
        }




        Debug.Log(haveLines_1p);
        //　端のレーンまで取得したら
        if (haveLines_1p >= 6)
        {
            Debug.Log(false);
            return false;
        }
        //　端のレーンまで取得してなかったら
        else
        {
            Debug.Log(true);
            return true;
        }
    }


    /// <summary>
    /// 全ラインをカラーコード配列の任意の色に変える
    /// </summary>
    /// <param name="colourNum">カラーコード指定用（0～4）</param>
    void OllLineColorChange(int colourNum)
    {
        // 全ラインに参照
        foreach (GameObject line in lines)
        {
            //カラーコードを指定
            if (ColorUtility.TryParseHtmlString(charaColorCode[colourNum],
                out colorCode))
            {
                // 透明度の設定
                colorCode.a = _lineAlpha;

                // 所持ラインを指定の色に変更する
                line.GetComponent<SpriteRenderer>().color = colorCode;
            }
        }
    }
    


    /// <summary>
    /// 全ラインをカラーコード配列の任意の色に変える
    /// </summary>
    /// <param name="colourNum">カラーコード指定用（0～4）</param>
    public void HalfLineColorChange(int colourNum ,int playerNum)
    {
        //　指定されたプレイヤー側のレーンを引数のキャラの色に変える（4は、灰色）
        switch(playerNum)
        {
            // 1p側のラインに参照
            case (int)ConstRepo.Player.P1:  
                
                // 4本分変える
                for (int i = 0; i < 4; i++)
                {
                    //カラーコードを指定
                    if (ColorUtility.TryParseHtmlString(charaColorCode[colourNum],
                        out colorCode))
                    {
                        // 透明度の設定
                        colorCode.a = _lineAlpha;

                        // 1P所持ラインを指定の色に変更する
                        lines[i].GetComponent<SpriteRenderer>().color = colorCode;
                    }
                }

                break;

            // 2p側のラインに参照
            case (int)ConstRepo.Player.P2:

                // 4本分変える
                for (int i = 0; i < 4; i++)
                {
                    //カラーコードを指定
                    if (ColorUtility.TryParseHtmlString(charaColorCode[colourNum],
                        out colorCode))
                    {
                        // 透明度の設定
                        colorCode.a = _lineAlpha;

                        // 2P所持ラインを指定の色に変更する
                        lines[7 - i].GetComponent<SpriteRenderer>().color = colorCode;
                    }
                }

                break;
        }
    }



    /// <summary>
    /// 1p2pのラインを各キャラカラーに変更する
    /// </summary>
    void SetLineColor_1p2p()
    {
        //レーン数分繰り返す
        for (int i = 0; i < lines.Count; i++)
        {
            if (i < 4)
            {
                //1PのカラーコードをcolorCodeに入れる
                ColorUtility.TryParseHtmlString(charaColorCode[testChara.count[0]], out colorCode);

                // 透明度の設定
                colorCode.a = _lineAlpha;

                // ラインの色を変更する
                lines[i].GetComponent<SpriteRenderer>().color = colorCode;

            }
            else
            {
                //2PのカラーコードをcolorCodeに入れる
                ColorUtility.TryParseHtmlString(charaColorCode[testChara.count[1]], out colorCode);

                // 透明度の設定
                colorCode.a = _lineAlpha;

                // ラインの色を変更する
                lines[i].GetComponent<SpriteRenderer>().color = colorCode;
            }
        }
    }


    /// <summary>
    /// ジャッジ演出の準備をする
    /// </summary>
    private void LineJudgmentSetup()
    {
        // 1Pの流れてるノーツを1pの色に変更
        for (int i = haveLines_1p; i <= 3; i++)
        {
            //真ん中まで流れてるノーツの色を1pの色に変更
            viewNotesManager.ActiveSimpleViewNotesColourChange(i, 0);
            viewNotesManager.ActiveHoldViewNotesColourChange(i, 0);
        }
        // 2Pの流れてるノーツを2pの色に変更
        for (int i = haveLines_1p; i >= 3; i--)
        {
            //真ん中まで流れてるノーツの色を2pの色に変更
            viewNotesManager.ActiveSimpleViewNotesColourChange(i + 1, 1);
            viewNotesManager.ActiveHoldViewNotesColourChange(i + 1, 1);
        }

        // レーン所持数の変動の許可を閉じる
        canFluctuation_1pHaveLines = false;
            
        // レーンの所持数の初期化
        haveLines_1p = 3;

        // スコア、クラウン等の勝敗のわかる、UIを消す
        FirstHideUIs();

        // 全ラインを灰色にする
        OllLineColorChange(4);

        // ジャッジUIをプレイ
        judgeUIAnimator.SetTrigger("JudgeUI_Active");

    }


    /// <summary>
    /// ジャッジ演出
    /// </summary>
    /// <returns></returns>

    public IEnumerator LineJudgmentPerformance()
    {
        //　勝敗条件「レーンの所持数」＞「スコアの量」

        // 1pが相手より多くレーンを所持していれば
        if (scoreScript[(int)ConstRepo.Player.P1].stolenLaneCount 
            > scoreScript[(int)ConstRepo.Player.P2].stolenLaneCount)
        {
            Debug.Log("ラインで判断　1P");
            winPlayerNum = (int)ConstRepo.Player.P1;
        }
        // 2pが相手より多くレーンを所持していれば
        else if (scoreScript[(int)ConstRepo.Player.P1].stolenLaneCount 
            < scoreScript[(int)ConstRepo.Player.P2].stolenLaneCount)
        {
            Debug.Log("ラインで判断　2P");
            winPlayerNum = (int)ConstRepo.Player.P2;
        }
        // 1P,2P共に、レーンの所持数が同じであれば
        else
        {
            //スコア量の多い方の勝利
            if (scoreScript[(int)ConstRepo.Player.P1].scoreValue
                > scoreScript[(int)ConstRepo.Player.P2].scoreValue)
            {
                Debug.Log("スコアで判断　1P");
                winPlayerNum = (int)ConstRepo.Player.P1;
            }
            else if (scoreScript[(int)ConstRepo.Player.P1].scoreValue
                < scoreScript[(int)ConstRepo.Player.P2].scoreValue) 
            {
                Debug.Log("スコアで判断　2P");
                winPlayerNum = (int)ConstRepo.Player.P2;
            }
            else
            {
                // 同点処理 プレイヤー数以上にする
                winPlayerNum = 10;
                Debug.Log("スコアで判断　" + winPlayerNum);
            }
        }


        //残りのを消す
        SecondHideUIs();


        // スキルのエフェクトを再生中なら、終わらせる
        skillManager.SkillEffectEnd((int)ConstRepo.Player.P1
            , playerManagerScript.playerCharaNum[(int)ConstRepo.Player.P1]);
        skillManager.SkillEffectEnd((int)ConstRepo.Player.P2
            , playerManagerScript.playerCharaNum[(int)ConstRepo.Player.P2]);


        // winPlayerNumの値が、プレイヤーの番号に該当するなら
        if ((int)ConstRepo.Player.P1 <= winPlayerNum
            && winPlayerNum <= (int)ConstRepo.Player.P2)
        {
            // 勝利ノーツを生成する
            winNotesCreator.generationWinNotes(winPlayerNum);


            yield return new WaitForSeconds(3.5f);
        }


        // ちょっと待つ
        yield return new WaitForSeconds(1f);
        
        // キャラの動きをスタートさせる
        StartCoroutine( 
            live2D_Canvas.GetComponent<MainLive2DCanvas>().Live2DJudgmentPerformance(winPlayerNum));

        // キャラの動きに合わせて、レーンを染めるために待つ
        yield return new WaitForSeconds(judgeStandbyTime);



        // 1pの勝ちなら
        if (winPlayerNum == (int)ConstRepo.Player.P1)
        {
            for (int i = 0; i < 8;)
            {
                // レーン奪取SE
                SEPlayer.instance.SEOneShot(8);


                //1PのカラーコードをcolorCodeに入れる
                ColorUtility.TryParseHtmlString(charaColorCode[testChara.count[0]], out colorCode);

                // 透明度の設定
                colorCode.a = _lineAlpha;

                // ラインの色を変更する
                lines[i].GetComponent<SpriteRenderer>().color = colorCode;

                i++;

                yield return new WaitForSeconds(judgeColourChangeStandbyTime);
            }
        }
        // 2pの勝ちなら
        else if (winPlayerNum == (int)ConstRepo.Player.P2)
        {
            for (int i = 0; i < 8;)
            {
                // レーン奪取SE
                SEPlayer.instance.SEOneShot(8);


                //2PのカラーコードをcolorCodeに入れる
                ColorUtility.TryParseHtmlString(charaColorCode[testChara.count[1]], out colorCode);

                // 透明度の設定
                colorCode.a = _lineAlpha;

                // ラインの色を変更する
                lines[7 - i].GetComponent<SpriteRenderer>().color = colorCode;

                i++;

                yield return new WaitForSeconds(judgeColourChangeStandbyTime);
            }
        }
        // 同点なら、レーン奪取を行わない
        else
        {

        }

    }

    /// <summary>
    /// クラウンを透明にして隠す
    /// </summary>
    void FirstHideUIs()
    {
        crownUIManager.HideCrownUIs();
    }


    /// <summary>
    /// スキル周りUI、コンボ、曲難易度、シークバー周り、スコアを透明にして隠す
    /// </summary>
    void SecondHideUIs()
    {
        seekBarManager.AllSeekBarObjHide();

        for (int i = 0; i < skillUIs.Count; i++)
        {
            skillUIs[i].HideSkillUIs();
        }

        for (int i = 0; i < comboScripts.Count; i++)
        {
            comboScripts[i].HideComboText();
        }

        for (int i = 0; i < musicLevelUIs.Count; i++)
        {
            musicLevelUIs[i].HideLevelText();
        }

        for (int i = 0; i < scoreScript.Count; i++)
        {
            scoreScript[i].HideScoreUIs();
        }
    }
}
