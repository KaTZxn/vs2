using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JsonReader : MonoBehaviour
{
    public enum NoteType
    {
        tap,
        hold
    }

    public TextAsset textJSON;

    //public TextAsset textJSON_2p;

    [System.Serializable]
    public class SongData
    {
        public string name;
        public int songnum;
        public string artist;
        public string illustrator;
        public string chartdesigner;
        public float bpm;
        public int offset;
        public int kiaitime;
    }

    [System.Serializable]
    public class NoteList
    {
        public int type;
        public int line;
        public long time;
        public long endtime;
        public bool isPoison;
    }

    [System.Serializable]
    public class Natural
    {
        public int level;
        public NoteList[] notelist;
    }
    
    [System.Serializable]
    public class HighSense
    {
        public int level;
        public NoteList[] notelist;
    }

    [System.Serializable]
    public class SixthSense
    {
        public int level;
        public NoteList[] notelist;
    }

    [System.Serializable]
    public class DiffList
    {
        public Natural natural;
        public HighSense highSense;
        public SixthSense sixthSense;
    }

    [System.Serializable]
    public class SongList
    {
        public SongData songdata;
        public DiffList difflist;
    }

    public SongList _songList;

    public SongList _songList_2P;


    public static JsonReader instance = null;





    void Awake() 
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }



        /*for(int i = 0; i<transform.childCount; i++)
        {
            _songList = JsonUtility.FromJson<SongList>(textJSON[i].text);
        }*/
        //textJSON = Resources.Load("BeatmapData/02") as TextAsset;
        _songList = JsonUtility.FromJson<SongList>(textJSON.text);

        _songList_2P = JsonUtility.FromJson<SongList>(textJSON.text);
    }
    void Update() 
    {
        
    }


    


    public void ChangeJson(int num)
    {
        textJSON = Resources.Load($"BeatmapData/0{num}") as TextAsset;
        _songList = JsonUtility.FromJson<SongList>(textJSON.text);
        _songList_2P = JsonUtility.FromJson<SongList>(textJSON.text);
    }

    public void ChangeTutorialJson()
    {
        textJSON = Resources.Load($"BeatmapData/Tutorial") as TextAsset;
        _songList = JsonUtility.FromJson<SongList>(textJSON.text);
        _songList_2P = JsonUtility.FromJson<SongList>(textJSON.text);
    }
}
