using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPreviewPlayer : MonoBehaviour
{
    private CriAtomEx.CueInfo[] SongcueInfoList;
    private CriAtomExPlayer SongPlayer;
    private CriAtomExAcb SongExAcb;
    private CriAtomExPlayback SongPlayback;

    public static MusicPreviewPlayer instance;
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
        SongExAcb = CriAtom.GetAcb("PreviewCue");
        SongcueInfoList = SongExAcb.GetCueInfoList();
        /* AtomExPlayerの生成 */
        SongPlayer = new CriAtomExPlayer(); 
        MusicPlay(0);
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

}
