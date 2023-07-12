using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Live2D_AnimController : MonoBehaviour
{
    Animator live2DAnimation;

    void Start()
    {
        live2DAnimation = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Anim_Choice();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Anim_Att();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Anim_Hit();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Anim_Win();
        }
    }

    public void Anim_Att()
    {
        
        live2DAnimation.SetTrigger("att_trigger");
    }

    public void Anim_Hit()
    {
        live2DAnimation.SetTrigger("hit_trigger");
    }

    public void Anim_Win()
    {
        live2DAnimation.SetTrigger("win_trigger");
    }

    public void Anim_Win_OnVoice()
    {
        this.gameObject.GetComponent<CharaVoicePlayer>().VoiceOneShot(1);
        live2DAnimation.SetTrigger("win_trigger");
    }

    public void Anim_Choice()
    {
        live2DAnimation.SetTrigger("choice_trigger");
    }

    public void Anim_Choice_OnVoice()
    {
        this.gameObject.GetComponent<CharaVoicePlayer>().VoiceOneShot(0);
        live2DAnimation.SetTrigger("choice_trigger");
    }
}
