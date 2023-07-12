using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaTest : MonoBehaviour
{
    private enum Chara
    {
        Sight = 1,
        Hear,
        Tactile,
        Smell
    }
    [SerializeField]
    private int playerNum = 1;
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
        atomExAcb = CriAtom.GetAcb("CueSheet_Hear");
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
            if (GUI.Button(new Rect(0, (Screen.height / cueInfoList.Length) * i, 200, Screen.height / cueInfoList.Length), cueInfoList[i].name)) {
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
