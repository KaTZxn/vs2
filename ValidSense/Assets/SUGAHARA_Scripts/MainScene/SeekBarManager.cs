using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekBarManager : MonoBehaviour
{
    /// <summary>
    ///  seek_BarPointEffect�ɎQ�Ƃ���p
    /// </summary>
    [SerializeField]
    SpriteRenderer seek_BarPointEffect;

    /// <summary>
    ///  seek_BarPoint�ɎQ�Ƃ���p
    /// </summary>
    [SerializeField]
    GameObject seek_BarPoint;

    /// <summary>
    ///  BarStart(1P)�ɎQ�Ƃ���p
    /// </summary>
    [SerializeField]
    SpriteRenderer seek_BarStart;

    /// <summary>
    ///  BarEnd(2P)�ɎQ�Ƃ���p
    /// </summary>
    [SerializeField]
    SpriteRenderer seek_BarEnd;


    /// <summary>
    /// �A���t�@�l����p
    /// </summary>
    [SerializeField]
    float alphaValueMax;

    /// <summary>
    /// �A���t�@�̍ő�l�ŏ��l�J�E���g�p
    /// </summary>
    [SerializeField]
    int alpha_MinMaxCount;

    /// <summary>
    /// �o�[����߂邽�߂̃J���[
    /// </summary>
    [SerializeField]
    string[] barColorCode;

    /// <summary>
    /// �v���C���[�}�l�[�W���[�擾����p
    /// </summary>
    PlayerManagerScript playerManagerScript;

    /// <summary>
    /// �J���[�R�[�h�󂯓n���p
    /// </summary>
    Color colorCode;

    [SerializeField]
    float moveDistance;

    [SerializeField]
    float[] musicEndTimes;

    // �y�Ȕԍ��擾�p
    int musicNum;

    [SerializeField]
    float totalMoveValue;

    private void Start()
    {
        //�v���C���[�}�l�[�W���[�擾
        playerManagerScript = GameObject.Find("SceneManager").GetComponent<PlayerManagerScript>();

        musicNum = GameObject.Find("Json").GetComponent<JsonReader>()._songList.songdata.songnum;


        // start�̕���1p�L�����ɍ������A�F��t����
        if (ColorUtility.TryParseHtmlString(
            barColorCode[playerManagerScript.playerCharaNum[(int)ConstRepo.Player.P1]], out colorCode))
        {
            seek_BarStart.color = colorCode;
        }

        // End�̕���1p�L�����ɍ������A�F��t����
        if (ColorUtility.TryParseHtmlString(
            barColorCode[playerManagerScript.playerCharaNum[(int)ConstRepo.Player.P2]], out colorCode))
        {
            seek_BarEnd.color = colorCode;
        }
    }



    private void FixedUpdate()
    {
        // �Ȃ��A�N�e�B�u��������
        if (MusicPlayer.instance.PlayTime >= 0)
        {
            //  �o�[�|�C���g�G�t�F�N�g���A�t�F�[�h�C���A�t�F�[�h�A�E�g���J��Ԃ�
            BarPointEffect_Flashing();

            // �o�[�|�C���g
            BarPointMove(Time.deltaTime);
        }

    }


    /// <summary>
    ///  �t�F�[�h�C���A�t�F�[�h�A�E�g���J��Ԃ�
    /// </summary>
    void BarPointEffect_Flashing()
    {
        // �J�E���g������āA������A���Q�̗]��ɂ���āA�A�����𕪂���
        if (alpha_MinMaxCount % 2 == 0)
        {
            // �s�����ɂ��Ă���
            seek_BarPointEffect.color += new Color(0f, 0f, 0f, Time.deltaTime);
        }
        else
        {
            // �����ɂ��Ă���
            seek_BarPointEffect.color -= new Color(0f, 0f, 0f, Time.deltaTime);
        }

        // �J���[�̃A���t�@�l���iMAX�̒l�j�ȏゾ������A1�ɑ�����
        if (seek_BarPointEffect.color.a >= alphaValueMax)
        {
            // �s�����x��ݒ肵���l�ɂ���
            seek_BarPointEffect.color = new Color(
                seek_BarPointEffect.color.r, seek_BarPointEffect.color.g, seek_BarPointEffect.color.b,
                alphaValueMax);

            // �J�E���g
            alpha_MinMaxCount++;
        }
        // �J���[�̃A���t�@�l���iMIN�̒l�j�ȉ���������A0�ɑ�����
        else if (seek_BarPointEffect.color.a <= 0)
        {
            // ���S�ɓ����ɂ���
            seek_BarPointEffect.color = new Color(
                seek_BarPointEffect.color.r, seek_BarPointEffect.color.g, seek_BarPointEffect.color.b,
                0);

            // �J�E���g
            alpha_MinMaxCount = 0;
        }
    }

    /// <summary>
    /// �o�[�|�C���g�̈ړ�
    /// </summary>
    void BarPointMove(float time)
    {
        //moveDistance musicEndTimes

        // �ړ��������Ȃ��悤�ɏ���
        if(totalMoveValue < moveDistance)
        {
            // 1�t���[�����̈ړ��������A�Ԃ�l�Ŗ���āA�ړ�
            seek_BarPoint.transform.position += new Vector3(MoveValue(time), 0, 0);

            // �ړ��ʂ̗݌v
            totalMoveValue += MoveValue(time);
        }
    }

    /// <summary>
    /// �ړ��ʊ���o���p
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    float MoveValue(float time)
    {
        //Debug.Log(moveDistance / (musicEndTimes[musicNum] / time));

        // �V�[�N�o�[�̃X�^�[�g����A�I���܂ł̋����@/ �Ȃ̏I���̎��ԁ@/�@�O�̃t���[���̎��Ԃ��擾
        return (moveDistance / (musicEndTimes[musicNum] / time));
    }

    /// <summary>
    /// �V�[�N�o�[����̌����ڂ�����
    /// </summary>
    public void AllSeekBarObjHide()
    {
        seek_BarPointEffect.sprite = null;
        seek_BarStart.sprite = null;
        seek_BarEnd.sprite = null;
        seek_BarPoint.GetComponent<SpriteRenderer>().sprite = null;
    }
}
