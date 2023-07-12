using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultBGMPlayer : MonoBehaviour
{
    private CriAtomEx.CueInfo[] SongcueInfoList;
    private CriAtomExPlayer SongPlayer;
    private CriAtomExAcb SongExAcb;

    public static ResultBGMPlayer instance;
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
        SongExAcb = CriAtom.GetAcb("ResultBGM");
        SongcueInfoList = SongExAcb.GetCueInfoList();
        /* AtomExPlayerの生成 */
        SongPlayer = new CriAtomExPlayer(); 
    }

    public void MusicPlay(int num)
    {
        if(SongPlayer.GetStatus() == CriAtomExPlayer.Status.Playing) 
        {
            SongPlayer.Stop();
        }
        SongPlayer.SetCue(SongExAcb,SongcueInfoList[num].name); 
        SongPlayer.Start();
    }

    public void StopPlayer()
    {
        SongPlayer.Stop();
    }
}
