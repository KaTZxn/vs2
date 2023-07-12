using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect_BGMPlayer : MonoBehaviour
{
    private CriAtomEx.CueInfo[] levelSelectBGM_CueInfoList;
    private CriAtomExPlayer levelSelectBGM_SongPlayer;
    private CriAtomExAcb levelSelectBGM_SongExAcb;

    public static LevelSelect_BGMPlayer instance;


    JsonReader jreader; 

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
        levelSelectBGM_SongExAcb = CriAtom.GetAcb("PreviewCue");
        levelSelectBGM_CueInfoList = levelSelectBGM_SongExAcb.GetCueInfoList();
        /* AtomExPlayerの生成 */
        levelSelectBGM_SongPlayer = new CriAtomExPlayer();
        

        //if (SceneManager.GetActiveScene().name == "CharaSelect")
        //    MusicPlay(0);
        //else if (SceneManager.GetActiveScene().name == "Title")
        //    MusicPlay(1);

        jreader = GameObject.Find("Json").GetComponent<JsonReader>();

        MusicPlay(jreader._songList.songdata.songnum);
    }


    public void MusicPlay(int num)
    {
        if (levelSelectBGM_SongPlayer.GetStatus() == CriAtomExPlayer.Status.Playing)
        {
            levelSelectBGM_SongPlayer.Stop();
        }
        levelSelectBGM_SongPlayer.SetCue(levelSelectBGM_SongExAcb, levelSelectBGM_CueInfoList[num].name);
        levelSelectBGM_SongPlayer.Start();
    }

    public void StopPlayer()
    {
        levelSelectBGM_SongPlayer.Stop();
    }

    private void OnDestroy()
    {
        StopPlayer();
    }
}
