using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene_Controller : MonoBehaviour
{
    /// <summary>
    /// BGM再生用
    /// </summary>
    [SerializeField]
    EndScene_Player ed_Player;

    /// <summary>
    /// キャラに干渉する用
    /// </summary>
    [SerializeField]
    List<GameObject> charaObj;

    /// <summary>
    /// プレイヤーのステータス参照する用
    /// </summary>
    PlayerManagerScript playerManagerScript;

    /// <summary>
    /// シーン遷移するためのスクリプト読み込み用
    /// </summary>
    Test sceneManager;

    /// <summary>
    /// ランダムな数値を使用する時の値を保存しておく用
    /// </summary>
    private int randNum;

    /// <summary>
    /// キャラのボイスを再生用
    /// </summary>
    [SerializeField]
    AllCharaVoicePlayer voicePlayer;

    /// <summary>
    /// どのキャラのボイスを再生するかの変数
    /// </summary>
    private int playVoiceNum;


    /// <summary>
    /// 次のシーンに行く許可
    /// </summary>
    private bool canToNextScene = false;

    /// <summary>
    /// シーン遷移直前アニメーションを再生する用
    /// </summary>
    [SerializeField]
    SceneChangeUI sceneChangeUI;

    // Start is called before the first frame update
    void Start()
    {
        // BGM再生
        ed_Player.BGOneShot(0);

        // プレイヤーの入力を許可を出す
        Invoke("SetTrue_CanToNextScene", 0.7f);

        // プレイヤーマネージャーを取得
        playerManagerScript = GameObject.Find("SceneManager").GetComponent<PlayerManagerScript>();

        // シーン遷移スクリプト取得
        sceneManager = GameObject.Find("SceneManager").GetComponent<Test>();


        // winキャラNumがキャラの配列を超えていなければ
        if (playerManagerScript.winCharaNum <= (int)ConstRepo.Chara.Hear)
        {
            // キャラの初期表示
            // キャラの配列分
            for (int i = 0; i < (int)ConstRepo.Chara.Hear + 1; i++)
            {
                // 勝利キャラなら、表示
                if( i == playerManagerScript.winCharaNum)
                {
                    charaObj[i].SetActive(true);
                }
                // 勝利キャラじゃなかったら、非表示
                else
                {
                    charaObj[i].SetActive(false);
                }
            }

            // ボイスを再生キャラを決定
            playVoiceNum = playerManagerScript.winCharaNum;
        }
        // winキャラNumがキャラの配列を超えてたら、同点だったため
        else
        {
            // 1p 2pでランダムで表示キャラを決める
            randNum = Random.Range((int)ConstRepo.Player.P1, (int)ConstRepo.Player.P2 + 1);

            // キャラの初期表示
            // キャラの配列分
            for (int i = 0; i < (int)ConstRepo.Chara.Hear + 1; i++)
            {
                // 勝利キャラなら、表示
                if (i == playerManagerScript.playerCharaNum[randNum])
                {
                    charaObj[i].SetActive(true);
                }
                // 勝利キャラじゃなかったら、非表示
                else
                {
                    charaObj[i].SetActive(false);
                }
            }


            // ボイスを再生キャラを決定
            playVoiceNum = playerManagerScript.playerCharaNum[randNum];
        }

        // オート
        Invoke("ToNextScene",0.9f);
    }

    // Update is called once per frame
    void Update()
    {
        //// 次のシーンに行く許可が出ていれば
        //if (canToNextScene)
        //{
        //    // 1Pメイン入力
        //    if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R) ||
        //        Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.F))
        //    {
        //        if (Input.GetKey(KeyCode.P) || Input.GetKey(KeyCode.O) || Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.U) ||
        //            Input.GetKey(KeyCode.L) || Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.J))
        //        {
        //            // 遷移が決定したため、許可を閉じる
        //            canToNextScene = false;

        //            // 勝利プレイヤーのキャラの感謝ボイス再生
        //            voicePlayer.OnShot_CharaVoice(playVoiceNum, 5);

        //            // 勝利プレイヤーのキャラのアニメーション再生
        //            charaObj[playVoiceNum].GetComponent<Live2D_AnimController>().Anim_Choice();

        //            // タイトル画面遷移を読み込み
        //            StartCoroutine(ToTitleScene());
        //        }
        //    }
        //    // 2Pメイン入力
        //    if (Input.GetKey(KeyCode.P) || Input.GetKey(KeyCode.O) || Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.U) ||
        //        Input.GetKey(KeyCode.L) || Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.J))
        //    {
        //        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R) ||
        //            Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.F))
        //        {
        //            // 遷移が決定したため、許可を閉じる
        //            canToNextScene = false;

        //            // 勝利プレイヤーのキャラの感謝ボイス再生
        //            voicePlayer.OnShot_CharaVoice(playVoiceNum, 5);

        //            // 勝利プレイヤーのキャラのアニメーション再生
        //            charaObj[playVoiceNum].GetComponent<Live2D_AnimController>().Anim_Choice();

        //            // タイトル画面遷移を読み込み
        //            StartCoroutine(ToTitleScene());
        //        }
        //    }

        //}
    }

    /// <summary>
    /// canToNextSceneをtrueにする
    /// </summary>
    void SetTrue_CanToNextScene()
    {
        canToNextScene = true;
    }

    void ToNextScene()
    {
        // 遷移が決定したため、許可を閉じる
        canToNextScene = false;

        // 勝利プレイヤーのキャラの感謝ボイス再生
        voicePlayer.OnShot_CharaVoice(playVoiceNum, 5);

        // 勝利プレイヤーのキャラのアニメーション再生
        charaObj[playVoiceNum].GetComponent<Live2D_AnimController>().Anim_Choice();

        // タイトル画面遷移を読み込み
        StartCoroutine(ToTitleScene());
    }




    IEnumerator ToTitleScene()
    {
        yield return new WaitForSeconds(4.2f);

        // 遷移直前アニメーションを再生する
        sceneChangeUI.SceneEndAnim();

        yield return new WaitForSeconds(0.7f);

        // BGMの停止
        ed_Player.BGStop();

        // タイトル画面に遷移
        sceneManager.ToTitleScene();
    }
}
