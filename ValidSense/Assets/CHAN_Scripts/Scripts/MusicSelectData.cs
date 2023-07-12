using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSelectData : MonoBehaviour
{
    [HeaderAttribute ("MusicData")]
    public Text name;
    public Text artistName, bPM, chartDesigner, illustrator;
    [HeaderAttribute ("Level")]
    public Text easyLevel;
    public Text normalLevel; 
    public Text hardLevel;

    public JsonReader jsonReader;
    // Start is called before the first frame update
    void Start()
    {
        jsonReader = GameObject.Find("Json").GetComponent<JsonReader>();
    }

    // Update is called once per frame
    void Update()
    {
        var songdata = jsonReader._songList.songdata;
        var difflist = jsonReader._songList.difflist;

        name.text = songdata.name;
        artistName.text = songdata.artist;
        bPM.text = "BPM:" + songdata.bpm;
        chartDesigner.text = "Chart Designer: " + songdata.chartdesigner;
        
        if(songdata.illustrator != null)
            illustrator.text = "Illusted by: " + songdata.illustrator;
        else
            illustrator.text = "";

        easyLevel.text = difflist.natural.level.ToString();
        normalLevel.text = difflist.highSense.level.ToString();
        hardLevel.text = difflist.sixthSense.level.ToString();
        
    }
}
