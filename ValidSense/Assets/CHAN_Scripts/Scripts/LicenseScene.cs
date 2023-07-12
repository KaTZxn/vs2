using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LicenseScene : MonoBehaviour
{
    ///// <summary>
    ///// �\������I�u�W�F�N�g
    ///// </summary>
    //public GameObject criware,otoLogic,QR;

    ///// <summary>
    ///// �V�[���J�ڋ���
    ///// </summary>
    //bool canToNextScene = false;

    /// <summary>
    /// �L�����{�C�X���Đ��p
    /// </summary>
    [SerializeField]
    AllCharaVoicePlayer allCharaVoicePlayer;

    /// <summary>
    /// �Đ�����{�C�X��ݒ�p
    /// </summary>
    private int playVoiceNum;

    /// <summary>
    /// �v���C���[�̃X�e�[�^�X���Q�Ƃ���p
    /// </summary>
    [SerializeField]
    PlayerManagerScript playerManagerScript;

    /// <summary>
    /// �V�[���J�ڒ��O�ɍĐ����鉉�o
    /// </summary>
    [SerializeField]
    SceneChangeUI sceneChangeUI;

    /// <summary>
    /// ���ʐ���c�[���̃��C�Z���X�e�L�X�g�\�L
    /// </summary>
    [SerializeField]
    Text tool_License;

    /// <summary>
    /// BGM SE�̃��C�Z���X�e�L�X�g�\�L
    /// </summary>
    [SerializeField]
    Text se_BGM_License;

    /// <summary>
    /// CriWare�̕\�L�摜
    /// </summary>
    [SerializeField]
    Image criWare_Logo;

    /// <summary>
    /// CriWare�̃��C�Z���X�e�L�X�g�\�L
    /// </summary>
    [SerializeField]
    Text criWare_License;

    /// <summary>
    /// cubism_Log�̕\�L�摜
    /// </summary>
    [SerializeField]
    Image cubism_Logo;

    /// <summary>
    /// live2D_Logo�̕\�L�摜
    /// </summary>
    [SerializeField]
    Image live2D_Logo;

    /// <summary>
    /// �t�F�[�h�̃X�s�[�h
    /// </summary>
    [SerializeField]
    float fadeSppd;
    
    /// <summary>
    /// �t�F�[�h�A�E�g�܂ł̎���
    /// </summary>
    [SerializeField]
    float showUITime;

    // Start is called before the first frame update
    void Start()
    {
        // �v���C���[�C���v�b�g�̎擾
        playerManagerScript = GameObject.Find("SceneManager").GetComponent<PlayerManagerScript>();

        ////�\���̐؂�ւ�
        //criware.SetActive(true);
        //otoLogic.SetActive(false);
        //QR.SetActive(false);
        
        // �����l�\��
        AddLicense_FirstUI_Color(new Color(0f, 0f, 0f, -1));
        AddLicense_SecondUI_Color(new Color(0f, 0f, 0f, -1));


        //// �V�[���J�ڒ��O�A�j���[�V�������Đ�����
        //Invoke("PlayScnenChangeAnim", 1.2f);

        ////�V�[���J��
        //Invoke("ChangeScene", 1.6f);

        // UI���X�V��������
        StartCoroutine(License_FirstUI_FadeIn());

        ////���̕\���̕\���̐؂�ւ��ǂݍ���
        //Invoke("Change",1.5f);

        Invoke("WinCharaVoicePlay",0.8f);
    }


    //private void Change()
    //{
    //    //�\���̐؂�ւ�
    //    criware.SetActive(false);
    //    otoLogic.SetActive(true);
    //    QR.SetActive(false);
    //    //Invoke("ShowQR", 3.2f);
        


    //    //canToNextScene = true;
    //}

    //void ShowQR()
    //{
    //    // �����v���C���[�L�����̃A���P�[�g���̓{�C�X�Đ�
    //    allCharaVoicePlayer.OnShot_CharaVoice(playerManagerScript.winCharaNum, 4);

    //    criware.SetActive(false);
    //    otoLogic.SetActive(false);
    //    QR.SetActive(true);

    //    canToNextScene = true;
    //}

    //private void Update()
    //{
    //    if(canToNextScene)
    //    {
    //        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R) ||
    //            Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.F))
    //        {
    //            if (Input.GetKey(KeyCode.P) || Input.GetKey(KeyCode.O) || Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.U) ||
    //                Input.GetKey(KeyCode.L) || Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.J))
    //            {
    //                canToNextScene = false;
    //                // �����v���C���[�̃L�����̊��Ӄ{�C�X�Đ�
    //                allCharaVoicePlayer.OnShot_CharaVoice(playerManagerScript.winCharaNum, 5);

    //                Invoke("ChangeScene", 3f);
    //            }
    //        }

    //        if (Input.GetKey(KeyCode.P) || Input.GetKey(KeyCode.O) || Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.U) ||
    //            Input.GetKey(KeyCode.L) || Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.J))
    //        {
    //            if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R) ||
    //                Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.F))
    //            {
    //                canToNextScene = false;
    //                // �����v���C���[�̃L�����̊��Ӄ{�C�X�Đ�
    //                allCharaVoicePlayer.OnShot_CharaVoice(playerManagerScript.winCharaNum, 5);

    //                Invoke("ChangeScene", 3f);
    //            }
    //        }

    //    }




    //}


    /// <summary>
    /// �V�[���J�ڒ��O�ɍĐ������A�j���[�V����
    /// </summary>
    private void PlayScnenChangeAnim()
    {
        sceneChangeUI.SceneEndAnim();
    }

    /// <summary>
    /// �V�[�������ɂ���
    /// </summary>
    private void ChangeScene()
    {
        GameObject.Find("SceneManager").SendMessage("SetScene", GameScene.Title);
    }

    /// <summary>
    /// �ŏ��ɕ\�������Ui�̐F��������p
    /// </summary>
    /// <param name="addColor"></param>
    void AddLicense_FirstUI_Color(Color addColor)
    {
        criWare_License.color += addColor;
        criWare_Logo.color += addColor;
        cubism_Logo.color += addColor;
        live2D_Logo.color += addColor;
    }

    /// <summary>
    /// ���ڂɂɕ\�������Ui�̐F��������p
    /// </summary>
    /// <param name="addColor"></param>
    void AddLicense_SecondUI_Color(Color addColor)
    {
        tool_License.color += addColor;
        se_BGM_License.color += addColor;
    }

    /// <summary>
    /// �ŏ��ɕ\�������UI�̃t�F�[�h�C��
    /// </summary>
    /// <returns></returns>
    IEnumerator License_FirstUI_FadeIn()
    {
        yield return new WaitForSeconds(0.8f);

        while (criWare_License.color.a < 1)
        {
            yield return null;

            AddLicense_FirstUI_Color(new Color(0f, 0f, 0f, Time.deltaTime * fadeSppd));
        }

        yield return new WaitForSeconds(showUITime);

        StartCoroutine(License_FirstUI_FadeIOut());
    }

    /// <summary>
    /// �ŏ��ɕ\�������UI�̃t�F�[�h�t�F�[�h�A�E�g
    /// </summary>
    /// <returns></returns>
    IEnumerator License_FirstUI_FadeIOut()
    {

        while (criWare_License.color.a > 0)
        {
            yield return null;

            AddLicense_FirstUI_Color(new Color(0f, 0f, 0f, -Time.deltaTime * fadeSppd));
        }

        StartCoroutine(License_SecondUI_FadeIn());
    }

    /// <summary>
    /// ���ڂɕ\�������UI�̃t�F�[�h�C��
    /// </summary>
    /// <returns></returns>
    IEnumerator License_SecondUI_FadeIn()
    {
        

        while (tool_License.color.a < 1)
        {
            yield return null;

            AddLicense_SecondUI_Color(new Color(0f, 0f, 0f, Time.deltaTime * fadeSppd));
        }

        yield return new WaitForSeconds(showUITime);

        StartCoroutine(License_SecondUI_FadeIOut());
    }

    /// <summary>
    /// ���ڂɕ\�������UI�̃t�F�[�h�A�E�g
    /// </summary>
    /// <returns></returns>
    IEnumerator License_SecondUI_FadeIOut()
    {

        while (tool_License.color.a > 0)
        {
            yield return null;

            AddLicense_SecondUI_Color(new Color(0f, 0f, 0f, -Time.deltaTime * fadeSppd));
        }

        // �V�[���J�ڒ��O�A�j���[�V�������Đ�����
        Invoke("PlayScnenChangeAnim", 0.2f);

        //�V�[���J��
        Invoke("ChangeScene", 1.0f);
    }

    void WinCharaVoicePlay()
    {

        // win�L����Num���L�����̔z��𒴂��Ă��Ȃ����
        if (playerManagerScript.winCharaNum <= (int)ConstRepo.Chara.Hear)
        {
            // �{�C�X���Đ��L����������
            playVoiceNum = playerManagerScript.winCharaNum;
        }
        else
        {
            // 1p 2p�Ń����_���ŕ\���L���������߂�
            playVoiceNum = Random.Range((int)ConstRepo.Player.P1, (int)ConstRepo.Player.P2 + 1);
        }

        // �����v���C���[�̃L�����̊��Ӄ{�C�X�Đ�
        allCharaVoicePlayer.OnShot_CharaVoice(playVoiceNum, 5);
    }
}
