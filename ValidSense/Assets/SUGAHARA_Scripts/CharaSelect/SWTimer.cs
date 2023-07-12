using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SWTimer : MonoBehaviour
{
    /// <summary>
    /// 遷移用
    /// </summary>
    [SerializeField]
    Test sceneManager;

    /// <summary>
    /// 遷移先を決める値
    /// </summary>
    [SerializeField]
    private int toNextSceneNam; 


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
    /// シーン遷移までの猶予時間
    /// </summary>
    [SerializeField]
    private int toNextStndby;

    /// <summary>
    /// タイマーのカウントダウンの制御用
    /// </summary>
    [SerializeField]
    private bool _canCountDown;

    // Start is called before the first frame update
    void Start()
    {
        sceneManager = GameObject.Find("SceneManager").GetComponent<Test>();

        timerText = gameObject.GetComponent<Text>();

        SWTimerReset();
    }

    private void Update()
    {
        if(_canCountDown)
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
                Invoke("ToNextScene", toNextStndby);
            }
        }
    }

    void TextUpdate()
    {
        timerText.text = timeValue.ToString("F0");
    }

    void ToNextScene()
    {
        switch(toNextSceneNam)
        {
            case 0:
                sceneManager.ToTitleScene();
                break;

            default:
                // 配列外は、何もしない
                break;
        }
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
        timeValue =  timeMaxValue;
    }
}
