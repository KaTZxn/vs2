using System.Collections.Generic;
using UnityEngine;

public class ViewNotesCreator : MonoBehaviour
{
    /// <summary>
    /// Viewノーツを管理するマネージャー
    [SerializeField]
    ViewNotesManager viewNotesManager;

    /// <summary>
    /// 譜面データを管理するスクリプト
    [SerializeField]
    NotesDataList notesDataList;


    /// <summary>
    /// 表示用ノーツを生成する為に、素材を格納してあるList
    /// </summary>
    [SerializeField]
    public List<GameObject> viewNotes;

    /// <summary>
    /// poisonNotesの画像保存用
    /// </summary>
    [SerializeField]
    public Sprite poisonNotesSprite;


    /// <summary>
    /// Viewノーツをどの高さから生成するかの、基準として使う数値
    /// </summary>
    [SerializeField]
    private float instantiatePosY;


    /// <summary>
    /// Viewノーツを各ラインに生成する時用の配列
    /// </summary>
    [SerializeField]
    private float[] instantiatePosX;


    /// <summary>
    /// Viewノーツをどの奥行で生成する時用の数値
    /// </summary>
    [SerializeField]
    private float instantiatePosZ;


    /// <summary>
    /// Viewノーツを、判定バー到達する時間より、m/s秒早く生成するかの値
    /// </summary>
    [SerializeField, Range(0, 9000)]
    private long viewNotesTime = 4500;


    /// <summary>
    /// キャラをいじってる子に参照用
    /// </summary>
    [SerializeField]
    TestChara testChara;


    /// <summary>
    /// ラインを管理してる子に参照する用
    /// </summary>
    [SerializeField]
    LinesManager linesManager;


    /// <summary>
    /// 通常Notesの色変更時使用するカラーコードの、配列
    /// </summary>
    public string[] charaNotesColorCode;

    /// <summary>
    /// PoisonNotesの色変更時使用するカラーコードの、配列
    /// </summary>
    public string[] poisonNotesColorCode;


    /// <summary>
    ///string型のカラーコードをColor変換した時の受け取り先
    /// </summary>
    Color colorCode;

    /// <summary>
    /// 各レーン用に次生成予定のノーツデータを格納
    /// </summary>
    [SerializeField]
    List<JsonReader.NoteList> nextCreatNotesDatas;

    /// <summary>
    /// ノーツ生成をレーンごとに許可する変数
    /// </summary>
    private bool[] canCreateNotes = {true, true, true, true, true, true, true, true};

    /// <summary>
    /// リンクノーツだけ、固定の位置に生成されるため、専用の変数
    /// </summary>
    private float instantiate_LinkNotesPosX = 29.5f;

    /// <summary>
    /// リンクノーツだけ、遅いほう依存で生成されるため、専用の変数
    /// </summary>
    private float linkNotesSpeed;

    // HoldNotes周り--------------------

    HoldNotesScript nextHoldNotes;

    //----------------------------------


    private void Start()
    {
        // 最初のノーツデータを各レーンに格納
        for (int i = 0; i < 8;)
        {
            // 指定のレーンのデータを該当レーン格納
            SetNextNotesData(i);
            i++;
        }

    }


    private void FixedUpdate()
    {
        // 1p分のviewNoteチェックと生成可能だったら生成
        for(int i = 0; i < 4;)
        {
            NotesTypeCheckAndCreat(i, 0);
            i++;
        }
        // 2p分のviewNoteチェックと生成可能だったら生成
        for (int i = 0; i < 4;)
        {
            NotesTypeCheckAndCreat(i+4, 1);
            i++;
        }
    }


