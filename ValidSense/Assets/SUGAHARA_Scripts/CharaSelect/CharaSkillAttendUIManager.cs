using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaSkillAttendUIManager : MonoBehaviour
{

    /// <summary>
    /// キャラのスキルの名前を表示するイメージ
    /// </summary>
    [SerializeField]
    private SpriteRenderer skillName;

    /// <summary>
    /// キャラのスキルの名前を表示するイメージに使用する、素材たち
    /// </summary>
    [SerializeField]
    private List<Sprite> skillName_Sprites;


    /// <summary>
    /// キャラのスキルの説明を表示するイメージ
    /// </summary>
    [SerializeField]
    private Image skillAttend;

    /// <summary>
    /// キャラのスキルの説明を表示するイメージに参照する用（位置移動をする）
    /// </summary>
    [SerializeField]
    GameObject skillAttendObj;

    /// <summary>
    /// キャラのスキルの説明を表示するイメージに使用する、素材たち
    /// </summary>
    [SerializeField]
    private List<Sprite> skillAttend_Sprites;

    /// <summary>
    /// スキルの説明画像の移動速度
    /// </summary>
    [SerializeField]
    private float skillAttendSpeed;

    /// <summary>
    /// スキルの説明画像の限界高度（それ以上になったらポジションリセット）
    /// </summary>
    [SerializeField]
    private float[] skillAttend_PosYMax;


    /// <summary>
    /// スキルのリセットポジションの上側
    /// </summary>
    [SerializeField]
    private float skillAttendUpper_PosY;


    [SerializeField]
    int charaNumNow;

    [SerializeField]
    float changeUI_SetPos;

    // Start is called before the first frame update
    void Start()
    {
        SkillAttendUI_ResetPosY_Upper();
    }

    private void FixedUpdate()
    {
        // 縦軸のみ、位置を加算
        skillAttendObj.transform.position += new Vector3(0, skillAttendSpeed, 0);

        // 上に行きすぎたら、ポジションをリセット
        if(skillAttendObj.transform.localPosition.y > skillAttend_PosYMax[charaNumNow])
        {
            SkillAttendUI_ResetPosY_Under();
        }
    }


    /// <summary>
    /// スキルの説明画像の位置を下にリセットする
    /// </summary>
    void SkillAttendUI_ResetPosY_Under()
    {
        skillAttendObj.transform.position = new Vector3(
            skillAttendObj.transform.position.x, 0, skillAttendObj.transform.position.z);
    }

    /// <summary>
    /// スキルの説明画像の位置を上にリセットする
    /// </summary>
    void SkillAttendUI_ResetPosY_Upper()
    {
        skillAttendObj.transform.position = new Vector3(
            skillAttendObj.transform.position.x, skillAttendUpper_PosY, skillAttendObj.transform.position.z);
    }


    /// <summary>
    /// 引数のキャラにあった、画像に差し替え
    /// </summary>
    /// <param name="charaNum"></param>
    public void SkillName_Attend_SetSprite(int charaNum)
    {
        //キャラ番号の記憶
        charaNumNow = charaNum;

        // 名前画像の差し替え
        skillName.sprite = skillName_Sprites[charaNum];

        // 説明画像の差し替え
        skillAttend.sprite = skillAttend_Sprites[charaNum];

        // 画像データのサイズ調整
        skillAttend.SetNativeSize();

        //新しい説明文のため、ポジションのリセット
        skillAttendObj.transform.position = new Vector3(
            skillAttendObj.transform.position.x, changeUI_SetPos, skillAttendObj.transform.position.z);
    }
}
