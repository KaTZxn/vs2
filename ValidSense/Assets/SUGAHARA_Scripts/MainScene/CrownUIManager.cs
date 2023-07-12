using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrownUIManager : MonoBehaviour
{
    /// <summary>
    /// クラウンUIのアニメーターに参照する用
    /// </summary>
    [SerializeField]
    List <Animator> crownsAnimator;

    /// <summary>
    /// クラウンUIの画像に参照する用
    /// </summary>
    [SerializeField]
    List<SpriteRenderer> crownsRenderer;

    /// <summary>
    /// クラウンを表示させる事を許可する変数
    /// </summary>
    [SerializeField]
    public bool canShowCrown ;

    /// <summary>
    /// フェードアウト.インするためにかかる時間
    /// </summary>
    [SerializeField]
    private float fadeTime;

    /// <summary>
    /// フェードアウト.インの時に、何回の繰り返しで終わらせるか
    /// </summary>
    [SerializeField] 
    private int forUseNum;

    /// <summary>
    /// フェードインを拒否用
    /// </summary>
    [SerializeField]
    bool[] cantFadeIn;

    /// <summary>
    /// スコア計に、干渉する用
    /// </summary>
    [SerializeField]
    List <ScoreScript> scoreScripts;


    private int butterPlayerNum;

    void Start()
    {
        // 初期値の代入
        canShowCrown = true;

        // 最初は非表示
        //for (int i = 0; i < crownsAnimator.Count; i++)
        //{
        //    //crownsAnimator[i].color = new Color (1f,1f,1f,0f);
        //}
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            ShowCrownUI();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            ShowCrownUI();
        }
    }


    /// <summary>
    /// 現在の勝敗をチェックし、有利な方を戻り値で返す、同点なら、3を返す
    /// </summary>
    /// <returns></returns>
    private int ButterPlayerCheck()
    {
        // レーン所持数が同じ
        if (scoreScripts[(int)ConstRepo.Player.P1].stolenLaneCount 
            == scoreScripts[(int)ConstRepo.Player.P2].stolenLaneCount)
        {
            //　スコア同点
            if(scoreScripts[(int)ConstRepo.Player.P1].scoreValue
            ==  scoreScripts[(int)ConstRepo.Player.P2].scoreValue)
            {
                return 3;
            }
            // 1pのスコアが多い
            else if (scoreScripts[(int)ConstRepo.Player.P1].scoreValue
            > scoreScripts[(int)ConstRepo.Player.P2].scoreValue)
            {
                return (int)ConstRepo.Player.P1;
            }
            //　上記以外（2pのスコアが多い）
            else
            {
                return (int)ConstRepo.Player.P2;
            }
        }
        // 1Pのレーン所持数が多い
        else if (scoreScripts[(int)ConstRepo.Player.P1].stolenLaneCount 
            > scoreScripts[(int)ConstRepo.Player.P2].stolenLaneCount)
        {
            return (int)ConstRepo.Player.P1;
        }
        //　上記以外（2pのレーン所持数が多い）
        else
        {
            return (int)ConstRepo.Player.P2;
        }
    }


    /// <summary>
    /// 表示することが許可されていれば、引数と同じ番号のクラウンUIのみ、表示
    /// </summary>
    /// <param name="playerNum"></param>
    public void ShowCrownUI()
    {
        //表示することが許可されていれば
        if(canShowCrown)
        {
            // 画像のカラーを可視化
            crownsRenderer[0].color = new Color(1, 1, 1, 1f);
            crownsRenderer[1].color = new Color(1, 1, 1, 1f);



            // 有利な方をチェックして、戻り値で受け渡し
            butterPlayerNum = ButterPlayerCheck();

            //Debug.Log(butterPlayerNum);

            //レーン所持数、スコア共に同点時
            if(butterPlayerNum == 3)
            {
                for (int i = 0; i < crownsAnimator.Count; i++)
                {
                    // 表示されているクラウンを非表示

                    if(cantFadeIn[i])
                    {
                        crownsAnimator[i].SetTrigger("FadeOut");
                        cantFadeIn[i] = false;
                    }

                    //StartCoroutine(CrownUIFadeOut(i));
                }
                //終了
                return;
            }
            // 現在表示されているクラウンと、同じなら
            else if (cantFadeIn[butterPlayerNum])
            {
                //終了
                return;
            }


            for (int i = 0; i < crownsAnimator.Count; i++)
            {
                // 有利なプレイヤー、と違う番号のクラウンは、非表示
                if (i != butterPlayerNum)
                {
                    //StartCoroutine(CrownUIFadeOut(i));

                    crownsAnimator[i].SetTrigger("FadeOut");
                    cantFadeIn[i] = false;
                }
                else
                {
                    // 有利なプレイヤー、と同じ番号のクラウンは、表示
                    //StartCoroutine(CrownUIFadeIn(butterPlayerNum));

                    crownsAnimator[butterPlayerNum].SetTrigger("FadeIn");
                    cantFadeIn[butterPlayerNum] = true;
                }
            }
        }
    }

    /// <summary>
    /// クラウンUIの非表示化
    /// </summary>
    public void HideCrownUIs()
    {
        crownsRenderer[0].sprite = null;
        crownsRenderer[1].sprite = null;
    }

    ///// <summary>
    ///// 引数の番号のクラウンUIをフェードインさせるコールチン
    ///// </summary>
    ///// <param name="playerNum"></param>
    ///// <returns></returns>
    //IEnumerator CrownUIFadeIn(int playerNum)
    //{
    //    cantFadeIn[playerNum] = true;

    //    if (crownsAnimator[playerNum].color.a >= 1f)
    //    {
    //        crownsAnimator[playerNum].color = new Color(1f,1f,1f,1f);

    //        yield break;
    //    }

    //    //フェードアウトにかかる時間待つ
    //    //yield return new WaitForSeconds(fadeTime);
    //    Debug.Log("In");

    //    for (int i = 0; i < forUseNum; i++)
    //    {
    //        crownsAnimator[playerNum].color += new Color(0f, 0f, 0f, 1f / forUseNum);

    //        if (crownsAnimator[playerNum].color.a >= 1f)
    //        {
    //            crownsAnimator[playerNum].color = new Color(1f, 1f, 1f, 1f);

    //            yield break;
    //        }

    //        yield return new WaitForSeconds(fadeTime / forUseNum);
    //    }
    //}



    ///// <summary>
    ///// 引数の番号のクラウンUIをフェードアウトさせるコールチン
    ///// </summary>
    ///// <param name="playerNum"></param>
    ///// <returns></returns>
    //IEnumerator CrownUIFadeOut(int playerNum)
    //{
    //    cantFadeIn[playerNum] = false;

    //    if (crownsAnimator[playerNum].color.a <= 0f)
    //    {
    //        crownsAnimator[playerNum].color = new Color(1f, 1f, 1f, 0f);

    //        yield break;
    //    }
    //    Debug.Log("Out");

    //    for (int i = 0; i < forUseNum; i++ )
    //    {

    //        crownsAnimator[playerNum].color -= new Color(0f, 0f, 0f, 1f / forUseNum);

    //        if (crownsAnimator[playerNum].color.a <= 0f)
    //        {
    //            crownsAnimator[playerNum].color = new Color(1f, 1f, 1f, 0f);

    //            yield break;
    //        }

    //        yield return new WaitForSeconds(fadeTime / forUseNum);
    //    }

    //}


}
