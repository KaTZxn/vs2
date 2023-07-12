using System.Collections.Generic;
using UnityEngine;

public class NotesDataList : MonoBehaviour
{

    /// <summary>
    /// 読み込む用の譜面データ（JsonReader）
    /// </summary>
    [SerializeField]
    private JsonReader _jsonReader_1p;


    /// <summary>
    /// 読み込む用の譜面データ（JsonReader）
    /// </summary>
    [SerializeField]
    private JsonReader _jsonReader_2p;

    /// <summary>
    /// プレイヤーマネージャー参照用
    /// </summary>
    [SerializeField]
    private PlayerManagerScript _playerManagerScript;

    /// <summary>
    /// 各レーンのノーツ総数
    /// </summary>
    public int[] listNumMax;

    /// <summary>
    /// ノーツデータのリストのリスト
    /// </summary>
    public List<NotesDataClass> notesLists;


    /// <summary>
    /// レーンごとに分かれていない譜面データ
    /// </summary>
    [SerializeField]
    public List<NotesDataClass> musicNotesDate;

    /// <summary>
    /// musicNoteDataの進行度記憶
    /// </summary>
    public int[] nowNoteDataNum;

    /// <summary>
    /// 難易度が同じ時のリンクノーツ排除のトリガー変数
    /// </summary>
    private bool _selectLevel_Same;

    /// <summary>
    /// プレイヤーの選んだ難易度を格納する用
    /// </summary>
    [SerializeField]
    private int[] _selectLevel;

    /// <summary>
    /// リンクノーツのかぶりを排除するときに使用する、リンクノーツカウンター
    /// </summary>
    [SerializeField]
    private int _linkNotesCount;

    /// <summary>
    /// 1pの譜面のリンクノーツの判定時間を格納する用
    /// </summary>
    [SerializeField]
    List<long> _linkNotesTimes_1P;

    /// <summary>
    /// 同じ判定時間のリンクノーツを取り出すときの、1pのカウント用
    /// </summary>
    int _count_1P;

    /// <summary>
    /// 1Pのリンクノーツのを複数はじかないようにするbool
    /// </summary>
    bool cantAddData_1P;

    /// <summary>
    /// 2pの譜面のリンクノーツの判定時間を格納する用
    /// </summary>
    [SerializeField]
    List<long> _linkNotesTimes_2P;

    /// <summary>
    /// 同じ判定時間のリンクノーツを取り出すときの、2pのカウント用
    /// </summary>
    int _count_2P;

    /// <summary>
    /// 2Pのリンクノーツのを複数はじかないようにするbool
    /// </summary>
    bool cantAddData_2P;

    /// <summary>
    /// 1pと2pの譜面のリンクノーツの判定時間が、同じリンクノーツの判定時間を格納する用
    /// </summary>
    [SerializeField]
    List<long> _1P2P_linkNotesSameTimes;



    void Start()
    {
        //初期値はfalse
        _selectLevel_Same = false;


        _playerManagerScript = GameObject.Find("SceneManager").GetComponent<PlayerManagerScript>();


        // 1Pの譜面データを代入
        _jsonReader_1p = GameObject.Find("Json").GetComponent<JsonReader>();

        // 2Pの譜面データを代入
        _jsonReader_2p = GameObject.Find("Json").GetComponent<JsonReader>();


        //プレイヤーの選んだ難易度を格納
        _selectLevel = _playerManagerScript.nowMusicLevel;


        //プレイヤーの選択難易度が同じなら
        if (_selectLevel[(int)ConstRepo.Player.P1] == _selectLevel[(int)ConstRepo.Player.P2])
        {
            //難易度が同じ時のリンクノーツ排除のトリガー変数にtrueを入れる
            _selectLevel_Same = true;
        }
        //プレイヤーの選択難易度が異なっていたら
        else
        {
            // 1P2Pのリンクノーツの判定時間を摘出して格納
            DownloadLinkNotesTime_1P();
            DownloadLinkNotesTime_2P();

            // P2Pのリンクノーツの判定時間を比較して、同じ時間の物を取り出す
            PickOut_SameTimeLinkNotes();
        }


        // リンクノーツカウンターを初期化
        _linkNotesCount = 0;

        // 2Pの譜面データロード
        DownloadNotesData_1P();


        // リンクノーツカウンターを初期化
        _linkNotesCount = 0;

        // 2Pの譜面データロード
        DownloadNotesData_2P();
    }


