using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    /// <summary>
    /// プレイヤーのスコア記憶用
    /// </summary>
    public int scoreValue;

    /// <summary>
    /// このオブジェクトを使用するプレイヤーの番号
    /// </summary>
    [SerializeField]
    private int _usePlayer;

    /// <summary>
    /// プレイヤーの入力成功をカウントする
    /// </summary>
    [SerializeField]
    public int totalHitsNum;

    /// <summary>
    /// プレイヤーのレーン奪取をカウントする
    /// </summary>
    public int stolenLaneCount;

    /// <summary>
    /// 自身のテキスト干渉用
    /// </summary>
    private Text _scoreValueText;


    [SerializeField]
    GameObject sceneManager;

    [SerializeField]
    CrownUIManager crownUIManager;

    [SerializeField]
    Text scoreText;



    private void Start()
    {
        // scoreText に自身を入れる
        _scoreValueText = GetComponent<Text>();

        // scoreの表示の初期化
        _scoreValueText.text = scoreValue.ToString();

        sceneManager = GameObject.Find("SceneManager");
    }


    public void ScoreUpdate()
    {
        _scoreValueText.text = scoreValue.ToString();


        crownUIManager.ShowCrownUI();
    }

    public void SetScores()
    {
        sceneManager.GetComponent<PlayerManagerScript>().score[_usePlayer] = scoreValue;
        sceneManager.GetComponent<PlayerManagerScript>().totalHitsNum [_usePlayer] = totalHitsNum;
        sceneManager.GetComponent<PlayerManagerScript>().stolenLane[_usePlayer] = stolenLaneCount;
    }

    /// <summary>
    /// スコア周りのUIの表示
    /// </summary>
    public void HideScoreUIs()
    {
        _scoreValueText.color -= new Color(0, 0, 0, 1f);
        scoreText.color -= new Color(0,0,0,1f);
    }
}
