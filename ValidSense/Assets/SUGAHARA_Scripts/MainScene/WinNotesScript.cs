using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinNotesScript : MonoBehaviour
{
    /// <summary>
    /// フェードアウト用に自身のspriteを取得用
    /// </summary>
    private SpriteRenderer _sprite;


    /// <summary>
    /// 60fpsで落下する速度
    /// </summary>
    public float notesSpeed;

    /// <summary>
    /// 判定基準時間
    /// </summary>
    public float notesJudgeTime;

    /// <summary>
    /// 判定を許可する時間（判定基準時間の前後）
    /// </summary>
    [SerializeField]
    private float canJudge_WithinTime;

    /// <summary>
    /// _activeがtrueになってからの経過時間
    /// </summary>
    [SerializeField]
    private float _activeTime;

    /// <summary>
    /// オブジェクトの活動開始
    /// </summary>
    [SerializeField]
    private bool _active;


    /// <summary>
    /// プレイヤーの入力の制御
    /// </summary>
    [SerializeField]
    private bool _canPlayerInput;

    /// <summary>
    /// 使用者明記（入力の変更に使用）
    /// </summary>
    public int usePlayer;

    /// <summary>
    /// エフェクト再生するように参照する
    /// </summary>
    NotesJudge notesJudge;


    private void Start()
    {
        // フェードアウト用に自身の_spriteを取得
        _sprite = GetComponent<SpriteRenderer>();

        // エフェクト再生するように参照する
        notesJudge = GameObject.Find("NotesManager").GetComponent<NotesJudge>();
    }

    

    /// <summary>
    /// 60fpsで落下する速度と、判定基準時間の設定と、アクティブON
    /// </summary>
    /// <param name="speed"></param>
    public void NotesSpeed_TIme_UsePlayer_Setter(float speed, float judgeTime, int playerNum)
    {
        // 使用者を指定
        usePlayer = playerNum;

        // 60fpsで落下する速度
        notesSpeed = speed;

        // 判定基準時間
        notesJudgeTime = judgeTime;

        // アクティブON
        _active = true;
        // 入力ON
        _canPlayerInput = true;
    }

 


    // 60fps
    void FixedUpdate()
    {
        // オブジェクトがアクティブになったら
        if (_active)
        {
            //  アクティブになってからの時間のカウント
            _activeTime += Time.deltaTime;

            // 60fpsでspeed分落下させる
            transform.position -= new Vector3(0, notesSpeed, 0);
        }
    }


    private void Update()
    {
        //// デバック用
        //if (notesJudgeTime -_activeTime  < 0)
        //{
        //    Debug.Log("WinNotes"+ (_activeTime - notesJudgeTime));
        //    _active = false;
        //}


        // もし、判定基準時間 + 判定可能時間 よりアクティブ時間が多くなったら自動フェードアウト
        if (notesJudgeTime + canJudge_WithinTime < _activeTime && _canPlayerInput)
        {
            //Debug.Log("WinNotes未入力");
            // 退場周りの処理
            NotesFadeOut();
        }


        //  判定可能時間範囲内になったら
        // かつ、入力が許可されていたら
        if (Mathf.Abs(_activeTime - notesJudgeTime) <= canJudge_WithinTime && _canPlayerInput)
        {

            // 使用者が1pだったら
            if (usePlayer == (int)ConstRepo.Player.P1)
            {
                // 1Pの全パネルインプットのどれでも反応
                if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.R)
                   || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.F))
                {
                    // リンクノーツの音再生
                    SEPlayer.instance.SEOneShot(3);

                    // 自分レーンにブリリアントの、テキスト以外のエフェクトを再生
                    for (int i = 0; i < 4; i++)
                    {

                        //                         (int)JudgeType.Briliant,  Line,  Color(ブリリアントは、設定しなくてOK)
                        notesJudge.NotesJudgeEffect(0, i, 0);
                    }

                    //Debug.Log("WinNotes入力");
                    // 退場周りの処理
                    NotesFadeOut();
                }
            }
            // 使用者が2pだったら
            else
            {
                // 2Pの全パネルインプットのどれでも反応
                if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.U)
                   || Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.J))
                {
                    // リンクノーツの音再生
                    SEPlayer.instance.SEOneShot(3);

                    // 自分レーンにブリリアントの、テキスト以外のエフェクトを再生
                    for (int i = 4; i < 8; i++)
                    {

                        //                         (int)JudgeType.Briliant,  Line,  Color(ブリリアントは、設定しなくてOK)
                        notesJudge.NotesJudgeEffect(0, i, 0);
                    }

                    //Debug.Log("WinNotes入力");
                    // 退場周りの処理
                    NotesFadeOut();
                }
            }
        }

    }




    /// <summary>
    /// コールチンの関数を利用して、フェードアウトして、非アクティブ化する
    /// </summary>
    public void NotesFadeOut()
    {
        // 入力をはじく
        _canPlayerInput = false;

        // フェードアウトを実行
        StartCoroutine(FadeOut());
    }


    /// <summary>
    /// 自身をフェードアウトして、非アクティブ化する
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeOut()
    {
        _sprite.color -= new Color(0f, 0f, 0f, 0.2f);
        yield return new WaitForSeconds(0.03f);

        _sprite.color -= new Color(0f, 0f, 0f, 0.3f);
        yield return new WaitForSeconds(0.03f);

        _sprite.color -= new Color(0f, 0f, 0f, 0.3f);
        yield return new WaitForSeconds(0.02f);

        this.gameObject.SetActive(false);
    }
}