    void DownloadNotesData_1P()
    {
        // 同じ難易度の時
        if (_selectLevel_Same)
        {
            // 1p 譜面データの読み込み
            switch (_selectLevel[(int)ConstRepo.Player.P1])
            {
                case 0:
                    // 難易度naturalの譜面データを読み込み
                    foreach (JsonReader.NoteList item in _jsonReader_1p._songList.difflist.natural.notelist)
                    {
                        addNotesData_Same_1P(item.line, item);
                    }
                    break;

                case 1:
                    // 難易度highSenseの譜面データを読み込み
                    foreach (JsonReader.NoteList item in _jsonReader_1p._songList.difflist.highSense.notelist)
                    {
                        addNotesData_Same_1P(item.line, item);
                    }
                    break;

                default:
                    // 難易度highSenseの譜面データを読み込み
                    foreach (JsonReader.NoteList item in _jsonReader_1p._songList.difflist.sixthSense.notelist)
                    {
                        addNotesData_Same_1P(item.line, item);
                    }
                    break;
            }
        }
        // 難易度が異なる時
        else
        {
            // 1p 譜面データの読み込み
            switch (_selectLevel[(int)ConstRepo.Player.P1])
            {
                case 0:
                    // 難易度naturalの譜面データを読み込み
                    foreach (JsonReader.NoteList item in _jsonReader_1p._songList.difflist.natural.notelist)
                    {
                        addNotesData_Vary_1P(item.line, item);
                    }
                    break;

                case 1:
                    // 難易度highSenseの譜面データを読み込み
                    foreach (JsonReader.NoteList item in _jsonReader_1p._songList.difflist.highSense.notelist)
                    {
                        addNotesData_Vary_1P(item.line, item);
                    }
                    break;

                default:
                    // 難易度highSenseの譜面データを読み込み
                    foreach (JsonReader.NoteList item in _jsonReader_1p._songList.difflist.sixthSense.notelist)
                    {
                        addNotesData_Vary_1P(item.line, item);
                    }
                    break;
            }
        }

    }


    void DownloadNotesData_2P()
    {
        // 同じ難易度の時
        if (_selectLevel_Same)
        {
            // 2p 譜面データの読み込み
            switch (_selectLevel[(int)ConstRepo.Player.P2])
            {
                case 0:
                    // 難易度naturalの譜面データを読み込み
                    foreach (JsonReader.NoteList item in _jsonReader_2p._songList_2P.difflist.natural.notelist)
                    {
                        addNotesData_Same_2P(item.line, item);
                    }
                    break;

                case 1:
                    // 難易度highSenseの譜面データを読み込み
                    foreach (JsonReader.NoteList item in _jsonReader_2p._songList_2P.difflist.highSense.notelist)
                    {
                        addNotesData_Same_2P(item.line, item);
                    }
                    break;

                default:
                    // 難易度highSenseの譜面データを読み込み
                    foreach (JsonReader.NoteList item in _jsonReader_2p._songList_2P.difflist.sixthSense.notelist)
                    {
                        addNotesData_Same_2P(item.line, item);
                    }
                    break;
            }
        }
        // 難易度が異なる時
        else
        {
            // 2p 譜面データの読み込み
            switch (_selectLevel[(int)ConstRepo.Player.P2])
            {
                case 0:
                    // 難易度naturalの譜面データを読み込み
                    foreach (JsonReader.NoteList item in _jsonReader_2p._songList_2P.difflist.natural.notelist)
                    {
                        addNotesData_Vary_2P(item.line, item);
                    }
                    break;

                case 1:
                    // 難易度highSenseの譜面データを読み込み
                    foreach (JsonReader.NoteList item in _jsonReader_2p._songList_2P.difflist.highSense.notelist)
                    {
                        addNotesData_Vary_2P(item.line, item);
                    }
                    break;

                default:
                    // 難易度highSenseの譜面データを読み込み
                    foreach (JsonReader.NoteList item in _jsonReader_2p._songList_2P.difflist.sixthSense.notelist)
                    {
                        addNotesData_Vary_2P(item.line, item);
                    }
                    break;
            }
        }

    }



