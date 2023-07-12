using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawFullComboAnimScript : MonoBehaviour
{
    /// <summary>
    /// 自身のアニメーションを再生用
    /// </summary>
    Animator drawFullCombo_Animator;

    /// <summary>
    /// フルコンボ時のサイズ
    /// </summary>
    [SerializeField]
    Vector3 fullcomboSize;

    /// <summary>
    /// ドロウ時のサイズ
    /// </summary>
    [SerializeField]
    Vector3 DrawSize;



    private void Start()
    {
        // 自身のアニメーションを取得
        drawFullCombo_Animator = GetComponent<Animator>();
    }


    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Z))
        //{
        //    DrawAnimPlay(0);
        //}
        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    DrawAnimPlay(1);

        //}
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    DrawAnimPlay(2);
        //}
        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    DrawAnimPlay(3);
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha0))
        //{
        //    FullcomboAnimPlay(0);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    FullcomboAnimPlay(1);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    FullcomboAnimPlay(2);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    FullcomboAnimPlay(3);
        //}
    }



    /// <summary>
    /// キャラに合わせたドロウアニメーションを再生
    /// </summary>
    /// <param name="CharaNum"></param>
    public void DrawAnimPlay(int CharaNum)
    {
        // ドロウUIサイズに合わせる
        gameObject.transform.localScale = DrawSize;

        // キャラに合わせたアニメーションを再生用スイッチ
        switch (CharaNum)
        {
            case (int)ConstRepo.Chara.Sight:
                drawFullCombo_Animator.SetTrigger("Darw_S");
                break;

            case (int)ConstRepo.Chara.Tactile:
                drawFullCombo_Animator.SetTrigger("Darw_T");
                break;

            case (int)ConstRepo.Chara.Smell_Taste:
                drawFullCombo_Animator.SetTrigger("Darw_ST");
                break;

            case (int)ConstRepo.Chara.Hear:
                drawFullCombo_Animator.SetTrigger("Darw_H");
                break;
        }
    }


    /// <summary>
    /// キャラに合わせたフルコンボアニメーションを再生
    /// </summary>
    /// <param name="CharaNum"></param>
    public void FullcomboAnimPlay(int CharaNum)
    {
        // フルコンボUIサイズに合わせる
        gameObject.transform.localScale = fullcomboSize;

        // キャラに合わせたアニメーションを再生用スイッチ
        switch (CharaNum)
        {
            case (int)ConstRepo.Chara.Sight:
                drawFullCombo_Animator.SetTrigger("Fullcombo_S");
                break;

            case (int)ConstRepo.Chara.Tactile:
                drawFullCombo_Animator.SetTrigger("Fullcombo_T");
                break;

            case (int)ConstRepo.Chara.Smell_Taste:
                drawFullCombo_Animator.SetTrigger("Fullcombo_ST");
                break;

            case (int)ConstRepo.Chara.Hear:
                drawFullCombo_Animator.SetTrigger("Fullcombo_H");
                break;
        }
    }
}
