using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekBarManager : MonoBehaviour
{
    /// <summary>
    ///  seek_BarPointEffectに参照する用
    /// </summary>
    [SerializeField]
    SpriteRenderer seek_BarPointEffect;

    /// <summary>
    ///  seek_BarPointに参照する用
    /// </summary>
    [SerializeField]
    GameObject seek_BarPoint;

    /// <summary>
    ///  BarStart(1P)に参照する用
    /// </summary>
    [SerializeField]
    SpriteRenderer seek_BarStart;

    /// <summary>
    ///  BarEnd(2P)に参照する用
    /// </summary>
    [SerializeField]
    SpriteRenderer seek_BarEnd;


    /// <summary>
    /// アルファ値代入用
    /// </summary>
    [SerializeField]
    float alphaValueMax;

    /// <summary>
    /// アルファの最大値最小値カウント用
    /// </summary>
    [SerializeField]
    int alpha_MinMaxCount;

    /// <summary>
    /// バーを染めるためのカラー
    /// </summary>
    [SerializeField]
    string[] barColorCode;

    /// <summary>
    /// プレイヤーマネージャー取得する用
    /// </summary>
    PlayerManagerScript playerManagerScript;

    /// <summary>
    /// カラーコード受け渡し用
    /// </summary>
    Color colorCode;

    [SerializeField]
    float moveDistance;

    [SerializeField]
    float[] musicEndTimes;

    // 楽曲番号取得用
    int musicNum;

    [SerializeField]
    float totalMoveValue;

    private void Start()
    {
        //プレイヤーマネージャー取得
        playerManagerScript = GameObject.Find("SceneManager").GetComponent<PlayerManagerScript>();

        musicNum = GameObject.Find("Json").GetComponent<JsonReader>()._songList.songdata.songnum;


        // startの方に1pキャラに合った、色を付ける
        if (ColorUtility.TryParseHtmlString(
            barColorCode[playerManagerScript.playerCharaNum[(int)ConstRepo.Player.P1]], out colorCode))
        {
            seek_BarStart.color = colorCode;
        }

        // Endの方に1pキャラに合った、色を付ける
        if (ColorUtility.TryParseHtmlString(
            barColorCode[playerManagerScript.playerCharaNum[(int)ConstRepo.Player.P2]], out colorCode))
        {
            seek_BarEnd.color = colorCode;
        }
    }



    private void FixedUpdate()
    {
        // 曲がアクティブだったら
        if (MusicPlayer.instance.PlayTime >= 0)
        {
            //  バーポイントエフェクトを、フェードイン、フェードアウトを繰り返す
            BarPointEffect_Flashing();

            // バーポイント
            BarPointMove(Time.deltaTime);
        }

    }


    /// <summary>
    ///  フェードイン、フェードアウトを繰り返す
    /// </summary>
    void BarPointEffect_Flashing()
    {
        // カウントを取って、それを、÷２の余りによって、、処理を分ける
        if (alpha_MinMaxCount % 2 == 0)
        {
            // 不透明にしていく
            seek_BarPointEffect.color += new Color(0f, 0f, 0f, Time.deltaTime);
        }
        else
        {
            // 透明にしていく
            seek_BarPointEffect.color -= new Color(0f, 0f, 0f, Time.deltaTime);
        }

        // カラーのアルファ値が（MAXの値）以上だったら、1に揃える
        if (seek_BarPointEffect.color.a >= alphaValueMax)
        {
            // 不透明度を設定した値にする
            seek_BarPointEffect.color = new Color(
                seek_BarPointEffect.color.r, seek_BarPointEffect.color.g, seek_BarPointEffect.color.b,
                alphaValueMax);

            // カウント
            alpha_MinMaxCount++;
        }
        // カラーのアルファ値が（MINの値）以下だったら、0に揃える
        else if (seek_BarPointEffect.color.a <= 0)
        {
            // 完全に透明にする
            seek_BarPointEffect.color = new Color(
                seek_BarPointEffect.color.r, seek_BarPointEffect.color.g, seek_BarPointEffect.color.b,
                0);

            // カウント
            alpha_MinMaxCount = 0;
        }
    }

    /// <summary>
    /// バーポイントの移動
    /// </summary>
    void BarPointMove(float time)
    {
        //moveDistance musicEndTimes

        // 移動しすぎないように処理
        if(totalMoveValue < moveDistance)
        {
            // 1フレーム分の移動距離を、返り値で貰って、移動
            seek_BarPoint.transform.position += new Vector3(MoveValue(time), 0, 0);

            // 移動量の累計
            totalMoveValue += MoveValue(time);
        }
    }

    /// <summary>
    /// 移動量割り出し用
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    float MoveValue(float time)
    {
        //Debug.Log(moveDistance / (musicEndTimes[musicNum] / time));

        // シークバーのスタートから、終わりまでの距離　/ 曲の終わりの時間　/　前のフレームの時間を取得
        return (moveDistance / (musicEndTimes[musicNum] / time));
    }

    /// <summary>
    /// シークバー周りの見た目を消す
    /// </summary>
    public void AllSeekBarObjHide()
    {
        seek_BarPointEffect.sprite = null;
        seek_BarStart.sprite = null;
        seek_BarEnd.sprite = null;
        seek_BarPoint.GetComponent<SpriteRenderer>().sprite = null;
    }
}
