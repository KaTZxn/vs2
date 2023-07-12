using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGameControl : MonoBehaviour
{
    public Text text;
    private bool isStart = false;

    public float playStandbyTime;

    [SerializeField]
    JsonReader json;


    // Start is called before the first frame update
    void Start()
    {
        json =  GameObject.Find("Json").GetComponent<JsonReader>();


        //MusicPlayer.holdNotesSEPlayer.MusicPlay(0);
        //
        StartCoroutine(PlayStart());
    }

    // Update is called once per frame
    void Update()
    {
        // �Ȃ̍Đ����ԕ\�L���X�V m/s
        text.text = MusicData.Timer+ "ms";

    }

    IEnumerator PlayStart()
    {
        yield return new WaitForSeconds(playStandbyTime);

        MusicPlayer.instance.MusicPlay(json._songList.songdata.songnum);
        isStart = true;
    }
}
