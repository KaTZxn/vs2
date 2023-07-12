using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttckEffectsManager : MonoBehaviour
{
    /// <summary>
    /// スキルアタックエフェクトのキラキラの方の、スプライトレンダラーを取得
    /// </summary>
    [SerializeField]
    List<SpriteRenderer> _basisEffectSprite;


    /// <summary>
    /// スキルアタックエフェクトのキラキラの方の、アニメーターを取得
    /// </summary>
    [SerializeField]
    List<Animator> _basisEffectAnimator;

    /// <summary>
    /// スキルアタックエフェクトのシンボルの方の、スプライトレンダラーを取得
    /// </summary>
    [SerializeField]
    List<SpriteRenderer> _symbolEffectSprite;

    /// <summary>
    /// スキルアタックエフェクトのシンボルの方の、アニメーターを取得
    /// </summary>
    [SerializeField]
    List<Animator> _symbolEffectAnimator;

    /// <summary>
    /// アルファ値の最大値
    /// </summary>
    [SerializeField]
    private float _alphaMax;

    /// <summary>
    /// 一回のアルファ値の操作で使用する数値
    /// </summary>
    [SerializeField]
    private float _alphaValue;

    /// <summary>
    /// 次のアルファ値操作までの時間
    /// </summary>
    [SerializeField]
    private float _standbyTime;

    /// <summary>
    /// フェードアウトを優先させるためのbool
    /// </summary>
    private bool _canShowBasisEffect;

    /// <summary>
    /// スキルアタックエフェクトのキラキラの方の用の、カラーコード
    /// </summary>
    [SerializeField]
    private string[] basis_ColorCodes;

    /// <summary>
    /// スキルアタックエフェクトのシンボルの方の用の、カラーコード
    /// </summary>
    [SerializeField]
    private string[] symbol_ColorCodes;

    /// <summary>
    /// スキルアタックエフェクトの色をいじるとき用の、カラーコード
    /// </summary>
    Color colorCode;

    /// <summary>
    /// キャラにの番号取得用
    /// </summary>
    PlayerManagerScript playerManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        // 初期化する
        _canShowBasisEffect = false;

        // プレイヤー使用キャラの番号を取得してるスクリプトに参照
        playerManagerScript = GameObject.Find("SceneManager").GetComponent<PlayerManagerScript>();


        // ベースエフェクトの色を設定
        for (int i = 0; i < 2; i++)
        {
            // プレイヤーの使用しているキャラにあった、色を設定（引数の番号のカラーコード）
            if (ColorUtility.TryParseHtmlString(
            basis_ColorCodes[playerManagerScript.playerCharaNum[i]], out colorCode))
            {
                _basisEffectSprite[i].color = colorCode;
            }
        }

        // シンボルエフェクトの色を設定
        for (int i = 0; i < 2; i++)
        {
            // プレイヤーの使用しているキャラにあった、色を設定（引数の番号のカラーコード）
            if (ColorUtility.TryParseHtmlString(
            symbol_ColorCodes[playerManagerScript.playerCharaNum[i]], out colorCode))
            {
                _symbolEffectSprite[i].color = colorCode;
            }
        }


        // 透明に設定
        for (int i = 0; i < 2; i++)
        {
            _basisEffectSprite[i].color -= new Color(0f, 0f, 0f, 1f);
        }
    }

    /// <summary>
    /// キャラにあった色に調整して、アタック用のアニメーションをスタートさせる
    /// </summary>
    public void AttckEffects_Start(int playrNum ,int charaNum )
    {


        // 引数のキャラにあった、スタートアニメーションを再生
        switch (charaNum)
        {
            case (int)ConstRepo.Chara.Sight:
                _symbolEffectAnimator[playrNum].SetTrigger("ATT_SymbolEf_S_Start");
                break;

            case (int)ConstRepo.Chara.Tactile:
                _symbolEffectAnimator[playrNum].SetTrigger("ATT_SymbolEf_T_Start");

                break;

            case (int)ConstRepo.Chara.Smell_Taste:
                _symbolEffectAnimator[playrNum].SetTrigger("ATT_SymbolEf_ST_Start");

                break;

            case (int)ConstRepo.Chara.Hear:
                _symbolEffectAnimator[playrNum].SetTrigger("ATT_SymbolEf_H_Start");

                break;
        }


        //キラキラエフェクトのフェードインを開始
        StartCoroutine(BasisEffect_FadeIn(playrNum));
    }
    /// <summary>
    /// アタック用のアニメーションを終わらせる
    /// </summary>
    public void AttckEffects_End(int playrNum, int charaNum)
    {

        // 引数のキャラにあった、エンドアニメーションを再生
        switch (charaNum)
        {
            case (int)ConstRepo.Chara.Sight:

                // サイトのスキルループエフェクトを再生中なら
                if(_symbolEffectAnimator[playrNum].GetCurrentAnimatorStateInfo(0).
                    IsName( "ATT_SymbolEf_S_Loop"))
                {
                    _symbolEffectAnimator[playrNum].SetTrigger("ATT_SymbolEf_S_End");
                }

                break;

            case (int)ConstRepo.Chara.Tactile:

                // タクタイル君のスキルループエフェクトを再生中なら
                if (_symbolEffectAnimator[playrNum].GetCurrentAnimatorStateInfo(0).
                    IsName("ATT_SymbolEf_T_Loop"))
                {
                    _symbolEffectAnimator[playrNum].SetTrigger("ATT_SymbolEf_T_End");
                }

                break;

            case (int)ConstRepo.Chara.Smell_Taste:

                // スメテイのスキルループエフェクトを再生中なら
                if (_symbolEffectAnimator[playrNum].GetCurrentAnimatorStateInfo(0).
                    IsName("ATT_SymbolEf_ST_Loop"))
                {
                    _symbolEffectAnimator[playrNum].SetTrigger("ATT_SymbolEf_ST_End");
                }

                break;

            case (int)ConstRepo.Chara.Hear:
                // ヒア君のスキルループエフェクトを再生中なら
                if (_symbolEffectAnimator[playrNum].GetCurrentAnimatorStateInfo(0).
                    IsName("ATT_SymbolEf_H_Loop"))
                {
                    _symbolEffectAnimator[playrNum].SetTrigger("ATT_SymbolEf_H_End");
                }

                break;
        }

        //キラキラエフェクトのフェードアウトを開始
        StartCoroutine(BasisEffect_FadeOut(playrNum));
    }

    /// <summary>
    /// スキルアタックエフェクトのキラキラをフェードインさせる
    /// </summary>
    /// <returns></returns>
    IEnumerator BasisEffect_FadeIn(int playerNum)
    {
        //フェードアウトが優先されるように設定
        _canShowBasisEffect = true;

        // エフェクトの再生（ループ）
        _basisEffectAnimator[playerNum].SetTrigger("ATT_BasisEffect_Loop");

        // フェードイン
        while (_basisEffectSprite[playerNum].color.a < _alphaMax && _canShowBasisEffect)
        {
            _basisEffectSprite[playerNum].color += new Color(0f, 0f, 0f, _alphaValue);
            yield return new WaitForSeconds(_standbyTime);
        }

    }


    /// <summary>
    /// スキルアタックエフェクトのキラキラをフェードアウトさせる
    /// </summary>
    /// <returns></returns>
    IEnumerator BasisEffect_FadeOut(int playerNum)
    {
        //フェードアウトが優先されるように設定
        _canShowBasisEffect = false;

        // フェードアウト
        while (_basisEffectSprite[playerNum].color.a > 0)
        {
            _basisEffectSprite[playerNum].color -= new Color(0f, 0f, 0f, _alphaValue);
            yield return new WaitForSeconds(_standbyTime);
        }

        // フェードアウト後に、Ideleに戻る（None設定なので、アニメーションをしない）
        _basisEffectAnimator[playerNum].SetTrigger("ATT_BasisEffect_Idle");
    }

}
