using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashingTextScript : MonoBehaviour
{
    /// <summary>
    ///  自身に参照する用
    /// </summary>
    Text text;

    /// <summary>
    /// アルファ値代入用
    /// </summary>
    float alphaValue;

    /// <summary>
    /// アルファの最大値用
    /// </summary>
    int alpha_MinMaxCount;

    private void Start()
    {
         // 自身に参照する
         text = GetComponent<Text>();

        // 初期値
        alphaValue = 0;
    }



    private void FixedUpdate()
    {
        // カウントを取って、それを、÷２の余りによって、、処理を分ける
        if (alpha_MinMaxCount % 2 == 0)
        {
            // 不透明にしていく
            alphaValue += Time.deltaTime;
        }
        else
        {
            // 透明にしていく
            alphaValue -= Time.deltaTime;
        }

        // カラーのアルファ値が1（MAXの値）以上だったら、1に揃える
        if (alphaValue >= 1)
        {
            alphaValue = 1;

            // カウント
            alpha_MinMaxCount++;
        }
        // カラーのアルファ値が0（MINの値）以下だったら、0に揃える
        else if (alphaValue <= 0)
        {
            alphaValue = 0;

            // カウント
            alpha_MinMaxCount = 0;
        }

        // アルファ値の代入
        text.color = new Color(1f, 1f, 1f, alphaValue);



    }


}
