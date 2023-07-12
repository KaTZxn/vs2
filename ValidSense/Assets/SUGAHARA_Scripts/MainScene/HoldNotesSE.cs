using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldNotesSE : MonoBehaviour
{
    private CriAtomEx.CueInfo[] holdNotesSEcueInfoList;
    private CriAtomExPlayer holdNotesSEExPlayer;
    private CriAtomExAcb holdNotesSEExAcb;
    public HoldNotesSE holdNotesSEPlayer;


    /// <summary>
    /// ホールドノーツSEの再生番号
    /// </summary>
    private int HoldNotesSENum = 2;

    private void Awake()
    {
        if (holdNotesSEPlayer == null)
        {
            holdNotesSEPlayer = this;
        }
    }
    IEnumerator Start()
    {
        /* キューシートファイルのロード待ち */
        while (CriAtom.CueSheetsAreLoading) { yield return null; }
        /* Cue情報の取得 */
        holdNotesSEExAcb = CriAtom.GetAcb("SECue");
        holdNotesSEcueInfoList = holdNotesSEExAcb.GetCueInfoList();

        /* AtomExPlayerの生成 */
        holdNotesSEExPlayer = new CriAtomExPlayer();

    }

    /// <summary>
    /// HoldNotesSEを再生中じゃなかったら、再生
    /// </summary>
    public void HoldNotesSEPlay()
    {
        holdNotesSEExPlayer.SetCue(holdNotesSEExAcb, holdNotesSEcueInfoList[HoldNotesSENum].name);
        holdNotesSEExPlayer.Start();

    }

    /// <summary>
    /// HoldNotesSEを再生中だったら、停止
    /// </summary>
    public void HoldNotesSEStop()
    {
        if (holdNotesSEExPlayer.GetStatus() == CriAtomExPlayer.Status.Playing)
        {
            holdNotesSEExPlayer.Stop();
        }
    }
}
