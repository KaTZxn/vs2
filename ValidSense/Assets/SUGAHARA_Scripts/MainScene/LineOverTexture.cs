using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineOverTexture : MonoBehaviour
{
    SpriteRenderer _sprite;

    [SerializeField]
    private float _alphaMax;

    [SerializeField]
    private float _alphaValue;

    [SerializeField]
    private float _standbyTime;

    public bool canShowLine;

    // Start is called before the first frame update
    void Start()
    {
        canShowLine = false;

        _sprite = GetComponent<SpriteRenderer>();

        _sprite.color = new Color(0f, 0f, 0f, 0f);
    }

    public void fadeIn()
    {
        
        StartCoroutine(LinOverTexture_FadeIn());
    }

    public void fadeOut()
    {
        
        StartCoroutine(LinOverTexture_FadeOut());
    }

    /// <summary>
    /// 自身をフェードインさせる
    /// </summary>
    /// <returns></returns>
    IEnumerator LinOverTexture_FadeIn()
    {
        //フェードアウトが優先されるように設定
        canShowLine = true;

        while (_sprite.color.a < _alphaMax && canShowLine)
        {
            _sprite.color += new Color(1f, 1f, 1f, _alphaValue);
            yield return new WaitForSeconds(_standbyTime);
        }

    }


    /// <summary>
    /// 自身をフェードアウトさせる
    /// </summary>
    /// <returns></returns>
    IEnumerator LinOverTexture_FadeOut()
    {
        //フェードアウトが優先されるように設定
        canShowLine = false;

        while (_sprite.color.a > 0)
        {
            _sprite.color -= new Color(0f, 0f, 0f, _alphaValue);
            yield return new WaitForSeconds(_standbyTime);
        }

    }
}
