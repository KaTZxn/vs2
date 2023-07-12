using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEPlayer : MonoBehaviour
{
    private CriAtomEx.CueInfo[] SEcueInfoList;
    private CriAtomExPlayer SEExPlayer;
    private CriAtomExAcb SEExAcb;
    public static SEPlayer instance;
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
        SEExAcb = CriAtom.GetAcb("SECue");
        SEcueInfoList = SEExAcb.GetCueInfoList();

        /* AtomExPlayerの生成 */
        SEExPlayer = new CriAtomExPlayer();    
        
    }
    public void SEOneShot(int SENum)
    {
        SEExPlayer.SetCue(SEExAcb,SEcueInfoList[SENum].name);
        SEExPlayer.Start();
    }
}
