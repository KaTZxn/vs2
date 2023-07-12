using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TutorialState
{
    Start,
    _1P,
    _2P,
    WinLose,
    Normal,
    Hold,
    Link,
    Skill,
    End
}

public enum TutorialImageType
{   
    None,
    Tap,
    Hold,
    Slide
}

public class TutorialControl : MonoBehaviour
{
    [SerializeField]Image fade;
    private float imageFadeTime = 0.5f;
    
    public float fadeTime = 0.3f;
    public float waitTime = 2f;
    public Image bgLeft,bgRight;
    public Text textLeft,textRight,textMid;
    [SerializeField, TextArea(1,3)]public string[] tutorialText;
    private int nowTextNum = 0;
    public TutorialState state;
    public Image[] tutorialImage;

    public SkillChargeRate rate_1P,rate_2P;
    public LinesManager linesManager;
    public LineOverTexture[] lineOverTexture1P,lineOverTexture2P;
    public TestChara testChara;
    public GameObject sceneManager;
    private float skipTime = 0f;
    public SceneChangeUI sceneChangeUI;

    public Tutorial_PlayerInput playerInput;

    // Start is called before the first frame update
    void Start()
    {
        sceneManager = GameObject.Find("SceneManager");
        UpdateGameState(TutorialState.Start);
        nowTextNum = 0;
        skipTime = 0f;
    }
    
    private void Update() 
    {
        SkipTutorial();
    }

    public void UpdateGameState(TutorialState newState)
    {
        state = newState;
        switch(newState)
        {
            case TutorialState.Start:
                StartCoroutine(TutorialStart());
                break;
            case TutorialState._1P:
                StartCoroutine(Line1P());
                break;
            case TutorialState._2P:
                StartCoroutine(Line2P());
                break;
            case TutorialState.WinLose:
                StartCoroutine(WinLose());
                break;
            case TutorialState.Normal:
                StartCoroutine(Normal());
                break;
            case TutorialState.Hold:
                StartCoroutine(Hold());
                break;
            case TutorialState.Link:
                StartCoroutine(Link());
                break;
            case TutorialState.Skill:
                StartCoroutine(Skill());
                break;
            case TutorialState.End:
                StartCoroutine(End());
                break;
            default:
                break;
        }
    }

    IEnumerator TutorialStart()
    {
        Debug.Log("TutorialStart");
        yield return FadeIn();
        yield return new WaitForSeconds(1);
        //Message 0
        yield return ShowTextMid();
        yield return new WaitForSeconds(1.5f);
        UpdateGameState(TutorialState._1P);
    }

    IEnumerator Line1P()
    {
        Debug.Log("1P");
        //Message 1
        yield return ShowTextRight();
        UpdateGameState(TutorialState._2P);
    }

    IEnumerator Line2P()
    {       
        Debug.Log("2P");
        //Message 2
        yield return ShowTextLeft();
        UpdateGameState(TutorialState.WinLose);
    }

    IEnumerator WinLose()
    {
        //Message 3
        yield return WinLoseText();

        UpdateGameState(TutorialState.Normal);
    }

    IEnumerator Normal()
    {
        //Message 6
        yield return ShowTextMid(4.5f,TutorialImageType.Tap);
        
        //normal
        yield return new WaitForSeconds(4f);

        UpdateGameState(TutorialState.Hold);
    }

    IEnumerator Hold()
    {
        yield return ShowTextMid(7,TutorialImageType.Hold);
        
        //hold
        yield return new WaitForSeconds(7);

        UpdateGameState(TutorialState.Link);
    }

    IEnumerator Link()
    {
        yield return ShowTextMid(4.5f,TutorialImageType.Tap);

        //link
        yield return new WaitForSeconds(4f);


        UpdateGameState(TutorialState.Skill);
    }

    IEnumerator Skill()
    {
        yield return ShowTextMid(4,TutorialImageType.Slide);

        rate_1P.skillValue = 300;
        rate_2P.skillValue = 300;

        rate_1P.SendMessage("skillChargeTextUpdate");
        rate_1P.SendMessage("SkillStockUpdate");
        rate_1P.SendMessage("ChargeImageColourUpdate");
        rate_2P.SendMessage("skillChargeTextUpdate");
        rate_2P.SendMessage("SkillStockUpdate");
        rate_2P.SendMessage("ChargeImageColourUpdate");

        yield return new WaitForSeconds(4.5f);
        playerInput.SetFalse_Tutorial_CanSkill();
        UpdateGameState(TutorialState.End);
    }

