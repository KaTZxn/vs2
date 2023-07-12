using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSelectTimer : MonoBehaviour
{
    public Text Timer;
    [SerializeField]private int time;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        Timer.text = time.ToString("00");
        while (time > 0)
        {
            yield return new WaitForSeconds(1);

            time--;
            Timer.text = time.ToString("00");
        }
        if(time == 0)
        {            
            MusicPreviewPlayer.instance.StopPlayer();
            GameObject.Find("SceneManager").SendMessage("SetScene", GameScene.Title);
        }
        //to title
    }
    
}
