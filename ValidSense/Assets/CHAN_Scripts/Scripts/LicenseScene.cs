using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LicenseScene : MonoBehaviour
{
    ///// <summary>
    ///// 表示するオブジェクト
    ///// </summary>
    //public GameObject criware,otoLogic,QR;

    ///// <summary>
    ///// シーン遷移許可
    ///// </summary>
    //bool canToNextScene = false;

    /// <summary>
    /// キャラボイスを再生用
    /// </summary>
    [SerializeField]
    AllCharaVoicePlayer allCharaVoicePlayer;

    /// <summary>
    /// 再生するボイスを設定用
    /// </summary>
    private int playVoiceNum;

    /// <summary>
    /// プレイヤーのステータスを参照する用
    /// </summary>
    [SerializeField]
    PlayerManagerScript playerManagerScript;

    /// <summary>
    /// シーン遷移直前に再生する演出
    /// </summary>
    [SerializeField]
    SceneChangeUI sceneChangeUI;

    /// <summary>
    /// 譜面製作ツールのライセンステキスト表記
    /// </summary>
    [SerializeField]
    Text tool_License;

    /// <summary>
    /// BGM SEのライセンステキスト表記
    /// </summary>
    [SerializeField]
    Text se_BGM_License;

    /// <summary>
    /// CriWareの表記画像
    /// </summary>
    [SerializeField]
    Image criWare_Logo;

    /// <summary>
    /// CriWareのライセンステキスト表記
    /// </summary>
    [SerializeField]
    Text criWare_License;

    /// <summary>
    /// cubism_Logの表記画像
    /// </summary>
    [SerializeField]
    Image cubism_Logo;

    /// <summary>
    /// live2D_Logoの表記画像
    /// </summary>
    [SerializeField]
    Image live2D_Logo;

    /// <summary>
    /// フェードのスピード
    /// </summary>
    [SerializeField]
    float fadeSppd;
    
    /// <summary>
    /// フェードアウトまでの時間
    /// </summary>
    [SerializeField]
    float showUITime;

    // Start is called before the first frame update
    void Start()
    {
        // プレイヤーインプットの取得
        playerManagerScript = GameObject.Find("SceneManager").GetComponent<PlayerManagerScript>();

        ////表示の切り替え
        //criware.SetActive(true);
        //otoLogic.SetActive(false);
        //QR.SetActive(false);
        
        // 初期値表示
        AddLicense_FirstUI_Color(new Color(0f, 0f, 0f, -1));
        AddLicense_SecondUI_Color(new Color(0f, 0f, 0f, -1));


        //// シーン遷移直前アニメーションを再生する
        //Invoke("PlayScnenChangeAnim", 1.2f);

        ////シーン遷移
        //Invoke("ChangeScene", 1.6f);

        // UIを更新し続ける
        StartCoroutine(License_FirstUI_FadeIn());

        ////次の表示の表示の切り替え読み込み
        //Invoke("Change",1.5f);

        Invoke("WinCharaVoicePlay",0.8f);
    }


    //private void Change()
    //{
    //    //表示の切り替え
    //    criware.SetActive(false);
    //    otoLogic.SetActive(true);
    //    QR.SetActive(false);
    //    //Invoke("ShowQR", 3.2f);
        


    //    //canToNextScene = true;
    //}

    //void ShowQR()
    //{
    //    // 勝利プレイヤーキャラのアンケート協力ボイス再生
    //    allCharaVoicePlayer.OnShot_CharaVoice(playerManagerScript.winCharaNum, 4);

    //    criware.SetActive(false);
    //    otoLogic.SetActive(false);
    //    QR.SetActive(true);

    //    canToNextScene = true;
    //}

    //private void Update()
    //{
    //    if(canToNextScene)
    //    {
    //        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R) ||
    //            Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.F))
    //        {
    //            if (Input.GetKey(KeyCode.P) || Input.GetKey(KeyCode.O) || Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.U) ||
    //                Input.GetKey(KeyCode.L) || Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.J))
    //            {
    //                canToNextScene = false;
    //                // 勝利プレイヤーのキャラの感謝ボイス再生
    //                allCharaVoicePlayer.OnShot_CharaVoice(playerManagerScript.winCharaNum, 5);

    //                Invoke("ChangeScene", 3f);
    //            }
    //        }

    //        if (Input.GetKey(KeyCode.P) || Input.GetKey(KeyCode.O) || Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.U) ||
    //            Input.GetKey(KeyCode.L) || Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.J))
    //        {
    //            if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R) ||
    //                Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.F))
    //            {
    //                canToNextScene = false;
    //                // 勝利プレイヤーのキャラの感謝ボイス再生
    //                allCharaVoicePlayer.OnShot_CharaVoice(playerManagerScript.winCharaNum, 5);

    //                Invoke("ChangeScene", 3f);
    //            }
    //        }

    //    }




    //}


    /// <summary>
    /// シーン遷移直前に再生されるアニメーション
    /// </summary>
    private void PlayScnenChangeAnim()
    {
        sceneChangeUI.SceneEndAnim();
    }

    /// <summary>
    /// シーンを次にする
    /// </summary>
    private void ChangeScene()
    {
        GameObject.Find("SceneManager").SendMessage("SetScene", GameScene.Title);
    }

    /// <summary>
    /// 最初に表示されるUiの色をいじる用
    /// </summary>
    /// <param name="addColor"></param>
    void AddLicense_FirstUI_Color(Color addColor)
    {
        criWare_License.color += addColor;
        criWare_Logo.color += addColor;
        cubism_Logo.color += addColor;
        live2D_Logo.color += addColor;
    }

    /// <summary>
    /// 二回目にに表示されるUiの色をいじる用
    /// </summary>
    /// <param name="addColor"></param>
    void AddLicense_SecondUI_Color(Color addColor)
    {
        tool_License.color += addColor;
        se_BGM_License.color += addColor;
    }

    /// <summary>
    /// 最初に表示されるUIのフェードイン
    /// </summary>
    /// <returns></returns>
    IEnumerator License_FirstUI_FadeIn()
    {
        yield return new WaitForSeconds(0.8f);

        while (criWare_License.color.a < 1)
        {
            yield return null;

            AddLicense_FirstUI_Color(new Color(0f, 0f, 0f, Time.deltaTime * fadeSppd));
        }

        yield return new WaitForSeconds(showUITime);

        StartCoroutine(License_FirstUI_FadeIOut());
    }

    /// <summary>
    /// 最初に表示されるUIのフェードフェードアウト
    /// </summary>
    /// <returns></returns>
    IEnumerator License_FirstUI_FadeIOut()
    {

        while (criWare_License.color.a > 0)
        {
            yield return null;

            AddLicense_FirstUI_Color(new Color(0f, 0f, 0f, -Time.deltaTime * fadeSppd));
        }

        StartCoroutine(License_SecondUI_FadeIn());
    }

    /// <summary>
    /// 二回目に表示されるUIのフェードイン
    /// </summary>
    /// <returns></returns>
    IEnumerator License_SecondUI_FadeIn()
    {
        

        while (tool_License.color.a < 1)
        {
            yield return null;

            AddLicense_SecondUI_Color(new Color(0f, 0f, 0f, Time.deltaTime * fadeSppd));
        }

        yield return new WaitForSeconds(showUITime);

        StartCoroutine(License_SecondUI_FadeIOut());
    }

    /// <summary>
    /// 二回目に表示されるUIのフェードアウト
    /// </summary>
    /// <returns></returns>
    IEnumerator License_SecondUI_FadeIOut()
    {

        while (tool_License.color.a > 0)
        {
            yield return null;

            AddLicense_SecondUI_Color(new Color(0f, 0f, 0f, -Time.deltaTime * fadeSppd));
        }

        // シーン遷移直前アニメーションを再生する
        Invoke("PlayScnenChangeAnim", 0.2f);

        //シーン遷移
        Invoke("ChangeScene", 1.0f);
    }

    void WinCharaVoicePlay()
    {

        // winキャラNumがキャラの配列を超えていなければ
        if (playerManagerScript.winCharaNum <= (int)ConstRepo.Chara.Hear)
        {
            // ボイスを再生キャラを決定
            playVoiceNum = playerManagerScript.winCharaNum;
        }
        else
        {
            // 1p 2pでランダムで表示キャラを決める
            playVoiceNum = Random.Range((int)ConstRepo.Player.P1, (int)ConstRepo.Player.P2 + 1);
        }

        // 勝利プレイヤーのキャラの感謝ボイス再生
        allCharaVoicePlayer.OnShot_CharaVoice(playVoiceNum, 5);
    }
}
