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
    /// �z�[���h�m�[�cSE�̍Đ��ԍ�
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
        /* �L���[�V�[�g�t�@�C���̃��[�h�҂� */
        while (CriAtom.CueSheetsAreLoading) { yield return null; }
        /* Cue���̎擾 */
        holdNotesSEExAcb = CriAtom.GetAcb("SECue");
        holdNotesSEcueInfoList = holdNotesSEExAcb.GetCueInfoList();

        /* AtomExPlayer�̐��� */
        holdNotesSEExPlayer = new CriAtomExPlayer();

    }

    /// <summary>
    /// HoldNotesSE���Đ�������Ȃ�������A�Đ�
    /// </summary>
    public void HoldNotesSEPlay()
    {
        holdNotesSEExPlayer.SetCue(holdNotesSEExAcb, holdNotesSEcueInfoList[HoldNotesSENum].name);
        holdNotesSEExPlayer.Start();

    }

    /// <summary>
    /// HoldNotesSE���Đ�����������A��~
    /// </summary>
    public void HoldNotesSEStop()
    {
        if (holdNotesSEExPlayer.GetStatus() == CriAtomExPlayer.Status.Playing)
        {
            holdNotesSEExPlayer.Stop();
        }
    }
}
