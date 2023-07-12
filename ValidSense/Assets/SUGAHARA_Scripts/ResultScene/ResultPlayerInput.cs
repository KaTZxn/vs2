using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultPlayerInput : MonoBehaviour
{
    /// <summary>
    /// プレイヤーのステータスを保管している所干渉する用
    /// </summary>
    [SerializeField]
    PlayerManagerScript sceneManager;

    /// <summary>
    /// ドロウ演出と、フルコンボ演出のアニメーション
    /// </summary>
    [SerializeField]
    Animator drawFullcombo_Anim;

    /// <summary>
    /// 自動で遷移するときに使用する、値
    /// </summary>
    [SerializeField]
    private float autoToNextSceneTime;

    /// <summary>
    /// 次のシーンに行く許可
    /// </summary>
    bool canToNextScene;

    /// <summary>
    /// スクリプトの使用プレイヤー
    /// </summary>
    [SerializeField]
    int usePlayer;

    /// <summary>
    /// ドロウ演出と、フルコンボ演出の
    /// アニメーションを何秒待ってから再生するかの値
    /// </summary>
    [SerializeField]
    float drawFullcombo_Anim_StanbyTime;

    /// <summary>
    /// シーンを遷移させるときに使用する、UI
    /// </summary>
    [SerializeField]
    SceneChangeUI sceneChangeUI;

    void Start()
    {
        // シーンマネージャーのプレイヤー周りの値を保持しているスクリプト参照する用
        sceneManager = GameObject.Find("SceneManager").GetComponentInParent<PlayerManagerScript>();

        // 同じループ内でリザルト画面の表示が初回なら
        if(sceneManager.GetComponent<PlayerManagerScript>().resultCount == 0)
        {
            // リザルトBGMを再生
            ResultBGMPlayer.instance.MusicPlay(1);
        }

        // 同点だったら
        if (sceneManager.score[(int)ConstRepo.Player.P1]
            == sceneManager.score[(int)ConstRepo.Player.P2])
        {
            // プレイヤーのキャラにあったDRAWアニメーションを再生
            StartCoroutine(DrawAnum_Play(sceneManager.playerCharaNum[usePlayer]));


            // この数字で、タイトルシーンの色を決めるので
            // キャラの番号以上の数字で、灰色指定している
            sceneManager.winCharaNum = 4;
        }
        // 先にリザルト画面が表示された方
        else if (sceneManager.resultCount == 0)
        {
            // リザルトBGMを再生
            ResultBGMPlayer.instance.MusicPlay(1);

            // 勝った方のキャラ番号を記憶
            sceneManager.winCharaNum = sceneManager.playerCharaNum[usePlayer];
        }



        // 同点じゃなかい
        // かつ、フルコンボだったら、
        if (sceneManager.score[(int)ConstRepo.Player.P1]
            != sceneManager.score[(int)ConstRepo.Player.P2]
            &&
            sceneManager.isPlayerFullcombo[usePlayer])
        {
            // フルコンボアニメーションを再生する
            StartCoroutine(FullcomboAnum_Play(sceneManager.playerCharaNum[usePlayer]));
        }


            // 0.5秒でタイトルに行くのを許可する
            Invoke("OnCanToTitle",0.5f);


        // autoToTitleTime秒経過後に適切なシーンに遷移をする
        StartCoroutine(AutoToTitleScene());
    }

    // Update is called once per frame
    void Update()
    {
        // プレイヤーの入力を許可する
        if(canToNextScene)
        {
            // 設定した値で、プレイヤーの入力を変える
            if (usePlayer == (int)ConstRepo.Player.P1)
            {
                // 1Pの入力
                if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.R) ||
                    Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.F))
                {
                    // シーン遷移先が決定したため、許可を閉じる
                    canToNextScene = false;

                    // 項目決定SEを再生
                    SEPlayer.instance.SEOneShot(6);

                    // シーン遷移する前に遷移前アニメーションを再生
                    sceneChangeUI.SceneEndAnim();

                    // シーン遷移
                    Invoke("ToNextScene", 0.7f);
                }
            }
            // 2Pの入力
            else
            {
                if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.U) ||
                    Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.J))
                {
                    // シーン遷移先が決定したため、許可を閉じる
                    canToNextScene = false;

                    SEPlayer.instance.SEOneShot(6);

                    // シーン遷移する前に遷移前アニメーションを再生
                    sceneChangeUI.SceneEndAnim();

                    // シーン遷移
                    Invoke("ToNextScene", 0.7f);
                }
            }

        }
    }


    /// <summary>
    /// シーン遷移を読み込むコールチン
    /// </summary>
    /// <returns></returns>
    IEnumerator AutoToTitleScene()
    {
        yield return new WaitForSeconds(autoToNextSceneTime - 0.7f);

        // シーン遷移先が決定したため、許可を閉じる
        canToNextScene = false;

        // シーン遷移する前に遷移前アニメーションを再生
        sceneChangeUI.SceneEndAnim();

        yield return new WaitForSeconds(0.7f);

        // シーン遷移
        ToNextScene();
    }

    /// <summary>
    /// 次のシーンに行くことを許可する
    /// </summary>
    private void OnCanToTitle()
    {
        canToNextScene = true;
    }


    /// <summary>
    /// 次のシーンを選択し、遷移する
    /// </summary>
    void ToNextScene()
    {
        // まだ負けたほうのリザルト画面が表示されていなかったら
        if (sceneManager.GetComponent<PlayerManagerScript>().resultCount == 0)
        {
            // リザルトの表示回数をプラスする
            sceneManager.GetComponent<PlayerManagerScript>().resultCount++;

            if(usePlayer == 0)
            {
                sceneManager.GetComponent<Test>().ToResult_2P_Scene();
            }
            else
            {
                sceneManager.GetComponent<Test>().ToResult_1P_Scene();
            }
        }
        // 両プレイヤーのリザルト画面が表示されたら
        else
        {
            sceneManager.GetComponent<PlayerManagerScript>().resultCount = 0;
            ResultBGMPlayer.instance.StopPlayer();
            sceneManager.GetComponent<Test>().ToLicenseScene() ;
        }
    }

    /// <summary>
    /// 使用キャラに合った、ドロウ演出再生
    /// </summary>
    /// <param name="charaNum"></param>
    /// <returns></returns>
    IEnumerator DrawAnum_Play(int charaNum)
    {
        yield return new WaitForSeconds(drawFullcombo_Anim_StanbyTime);

        switch(charaNum)
        {
            case (int)ConstRepo.Chara.Sight:
                drawFullcombo_Anim.SetTrigger("Darw_S");
                break;

            case (int)ConstRepo.Chara.Tactile:
                drawFullcombo_Anim.SetTrigger("Darw_T");
                break;

            case (int)ConstRepo.Chara.Smell_Taste:
                drawFullcombo_Anim.SetTrigger("Darw_ST");
                break;

            case (int)ConstRepo.Chara.Hear:
                drawFullcombo_Anim.SetTrigger("Darw_H");
                break;
        }
    }

    /// <summary>
    /// 使用キャラに合った、フルコンボ演出再生
    /// </summary>
    /// <param name="charaNum"></param>
    /// <returns></returns>
    IEnumerator FullcomboAnum_Play(int charaNum)
    {
        yield return new WaitForSeconds(drawFullcombo_Anim_StanbyTime);

        switch (charaNum)
        {
            case (int)ConstRepo.Chara.Sight:
                drawFullcombo_Anim.SetTrigger("Fullcombo_S");
                break;

            case (int)ConstRepo.Chara.Tactile:
                drawFullcombo_Anim.SetTrigger("Fullcombo_T");
                break;

            case (int)ConstRepo.Chara.Smell_Taste:
                drawFullcombo_Anim.SetTrigger("Fullcombo_ST");
                break;

            case (int)ConstRepo.Chara.Hear:
                drawFullcombo_Anim.SetTrigger("Fullcombo_H");
                break;
        }
    }
    
}
