using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene_Player : MonoBehaviour
{
    private CriAtomEx.CueInfo[] EPcueInfoList;
    private CriAtomExPlayer EPExPlayer;
    private CriAtomExAcb EPExAcb;
    public static EndScene_Player instance;
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
        EPExAcb = CriAtom.GetAcb("ResultBGM");
        EPcueInfoList = EPExAcb.GetCueInfoList();

        /* AtomExPlayerの生成 */
        EPExPlayer = new CriAtomExPlayer();

    }
    public void BGOneShot(int SENum)
    {
        EPExPlayer.SetCue(EPExAcb, EPcueInfoList[SENum].name);
        EPExPlayer.Start();
    }

    public void BGStop()
    {
        EPExPlayer.Stop();
    }
}
