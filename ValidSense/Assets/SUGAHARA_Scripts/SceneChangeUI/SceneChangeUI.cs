using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeUI : MonoBehaviour
{
    /// <summary>
    /// 自身のアニメーションを再生する用
    /// </summary>
    Animator myAnimator;


    SpriteRenderer mySpriteRenderer;

    /// <summary>
    /// 最初のアニメーションの待機時間
    /// </summary>
    [SerializeField]
    private float startAnimStandbyTime;

    private void Start()
    {
        // 自身のアニメーションを取得
        myAnimator = GetComponent<Animator>();

        
        if(GetComponent<SpriteRenderer>() != null )
        {
            mySpriteRenderer = GetComponent<SpriteRenderer>();
            mySpriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        }

        Invoke("SceneStartAnim", startAnimStandbyTime);
    }


    /// <summary>
    /// シーン読み込み直後のアニメーションを再生
    /// </summary>
    public void SceneStartAnim()
    {

        myAnimator.SetTrigger("SceneChange_Out");
    }

    /// <summary>
    /// シーン遷移直前のアニメーションを再生する
    /// </summary>
    public void SceneEndAnim()
    {        
        //難易度選択以外は、画面遷移時SEを再生
        if(SceneManager.GetActiveScene().name != "LevelScene")
        {
            SEPlayer.instance.SEOneShot(10);
        }

        myAnimator.SetTrigger("SceneChange_In");
    }
}
