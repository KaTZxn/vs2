using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainLive2DCanvas : MonoBehaviour
{
    /// <summary>
    /// 自身のレイヤー数に干渉用
    /// </summary>
    Canvas canvas;

    /// <summary>
    /// キャラ参照用
    /// </summary>
    [SerializeField]
    TestChara testChara;


    /// <summary>
    /// 1P,2PのLive2Dのtexture格納用
    /// </summary>
    [SerializeField]
    List <RawImage> live2D;

    /// <summary>
    /// ジャッチ演出時の移動先
    /// </summary>
    [SerializeField]
    List<RectTransform> judgeWinPos;
    [SerializeField]
    List<RectTransform> judgeLosePos;

    
    [SerializeField]
    RawImage[] addPosObg;

    /// <summary>
    /// Live2Dを何秒で移動を完了させるか
    /// </summary>
    [SerializeField]
    float live2DMoveTime;

    /// <summary>
    /// Live2Dの移動スピード
    /// </summary>
    [SerializeField]
    float[] live2DMoveSpeed;


    // Start is called before the first frame update
    void Start()
    {
        // 自身のキャンバスに参照する
        canvas = GetComponent<Canvas>();
    }

    

    /// <summary>
    /// ジャッチ演出時のLive2D操作
    /// </summary>
    /// <param name="winPlayerNum"></param>
    /// <returns></returns>
    public IEnumerator Live2DJudgmentPerformance( int winPlayerNum)
    {

        //スピードの設定
        // 1P 勝利時
        if (winPlayerNum == 0)
        {
            live2DMoveSpeed[0] =
                (((judgeWinPos[0].position.x + addPosObg[testChara.count[0]].rectTransform.position.x)
                - live2D[0].rectTransform.position.x) / 60);

            live2DMoveSpeed[1] = (((judgeLosePos[1].position.x)
                - live2D[1].rectTransform.position.x) / 60);
        }
        // 2P 勝利時
        else if (winPlayerNum == 1)
        {
            live2DMoveSpeed[1] =
                (((judgeWinPos[1].position.x - addPosObg[testChara.count[0]].rectTransform.position.x)
                - live2D[1].rectTransform.position.x) / 60);

            live2DMoveSpeed[0] = (((judgeLosePos[0].position.x)
                - live2D[0].rectTransform.position.x) / 60);
        }
        // 同点時
        else
        {
            live2DMoveSpeed[0] = (((judgeLosePos[0].position.x)
                - live2D[0].rectTransform.position.x) / 60);

            live2DMoveSpeed[1] = (((judgeLosePos[1].position.x)
                - live2D[1].rectTransform.position.x) / 60);
        }



        // レイヤーを最上段（11）にする
        //for (int i = -2; i <= 11; i++)
        //{
        //    canvas.sortingOrder = i;

        //    yield return new WaitForSeconds(0.02f);
        //}

        //yield return new WaitForSeconds(0.8f);

        // 少し待つ
        yield return new WaitForSeconds(1.2f);

        // レーンより上に表示
        canvas.sortingOrder = 11;
    

        // 少し待つ
        yield return new WaitForSeconds(0.65f);


        // 勝利キャラを教えてくれる数値に合わせて、キャラクターのアニメーションを再生する
        if (winPlayerNum == 0)
        {
            testChara.Chara_Anim_Win(0);
            yield return new WaitForSeconds(0.02f);
            //testChara.Chara_Anim_Hit(1);
        }
        else if (winPlayerNum == 1)
        {
            testChara.Chara_Anim_Win(1);
            yield return new WaitForSeconds(0.02f);
            //testChara.Chara_Anim_Hit(0);
        }
        else
        {
            yield return new WaitForSeconds(0.02f);
            //testChara.Chara_Anim_Hit(0);
            //testChara.Chara_Anim_Hit(1);
        }

        // ちょっと待つ
        yield return new WaitForSeconds(0.3f);

        //相手がもともといた場所くらいまで移動する
        if (winPlayerNum == 0)
        {
            while (live2D[0].rectTransform.position.x <= judgeWinPos[0].position.x
                + addPosObg[testChara.count[0]].rectTransform.position.x)
            {
                live2D[0].rectTransform.position += new Vector3(
                    live2DMoveSpeed[0] / live2DMoveTime, 0, 0);

                live2D[1].rectTransform.position += new Vector3(
                    live2DMoveSpeed[1] / live2DMoveTime, 0, 0);

                yield return null;
            }

        }
        else if(winPlayerNum == 1)
        {
            while (live2D[1].rectTransform.position.x >= judgeWinPos[1].position.x
                - addPosObg[testChara.count[1]].rectTransform.position.x)
            {
                live2D[0].rectTransform.position += new Vector3(
                    live2DMoveSpeed[0] / live2DMoveTime, 0, 0);

                live2D[1].rectTransform.position += new Vector3(
                    live2DMoveSpeed[1] / live2DMoveTime, 0, 0);

                yield return null;
            }
        }
        else
        {
            while (live2D[1].rectTransform.position.x >= judgeWinPos[1].position.x
                - addPosObg[testChara.count[1]].rectTransform.position.x)
            {
                live2D[0].rectTransform.position += new Vector3(
                    live2DMoveSpeed[0] / live2DMoveTime, 0, 0);

                live2D[1].rectTransform.position += new Vector3(
                    live2DMoveSpeed[1] / live2DMoveTime, 0, 0);

                yield return null;
            }
        }


        if (winPlayerNum == 0)
        {
            while (live2D[1].rectTransform.position.x <= judgeLosePos[1].position.x)
            {

                live2D[1].rectTransform.position += new Vector3(
                    live2DMoveSpeed[1] / live2DMoveTime, 0, 0);

                yield return null;
            }

        }
        else if(winPlayerNum == 1)
        {
            while (live2D[0].rectTransform.position.x >= judgeLosePos[0].position.x)
            {
                live2D[0].rectTransform.position += new Vector3(
                    live2DMoveSpeed[0] / live2DMoveTime, 0, 0);


                yield return null;
            }
        }
        else
        {

        }
    }
}