    /// <summary>
    /// ノーツのデータを各レーンに振り分け ノーツのデータをListに追加（直列）
    /// </summary>
    /// <param name="Line"></param>
    void addNotesData_Same_1P(int Line, JsonReader.NoteList item)
    {
        //今のNotesDataがリンクノーツなら
        if (item.type == (int)ConstRepo.NotesType.Link)
        {

            _linkNotesCount++;
        }


        //同じ難易度を選択しているかつ、今のNotesDataがリンクノーツ
        //かつ、_linkNotesCount ÷ 2 のあまりが 0 なら
        if (_selectLevel_Same && item.type == (int)ConstRepo.NotesType.Link
            && _linkNotesCount % 2 == (int)ConstRepo.Player.P1)
        {
            //なにもしない！！！！
        }
        else
        {
            //振り分け
            listNumMax[Line]++;
            notesLists[Line].notesList.Add(item);

            //直列
            musicNotesDate[(int)ConstRepo.Player.P1].notesList.Add
                (notesLists[Line].notesList[notesLists[Line].notesList.Count - 1]);
        }
    }

    /// <summary>
    /// ノーツのデータを各レーンに振り分け ノーツのデータをListに追加（直列）
    /// </summary>
    /// <param name="Line"></param>
    void addNotesData_Same_2P(int Line, JsonReader.NoteList item)
    {
        //今のNotesDataがリンクノーツなら
        if (item.type == (int)ConstRepo.NotesType.Link)
        {
            _linkNotesCount++;
        }


        //同じ難易度を選択しているかつ、今のNotesDataがリンクノーツ
        //かつ、_linkNotesCount ÷ 2 のあまりが 1 なら
        if (_selectLevel_Same && item.type == (int)ConstRepo.NotesType.Link
            && _linkNotesCount % 2 == (int)ConstRepo.Player.P2)
        {
            //なにもしない！！！！
        }
        else
        {
            // 2p用にlineを4-Line(ノーツ配置ミラー)
            //振り分け
            listNumMax[7 - Line]++;
            notesLists[7 - Line].notesList.Add(item);

            //直列
            musicNotesDate[(int)ConstRepo.Player.P2].notesList.Add
                (notesLists[7 - Line].notesList[notesLists[7 - Line].notesList.Count - 1]);
        }
    }



    void DownloadLinkNotesTime_1P()
    {
        // 2p リンクの判定時間の読み込み
        switch (_selectLevel[(int)ConstRepo.Player.P1])
        {
            case 0:
                // 難易度naturalの譜面データを読み込み
                foreach (JsonReader.NoteList item in _jsonReader_1p._songList.difflist.natural.notelist)
                {
                    if (item.type == (int)ConstRepo.NotesType.Link)
                    {
                        _linkNotesTimes_1P.Add(item.time);
                    }
                }
                break;

            case 1:
                // 難易度highSenseの譜面データを読み込み
                foreach (JsonReader.NoteList item in _jsonReader_1p._songList.difflist.highSense.notelist)
                {
                    if (item.type == (int)ConstRepo.NotesType.Link)
                    {
                        Debug.Log(item.time);
                        _linkNotesTimes_1P.Add(item.time);
                    }
                }
                break;

            default:
                // 難易度highSenseの譜面データを読み込み
                foreach (JsonReader.NoteList item in _jsonReader_1p._songList.difflist.sixthSense.notelist)
                {
                    if (item.type == (int)ConstRepo.NotesType.Link)
                    {
                        _linkNotesTimes_1P.Add(item.time);
                    }
                }
                break;
        }
    }


    void DownloadLinkNotesTime_2P()
    {
        // 2p リンクの判定時間の読み込み
        switch (_selectLevel[(int)ConstRepo.Player.P2])
        {
            case 0:
                // 難易度naturalの譜面データを読み込み
                foreach (JsonReader.NoteList item in _jsonReader_2p._songList_2P.difflist.natural.notelist)
                {
                    if(item.type == (int)ConstRepo.NotesType.Link)
                    {
                        _linkNotesTimes_2P.Add(item.time);
                    }
                }
                break;

            case 1:
                // 難易度highSenseの譜面データを読み込み
                foreach (JsonReader.NoteList item in _jsonReader_2p._songList_2P.difflist.highSense.notelist)
                {
                    if (item.type == (int)ConstRepo.NotesType.Link)
                    {
                        _linkNotesTimes_2P.Add(item.time);
                    }
                }
                break;

            default:
                // 難易度highSenseの譜面データを読み込み
                foreach (JsonReader.NoteList item in _jsonReader_2p._songList_2P.difflist.sixthSense.notelist)
                {
                    if (item.type == (int)ConstRepo.NotesType.Link)
                    {
                        _linkNotesTimes_2P.Add(item.time);
                    }
                }
                break;
        }
    }

