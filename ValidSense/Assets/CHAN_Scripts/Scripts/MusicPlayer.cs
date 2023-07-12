using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private CriAtomEx.CueInfo[] SongcueInfoList;
    private CriAtomExPlayer SongPlayer;
    private CriAtomExAcb SongExAcb;
    private CriAtomExPlayback SongPlayback;
    public static MusicPlayer instance;
    public long PlayTime;
    public static int SongNum = 0;

    [SerializeField]
    List<ScoreScript> score;

    [SerializeField]
    LinesManager linesManager;

    public bool notMusicEnd = true;

    [SerializeField]
    SceneChangeUI sceneChangeUI;
    

    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    IEnumerator Start()
    {
        /* キューシートファイルのロード待ち */
        while (CriAtom.CueSheetsAreLoading) { yield return null; }
        /* Cue情報の取得 */
        SongExAcb = CriAtom.GetAcb("BGM");
        SongcueInfoList = SongExAcb.GetCueInfoList();

        /* AtomExPlayerの生成 */
        SongPlayer = new CriAtomExPlayer(true); 
        
    }
    private void Update() 
    {

        PlayTime = SongPlayback.GetTime();

        MusicData.Timer = PlayTime;
        MusicEndCheck();
    }


    public void MusicPlay(int num)
    {
        if(SongPlayer.GetStatus() == CriAtomExPlayer.Status.Playing) 
        {
            SongPlayer.Stop();
        }
        SongPlayer.SetCue(SongExAcb,SongcueInfoList[num].name); 
        SongPlayback = SongPlayer.Start();
    }

    public void StopPlayer()
    {
        SongPlayer.Stop();
    }

    public void MusicEndCheck()
    {
        if (SongPlayer.GetStatus() == CriAtomExPlayer.Status.PlayEnd)
        {
            if (notMusicEnd)
            {
                notMusicEnd = false;

                score[0].SetScores();
                score[1].SetScores();

                StartCoroutine(linesManager.LineJudgmentPerformance());

                StartCoroutine(ToNextScene());
            }

        }
    }

    IEnumerator ToNextScene()
    {

        yield return new WaitForSeconds(8.8f);

        sceneChangeUI.SceneEndAnim();

        yield return new WaitForSeconds(0.6f);

        //　勝敗条件「レーンの所持数」＞「スコアの量」

        // 1pが相手より多くレーンを多く奪っていれば
        if (score[(int)ConstRepo.Player.P1].stolenLaneCount
                 > score[(int)ConstRepo.Player.P2].stolenLaneCount)
        {
            yield return new WaitForSeconds(3f);
            GameObject.Find("SceneManager").SendMessage("SetScene", GameScene.Result_1P);
        }
        // 2pが相手より多くレーンを多く奪っていれば
        else if (score[(int)ConstRepo.Player.P1].stolenLaneCount
                     < score[(int)ConstRepo.Player.P2].stolenLaneCount)
        {
            yield return new WaitForSeconds(3f);
            GameObject.Find("SceneManager").SendMessage("SetScene", GameScene.Result_2P);
        }
        // 1P,2P共に、レーンの所持数が同じであれば
        else
        {
            //1pのスコアが2pを上回っていたら
            if (score[(int)ConstRepo.Player.P1].scoreValue 
                > score[(int)ConstRepo.Player.P2].scoreValue)
            {
                yield return new WaitForSeconds(3f);
                GameObject.Find("SceneManager").SendMessage("SetScene", GameScene.Result_1P);
            }
            //2pのスコアが1pを上回っていたら
            else if (score[(int)ConstRepo.Player.P1].scoreValue
                < score[(int)ConstRepo.Player.P2].scoreValue)
            {
                yield return new WaitForSeconds(3f);
                GameObject.Find("SceneManager").SendMessage("SetScene", GameScene.Result_2P);
            }
            // 同点なら
            else 
            {
                GameObject.Find("SceneManager").SendMessage("SetScene", GameScene.Result_1P);
            }
        }

    }

}
