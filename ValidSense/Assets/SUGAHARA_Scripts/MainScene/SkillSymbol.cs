using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillSymbol : MonoBehaviour
{
    /// <summary>
    /// キャラのシンボル画像
    /// </summary>
    [SerializeField]
    List <RawImage> skillSymbols;
    

    /// <summary>
    /// 使用しているプレイヤーの番号
    /// </summary>
    [SerializeField]
    private int usePlayer;


    /// <summary>
    /// キャラの値を保存している子
    /// </summary>
    [SerializeField]
    TestChara testChara;



    void Start()
    {
        // 現在のプレイヤーのキャラに合わせた画像だけ、表示
        // 1P時
        if (usePlayer == (int)ConstRepo.Player.P1)
        {
            for(int i = 0; i < skillSymbols.Count; i++)
            {
                if (testChara.count[(int)ConstRepo.Player.P1] == i)
                {
                    skillSymbols[i].gameObject.SetActive(true);
                }
                else
                {
                    skillSymbols[i].gameObject.SetActive(false);
                }

            }
        }
        // 2P時
        else if (usePlayer == (int)ConstRepo.Player.P2)
        {
            for (int i = 0; i < skillSymbols.Count; i++)
            {
                if (testChara.count[(int)ConstRepo.Player.P2] == i)
                {
                    skillSymbols[i].gameObject.SetActive(true);
                }
                else
                {
                    skillSymbols[i].gameObject.SetActive(false);
                }

            }
        }
    }


    /// <summary>
    /// スキルシンボルUIを非表示にする
    /// </summary>
    public void HideSkillSymbols()
    {
        if (usePlayer == (int)ConstRepo.Player.P1)
        {
            for (int i = 0; i < skillSymbols.Count; i++)
            {
                if (testChara.count[(int)ConstRepo.Player.P1] == i)
                {
                    skillSymbols[i].color -= new Color(0, 0, 0, 1f);
                }
            }
        }
        // 2P時
        else if (usePlayer == (int)ConstRepo.Player.P2)
        {
            for (int i = 0; i < skillSymbols.Count; i++)
            {
                if (testChara.count[(int)ConstRepo.Player.P2] == i)
                {
                    skillSymbols[i].color -= new Color(0, 0, 0, 1f);
                }
            }
        }
    }
}
