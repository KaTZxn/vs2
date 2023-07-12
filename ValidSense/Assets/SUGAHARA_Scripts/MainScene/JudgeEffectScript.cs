using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JudgeEffectScript : MonoBehaviour
{
    /// <summary>
    /// 自分のカラーコードに干渉する用
    /// </summary>
    RawImage sprite;

    [SerializeField]
    private float fadeTime;

    [SerializeField]
    private float updateTime;

    [SerializeField]
    private float moveValue;

    [SerializeField]
    private float updateNumber;

    [SerializeField]
    private float fadeNumber;

    RectTransform rectTransform;

    [SerializeField]
    float positionY;

    private void Start()
    {
        // 自分のSpriteRendererを入れる
        sprite = GetComponent<RawImage>();
        rectTransform = GetComponent<RectTransform>();

        rectTransform.localPosition += new Vector3(0f, positionY, 0f);

        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        //for(int i = 0; i < updateNumber; i++)
        //{   
        //    rectTransform.localPosition += new Vector3(0f, moveValue / updateNumber, 0f);
        //    yield return new WaitForSeconds(0.1f/ updateNumber);
        //}

        yield return new WaitForSeconds(0.1f);


        for (int i = 0; i < fadeNumber; i++)
        {
            sprite.color -= new Color(0f, 0f, 0f, 0.9f / fadeNumber);
            yield return new WaitForSeconds(fadeTime / fadeNumber);
        }

        this.gameObject.SetActive(false);
    }
}
