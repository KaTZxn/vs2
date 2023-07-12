using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MusicListControl : MonoBehaviour
{
    [SerializeField]
    GameObject sceneManager;

    [SerializeField]
    AllCharaVoicePlayer allCharaVoice;
    [SerializeField]
    GameObject[] Arrow;


    private Vector3[] musicListPosDatas;
    private Quaternion[] musicListRotationDatas;

    private GameObject[] musicList;

    private int musicListNum;
    private int nowMusicNum = 1;
    private bool isScrolling = false;
    [SerializeField]private float scrollTime = 0.1f;
    private GameObject jsonReader;

    private bool canSelectMusic = true;
    public SceneChangeUI sceneChangeUI;

    void Start()
    {
        nowMusicNum = 1;//nowMusicNum reset
        jsonReader = GameObject.Find("Json");
        jsonReader.SendMessage("ChangeJson",nowMusicNum);
        sceneManager = GameObject.Find("SceneManager");
        musicListNum = transform.childCount;
        musicListPosDatas = new Vector3[musicListNum];
        musicListRotationDatas = new Quaternion[musicListNum];
        musicList = new GameObject[musicListNum];
        

        for(int i = 0; i < musicListNum; i++)
        {
            musicList[i] = transform.GetChild(i).gameObject;
        }
    }

    void Update()
    {
        if(!canSelectMusic)
            return;
            
        if (Input.GetKeyDown(KeyCode.W)|| Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(ScrollUp());
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(ScrollDown());
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            canSelectMusic = false;
            // 1p�̃L�����̋Ȍ���{�C�X�Đ�
            allCharaVoice.OnShot_CharaVoice
                (sceneManager.GetComponent<PlayerManagerScript>().playerCharaNum[0],3);
            Invoke("SceneEndAnim", 1.5f);
            //SEPlayer.holdNotesSEPlayer.SEOneShot(7);
            Invoke("ToMainScene", 3);
        }
    }

    private IEnumerator ScrollUp()
    {
        //スクロール可能の判定
        if(isScrolling) { yield break; }

        //スクロール開始
        isScrolling = true;
        ScrollUpNumChange();
        //スクロールSE
        SEPlayer.instance.SEOneShot(5);

        //矢印を非表示
        Arrow[0].SetActive(false);
        Arrow[1].SetActive(false);
        
        //スクロールアニメーション
        yield return ScrollUpAnimation();

        //矢印を表示
        Arrow[0].SetActive(true);
        Arrow[1].SetActive(true);

        //曲データの表示変換
        jsonReader.SendMessage("ChangeJson",nowMusicNum);
        //曲を流す
        MusicPreviewPlayer.instance.MusicPlay(nowMusicNum - 1);
        //スクロール完了
        isScrolling = false;
    }

    private IEnumerator ScrollDown()
    {
        //スクロール可能の判定
        if(isScrolling) { yield break; }

        //スクロール開始
        isScrolling = true;
        ScrollDownNumChange();

        //スクロールSE
        SEPlayer.instance.SEOneShot(5);

        //矢印を非表示
        Arrow[0].SetActive(false);
        Arrow[1].SetActive(false);

        //スクロールアニメーション
        yield return ScrollDownAnimation();
        
        //矢印を非表示
        Arrow[0].SetActive(true);
        Arrow[1].SetActive(true);
        
        //曲データの表示変換
        jsonReader.SendMessage("ChangeJson",nowMusicNum);
        //曲を流す
        MusicPreviewPlayer.instance.MusicPlay(nowMusicNum - 1);
        //スクロール完了
        isScrolling = false;
    }

    private IEnumerator ScrollUpAnimation()
    {
        for(int i = musicListNum - 1; i >= 0; i--)
        {
            musicListPosDatas[i] = musicList[i].transform.position;
            musicListRotationDatas[i] = musicList[i].transform.rotation;
        }

        float t = 0f;
        while(t < 1.0f)
        {   
            t += Time.deltaTime / scrollTime;
            
            if(nowMusicNum - 1 < 0 )
                {musicList[3].GetComponent<SpriteRenderer>().color =new Color(1,1,1,0f);}
            else
                {musicList[nowMusicNum - 1].GetComponent<SpriteRenderer>().color =new Color(1,1,1,0f);}
            
            for(int i = 0; i < musicListNum ; i++)
            {
                if(i == (musicListNum - 1))
                {
                    musicList[i].transform.position = Vector3.Lerp(musicListPosDatas[i], musicListPosDatas[0], t);
                    musicList[i].transform.rotation = Quaternion.Lerp(musicListRotationDatas[i], musicListRotationDatas[0], t);
                    
                }else
                {
                    musicList[i].transform.position = Vector3.Lerp(musicListPosDatas[i], musicListPosDatas[i + 1], t);
                    musicList[i].transform.rotation = Quaternion.Lerp(musicListRotationDatas[i], musicListRotationDatas[i + 1], t);
                }
            }
            yield return null;
                    

        }
        if(nowMusicNum - 1 < 0 )
            {musicList[3].GetComponent<SpriteRenderer>().color =new Color(1,1,1,1f);}
        else
            {musicList[nowMusicNum - 1].GetComponent<SpriteRenderer>().color =new Color(1,1,1,1f);}
    }

    private IEnumerator ScrollDownAnimation()
    {
        for(int i = musicListNum - 1; i >= 0; i--)
        {
            musicListPosDatas[i] = musicList[i].transform.position;
            musicListRotationDatas[i] = musicList[i].transform.rotation;
        }
        float t = 0f;
        while(t < 1.0f)
        {   
            t += Time.deltaTime / scrollTime;
            
            if(nowMusicNum - 2 < 0)
                {musicList[musicListNum - 1].GetComponent<SpriteRenderer>().color =new Color(1,1,1,0f);}
            else
                {musicList[nowMusicNum - 2].GetComponent<SpriteRenderer>().color =new Color(1,1,1,0f);}
            
            for(int i = musicListNum - 1; i >= 0; i--)
            {
                if(i == 0)
                {
                    musicList[i].transform.position = Vector3.Lerp(musicListPosDatas[i], musicListPosDatas[musicListNum - 1], t);
                    musicList[i].transform.rotation = Quaternion.Lerp(musicListRotationDatas[i], musicListRotationDatas[musicListNum - 1], t);
                    
                }else
                {
                    musicList[i].transform.position = Vector3.Lerp(musicListPosDatas[i], musicListPosDatas[i - 1], t);
                    musicList[i].transform.rotation = Quaternion.Lerp(musicListRotationDatas[i], musicListRotationDatas[i - 1], t);
                }
            }
            yield return null;
        }
        
        if(nowMusicNum -2 < 0)
            {musicList[musicListNum - 1].GetComponent<SpriteRenderer>().color =new Color(1,1,1,1);}
        else
            {musicList[nowMusicNum - 2].GetComponent<SpriteRenderer>().color =new Color(1,1,1,1);}
            
        
    }

    private void ScrollUpNumChange()
    {
        if(nowMusicNum > 1)
        {
            nowMusicNum--;
        }
        else
        {
            nowMusicNum = musicListNum;
        }
    }

    private void ScrollDownNumChange()
    {
        if(nowMusicNum < musicListNum)
        {
            nowMusicNum++;
        }
        else
        {
            nowMusicNum = 1;
        }
    }
    //Move to MainScene
    private void ToMainScene()
    {
        sceneManager.GetComponent<Test>().ToLevelSelectScene();
        MusicPreviewPlayer.instance.StopPlayer();
    }

    private void SceneEndAnim()
    {
        sceneChangeUI.SceneEndAnim();
    }
}
