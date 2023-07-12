using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainBGMPlayer : MonoBehaviour
{
    //      チン君のスクリプトの見よう見まねで作成…
    // タイトル画面のBGM再生


    private CriAtomEx.CueInfo[] SongcueInfoList;
    private CriAtomExPlayer SongPlayer;
    private CriAtomExAcb SongExAcb;

    public static MainBGMPlayer instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    IEnumerator Start()
    {
        /* キューシートファイルのロード待ち */
        while (CriAtom.CueSheetsAreLoading) { yield return null; }
        /* Cue情報の取得 */
        SongExAcb = CriAtom.GetAcb("TitleCue");
        SongcueInfoList = SongExAcb.GetCueInfoList();
        /* AtomExPlayerの生成 */
        SongPlayer = new CriAtomExPlayer();


        if (SceneManager.GetActiveScene().name == "CharaSelect")
        {
            MusicPlay(0);
        }
        else if (SceneManager.GetActiveScene().name == "Title")
        {
            MusicPlay(2);
        }
    }

    public void MusicPlay(int num)
    {
        if (SongPlayer.GetStatus() == CriAtomExPlayer.Status.Playing)
        {
            SongPlayer.Stop();
        }
        SongPlayer.SetCue(SongExAcb, SongcueInfoList[num].name);
        SongPlayer.Start();
    }

    public void StopPlayer()
    {
        SongPlayer.Stop();
    }

}
