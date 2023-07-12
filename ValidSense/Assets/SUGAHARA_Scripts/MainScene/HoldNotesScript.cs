using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldNotesScript : MonoBehaviour
{
    /// <summary>
    /// フェードアウト用に子オブジェクトのspriteを取得用
    /// </summary>
    [SerializeField] 
    List<SpriteRenderer> _childrenSprites;


    /// <summary>
    /// 位置や、大きさを設定用に子オブジェクトのspriteを取得用
    /// </summary>
    [SerializeField]
    List<GameObject> _childrens;

    /// <summary>
    /// 60fpsで落下する速度
    /// </summary>
    private float notesSpeed;

    /// <summary>
    /// 新しいカラーコードを格納用
    /// </summary>
    private Color newColor;

    /// <summary>
    /// 真ん中の部分の透明度設定用
    /// </summary>
    [Range(0.1f, 1f)]
    public float alpha;

    public long[] times;
    


    /// <summary>
    /// 60fpsで落下する速度の設定
    /// </summary>
    /// <param name="speed"></param>
    public void Set_SppedAndScale(double speed , float time)
    {
        notesSpeed = (float)speed;

        // HoldNotesの真ん中のサイズを決定(20 は、サイズと距離の比率)
        _childrens[1].transform.localScale += new Vector3(0, (float)speed * time / 20f, 0);
        // HoldNotesのおしりの位置を決定
        _childrens[2].transform.localPosition += new Vector3(0, (float)speed * time, 0);
    }


    void FixedUpdate()
    {
        // 60fpsでspeed分落下させる
        transform.position -= new Vector3(0, notesSpeed, 0);
    }



    /// <summary>
    /// 子オブジェクトを引数のカラーコードの色に変更する
    /// </summary>
    /// <param name="colorCode"></param>
    public void NotesColorCange(string colorCode)
    {
        for (int i = 0; i < _childrenSprites.Count; i++)
        {
            if (ColorUtility.TryParseHtmlString(colorCode, out newColor))
            {
                //　真ん中の部分だけ半透明にする
                if (i == 1)
                {
                    newColor -= new Color(0, 0, 0, alpha);
                }
                _childrenSprites[i].color = newColor;
            }
        }
    }


    public void AllFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    /// <summary>
    /// 自身をフェードアウトして、非アクティブ化する
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeOut()
    {
        for(int i = 0; i < _childrenSprites.Count;)
        {
            if(i == 0 || i == 2)
            {
                _childrenSprites[i].color -= new Color(0f, 0f, 0f, 0.2f);
            }
            i++;
        }
        yield return new WaitForSeconds(0.03f);

        for (int i = 0; i < _childrenSprites.Count;)
        {
            _childrenSprites[i].color -= new Color(0f, 0f, 0f, 0.3f);
            i++;
        }
        yield return new WaitForSeconds(0.03f);

        for (int i = 0; i < _childrenSprites.Count;)
        {
            _childrenSprites[i].color -= new Color(0f, 0f, 0f, 0.3f);
            i++;
        }
        yield return new WaitForSeconds(0.02f);

        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 子オブジェクトのレイヤーをHoldNotesに変更する
    /// </summary>
    public void ChangeLayers()
    {
        for(int i = 0; i< 3;i++)
        {
            _childrens[i].layer = 7;
        }
    }
}
