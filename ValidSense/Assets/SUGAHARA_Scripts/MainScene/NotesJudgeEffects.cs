using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotesJudgeEffects : MonoBehaviour
{
    /// <summary>
    /// ノーツ判定時のエフェクトのの後ろ側のやつのアニメーター
    /// </summary>
    [SerializeField]
    private Animator bgAnimator;

    /// <summary>
    /// ノーツ判定時のエフェクトのキラキラエフェクトのアニメーター
    /// </summary>
    [SerializeField]
    private Animator glitterAnimator;


    /// <summary>
    /// ノーツ判定時のエフェクトのの後ろ側のやつのスプライト
    /// </summary>
    [SerializeField]
    private SpriteRenderer bgSprite;

    /// <summary>
    /// ノーツ判定時のエフェクトのキラキラエフェクトのスプライト
    /// </summary>
    [SerializeField]
    private SpriteRenderer glitterSprite;


    /// <summary>
    /// 色変更時使用するカラーコードの、配列
    /// </summary>
    [SerializeField]
    private string[] charaColorCode;

    /// <summary>
    ///string型のカラーコードをColor変換した時の受け取り先
    /// </summary>
    Color colourCode;



    /// <summary>
    /// Briliantのエフェクト
    /// </summary>
    public void BriliantEffect()
    {
        // 基本の色のカラーコード指定
        SetColour(4);

        bgAnimator.SetTrigger("BrilliantEffect");

        glitterAnimator.SetTrigger("BrilliantEffect_S");
    }

    /// <summary>
    /// Greatのエフェクト
    /// </summary>
    /// <param name="colourNum"></param>
    public void GreatEffect(int colourNum)
    {
        SetColour(colourNum);

        bgAnimator.SetTrigger("NormalEffect");

        glitterAnimator.SetTrigger("GreatEffect_S");
    }

    /// <summary>
    /// Goodのエフェクト
    /// </summary>
    /// <param name="colourNum"></param>
    public void GoodEffect(int colourNum)
    {
        SetColour(colourNum);

        bgAnimator.SetTrigger("NormalEffect");
    }

    /// <summary>
    /// カラーコードの色に各UIの色をセット
    /// </summary>
    /// <param name="colourNum"></param>
    void SetColour(int colourNum)
    {
        if (ColorUtility.TryParseHtmlString(charaColorCode[colourNum],
        out colourCode))
        {
            // キャラにあった色に変更
            bgSprite.color = colourCode;
            glitterSprite.color = colourCode;
        }
    }
}
