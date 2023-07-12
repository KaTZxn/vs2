using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManagerScript : MonoBehaviour 
{
    /// <summary>
    /// プレイヤーの勝利数記憶用
    /// </summary>
    
    public int[] playerWinCount = { 0, 0 };
    /// <summary>
    /// プレイヤー選択キャラクター記憶用
    /// </summary>
    public int[] playerCharaNum = { 0, 0 };
    /// <summary>
    /// 現在選択楽曲番号記憶用
    /// </summary>
    //public int[] nowMusicNum = { 0, 0 }; jsonDataに保存してあるため使用しなくなる予定
    /// <summary>
    /// 難易度記憶用
    /// </summary>
    public int[] nowMusicLevel = { 0, 0 };
    /// <summary>
    /// スコア記憶用
    /// </summary>
    public int[] score = { 0, 0 };
    /// <summary>
    /// 奪ったレーン数記憶用
    /// </summary>
    public int[] stolenLane = { 0, 0 };
    /// <summary>
    /// 総打数記憶用
    /// </summary>
    public int[] totalHitsNum = { 0, 0 };
    /// <summary>
    /// 勝利キャラクター記憶用
    /// </summary>
    public int winCharaNum;
    /// <summary>
    /// リザルト画面のカウント用
    /// </summary>
    public int resultCount;

    /// <summary>
    /// プレイヤーがフルコンボかどうか ※ゲームメイン部分のJudgeマネージャーで初期化＆操作
    /// </summary>
    public bool[] isPlayerFullcombo;
}