    /// <summary>
    /// 次のノーツデータをの時間を参照して、生成するかを確認して、生成可能なら、タイプにあった生成用関数呼び出し
    /// </summary>
    /// <param name="line"></param>
    /// <param name="playerNum"></param>
    void NotesTypeCheckAndCreat(int line, int playerNum)
    {
        // 次のノーツデータを格納してるオブジェクトのデータを参照
        // criwearの時間を基準として判定バー到達予定時間 - viewNotesTimeの時間に実行
        if ((nextCreatNotesDatas[line].time - viewNotesTime) <= MusicData.Timer && canCreateNotes[line])
        {
            // 生成するノーツのタイプで生成用関数をスイッチ
            switch (nextCreatNotesDatas[line].type)
            {
                // リンクノーツ生成
                case 3:
                    CreatLinkViewNotes(line, playerNum);
                    break;
                // ホールドノーツ生成
                case 2:
                    CreateHoldViewNotes(line, playerNum);
                    break;
                // それ以外の、ノーマルノーツか、スライドノーツを生成
                default:
                    CreatSimpleViewNotes(line, playerNum);
                    break;
            }
        }
    }


    /// <summary>
    /// ノーマルノーツとスライドノーツ生成用の関数
    /// </summary>
    /// <param name="line"></param>
    /// <param name="playerNum"></param>
    void CreatSimpleViewNotes(int line , int playerNum)
    {
        // ラインのViewノーツを管理するListに、追加する
        viewNotesManager.simpleViewNotesLists[line].viewNotesList.Add(
            // ViewNotesを、typeで選び、生成し
            Instantiate(viewNotes[nextCreatNotesDatas[line].type],
            // ライン用のx軸Pos
            new Vector3(instantiatePosX[line],
            //基本の値 * speed * その倍率で、高さを設定
            instantiatePosY * viewNotesManager.viewNotesSpeed[playerNum]
            * viewNotesManager.NotesSpeedLeverage[playerNum], instantiatePosZ),
            // 回転は、特になし
            new Quaternion(0, 0, 0, 0)) as GameObject);


        // 生成したViewNotesの流れるspeedを設定
        viewNotesManager.simpleViewNotesLists[line].viewNotesList[viewNotesManager.simpleViewNotesLineNum[line]]
            .GetComponent<ViewNotesScript>().
            // 距離（基本の値 * speed * その倍率）
            NotesSpeedSetter((instantiatePosY * viewNotesManager.viewNotesSpeed[playerNum]
            * viewNotesManager.NotesSpeedLeverage[playerNum])
            // 時間（m/s を　m/s^2 に変換）/ 60fps用に設定
            / (viewNotesTime / 1000f) / 60);


        // 生成したViewNotesに対応する譜面データが毒ノーツなら
        if (nextCreatNotesDatas[line].isPoison)
        {
            // 生成したViewNotesの見た目をポイズンノーツに変更
            viewNotesManager.simpleViewNotesLists[line].viewNotesList[viewNotesManager.simpleViewNotesLineNum[line]].
                GetComponent<SpriteRenderer>().sprite = poisonNotesSprite;


            //ポイズンノーツのカラーコードで、現在のキャラと所持ラインに合わせて色を変える
            if (linesManager.haveLines_1p >= line)
            {
                if (ColorUtility.TryParseHtmlString(poisonNotesColorCode[testChara.count[0]],
                    out colorCode))
                {
                    viewNotesManager.simpleViewNotesLists[line].viewNotesList[viewNotesManager.simpleViewNotesLineNum[line]].
                   GetComponent<SpriteRenderer>().color = colorCode;
                }
            }
            else
            {
                if (ColorUtility.TryParseHtmlString(poisonNotesColorCode[testChara.count[1]],
                    out colorCode))
                {
                    viewNotesManager.simpleViewNotesLists[line].viewNotesList[viewNotesManager.simpleViewNotesLineNum[line]].
                   GetComponent<SpriteRenderer>().color = colorCode;
                }
            }

        }
        //通常ノーツ
        else
        {
            //通常ノーツのカラーコードで、現在のキャラと所持ラインに合わせて色を変える
            // Color型への変換成功するとcolorにColor型の赤色が代入される）outキーワードで参照渡しにする
            if (linesManager.haveLines_1p >= line)
            {
                if (ColorUtility.TryParseHtmlString(charaNotesColorCode[testChara.count[0]],
                    out colorCode))
                {
                    viewNotesManager.simpleViewNotesLists[line].viewNotesList[viewNotesManager.simpleViewNotesLineNum[line]].
                   GetComponent<SpriteRenderer>().color = colorCode;
                }
            }
            else
            {
                if (ColorUtility.TryParseHtmlString(charaNotesColorCode[testChara.count[1]],
                    out colorCode))
                {
                    viewNotesManager.simpleViewNotesLists[line].viewNotesList[viewNotesManager.simpleViewNotesLineNum[line]].
                   GetComponent<SpriteRenderer>().color = colorCode;
                }
            }
        }


        // 干渉するノーツを次にする
        viewNotesManager.simpleViewNotesLineNum[line]++;

        // ノーツの生成が完了したため、干渉するノーツを次にする
        notesDataList.nowNoteDataNum[playerNum]++;

        // 次のノーツデータを格納
        SetNextNotesData(line);
    }


