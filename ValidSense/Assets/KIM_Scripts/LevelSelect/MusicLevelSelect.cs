using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MusicLevelSelect : MonoBehaviour
{
    [SerializeField]
    BackGround background_sprite;

    [SerializeField]
    GameObject sceneManager;

    [SerializeField]
    PlayerManagerScript playerManager;

    [SerializeField]
    LevelUI_Num level_Num;

    [SerializeField]
    Timer timer;

    [SerializeField]
    private Vector3[] _cursorPosY_1P;
    [SerializeField]
    private Vector3[] _cursorPosY_2P;

    [SerializeField]
    private int _cursorPosNum_1P;

    [SerializeField]
    private int _cursorPosNum_2P;

    [SerializeField]
    public int levelMax;

    [SerializeField]
    private GameObject cursor_1p;

    [SerializeField]
    private GameObject cursor_2p;

    [SerializeField]
    public bool control_cursor;

    public bool decide_player1;

    public bool decide_player2;

    // if文の中身を一回のみ読み込むため
    private bool _onShot;


    [SerializeField]
    SceneChangeUI sceneChangeUI;

    // Start is called before the first frame update
    void Start()
    {
        // 値の初期化
        _onShot = true;


        MoveCursor1P(0);
        MoveCursor2P(0);

        decide_player1 = false;
        decide_player2 = false;
        control_cursor = true;

        level_Num = GameObject.Find("LevelText").GetComponent < LevelUI_Num > ();

        sceneManager = GameObject.Find("SceneManager");
        playerManager = GameObject.Find("SceneManager").GetComponent<PlayerManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //1p
        if (control_cursor)
        {
            //1Pの決定Keyと決定すると出る効果音
            if (Input.GetKeyDown(KeyCode.Q))
            {
                switch (_cursorPosNum_1P) {
                    case 0:
                        if (level_Num.NaturalNum == 0)
                        {
                            SEPlayer.instance.SEOneShot(4);
                            decide_player1 = false;
                        }
                        else
                        {
                            SEPlayer.instance.SEOneShot(6);
                            decide_player1 = true;
                        }
                        break;
                    case 1:
                        if (level_Num.HighSenseNum == 0)
                        {
                            SEPlayer.instance.SEOneShot(4);
                            decide_player1 = false;
                        }
                        else
                        {
                            SEPlayer.instance.SEOneShot(6);
                            decide_player1 = true;
                        }
                        break;
                    case 2:
                        if (level_Num.SixthSenseNum == 0)
                        {
                            SEPlayer.instance.SEOneShot(4);
                            decide_player1 = false;
                        }
                        else
                        {
                            SEPlayer.instance.SEOneShot(6);
                            decide_player1 = true;
                        }
                        break;

                }
                
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                SEPlayer.instance.SEOneShot(6);
                decide_player1 = false;
            }
            //1Pの決定Keyを押さなかった時のスクリプト
            if (!decide_player1)
            {
                //R,を押したら上に上がる
                if (Input.GetKeyDown(KeyCode.R))
                {
                    if (_cursorPosNum_1P > 0)
                    {
                        _cursorPosNum_1P--;
                    }
                    else if (_cursorPosNum_1P <= 0)
                    {
                        _cursorPosNum_1P = (levelMax - 1);
                    }
                    MoveCursor1P(_cursorPosNum_1P);
                    SEPlayer.instance.SEOneShot(5);
                }
                //Fを押したら下がる
                else if (Input.GetKeyDown(KeyCode.F))
                {
                    _cursorPosNum_1P = (_cursorPosNum_1P + 1) % levelMax;

                    MoveCursor1P(_cursorPosNum_1P);
                    SEPlayer.instance.SEOneShot(5);
                }
            }
        }

        //2p
        if (control_cursor)
        {
            //2Pの決定Keyと決定すると出る効果音
            if (Input.GetKeyDown(KeyCode.P))
            {
                switch (_cursorPosNum_2P)
                {
                    case 0:
                        if (level_Num.NaturalNum == 0)
                        {
                            SEPlayer.instance.SEOneShot(4);
                            decide_player2 = false;
                        }
                        else
                        {
                            SEPlayer.instance.SEOneShot(6);
                            decide_player2 = true;
                        }
                        break;
                    case 1:
                        if (level_Num.HighSenseNum == 0)
                        {
                            SEPlayer.instance.SEOneShot(4);
                            decide_player2 = false;
                        }
                        else
                        {
                            SEPlayer.instance.SEOneShot(6);
                            decide_player2 = true;
                        }
                        break;
                    case 2:
                        if (level_Num.SixthSenseNum == 0)
                        {
                            SEPlayer.instance.SEOneShot(4);
                            decide_player2 = false;
                        }
                        else
                        {
                            SEPlayer.instance.SEOneShot(6);
                            decide_player2 = true;
                        }
                        break;
                }
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                SEPlayer.instance.SEOneShot(6);
                decide_player2 = false;
            }
            //2Pの決定Keyを押さなかった時のスクリプト
            if (!decide_player2)
            {
                //Uを押したら上に上がる
                if (Input.GetKeyDown(KeyCode.U))
                {
                    if (_cursorPosNum_2P > 0)
                    {
                        _cursorPosNum_2P--;
                    }
                    else if (_cursorPosNum_2P <= 0)
                    {
                        _cursorPosNum_2P = (levelMax - 1);
                    }
                    MoveCursor2P(_cursorPosNum_2P);
                    SEPlayer.instance.SEOneShot(5);
                }
                //Jを押したら下がる
                else if (Input.GetKeyDown(KeyCode.J))
                {
                    _cursorPosNum_2P = (_cursorPosNum_2P + 1) % levelMax;

                    MoveCursor2P(_cursorPosNum_2P);
                    SEPlayer.instance.SEOneShot(5);
                }
            }
        }


        if (decide_player1 && decide_player2 && _onShot)
        {
            // 一回のみ読み込まれるようにする
            _onShot = false;

            SEPlayer.instance.SEOneShot(7);

            Set_NowMusicLevel();

            timer.can_count = false;

            background_sprite.NextSprite();


            Invoke("SceneChangeAnim", 2.5f);

            Invoke("ToMainScene", 3.2f);

            control_cursor = false;
            
        }
    }

    void Set_NowMusicLevel()
    {

        playerManager.nowMusicLevel[(int)ConstRepo.Player.P1] = _cursorPosNum_1P;
        playerManager.nowMusicLevel[(int)ConstRepo.Player.P2] = _cursorPosNum_2P;
    }

    /// <summary>
    /// タイムアップ時のセットアップ
    /// </summary>
    public void TimeUpSet_NowMusicLevel()
    {
        // 1pカーソル位置に対応するレベルが0なら、ニュートラルをセット
        // 0じゃなかったら、カーソル位置に対応するレベルをセット
        switch (_cursorPosNum_1P)
        {
            case 0:
                if (level_Num.NaturalNum == 0)
                {
                    playerManager.nowMusicLevel[(int)ConstRepo.Player.P1] = 0;
                }
                else
                {
                    playerManager.nowMusicLevel[(int)ConstRepo.Player.P1] = _cursorPosNum_1P;
                }
                break;
            case 1:
                if (level_Num.HighSenseNum == 0)
                {
                    playerManager.nowMusicLevel[(int)ConstRepo.Player.P1] = 0;
                }
                else
                {
                    playerManager.nowMusicLevel[(int)ConstRepo.Player.P1] = _cursorPosNum_1P;
                }
                break;
            case 2:
                if (level_Num.SixthSenseNum == 0)
                {
                    playerManager.nowMusicLevel[(int)ConstRepo.Player.P1] = 0;
                }
                else
                {
                    playerManager.nowMusicLevel[(int)ConstRepo.Player.P1] = _cursorPosNum_1P;
                }
                break;

        }


        // 2pカーソル位置に対応するレベルが0なら、ニュートラルをセット
        // 0じゃなかったら、カーソル位置に対応するレベルをセット
        switch (_cursorPosNum_2P)
        {
            case 0:
                if (level_Num.NaturalNum == 0)
                {
                    playerManager.nowMusicLevel[(int)ConstRepo.Player.P2] = 0;
                }
                else
                {
                    playerManager.nowMusicLevel[(int)ConstRepo.Player.P2] = _cursorPosNum_2P;
                }
                break;
            case 1:
                if (level_Num.HighSenseNum == 0)
                {
                    playerManager.nowMusicLevel[(int)ConstRepo.Player.P2] = 0;
                }
                else
                {
                    playerManager.nowMusicLevel[(int)ConstRepo.Player.P2] = _cursorPosNum_2P;
                }
                break;
            case 2:
                if (level_Num.SixthSenseNum == 0)
                {
                    playerManager.nowMusicLevel[(int)ConstRepo.Player.P2] = 0;
                }
                else
                {
                    playerManager.nowMusicLevel[(int)ConstRepo.Player.P2] = _cursorPosNum_2P;
                }
                break;

        }

    }

    private void MoveCursor1P(int posNum)
    {
        cursor_1p.transform.position = _cursorPosY_1P[posNum];
    }

    private void MoveCursor2P(int posNum)
    {
        cursor_2p.transform.position = _cursorPosY_2P[posNum];
    }

    /// <summary>
    /// シーン切り替え前の演出の再生
    /// </summary>
    void SceneChangeAnim()
    {
        sceneChangeUI.SceneEndAnim();
    }

    //メインシーン読み込み関数
    public void ToMainScene()
    {
        //メインシーン読み込み
        sceneManager.GetComponent<Test>().ToMainScene();
    }

}
