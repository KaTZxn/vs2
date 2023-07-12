using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillChargeRate : MonoBehaviour
{

    /// <summary>
    /// 自身のTextを管理するオブジェクト
    /// </summary>
    Text skillChargeText;


    /// <summary>
    /// スキルのチャージ率の値
    /// </summary>
    [SerializeField]
    public int skillValue;

    [SerializeField]
    int usePlayer;

    /// <summary>
    /// スキルのチャージ率の値の最大値
    /// </summary>
    [SerializeField]
    private int _skillValueMax;


    /// <summary>
    /// キャラをいじってる子に参照用
    /// </summary>
    [SerializeField]
    TestChara testChara;


    /// <summary>
    /// 色変更時使用するカラーコードの、配列
    /// </summary>
    public string[] charaColorCode;


    /// <summary>
    ///string型のカラーコードをColor変換した時の受け取り先
    /// </summary>
    Color colourCode;

    /// <summary>
    /// スキルストックスクリプト読み込み用
    /// </summary>
    [SerializeField]
    SkillStock skillStock;

    /// <summary>
    /// スキルシンボルスクリプト読み込み用
    /// </summary>
    [SerializeField]
    SkillSymbol skillSymbol;

    /// <summary>
    /// チャージテキストイメージに干渉する用
    /// </summary>
    [SerializeField]
    RawImage chargeImage;

    /// <summary>
    /// rawImageBGを消す用
    /// </summary>
    [SerializeField]
    RawImage chargeImageBG;

    /// <summary>
    /// スキルチャージ率が100の倍率を突破したときに再生するエフェクトに干渉する用
    /// </summary>
    [SerializeField]
    SpriteRenderer skillChageEffect_Sprite;

    /// <summary>
    /// スキルチャージ率が100の倍率を突破したときに再生するエフェクト再生する用
    /// </summary>
    [SerializeField]
    Animator skillChageEffect_Anim;

    /// <summary>
    /// スキルチャージ率が100の倍率を突破したときをカウントする用
    /// </summary>
    [SerializeField]
    int skillChageEffect_Count;

    // Start is called before the first frame update
    void Start()
    {
        // 自分のTextを挿入
        skillChargeText = GetComponent<Text>();

        //このスクリプトに付属する全てのUIの表記を更新
        AllUI_Update();
    }


    /// <summary>
    /// skillValueに引数を足す関数
    /// </summary>
    /// <param name="addValue">skillValueに足す値</param>
    public void AddSkillValue(int addValue)
    {
        // skillValue + addValue がMaxを越えなければ、そのまま足して、超えるのであれば
        // skillValue にMaxの値を入れる
        if (skillValue + addValue > _skillValueMax) 
        {
            skillValue = _skillValueMax;
        }
        else
        {
            //引数分足す
            skillValue += addValue;

        }


        //このスクリプトに付属する全てのUIの表記を更新
        AllUI_Update();
    }


    /// <summary>
    /// スキルを使用した時の処理
    /// </summary>
    public void UseSkill()
    {
        //スキル使用時の消費量分引く
        skillValue -= 100;

        //エフェクトの発生制御を、skillValueの消費に合わせて、低くする
        skillChageEffect_Count--;

        //このスクリプトに付属する全てのUIの表記を更新
        AllUI_Update();
    }

    /// <summary>
    /// チャージ率の表記更新
    /// </summary>
    void skillChargeTextUpdate()
    {
        //スキルのチャージ率が100以上なら、カラー表記
        if( 100 <=  skillValue)
        {
            // Color型への変換成功するとcolorにColor型の赤色が代入される）outキーワードで参照渡しにする
            if (ColorUtility.TryParseHtmlString(charaColorCode[testChara.count[usePlayer]],
                out colourCode))
            {
                // キャラにあった色に変更
                skillChargeText.color = colourCode;
            }
        }
        //スキルのチャージ率が100未満なら、グレー表記
        else
        {
            // Color型への変換成功するとcolorにColor型の赤色が代入される）outキーワードで参照渡しにする
            if (ColorUtility.TryParseHtmlString(charaColorCode[4],
                out colourCode))
            {
                // グレーに変更
                skillChargeText.color = colourCode;
            }
        }



        skillChargeText.text = skillValue.ToString() + "%";
    }


    /// <summary>
    /// スキルストックUIの見た目を更新する
    /// </summary>
    private void SkillStockUpdate()
    {   
        for (int i = 0; i < 3; i++)
        {
            //　i を掛け算に使用する時に０だと、都合が悪いので
            if (skillValue >= 100 * (i+1))
            {
                skillStock.SetStockSprite_Colour(i);
            }
            else
            {
                skillStock.SetStockSprite_Gray(i);
            }

        }
    }


    /// <summary>
    /// スキルのチャージ率依存で、カラーを三段階で、スイッチ
    /// </summary>
    private void ChargeImageColourUpdate()
    {
        //スキルのチャージ率が200以上なら、カラー表記
        if (200 <= skillValue)
        {
            // Color型への変換成功するとcolorにColor型の赤色が代入される）outキーワードで参照渡しにする
            if (ColorUtility.TryParseHtmlString(charaColorCode[testChara.count[usePlayer]],
                out colourCode))
            {
                // キャラにあった色に変更
                chargeImage.color = colourCode;
            }
        }
        //スキルのチャージ率が100以上なら、黒表記
        else if (100 <= skillValue)
        {
            // Color型への変換成功するとcolorにColor型の赤色が代入される）outキーワードで参照渡しにする
            if (ColorUtility.TryParseHtmlString(charaColorCode[5],
                out colourCode))
            {
                // 黒色に変更
                chargeImage.color = colourCode;
            }
        }
        else
        {
            // Color型への変換成功するとcolorにColor型の赤色が代入される）outキーワードで参照渡しにする
            if (ColorUtility.TryParseHtmlString(charaColorCode[4],
                out colourCode))
            {
                // 灰色に変更
                chargeImage.color = colourCode;
            }
        }
    }


    /// <summary>
    /// スキル周りのUIの表示を更新する
    /// </summary>
    void AllUI_Update()
    {
        //スキルストック表記の更新
        SkillStockUpdate();

        //エフェクトを再生する時なら再生する
        EffectPlayControl();

        //スキルのチャージ率表記の更新
        skillChargeTextUpdate();

        //チャージ画像の色の更新
        ChargeImageColourUpdate();
    }

    /// <summary>
    /// 100の倍率を超えたときのみ再生
    /// </summary>
    void EffectPlayControl()
    {
        if( 100 + 100 * skillChageEffect_Count <= skillValue)
        {
            skillChageEffect_Play();
        }

    }


    /// <summary>
    /// スキルチャージ率が一定を超えたときに流すエフェクト
    /// </summary>
    private void skillChageEffect_Play()
    {
        //skillChageEffect_Countが、1以上なら、カラー表記
        if (skillChageEffect_Count >= 1)
        {
            // Color型への変換成功するとcolorにColor型の赤色が代入される）outキーワードで参照渡しにする
            if (ColorUtility.TryParseHtmlString(charaColorCode[testChara.count[usePlayer]],
                out colourCode))
            {
                // キャラにあった色に変更
                skillChageEffect_Sprite.color = colourCode;
            }
        }
        //skillChageEffect_Countが、1未満なら、黒表記
        else
        {
            // Color型への変換成功するとcolorにColor型の赤色が代入される）outキーワードで参照渡しにする
            if (ColorUtility.TryParseHtmlString(charaColorCode[5],
                out colourCode))
            {
                // 黒に変更
                skillChageEffect_Sprite.color = colourCode;
            }
        }


        //アニメーションの再生
        skillChageEffect_Anim.SetTrigger("SkillChareAnim");


        //カウント
        skillChageEffect_Count++;
    }


    /// <summary>
    /// スキル周りのUIを隠す用
    /// </summary>
    public void HideSkillUIs()
    {
        skillChargeText.color -= new Color(0, 0, 0, 1f);

        skillStock.HideSkillStocks();

        skillSymbol.HideSkillSymbols();

        chargeImage.color = new Color(0, 0, 0, 0f);

        chargeImageBG.color = new Color(0, 0, 0, 0f);

        skillChageEffect_Sprite.color -= new Color(0, 0, 0, 1f);
    }
}
