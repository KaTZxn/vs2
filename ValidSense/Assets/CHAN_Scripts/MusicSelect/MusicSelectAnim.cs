using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSelectAnim : MonoBehaviour
{
    public GameObject cur;
    public Image[] attend;
    [Range(-20, 20)]public float rotSpeed = -1f;
    [Range(0.5f, 3)]public float fadeSpeed = 1f;
    private void Start() 
    {
        StartCoroutine(Fade());
    }
    private void FixedUpdate() 
    {
        cur.transform.Rotate(new Vector3(0, 0, rotSpeed));
    }
    IEnumerator Fade()
    {
        var i = 1f;
        while(true)
        {
            while(i > 0f)
            {
                attend[0].color = new Color(255,255,255,i);
                attend[1].color = new Color(255,255,255,i);
                i -= Time.deltaTime / fadeSpeed;
                yield return null;
            }
            while(i < 1f)
            {
                attend[0].color = new Color(255,255,255,i);
                attend[1].color = new Color(255,255,255,i);
                i += Time.deltaTime / fadeSpeed;
                yield return null;
            }
        }
    }
}
