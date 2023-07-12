using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private GameObject sceneManager;
    private void Start() 
    {       
        sceneManager = GameObject.Find("SceneManager");
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            sceneManager.SendMessage("SetScene", GameScene.Title);
        }

    }
    public void ToCharaSelectScene()
    {
        sceneManager.SendMessage("SetScene", GameScene.CharaSelect);
    }


    public void ToMusicSelectScene()
    {
        sceneManager.SendMessage("SetScene", GameScene.MusicSelect);
    }

    public void ToLevelSelectScene()
    {
        sceneManager.SendMessage("SetScene", GameScene.LevelSelect);
    }

    public void ToMainScene()
    {
        sceneManager.SendMessage("SetScene", GameScene.Playing);
    }

    public void ToResult_1P_Scene()
    {
        sceneManager.SendMessage("SetScene", GameScene.Result_1P);
    }

    public void ToResult_2P_Scene()
    {
        sceneManager.SendMessage("SetScene", GameScene.Result_2P);
    }

    public void ToLicenseScene()
    {
        sceneManager.SendMessage("SetScene", GameScene.LicenseScene);
    }

    public void EndScene()
    {
        sceneManager.SendMessage("Set Scene", GameScene.EndScene);
    }

    public void ToTitleScene()
    {
        sceneManager.SendMessage("SetScene", GameScene.Title);
    }

    public void ToTutorialScene()
    {
        sceneManager.SendMessage("SetScene", GameScene.Tutorial);
    }
}


