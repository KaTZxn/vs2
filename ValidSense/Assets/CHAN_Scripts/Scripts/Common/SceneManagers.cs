using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameScene
{
    Title,
    Tutorial,
    CharaSelect,
    MusicSelect,
    LevelSelect,
    Playing,
    Result_1P,
    Result_2P,
    LicenseScene,
    EndScene,
}

public class SceneManagers : MonoBehaviour
{
    public static SceneManagers instance = null;
    private GameScene currentGameScene;

    private void Awake()
    {   
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SetScene(GameScene scene)
    {
        currentGameScene = scene;
        OnGameSceneChanged (currentGameScene);
    }

    public void OnGameSceneChanged(GameScene scene)
    {
        switch(scene)
        {
            case GameScene.Title:
                SceneManager.LoadScene("Title");
                break;
            case GameScene.CharaSelect:
                SceneManager.LoadScene("CharaSelect");
                break;
            case GameScene.MusicSelect:
                SceneManager.LoadScene("MusicSelect");
                break;
            case GameScene.LevelSelect:
                SceneManager.LoadScene("LevelScene");
                break;
            case GameScene.Playing:
                SceneManager.LoadScene("MainScene");
                break;
            case GameScene.Result_1P:
                SceneManager.LoadScene("ResultScene_1P");
                break;
            case GameScene.Result_2P:
                SceneManager.LoadScene("ResultScene_2P");
                break;
            case GameScene.LicenseScene:
                SceneManager.LoadScene("LicenseScene");
                break;
            case GameScene.EndScene:
                SceneManager.LoadScene("EndScene");
                break;
            case GameScene.Tutorial:
                SceneManager.LoadScene("Tutorial");
                break;
            default:
                break;
        }
    }
}
