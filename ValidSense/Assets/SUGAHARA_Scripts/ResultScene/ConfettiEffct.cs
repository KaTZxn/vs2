using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiEffct : MonoBehaviour
{
    /// <summary>
    /// 使用するプレイヤー
    /// </summary>
    [SerializeField]
    private int usePlayer;

    /// <summary>
    /// 色変更時使用するカラーコードの、配列
    /// </summary>
    [SerializeField]
    private string[] charaColorCode;

    /// <summary>
    ///string型のカラーコードをColor変換した時の受け取り先
    /// </summary>
    Color colourCode;

    [SerializeField]
    PlayerManagerScript playerManagerScript;

    SpriteRenderer _sprite;


    // Start is called before the first frame update
    void Start()
    {

        playerManagerScript = GameObject.Find("SceneManager").GetComponent<PlayerManagerScript>();

        _sprite = GetComponent<SpriteRenderer>();


        // リザルトが一回目の表示だったらカラー
        // かつ、同点じゃなかったら
        if(playerManagerScript.resultCount == 0 
            &&
            playerManagerScript.score[(int)ConstRepo.Player.P1] 
            != playerManagerScript.score[(int)ConstRepo.Player.P2])
        {
            //　キャラカラー表記
            if (ColorUtility.TryParseHtmlString
                    (charaColorCode[playerManagerScript.playerCharaNum[usePlayer]],
                    out colourCode))
            {
                _sprite.color = colourCode;
            }
        }
        else
        {
            //　グレー表記
            if (ColorUtility.TryParseHtmlString
                    (charaColorCode[4],
                    out colourCode))
            {
                _sprite.color = colourCode;
            }
        }




        StartCoroutine(FadeOut());
    }


    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(17.3f);


        _sprite.color -= new Color(0f, 0f, 0f, 0.2f);
        yield return new WaitForSeconds(0.03f);

        _sprite.color -= new Color(0f, 0f, 0f, 0.3f);
        yield return new WaitForSeconds(0.03f);

        _sprite.color -= new Color(0f, 0f, 0f, 0.3f);
        yield return new WaitForSeconds(0.02f);

        this.gameObject.SetActive(false);
    }


}
