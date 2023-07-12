using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultText : MonoBehaviour
{
    [SerializeField]
    PlayerManagerScript sceneManager;

    [SerializeField]
    int[] playerNum;

    [SerializeField]
    private List<Text>  texts;

    //[SerializeField]
    //Font useEnglishFont;

    //[SerializeField]
    //Font useJapaneseFont;

    [SerializeField]
    JsonReader json;


    private int _level;



    // Start is called before the first frame update
    void Start()
    {
        json = GameObject.Find("Json").GetComponent<JsonReader>(); ;

        sceneManager = GameObject.Find("SceneManager").GetComponent<PlayerManagerScript>();




        switch(sceneManager.nowMusicLevel[playerNum[0]])
        {
            case 0:
                _level = json._songList.difflist.natural.level;
                break;

            case 1:
                _level = json._songList.difflist.highSense.level;
                break;

            case 2:
                _level = json._songList.difflist.sixthSense.level;
                break;

        }



        texts[0].text = sceneManager.score[playerNum[0]].ToString();


        texts[1].text = "       " + sceneManager.stolenLane[playerNum[0]] + "\n" +
            "          " + sceneManager.stolenLane[playerNum[1]] + "\n" +
            "" + sceneManager.totalHitsNum[playerNum[0]];


        texts[2].text = json._songList.songdata.name;
        texts[3].text = "     " + json._songList.songdata.artist;
        texts[4].text = "     " + _level.ToString();



        //if (json._songList.songdata.songnum == 2)
        //{
        //    texts[3].font = useJapaneseFont;
        //}

    }

}