    IEnumerator End()
    {
        yield return ShowTextMid();

        testChara.Chara_Anim();
        yield return new WaitForSeconds(2);

        yield return FadeOut();

        sceneChangeUI.SceneEndAnim();
        yield return new WaitForSeconds(2);

        MusicPlayer.instance.StopPlayer();
        sceneManager.GetComponent<Test>().ToCharaSelectScene();

    }
    IEnumerator Skip()
    {
        yield return FadeOut();
        sceneChangeUI.SceneEndAnim();
        yield return new WaitForSeconds(2);
        MusicPlayer.instance.StopPlayer();
        sceneManager.GetComponent<Test>().ToCharaSelectScene();
    }
    IEnumerator ShowTextLeft(float _waitTime = 3f)
    {
        textLeft.text = tutorialText[nowTextNum];
        linesManager.HalfLineColorChange(4,0);

        for(int i = 0;i<4;i++)
        {
            lineOverTexture2P[i].fadeIn();
        }

        float t = 0f;
        while(t < 1.0f)
        {   
            t += Time.deltaTime / fadeTime;
            Color color = new Color(1,1,1,t);
            bgLeft.color = color;
            textLeft.color = color;
            yield return null;
        }
        yield return new WaitForSeconds(_waitTime);
        while(t > 0f)
        {   
            t -= Time.deltaTime / fadeTime;
            Color color = new Color(1,1,1,t);
            bgLeft.color = color;
            textLeft.color = color;
            yield return null;
        }
        linesManager.HalfLineColorChange(0,0);
        for(int i = 0;i<4;i++)
        {
            lineOverTexture2P[i].fadeOut();
        }
        nowTextNum++;
    }
    IEnumerator ShowTextRight(float _waitTime = 3f)
    {
        textRight.text = tutorialText[nowTextNum];
        linesManager.HalfLineColorChange(4,1);
        for(int i = 0;i<4;i++)
        {
            lineOverTexture1P[i].fadeIn();
        }
        float t = 0f;
        while(t < 1.0f)
        {   
            t += Time.deltaTime / fadeTime;
            Color color = new Color(1,1,1,t);
            bgRight.color = color;
            textRight.color = color;
            yield return null;
        }
        yield return new WaitForSeconds(_waitTime);
        while(t > 0f)
        {   
            t -= Time.deltaTime / fadeTime;
            Color color = new Color(1,1,1,t);
            bgRight.color = color;
            textRight.color = color;
            yield return null;
        }
        linesManager.HalfLineColorChange(1,1);
        for(int i = 0;i<4;i++)
        {
            lineOverTexture1P[i].fadeOut();
        }
        nowTextNum++;
    }
    IEnumerator ShowTextMid(float _waitTime = 3f,TutorialImageType imageType = TutorialImageType.None)
    {
        textMid.text = tutorialText[nowTextNum];
        float t = 0f;
        while(t < 1.0f)
        {   
            t += Time.deltaTime / fadeTime;
            Color color = new Color(1,1,1,t);
            bgLeft.color = color;
            bgRight.color = color;
            textMid.color = color;
            
            switch(imageType)
            {
                case TutorialImageType.None:
                    break;
                case TutorialImageType.Tap:
                    tutorialImage[0].color = color;
                    tutorialImage[1].color = color;
                    break;
                case TutorialImageType.Hold:
                    tutorialImage[2].color = color;
                    tutorialImage[3].color = color;
                    break;
                case TutorialImageType.Slide:
                    tutorialImage[4].color = color;
                    tutorialImage[5].color = color;
                    break;
                default:
                    break;
            }
            yield return null;
        }

        
        yield return new WaitForSeconds(_waitTime);


        while(t > 0f)
        {   
            t -= Time.deltaTime / fadeTime;
            Color color = new Color(1,1,1,t);
            bgLeft.color = color;
            bgRight.color = color;
            textMid.color = color;

            switch(imageType)
            {
                case TutorialImageType.None:
                    break;
                case TutorialImageType.Tap:
                    tutorialImage[0].color = color;
                    tutorialImage[1].color = color;
                    break;
                case TutorialImageType.Hold:
                    tutorialImage[2].color = color;
                    tutorialImage[3].color = color;
                    break;
                case TutorialImageType.Slide:
                    tutorialImage[4].color = color;
                    tutorialImage[5].color = color;
                    break;
                default:
                    break;
            }

            yield return null;
        }
        nowTextNum++;
    }
    
    IEnumerator WinLoseText()
        {
        textMid.text = tutorialText[nowTextNum];
        float t = 0f;
        while(t < 1.0f)
        {   
            t += Time.deltaTime / fadeTime;
            Color color = new Color(1,1,1,t);
            bgLeft.color = color;
            bgRight.color = color;
            textMid.color = color;
            
            yield return null;
        }

        
        yield return new WaitForSeconds(4f);
        while(t > 0f)
        {   
            t -= Time.deltaTime / fadeTime;
            Color color = new Color(1,1,1,t);
            textMid.color = color;
            yield return null;
        }
        
        nowTextNum++;
        textMid.text = tutorialText[nowTextNum];
        
        while(t < 1.0f)
        {   
            t += Time.deltaTime / fadeTime;
            Color color = new Color(1,1,1,t);
            textMid.color = color;
            yield return null;
        }
        
        
        yield return new WaitForSeconds(3.6f);

        while(t > 0f)
        {   
            t -= Time.deltaTime / fadeTime;
            Color color = new Color(1,1,1,t);
            textMid.color = color;

            yield return null;
        }

        nowTextNum++;
        textMid.text = tutorialText[nowTextNum];

        while(t < 1.0f)
        {   
            t += Time.deltaTime / fadeTime;
            Color color = new Color(1,1,1,t);
            textMid.color = color;
            yield return null;
        }
        
        yield return new WaitForSeconds(3.6f);


        while(t > 0f)
        {   
            t -= Time.deltaTime / fadeTime;
            Color color = new Color(1,1,1,t);
            bgLeft.color = color;
            bgRight.color = color;
            textMid.color = color;

            yield return null;
        }
        nowTextNum++;
    }

    IEnumerator FadeIn()
    {
        float i = 1f;
        while(i>0)
        {
            i -= Time.deltaTime / imageFadeTime;
            fade.color = new Color(1,1,1,i);
            yield return null;
        }
        
    }
    IEnumerator FadeOut()
    {
        float i = 0f;
        while(i<1)
        {
            i += Time.deltaTime / imageFadeTime;
            fade.color = new Color(1,1,1,i);
            yield return null;
        }
    }

    private void SkipTutorial()
    {
        if(Input.GetKey(KeyCode.Q)&&Input.GetKey(KeyCode.P))
        {
            skipTime += Time.deltaTime;
        }
        if(Input.GetKeyUp(KeyCode.Q)||Input.GetKeyUp(KeyCode.P))
        {
            skipTime = 0f;
        }
        if(skipTime>2.5f)
        {
            StartCoroutine(Skip());
            skipTime = 0f;
        }
    }
}
