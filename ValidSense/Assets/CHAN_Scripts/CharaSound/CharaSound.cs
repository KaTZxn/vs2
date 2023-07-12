using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharaSound : MonoBehaviour
{
    private CriAtomEx.CueInfo[] cueInfoList;
    private CriAtomExPlayer atomExPlayer;
    private CriAtomExAcb atomExAcb;

    IEnumerator Start()
    {
        /* キューシートファイルのロード待ち */
        while (CriAtom.CueSheetsAreLoading) {
            yield return null;
        }

        /* AtomExPlayerの生成 */
        atomExPlayer = new CriAtomExPlayer();

        /* Cue情報の取得 */
        atomExAcb = CriAtom.GetAcb("CueSheet_Smell");
        cueInfoList = atomExAcb.GetCueInfoList();
    }
    private void OnDestroy()
    {
        atomExPlayer.Dispose();
    }

    void OnGUI()
    {
        /* キュー名再生ボタンの生成 */
        for (int i = 0; i < cueInfoList.Length; i++) {
            if (GUI.Button(new Rect(Screen.width - 200, (Screen.height / cueInfoList.Length) * i, 200, Screen.height / cueInfoList.Length), cueInfoList[i].name)) {
                /* 再生中の場合は停止 */
                if(atomExPlayer.GetStatus() == CriAtomExPlayer.Status.Playing) {
                    atomExPlayer.Stop();
                }
                atomExPlayer.SetCue(atomExAcb, cueInfoList[i].name);
                atomExPlayer.Start();
            }
        }
    }
}