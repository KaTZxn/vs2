using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleTimer : MonoBehaviour
{
    /// <summary>
    /// 自身のテキストを取得用
    /// </summary>
    Text timerText;

    /// <summary>
    /// タイマー最大値
    /// </summary>
    [SerializeField]
    public float timeMaxValue;

    /// <summary>
    /// タイマーの数値
    /// </summary>
    private float timeValue;


    /// <summary>
    /// タイマーのカウントダウンの制御用
    /// </summary>
    [SerializeField]
    private bool _canCountDown;

    [SerializeField]
    Move_Entry moveEntry;


    [SerializeField]
    TutorialAtendUIManager tutorialAtendUIManager;

    [SerializeField]
    Title_PlayerInput titlePlayerInput;

    [SerializeField]
    SceneChangeUI sceneChangeUI;


    // Start is called before the first frame update
    void Start()
    {
        // 自身を取得
        timerText = gameObject.GetComponent<Text>();

        // タイマーを初期表記
        TextUpdate();
    }

    private void Update()
    {
        if (_canCountDown)
        {
            if (timeValue >= 0)
            {
                TextUpdate();
                timeValue -= Time.deltaTime;
            }
            else
            {
                timeValue = 0;
                TextUpdate();

                TitleReset();
            }
        }
    }

    void TextUpdate()
    {
        timerText.text = timeValue.ToString("F0");
    }


    /// <summary>
    /// カウントダウンの停止を命令する関数
    /// </summary>
    public void CountDownStop()
    {
        _canCountDown = false;
    }


    /// <summary>
    /// タイマーのカウントダウンをスタートする用
    /// </summary>
    public void CountDownStart()
    {
        _canCountDown = true;
    }

    /// <summary>
    /// タイマーの値をリセットする用
    /// </summary>
    public void SWTimerReset()
    {
        timeValue = timeMaxValue;
        TextUpdate();
    }

    /// <summary>
    /// タイトル画面をリセット
    /// </summary>
    void TitleReset()
    {
        _canCountDown = false;

        moveEntry.ResetUI_Input();

        tutorialAtendUIManager.AllHaveUI_Reset();

        titlePlayerInput.SetFalse_CanPlayerInput();

        sceneChangeUI.SceneStartAnim();
    }
}
