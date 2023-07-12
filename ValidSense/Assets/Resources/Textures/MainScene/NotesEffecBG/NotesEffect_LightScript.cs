using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesEffect_LightScript : MonoBehaviour
{
    /// <summary>
    /// パーティクルの色を変更する用
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
    /// 生成するエフェクトの色を引数の色にする
    /// </summary>
    /// <param name="setColor"></param>
    public void SetParticleColor(Color setColor)
    {
        parModule.startColor = setColor;
    }

    /// <summary>
    /// エフェクトを生成する
    /// </summary>
    public void ParticlePlay()
    {
        parSystem.Play();
    }

    /// <summary>
    /// エフェクトの生成を止める
    /// </summary>
    public void ParticleStop()
    {
        parSystem.Stop();
        parSystem.Clear();
    }

}
