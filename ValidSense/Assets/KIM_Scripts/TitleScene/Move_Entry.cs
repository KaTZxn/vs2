using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Entry : MonoBehaviour
{
    [SerializeField]
    float speed;

    [SerializeField]
    GameObject player1;

    [SerializeField]
    GameObject player2;

    [SerializeField]
    GameObject sceneManager;

    [SerializeField]
    private float inputStandbyTime;

    public bool player1_IsMove;

    public bool player1_EndMove;

    public bool player2_IsMove;

    public bool player2_EndMove;

    bool canPlayer_Input;

    /// <summary>
    /// シーンチェンジUIをインさせる用
    /// </summary>
    [SerializeField]
    SceneChangeUI sceneChangeUI;

    /// <summary>
    /// アニメーションをワンショット用
    /// </summary>
    private bool isSceneChangeUI_End;

    /// <summary>
    /// チュートリアルアテンドUIマネージャー
    /// </summary>
    [SerializeField]
    TutorialAtendUIManager tutorialAttend;

    /// <summary>
    /// プレイヤーのインプット
    /// </summary>
    [SerializeField]
    Title_PlayerInput title_PlayerInput;

    /// <summary>
    /// タイマーリセット用
    /// </summary>
    [SerializeField]
    TitleTimer titleTimer;


    int playMusicNum = 2;

    // Start is called before the first frame update
    void Start()
    {
        // 初期値
        isSceneChangeUI_End = false;


        sceneManager = GameObject.Find("SceneManager");


        canPlayer_Input = false;

        Invoke("Ready_Input", inputStandbyTime);

        player1_IsMove = false;

        player1_EndMove = false;

        player2_IsMove = false;

        player2_EndMove = false;

        // 選択している楽曲レベルをリセット
        sceneManager.GetComponent<PlayerManagerScript>().nowMusicLevel[0] = 0;
        sceneManager.GetComponent<PlayerManagerScript>().nowMusicLevel[1] = 0;

        //// BGM再生
        //MainBGMPlayer.holdNotesSEPlayer.MusicPlay(2);
    }

    // Update is called once per frame
    void Update()
    {
        //1pがボタンを押すとスクリプトが作動する
        if (canPlayer_Input && !player1_EndMove)
        {
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.R)
                || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.F))
            {
                player1_IsMove = true;
                player1_EndMove = true;
                SEPlayer.instance.SEOneShot(6);
                Invoke("MusicChange_num1",0.8f);
            }
        }
        //エントリー画面がspeed*時間の速度だけ下がる
        if (player1.transform.localPosition.y >= -50 && player1_IsMove)
        {
            player1.transform.localPosition -= new Vector3(0, speed * Time.deltaTime, 0);
        }

        //エントリーが画面外に出ないように調整
        else if (player1_IsMove)
        {
            player1_IsMove = false;
            player1.transform.localPosition = new Vector3(player1.transform.localPosition.x, 5, 0);
        }
        /*
        if (Input.GetKeyDown(KeyCode.W))
        {
            player1_EndMove = true;
        }
        if (player1.transform.localPosition.y < 1038 && player1_EndMove)
        {
            player1.transform.localPosition += new Vector3(0, speed * Time.deltaTime, 0);
        }
        else if (player1_EndMove)
        {
            player1_EndMove = false;
            player1.transform.localPosition = new Vector3(player1.transform.localPosition.x, 1038, 0);
        }
        */
        //2P
        if (canPlayer_Input && !player2_EndMove)
        {
            if (Input.GetKeyDown(KeyCode.U) || Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.P)
                || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.L))
            {
                player2_IsMove = true;
                player2_EndMove = true;
                SEPlayer.instance.SEOneShot(6);
                Invoke("MusicChange_num1", 0.8f);
            }
        }
        if (player2.transform.localPosition.y >= -50 && player2_IsMove)
        {
            player2.transform.localPosition -= new Vector3(0, speed * Time.deltaTime, 0);
        }
        else if (player2_IsMove)
        {
            player2_IsMove = false;
            player2.transform.localPosition = new Vector3(player2.transform.localPosition.x, 5.27f, 0);
        }
        /*
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            player2_EndMove = true;
        }
        if (player2.transform.localPosition.y < 1060 && player2_EndMove)
        {
            player2.transform.localPosition += new Vector3(0, speed * Time.deltaTime, 0);
        }
        else if (player2_EndMove)
        {
            player2_EndMove = false;
            player2.transform.localPosition = new Vector3(player2.transform.localPosition.x, 1060, 0);
        }
        */
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            ResetUI_Input();
        }


        if (player1_EndMove && player2_EndMove)
        {
            // エンドアニメーションをまだ再生してなかったら
            if(!isSceneChangeUI_End)
            {
                // タイマーを初期値にセット
                titleTimer.SWTimerReset();

                // エンドアニメーション再生
                Invoke("SceneChangeUI_PlayEndAnim",1.3f);

                // エンドアニメーションを再生済み
                isSceneChangeUI_End = true;

            }
            //Invoke("ToCharaSelectScene", 1.5f);
        }

    }


    void SceneChangeUI_PlayEndAnim()
    {
        sceneChangeUI.SceneEndAnim();
        tutorialAttend.FadeIn();
        StartCoroutine( title_PlayerInput.SetTrue_CanPlayerInput());

        
    }


    void MusicChange_num1()
    {
        if(playMusicNum != 1)
        {

            playMusicNum = 1;

            // BGMの切り替え
            MainBGMPlayer.instance.MusicPlay(playMusicNum);
        }

    }

    void MusicChange_num2()
    {
        if (playMusicNum != 2)
        {

            playMusicNum = 2;

            // BGMの切り替え
            MainBGMPlayer.instance.MusicPlay(playMusicNum);
        }
    }

    public void Ready_Input()
    {
        canPlayer_Input = true;
    }




    public void ToCharaSelectScene()
    {
        
        sceneManager.GetComponent<Test>().ToCharaSelectScene();
    }


    /// <summary>
    /// UIと、フラグをリセットする
    /// </summary>
    public void ResetUI_Input()
    {

        // BGMを最初に戻す
        MusicChange_num2();

        isSceneChangeUI_End = false;

        player1_IsMove = false;
        player2_IsMove = false;


        player1_EndMove = false;
        player2_EndMove = false;

        player1.transform.localPosition = new Vector3(player1.transform.localPosition.x, 1113, 0);
        player2.transform.localPosition = new Vector3(player2.transform.localPosition.x, 1113, 0);
    }

    private void OnDestroy()
    {
        MainBGMPlayer.instance.StopPlayer();
    }
}

