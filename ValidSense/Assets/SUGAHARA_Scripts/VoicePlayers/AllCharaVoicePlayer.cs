using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllCharaVoicePlayer : MonoBehaviour
{
    [SerializeField]
    List<CharaVoiceOnlyClass> charaVoices;

    public void OnShot_CharaVoice(int charaNum, int voiceNum)
    {
        // キャラの配列内だったら
        if((int)ConstRepo.Chara.Sight <= charaNum
            && charaNum <= (int)ConstRepo.Chara.Hear)
        {
            charaVoices[charaNum].VoiceOneShot(voiceNum);
        }
        // キャラの配列外だったら
        else
        {
            // ランダムなキャラでボイス再生
            charaNum = Random.Range((int)ConstRepo.Chara.Sight, (int)ConstRepo.Chara.Hear + 1);
            charaVoices[charaNum].VoiceOneShot(voiceNum);
            
        }
    }

}
