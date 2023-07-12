using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewNotesScript : MonoBehaviour
{
    /// <summary>
    /// フェードアウト用に自身のspriteを取得用
    /// </summary>
    private SpriteRenderer _sprite;


    /// <summary>
    /// 60fpsで落下する速度
    /// </summary>
    public float notesSpeed;

    private void Start()
    {
        // フェードアウト用に自身の_spriteを取得
        _sprite = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 60fpsで落下する速度の設定
    /// </summary>
    /// <param name="speed"></param>
    public void NotesSpeedSetter(double speed)
    {
        notesSpeed = (float)speed;
    }


    void FixedUpdate()
    {
        // 60fpsでspeed分落下させる
        transform.position -= new Vector3(0, notesSpeed, 0);
    }



    /// <summary>
    /// コールチンの関数を利用して、フェードアウトして、非アクティブ化する
    /// </summary>
    public void NotesFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    /// <summary>
    /// 自身をフェードアウトして、非アクティブ化する
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeOut()
    {
        _sprite.color -= new Color( 0f, 0f, 0f, 0.2f);
        yield return new WaitForSeconds(0.03f);

        _sprite.color -= new Color( 0f, 0f, 0f, 0.3f);
        yield return new WaitForSeconds(0.03f);

        _sprite.color -= new Color( 0f, 0f, 0f, 0.3f);
        yield return new WaitForSeconds(0.02f);

        this.gameObject.SetActive(false);
    }
}
