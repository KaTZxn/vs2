using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUseValueClass : MonoBehaviour
{

    /// <summary>
    /// スキルチャージ率の加算時に使用される変数（判定により配列番号指定）
    /// </summary>
    public int[] skillValues;

    /// <summary>
    /// スコアの加算時に使用される変数（判定により配列番号指定）
    /// </summary>
    public int[] scoreValues;

    public int addScoreCount;

    /// <summary>
    /// レーンを奪う時の基準、比較対象は、ノーツ判定時のスコア加算量の和の変数
    /// 難易度で、変数を変更する
    /// </summary>
    public int lineSteal_NeedScore;

    /// <summary>
    /// レーンを奪った時のスコア加算量
    /// </summary>
    public int[] lineSteal_AddScore;
}
