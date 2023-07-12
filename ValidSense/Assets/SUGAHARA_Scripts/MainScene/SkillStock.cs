using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillStock : MonoBehaviour
{
    /// <summary>
    /// スキルストックUI
    /// </summary>
    [SerializeField]
    List<RawImage> skillStocks;

    /// <summary>
    /// スキルストックのカラー版画像
    /// </summary>
    [SerializeField]
    List<Texture> StockImage_Colour;

    /// <summary>
    /// スキルストックのグレー版画像
    /// </summary>
    [SerializeField]
    List<Texture> StockImage_Gray;

    [SerializeField]
    TestChara testChara;

    /// <summary>
    /// どっちのプレイヤーが使用しているか
    /// </summary>
    [SerializeField]
    int usePlayer;

    private int charaNum;

    [SerializeField]
    private string tactileColorCode;

    private Color colorCode;

    // Start is called before the first frame update
    void Start()
    {
        charaNum = testChara.count[usePlayer];



        for (int i = 0; i < skillStocks.Count; i++)
        {
            SetStockSprite_Gray(i);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetStockSprite_Gray(int i)
    {
        skillStocks[i].texture = StockImage_Gray[charaNum];
        // タクタイル君だけ、色を重ねる
        if (charaNum == (int)ConstRepo.Chara.Tactile)
        {
            skillStocks[i].color = new Color(1f,1f,1f,1f);
        }
    }


    public void SetStockSprite_Colour(int i)
    {

        skillStocks[i].texture = StockImage_Colour[charaNum];

        // タクタイル君だけ、色を重ねる
        if(charaNum == (int)ConstRepo.Chara.Tactile)
        {
            if (ColorUtility.TryParseHtmlString(tactileColorCode, out colorCode))
            {
                skillStocks[i].color = colorCode;
            }
        }
    }


    public void HideSkillStocks()
    {
        for (int i = 0; i < skillStocks.Count; i++)
        {
            skillStocks[i].color -= new Color(0, 0, 0, 1f);
        }
    }
}
