using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title_PlayerInput : MonoBehaviour
{
    /// <summary>
    /// シーンマネージャー参照する用
    /// </summary>
    [SerializeField]
    GameObject sceneManagerTest;

    /// <summary>
    /// ボイス再生用スクリプト参照する用
    /// </summary>
    [SerializeField]
    AllCharaVoicePlayer allVoicePlayer;

    /// <summary>
    /// 次のシーンに遷移するまでの時間
    /// </summary>
    [SerializeField]
    private float _toNextSceneTime;

    /// <summary>
    /// 音声再生までの時間
    /// </summary>
    [SerializeField]
    private float voiceStandbyTime;

    /// <summary>
    /// プレイヤーの入力を許可する
    /// </summary>
    private bool _canPlayerInput = false;

    /// <summary>
    /// キャラ選択画面に遷移するかどうか
    /// </summary>
    private bool toCharaSelectScene = false;

    /// <summary>
    /// チュートリアルのアテンドを管理しているオブジェクトに干渉する用
    /// </summary>
    [SerializeField]
    TutorialAtendUIManager tutorialAtendUIManager;

    /// <summary>
    /// シーン遷移直前に再生するアニメーション
    /// </summary>
    [SerializeField]
    SceneChangeUI sceneChangeUIUpper;



    private void Start()
    {
        // シーンマネージャーを参照する用
        sceneManagerTest = GameObject.Find("SceneManager");

        // ウェルカムボイスを再生
        StartCoroutine(VoicePlay());

        //MainBGMPlayer.holdNotesSEPlayer.MusicPlay(0);

        var json = GameObject.Find("Json").GetComponent<JsonReader>();

        json.SendMessage("ChangeTutorialJson");
    }


    // Update is called once per frame
    void Update()
    {
        // プレイヤーの入力が許可されていたら
        if(_canPlayerInput)
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                // プレイヤーの入力を閉じる
                _canPlayerInput = false;

                //遷移先を決定
                toCharaSelectScene = false;
                // 遷移読み込み
                Invoke("ToNextScene", _toNextSceneTime);

                // チュートリアルアテンドUIをフェードアウトさせる
                tutorialAtendUIManager.FadeOut();

                // シーン遷移直前に再生するアニメーション
                sceneChangeUIUpper.SceneEndAnim();

                // 項目決定音を再生する
                SEPlayer.instance.SEOneShot(6);
            }
            else if(Input.GetKeyDown(KeyCode.P))
            {
                // プレイヤーの入力を閉じる
                _canPlayerInput = false;

                //遷移先を決定
                toCharaSelectScene = true;
                // 遷移読み込み
                Invoke("ToNextScene", _toNextSceneTime);

                // チュートリアルアテンドUIをフェードアウトさせる
                tutorialAtendUIManager.FadeOut();

                // シーン遷移直前に再生するアニメーション
                sceneChangeUIUpper.SceneEndAnim();

                // 項目決定音を再生する
                SEPlayer.instance.SEOneShot(6);
            }
        }
    }

    /// <summary>
    /// プレイヤーの入力許可する
    /// </summary>
    public IEnumerator SetTrue_CanPlayerInput()
    {
        yield return new WaitForSeconds(1.3f);
        _canPlayerInput = true;
    }

    /// <summary>
    /// プレイヤーの入力許可を閉じる
    /// </summary>
    public void SetFalse_CanPlayerInput()
    {
        _canPlayerInput = false;
    }


    /// <summary>
    /// 次のシーンを選んで遷移
    /// </summary>
    void ToNextScene()
    {

        // キャラ選択画面遷移がtrueなら
        if(toCharaSelectScene)
        {
            // キャラ選択画面
            sceneManagerTest.GetComponent<Test>().ToCharaSelectScene();
        }
        else
        {
            // チュートリアルキャラのセット
            sceneManagerTest.GetComponent<PlayerManagerScript>()
                .playerCharaNum[(int)ConstRepo.Player.P1] = (int)ConstRepo.Chara.Sight ;
            sceneManagerTest.GetComponent<PlayerManagerScript>()
                .playerCharaNum[(int)ConstRepo.Player.P2] = (int)ConstRepo.Chara.Tactile;

            // チュートリアル画面
            sceneManagerTest.GetComponent<Test>().ToTutorialScene();
        }
    }


    /// <summary>
    /// ウェルカムボイス再生
    /// </summary>
    /// <returns></returns>
    IEnumerator VoicePlay ()
    {
        yield return  new WaitForSeconds(voiceStandbyTime);
        //勝利キャラのウェルカムボイスを再生
        allVoicePlayer.OnShot_CharaVoice(
        sceneManagerTest.GetComponent<PlayerManagerScript>().winCharaNum, 6);
    }
}
