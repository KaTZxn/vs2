using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaSet : MonoBehaviour
{
    [SerializeField]
    List<CharaSelect_CahraClass> charaClassList;
    public PlayerManagerScript playerManagerScript;
    // Start is called before the first frame update
    void Start()
    {
        playerManagerScript = GameObject.Find("SceneManager").GetComponent<PlayerManagerScript>();
        //初期表示更新 表示キャラクター
        charaClassList[(int)ConstRepo.Player.P1]
            .CharaUI_1P_DisplaySwitching(playerManagerScript.playerCharaNum[(int)ConstRepo.Player.P1]);
        charaClassList[(int)ConstRepo.Player.P2]
            .CharaUI_1P_DisplaySwitching(playerManagerScript.playerCharaNum[(int)ConstRepo.Player.P2]);
    }

}
