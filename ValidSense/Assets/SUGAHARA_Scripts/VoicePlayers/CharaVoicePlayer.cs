using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaVoicePlayer : MonoBehaviour
{
    private CriAtomEx.CueInfo[] cueInfoList;
    private CriAtomExPlayer ExPlayer;
    private CriAtomExAcb ExAcb;

    [SerializeField]
    private string charaName;


    IEnumerator Start()
    {
        /* キューシートファイルのロード待ち */
        while (CriAtom.CueSheetsAreLoading) { yield return null; }
        /* Cue情報の取得 */
        ExAcb = CriAtom.GetAcb($"CueSheet_{charaName}");
        cueInfoList = ExAcb.GetCueInfoList();

        /* AtomExPlayerの生成 */
        ExPlayer = new CriAtomExPlayer();

    }
    public void VoiceOneShot(int VoiceNum)
    {
        Debug.Log("Voice"+ VoiceNum);
        ExPlayer.SetCue(ExAcb, cueInfoList[VoiceNum].name);
        ExPlayer.Start();
    }
}

