using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaSelect_CahraClass : MonoBehaviour
{
    /// <summary>
    /// Live2Dを格納用（表示非表示に使用）
    /// </summary>
    [SerializeField]
    List<GameObject> charaLive2D;

    /// <summary>
    /// キャラのカタカナ表記の名前UIの表示用（表示非表示に使用）
    /// </summary>
    [SerializeField]
    List<GameObject> chara_Name;

    /// <summary>
    /// キャラの英語表記の名前UIの表示用（表示非表示に使用）
    /// </summary>
    [SerializeField]
    List<GameObject> chara_Name_2;

    /// <summary>
    /// キャラのシンボルUIの表示用（表示非表示に使用）
    /// </summary>
    [SerializeField]
    List<GameObject> chara_Name_Symbols;

    /// <summary>
    /// キャラを画面に表示させるのに使用しているキャンバスに参照する用
    /// </summary>
    [SerializeField]
    Canvas charaCanvas;


    /// <summary>
    /// キャラ表示キャンバスの描画順番号の最大値
    /// </summary>
    [SerializeField]
    int orderInLayer_Max;

    /// <summary>
    /// キャラ表示キャンバスの描画順番号の最小値
    /// </summary>
    [SerializeField]
    int orderInLayer_Min;


    /// <summary>
    /// 引数以外のcharaClassListの非表示にして、引数のは、表示する
    /// </summary>
    /// <param name="showNum"></param>
    public void CharaUI_1P_DisplaySwitching(int showNum)
    {
        for (int i = 0; i < charaLive2D.Count;)
        {
            if (i == showNum)
            {
                charaLive2D[i].SetActive(true);
                chara_Name[i].SetActive(true);
                chara_Name_2[i].SetActive(true);
                chara_Name_Symbols[i].SetActive(true);
            }
            else
            {
                charaLive2D[i].SetActive(false);
                chara_Name[i].SetActive(false);
                chara_Name_2[i].SetActive(false);
                chara_Name_Symbols[i].SetActive(false);
            }

            i++;
        }
    }

    /// <summary>
    /// キャラ選択時のアニメーションを再生する
    /// </summary>
    /// <param name="charaNum"></param>
    public void SelectChara_AimChoice(int charaNum)
    {
        Debug.Log(charaNum + charaLive2D[charaNum].name);

        
        charaLive2D[charaNum].GetComponent<Live2D_AnimController>().Anim_Choice_OnVoice();
    }

    /// <summary>
    /// キャラ表示キャンバスの描画順を最大にする
    /// </summary>
    public void SetCharaOrderInLayer_Max()
    {
        charaCanvas.sortingOrder = orderInLayer_Max;
    }

    /// <summary>
    /// キャラ表示キャンバスの描画順を最小にする
    /// </summary>
    public void SetCharaOrderInLayer_Min()
    {
        charaCanvas.sortingOrder = orderInLayer_Min;
    }
}
