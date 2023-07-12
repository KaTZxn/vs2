using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result_Live2DController : MonoBehaviour
{
    /// <summary>
    /// プレイヤーの情報を持ってる子に参照する用
    /// </summary>
    [SerializeField]
    PlayerManagerScript sceneManager;

    /// <summary>
    /// Live2Dに参照する用(キャラ)
    /// </summary>
    [SerializeField]
    List<GameObject> live2D;

    /// <summary>
    /// 使用者を設定する用
    /// </summary>
    [SerializeField]
    int usePlayer;


    // Start is called before the first frame update
    void Start()
    {
        // プレイヤーの情報を持ってる子に参照する用
        sceneManager = GameObject.Find("SceneManager").GetComponent<PlayerManagerScript>();

        //使用プレイヤーのキャラだけを表示
        ShowLive2D(sceneManager.playerCharaNum[usePlayer]);


        // 同点じゃなかったら
        if(sceneManager.score[(int)ConstRepo.Player.P1]
            != sceneManager.score[(int)ConstRepo.Player.P2])
        {
            // 勝利アニメーションと、voiceを再生
            StartCoroutine(WinPlayerAnim());
        }

    }


    /// <summary>
    /// 使用プレイヤーのキャラだけを表示
    /// </summary>
    /// <param name="charaNum"></param>
    void ShowLive2D(int charaNum)
    {
        for (int i = 0; i < live2D.Count;) 
        {
            if(i == charaNum)
            {
                live2D[i].SetActive(true);


            }
            else
            {
                live2D[i].SetActive(false);
            }
            
            i++;
        }
    }

    /// <summary>
    /// 勝利アニメーションと、voiceを再生
    /// </summary>
    /// <returns></returns>
    IEnumerator WinPlayerAnim()
    {
        yield return new WaitForSeconds(1);

        // リザルトの表示回数が 0 (勝利プレイヤーが基本的に先に表示されるため)
        if (sceneManager.resultCount == 0)
        {
            // アニメーション再生
            live2D[sceneManager.playerCharaNum[usePlayer]]
                .GetComponent<Live2D_AnimController>().Anim_Choice();

            // 音声の切替(勝った数で音声の切替)
            if (sceneManager.playerWinCount[usePlayer] == 0)
            {
                live2D[sceneManager.playerCharaNum[usePlayer]]
                .GetComponent<CharaVoicePlayer>().VoiceOneShot(2);

                // 次の音声にする
                sceneManager.playerWinCount[usePlayer]++;
            }else
            {
                live2D[sceneManager.playerCharaNum[usePlayer]]
                .GetComponent<CharaVoicePlayer>().VoiceOneShot(1);

                // 初期化
                sceneManager.playerWinCount[usePlayer] = 0;
            }

        }
    }
}
