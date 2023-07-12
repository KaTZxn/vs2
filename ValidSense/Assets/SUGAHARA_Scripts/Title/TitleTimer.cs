using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleTimer : MonoBehaviour
{
    /// <summary>
    /// ���g�̃e�L�X�g���擾�p
    /// </summary>
    Text timerText;

    /// <summary>
    /// �^�C�}�[�ő�l
    /// </summary>
    [SerializeField]
    public float timeMaxValue;

    /// <summary>
    /// �^�C�}�[�̐��l
    /// </summary>
    private float timeValue;


    /// <summary>
    /// �^�C�}�[�̃J�E���g�_�E���̐���p
    /// </summary>
    [SerializeField]
    private bool _canCountDown;

    [SerializeField]
    Move_Entry moveEntry;


    [SerializeField]
    TutorialAtendUIManager tutorialAtendUIManager;

    [SerializeField]
    Title_PlayerInput titlePlayerInput;

    [SerializeField]
    SceneChangeUI sceneChangeUI;


    // Start is called before the first frame update
    void Start()
    {
        // ���g���擾
        timerText = gameObject.GetComponent<Text>();

        // �^�C�}�[�������\�L
        TextUpdate();
    }

    private void Update()
    {
        if (_canCountDown)
        {
            if (timeValue >= 0)
            {
                TextUpdate();
                timeValue -= Time.deltaTime;
            }
            else
            {
                timeValue = 0;
                TextUpdate();

                TitleReset();
            }
        }
    }

    void TextUpdate()
    {
        timerText.text = timeValue.ToString("F0");
    }


    /// <summary>
    /// �J�E���g�_�E���̒�~�𖽗߂���֐�
    /// </summary>
    public void CountDownStop()
    {
        _canCountDown = false;
    }


    /// <summary>
    /// �^�C�}�[�̃J�E���g�_�E�����X�^�[�g����p
    /// </summary>
    public void CountDownStart()
    {
        _canCountDown = true;
    }

    /// <summary>
    /// �^�C�}�[�̒l�����Z�b�g����p
    /// </summary>
    public void SWTimerReset()
    {
        timeValue = timeMaxValue;
        TextUpdate();
    }

    /// <summary>
    /// �^�C�g����ʂ����Z�b�g
    /// </summary>
    void TitleReset()
    {
        _canCountDown = false;

        moveEntry.ResetUI_Input();

        tutorialAtendUIManager.AllHaveUI_Reset();

        titlePlayerInput.SetFalse_CanPlayerInput();

        sceneChangeUI.SceneStartAnim();
    }
}