    /// <summary>
    /// ホールドノーツ生成用の関数
    /// </summary>
    /// <param name="line"></param>
    /// <param name="playerNum"></param>
    void CreateHoldViewNotes(int line, int playerNum)
    {
        // ラインのHoldViewノーツを管理するListに、追加する
        viewNotesManager.holdViewNotesLists[line].viewNotesList.Add(
            // HoldViewNotesを、生成し
            Instantiate(viewNotes[2],
            // ライン用のx軸Pos
            new Vector3(instantiatePosX[line],
            //基本の値 * speed * その倍率で、高さを設定
            instantiatePosY * viewNotesManager.viewNotesSpeed[playerNum]
            * viewNotesManager.NotesSpeedLeverage[playerNum], instantiatePosZ),
            // 回転は、特になし
            new Quaternion(0, 0, 0, 0)) as GameObject);

        nextHoldNotes = viewNotesManager.holdViewNotesLists[line].viewNotesList[viewNotesManager.holdViewNotesLineNum[line]].
            GetComponent<HoldNotesScript>();

        // 生成したHoldViewNotesの流れるspeedを設定
        nextHoldNotes.
            // 距離（基本の値 * speed * その倍率）
            Set_SppedAndScale((instantiatePosY * viewNotesManager.viewNotesSpeed[playerNum]
            * viewNotesManager.NotesSpeedLeverage[playerNum])
            // 時間（m/s を　m/s^2 に変換）60fps用に設定
            / (viewNotesTime / 1000f) / 60 ,
            ((nextCreatNotesDatas[line].endtime - nextCreatNotesDatas[line].time) / 1000f) * 60);


        nextHoldNotes.times[0] = nextCreatNotesDatas[line].time;
        nextHoldNotes.times[1] = nextCreatNotesDatas[line].endtime;



        //ノーツを現在のキャラと所持ラインに合わせて色を変える
        if (linesManager.haveLines_1p >= line)
        {
            nextHoldNotes.
                NotesColorCange(charaNotesColorCode[testChara.count[(int)ConstRepo.Player.P1]]);
        }
        else
        {
            nextHoldNotes.
                NotesColorCange(charaNotesColorCode[testChara.count[(int)ConstRepo.Player.P2]]);
        }



        // 干渉するノーツを次にする
        viewNotesManager.holdViewNotesLineNum[line]++;

        // ノーツの生成が完了したため、干渉するノーツを次にする
        notesDataList.nowNoteDataNum[playerNum]++;

        // 次のノーツデータを格納
        SetNextNotesData(line);
    }




