using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesEffect_LightScript : MonoBehaviour
{
    /// <summary>
    /// �p�[�e�B�N���̐F��ύX����p
    /// </summary>
    ParticleSystem.MainModule parModule;

    ParticleSystem parSystem;

    private void Start()
    {
        parModule = gameObject.GetComponent<ParticleSystem>().main;

        parSystem = gameObject.GetComponent<ParticleSystem>();

        ParticleStop();
    }

    /// <summary>
    /// ��������G�t�F�N�g�̐F�������̐F�ɂ���
    /// </summary>
    /// <param name="setColor"></param>
    public void SetParticleColor(Color setColor)
    {
        parModule.startColor = setColor;
    }

    /// <summary>
    /// �G�t�F�N�g�𐶐�����
    /// </summary>
    public void ParticlePlay()
    {
        parSystem.Play();
    }

    /// <summary>
    /// �G�t�F�N�g�̐������~�߂�
    /// </summary>
    public void ParticleStop()
    {
        parSystem.Stop();
        parSystem.Clear();
    }

}
