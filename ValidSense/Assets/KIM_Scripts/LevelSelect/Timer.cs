using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    public Text timer;

    [SerializeField]
    public float time;

    [SerializeField]
    public float countdown;

    [SerializeField]
    MusicLevelSelect musicSelect;

    [SerializeField]
    GameObject sceneManager;

    [SerializeField]
    public bool can_count;
    // Start is called before the first frame update
    void Start()
    {
        countdown = time;
        can_count = true;

        sceneManager = GameObject.Find("SceneManager");
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Floor(countdown)<=0)
        {

        }
        else if(can_count)
        {
            countdown -= Time.deltaTime;
            timer.text = Mathf.Floor(countdown).ToString();
        }

        if (countdown < 1)
        {
            musicSelect.control_cursor = false;
            musicSelect.TimeUpSet_NowMusicLevel();
            Invoke("ToMainScene", 3);
        }
    }

    public void ToMainScene()
    {
        sceneManager.GetComponent<Test>().ToMainScene();
    }
}