    void CreatLinkViewNotes(int line, int playerNum)
    {
        // 現在の譜面速度の遅いほうを採用する
        if(viewNotesManager.viewNotesSpeed[(int)ConstRepo.Player.P1]
                * viewNotesManager.NotesSpeedLeverage[(int)ConstRepo.Player.P1] 
            >
            viewNotesManager.viewNotesSpeed[(int)ConstRepo.Player.P2]
                * viewNotesManager.NotesSpeedLeverage[(int)ConstRepo.Player.P2])
        {
            linkNotesSpeed = viewNotesManager.viewNotesSpeed[(int)ConstRepo.Player.P2]
                * viewNotesManager.NotesSpeedLeverage[(int)ConstRepo.Player.P2];
        }
        else
        {
            linkNotesSpeed = viewNotesManager.viewNotesSpeed[(int)ConstRepo.Player.P1]
                * viewNotesManager.NotesSpeedLeverage[(int)ConstRepo.Player.P1];
        }

        // ラインのViewノーツを管理するListに、追加する
        viewNotesManager.simpleViewNotesLists[line].viewNotesList.Add(
            // ViewNotesを、typeで選び、生成し
            Instantiate(viewNotes[nextCreatNotesDatas[line].type],
            // ライン用のx軸Pos
            new Vector3(instantiate_LinkNotesPosX,
            //基本の値 * speed * その倍率で、高さを設定
            instantiatePosY * linkNotesSpeed, instantiatePosZ),
            // 回転は、特になし
            new Quaternion(0, 0, 0, 0)) as GameObject);


        // 生成したViewNotesの流れるspeedを設定
        viewNotesManager.simpleViewNotesLists[line].viewNotesList[viewNotesManager.simpleViewNotesLineNum[line]]
            .GetComponent<ViewNotesScript>().
            // 距離（基本の値 * speed * その倍率）
            NotesSpeedSetter((instantiatePosY * linkNotesSpeed)
            // 時間（m/s を　m/s^2 に変換）/ 60fps用に設定
            / (viewNotesTime / 1000f) / 60);



        ////ノーツを現在のキャラと所持ラインに合わせて色を変える
        //// Color型への変換成功するとcolorにColor型の赤色が代入される）outキーワードで参照渡しにする
        //if (linesManager.haveLines_1p >= line)
        //{
        //    if (ColorUtility.TryParseHtmlString(charaNotesColorCode[testChara.count[0]],
        //        out colorCode))
        //    {
        //        viewNotesManager.simpleViewNotesLists[line].viewNotesList[viewNotesManager.simpleViewNotesLineNum[line]].
        //       GetComponent<SpriteRenderer>().color = colorCode;
        //    }
        //}
        //else
        //{
        //    if (ColorUtility.TryParseHtmlString(charaNotesColorCode[testChara.count[1]],
        //        out colorCode))
        //    {
        //        viewNotesManager.simpleViewNotesLists[line].viewNotesList[viewNotesManager.simpleViewNotesLineNum[line]].
        //       GetComponent<SpriteRenderer>().color = colorCode;
        //    }
        //}



        // 干渉するノーツを次にする
        viewNotesManager.simpleViewNotesLineNum[line]++;

        // ノーツの生成が完了したため、干渉するノーツを次にする
        notesDataList.nowNoteDataNum[playerNum]++;

        // 次のノーツデータを格納
        SetNextNotesData(line);
    }



    /// <summary>
    /// 指定のnextCreatNotesDatasに、同じラインのノーツデータを格納(配列外参照防止付き)
    /// </summary>
    /// <param name="line"></param>
    private void SetNextNotesData(int line)
    {
        // 配列の総数を超えて配列参照しないようにする制御
        if (notesDataList.listNumMax[line] > 
            viewNotesManager.simpleViewNotesLineNum[line] + viewNotesManager.holdViewNotesLineNum[line])
        {
            // 指定のレーンのデータを該当レーン格納
            nextCreatNotesDatas[line] =
            notesDataList.notesLists[line].notesList[
                viewNotesManager.simpleViewNotesLineNum[line] + viewNotesManager.holdViewNotesLineNum[line]];
        }
        // Notesの生成許可を閉じる
        else
        {
            canCreateNotes[line] = false;
        }
    }


    /// <summary>
    /// ViewNotesTimeをミリセックから、秒速に変換して、値を渡す
    /// </summary>
    /// <returns></returns>
    public float Get_ViewNotesTime()
    {
        // ミリセックから、秒速に変換して、値を渡す
        return (float)(viewNotesTime * 0.001);
    }
}