    /// <summary>
    /// 1pと2pの同じ時間の、リンクノーツの判定時間を取得
    /// </summary>
    void PickOut_SameTimeLinkNotes()
    {
        // どちらかが、カウント用変数がリストの要素数を超えたら
        while (_count_1P < _linkNotesTimes_1P.Count &&
            _count_2P < _linkNotesTimes_2P.Count)
        {
            // タイムが同じなら
            if (_linkNotesTimes_1P[_count_1P] == _linkNotesTimes_2P[_count_2P])
            {
                // 格納
                _1P2P_linkNotesSameTimes.Add(_linkNotesTimes_1P[_count_1P]);

                // カウント＋
                _count_1P++;
                _count_2P++;
            }
            // 1Pが大きかったら
            else if (_linkNotesTimes_1P[_count_1P] > _linkNotesTimes_2P[_count_2P])
            {
                // 2Pカウント＋
                _count_2P++;
            }
            // 2Pが大きかったら
            else if (_linkNotesTimes_1P[_count_1P] < _linkNotesTimes_2P[_count_2P])
            {
                // 1Pカウント＋
                _count_1P++;
            }
        }
    }


    /// <summary>
    /// プレイヤーが異なる難易度を選択した時の譜面データ読込_1P
    /// </summary>
    /// <param name="Line"></param>
    /// <param name="item"></param>
    void addNotesData_Vary_1P(int Line, JsonReader.NoteList item)
    {
        //今のNotesDataの判定時間が、
        //リンクノーツの判定時間が1P2Pで同じな時間を、集めた配列の時間と同じなら
        if (item.time == _1P2P_linkNotesSameTimes[_linkNotesCount])
        {
            // 配列外参照を避ける制御
            if(_linkNotesCount < _1P2P_linkNotesSameTimes.Count - 1)
            {

                //はじく処理の制御を解除する
                cantAddData_1P = false;
                _linkNotesCount++;
            }

            //　このタイムでの弾きが初回かつ
            //　linkNotesCount ÷ 2 のあまりが 0 なら
            if (!cantAddData_1P
                && _linkNotesCount % 2 == (int)ConstRepo.Player.P1)
            {
                // 再度同じデータではじかないように、はじく処理に制御をかける
                cantAddData_1P = true;


                //なにもしない！！！！
                return;
            }
        }


        //振り分け
        listNumMax[Line]++;
        notesLists[Line].notesList.Add(item);

        //直列
        musicNotesDate[(int)ConstRepo.Player.P1].notesList.Add
            (notesLists[Line].notesList[notesLists[Line].notesList.Count - 1]);
    }


    /// <summary>
    /// プレイヤーが異なる難易度を選択した時の譜面データ読込_2P
    /// </summary>
    /// <param name="Line"></param>
    /// <param name="item"></param>
    void addNotesData_Vary_2P(int Line, JsonReader.NoteList item)
    {
        //今のNotesDataの判定時間が、
        //リンクノーツの判定時間が1P2Pで同じな時間を、集めた配列の時間と同じなら
        if (item.time == _1P2P_linkNotesSameTimes[_linkNotesCount])
        {
            // 配列外参照を避ける制御
            if (_linkNotesCount < _1P2P_linkNotesSameTimes.Count - 1)
            {
                //はじく処理の制御を解除する
                cantAddData_2P = false;
                _linkNotesCount++;
            }

            //　このタイムでの弾きが初回かつ
            //　linkNotesCount ÷ 2 のあまりが 0 なら
            if (!cantAddData_2P
                && _linkNotesCount % 2 == (int)ConstRepo.Player.P2)
            {
                // 再度同じデータではじかないように、はじく処理に制御をかける
                cantAddData_2P = true;


                //なにもしない！！！！
                return;
            }
        }


        // 2p用にlineを4-Line(ノーツ配置ミラー)
        //振り分け
        listNumMax[7 - Line]++;
        notesLists[7 - Line].notesList.Add(item);

        //直列
        musicNotesDate[(int)ConstRepo.Player.P2].notesList.Add
            (notesLists[7 - Line].notesList[notesLists[7 - Line].notesList.Count - 1]);
    }
}
