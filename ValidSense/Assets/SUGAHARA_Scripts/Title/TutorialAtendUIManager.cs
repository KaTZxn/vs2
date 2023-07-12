using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialAtendUIManager : MonoBehaviour
{
    /// <summary>
    /// チュートリアルアテンドテキスト 英語
    /// </summary>
    [SerializeField]
    Text tutorialText_Eng;

    /// <summary>
    /// チュートリアルアテンドテキスト 日本語
    /// </summary>
    [SerializeField]
    Text tutorialText_Jap;

    /// <summary>
    /// チュートリアルアテンドYseスクリプト
    /// </summary>
    [SerializeField]
    SpriteRenderer tutorial_YesSprit;

    /// <summary>
    /// チュートリアルアテンドYseをリセットするときに使用する、画像
    /// </summary>
    [SerializeField]
    Sprite tutorial_YesStartSprit;

    /// <summary>
    /// チュートリアルアテンドYseアニメーター
    /// </summary>
    [SerializeField]
    Animator tutorial_YesAnimator;

    /// <summary>
    /// チュートリアルアテンドNoテキストのスクリプト
    /// </summary>
    [SerializeField]
    SpriteRenderer tutorial_YesTextSprit;



    /// <summary>
    /// チュートリアルアテンドNoスクリプト
    /// </summary>
    [SerializeField]
    SpriteRenderer tutorial_NoSprit;


    /// <summary>
    /// チュートリアルアテンドNoをリセットするときに使用する、画像
    /// </summary>
    [SerializeField]
    Sprite tutorial_NoStartSprit;



    /// <summary>
    /// チュートリアルアテンドNoアニメーター
    /// </summary>
    [SerializeField]
    Animator tutorial_NoAnimator;

    /// <summary>
    /// チュートリアルアテンドNoテキストのスクリプト
    /// </summary>
    [SerializeField]
    SpriteRenderer tutorial_NoTextSprit;

    /// <summary>
    /// フェードインが、終了しているかどうか
    /// </summary>
    [SerializeField]
    private bool isFadeInEnd;

    /// <summary>
    /// 代入する時のアルファ値の変数
    /// </summary>
    private float alphaValue;

    /// <summary>
    /// アルファ値の変数の、倍率
    /// </summary>
    [SerializeField]
    private float addValueRage;

    /// <summary>
    /// チュートリアルアテンドUIのBG
    /// </summary>
    [SerializeField]
    private Image tutorialAttendUI_BG;


    /// <summary>
    /// タイトル画面で、使用しているタイマーをいじる用
    /// </summary>
    [SerializeField]
    Text turorialAttendTimerText;


    /// <summary>
    /// タイトル画面で、使用しているタイマー
    /// </summary>
    [SerializeField]
    TitleTimer turorialAttendTimer;

    // Start is called before the first frame update
    void Start()
    {
        // 初期値を設定
        alphaValue = 0;
        isFadeInEnd = false;

        // 所持してるUIを透明化させる
        AllHaveUI_AddColor(new Color(1f, 1f, 1f, -1f));

        // タイマーを止める
        turorialAttendTimer.CountDownStop();
    }




    /// <summary>
    /// フェードイン処理
    /// </summary>
    public void FadeIn()
    {
        // タイマーのカウントダウンを止める
        turorialAttendTimer.CountDownStop();

        StartCoroutine(UIFadeIn());
    }


    /// <summary>
    /// フェードアウト処理
    /// </summary>
    public void FadeOut()
    {
        StartCoroutine(UIFadeOut());
    }


    /// <summary>
    /// フェードインコールチン
    /// </summary>
    /// <returns></returns>
    IEnumerator UIFadeIn() 
    {

        while(!isFadeInEnd)
        {
            // アルファ値を代入するための値を設定
            alphaValue = (Time.deltaTime * addValueRage);


            // アルファ値を加算
            AllHaveUI_AddColor(new Color(0f, 0f, 0f, alphaValue));



            // 代入するための値が1以上なら
            if (tutorialText_Eng.color.a >= 1)
            {

                // アニメーションを再生
                tutorial_YesAnimator.SetTrigger("PlayAnim");
                tutorial_NoAnimator.SetTrigger("PlayAnim");

                // タイマーのカウントダウンを再開する
                turorialAttendTimer.CountDownStart();

                // ループを抜ける
                isFadeInEnd = true;
            }

            // 1フレーム待つ
            yield return null;
        }
    }


    /// <summary>
    /// フェードアウトコールチン
    /// </summary>
    /// <returns></returns>
    IEnumerator UIFadeOut()
    {
        while (isFadeInEnd)
        {
            // アルファ値を代入するための値を設定
            alphaValue = (Time.deltaTime * addValueRage);

            // アルファ値を減算
            AllHaveUI_AddColor(new Color(0f, 0f, 0f, alphaValue));

            // 代入するための値が1以上なら
            if (alphaValue <= 0)
            {

                // ループを抜ける
                isFadeInEnd = false;
            }

            // 1フレーム待つ
            yield return null;
        }
    }

    /// <summary>
    /// 所持してるUIの色ををいじる
    /// </summary>
    /// <param name="addColor"></param>
    public void AllHaveUI_AddColor(Color addColor)
    {
        tutorialText_Eng.color += addColor;
        tutorialText_Jap.color += addColor;

        tutorial_YesTextSprit.color += addColor;
        tutorial_NoTextSprit.color += addColor;

        tutorial_YesSprit.color += addColor;
        tutorial_NoSprit.color += addColor;

        turorialAttendTimerText.color += addColor;

        //　BGだけ、色固定
        tutorialAttendUI_BG.color += new Color(0f, 0f, 0f, addColor.a);
    }


    /// <summary>
    /// UIを初期化させる
    /// </summary>
    public void AllHaveUI_Reset()
    {
        // アニメーションの停止
        tutorial_YesAnimator.SetTrigger("ToIdle");
        tutorial_NoAnimator.SetTrigger("ToIdle");

        // 初期画像を代入
        tutorial_YesSprit.sprite = tutorial_YesStartSprit;
        tutorial_NoSprit.sprite = tutorial_NoStartSprit;


        // 所持してるUIを透明化させる
        AllHaveUI_AddColor(new Color(1f, 1f, 1f, -1f));

        isFadeInEnd = false;
    }
}
