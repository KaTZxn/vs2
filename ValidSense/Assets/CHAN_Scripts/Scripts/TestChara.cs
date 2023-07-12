using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestChara : MonoBehaviour
{
    [SerializeField]
    public GameObject[] chara1P, chara2P;
    // private ���� pubric�ɕύX ���߂̒l�� 1 ���� 0 �ɕύX by�����@7/1

    /// <summary>
    /// �v���C���[���ǂ̃L�������g�p���Ă��邩�̕ϐ�(0��1P�A1��2P)
    /// </summary>
    public int[] count;
    // Update is called once per frame

    [SerializeField]
    GameObject sceneManager;


    private void Awake()
    {
        sceneManager = GameObject.Find("SceneManager");

        count[0] = sceneManager.GetComponent<PlayerManagerScript>()
            .playerCharaNum[(int)ConstRepo.Player.P1];

        count[1] = sceneManager.GetComponent<PlayerManagerScript>()
            .playerCharaNum[(int)ConstRepo.Player.P2];
    }



    private void Start()
    {
        switch (count[0])
        {
            // case�̎n�܂�̒l�� 0����ɕύX by�����@7/1
            case 0:
                chara1P[0].SetActive(true);
                chara1P[1].SetActive(false);
                chara1P[2].SetActive(false);
                chara1P[3].SetActive(false);
                break;

            case 1:
                chara1P[0].SetActive(false);
                chara1P[1].SetActive(true);
                chara1P[2].SetActive(false);
                chara1P[3].SetActive(false);
                break;
            case 2:
                chara1P[0].SetActive(false);
                chara1P[1].SetActive(false);
                chara1P[2].SetActive(true);
                chara1P[3].SetActive(false);
                break;
            case 3:
                chara1P[0].SetActive(false);
                chara1P[1].SetActive(false);
                chara1P[2].SetActive(false);
                chara1P[3].SetActive(true);
                break;
            default:
                break;
        }



        switch (count[1])
        {
            case 0:
                chara2P[0].SetActive(true);
                chara2P[1].SetActive(false);
                chara2P[2].SetActive(false);
                chara2P[3].SetActive(false);
                break;

            case 1:
                chara2P[0].SetActive(false);
                chara2P[1].SetActive(true);
                chara2P[2].SetActive(false);
                chara2P[3].SetActive(false);
                break;
            case 2:
                chara2P[0].SetActive(false);
                chara2P[1].SetActive(false);
                chara2P[2].SetActive(true);
                chara2P[3].SetActive(false);
                break;
            case 3:
                chara2P[0].SetActive(false);
                chara2P[1].SetActive(false);
                chara2P[2].SetActive(false);
                chara2P[3].SetActive(true);
                break;
            default:
                break;
        }

        // メインシーンのみ
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            // シャッターが開いた辺りで、アニメーション再生
            Invoke("AllChara_Anim_Choice", 1.5f);
        }

    }

    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Z))
        //{
        //    if (count[0] == 3)
        //    {
        //        count[0] = 0;
        //        return;
        //    }
        //    count[0]++;
        //}
        //else if(Input.GetKeyDown(KeyCode.X))
        //{
        //    if(count[0] == 0)
        //    {
        //        count[0] = 3;
        //        return;
        //    }
        //    count[0]--;
        //}
        //switch (count[0])
        //{
        //    // case�̎n�܂�̒l�� 0����ɕύX by�����@7/1
        //    case 0:
        //        chara1P[0].SetActive(true);
        //        chara1P[1].SetActive(false);
        //        chara1P[2].SetActive(false);
        //        chara1P[3].SetActive(false);
        //        break;

        //    case 1:
        //        chara1P[0].SetActive(false);
        //        chara1P[1].SetActive(true);
        //        chara1P[2].SetActive(false);
        //        chara1P[3].SetActive(false);
        //        break;
        //    case 2:
        //        chara1P[0].SetActive(false);
        //        chara1P[1].SetActive(false);
        //        chara1P[2].SetActive(true);
        //        chara1P[3].SetActive(false);
        //        break;
        //    case 3:
        //        chara1P[0].SetActive(false);
        //        chara1P[1].SetActive(false);
        //        chara1P[2].SetActive(false);
        //        chara1P[3].SetActive(true);
        //        break;
        //    default:
        //        break;
        //}



        //if(Input.GetKeyDown(KeyCode.C))
        //{
        //    if(count[1] == 3)
        //    {
        //        count[1] = 0;
        //        return;
        //    }
        //    count[1]++;
        //}
        //else if(Input.GetKeyDown(KeyCode.V))
        //{
        //    if(count[1] == 0)
        //    {
        //        count[1] = 3;
        //        return;
        //    }
        //    count[1]--;
        //}
        //switch (count[1])
        //{
        //    case 0:
        //        chara2P[0].SetActive(true);
        //        chara2P[1].SetActive(false);
        //        chara2P[2].SetActive(false);
        //        chara2P[3].SetActive(false);
        //        break;

        //    case 1:
        //        chara2P[0].SetActive(false);
        //        chara2P[1].SetActive(true);
        //        chara2P[2].SetActive(false);
        //        chara2P[3].SetActive(false);
        //        break;
        //    case 2:
        //        chara2P[0].SetActive(false);
        //        chara2P[1].SetActive(false);
        //        chara2P[2].SetActive(true);
        //        chara2P[3].SetActive(false);
        //        break;
        //    case 3:
        //        chara2P[0].SetActive(false);
        //        chara2P[1].SetActive(false);
        //        chara2P[2].SetActive(false);
        //        chara2P[3].SetActive(true);
        //        break;
        //    default:
        //        break;
        //}

        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    Chara_Anim_Att(0);
        //}
        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    Chara_Anim_Hit(0);
        //}
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    Chara_Anim_Win(0);
        //}


        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    Chara_Anim_Att(1);
        //}
        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    Chara_Anim_Hit(1);
        //}
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    Chara_Anim_Win(1);
        //}
    }

    /// <summary>
    /// 両プレイヤーのキャラに、チョイスアニメーションをさせる
    /// </summary>
    private void AllChara_Anim_Choice()
    {
        chara1P[count[(int)ConstRepo.Player.P1]].GetComponent<Live2D_AnimController>().Anim_Choice();
        chara2P[count[(int)ConstRepo.Player.P2]].GetComponent<Live2D_AnimController>().Anim_Choice();
    }

    /// <summary>
    /// 引数のプレイヤーのキャラに、アタックアニメーションをさせる
    /// </summary>
    /// <param name="usePlayer"></param>
    public void Chara_Anim_Att(int usePlayer)
    {
        if (usePlayer == 0)
        {
            chara1P[count[0]].GetComponent<Live2D_AnimController>().Anim_Att();
        }
        else
        {
            chara2P[count[1]].GetComponent<Live2D_AnimController>().Anim_Att();
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public void Chara_Anim()
    {
        chara1P[count[0]].GetComponent<Live2D_AnimController>().Anim_Choice();
        chara2P[count[1]].GetComponent<Live2D_AnimController>().Anim_Choice();
    }

    public void Chara_Anim_Hit(int usePlayer)
    {
        if (usePlayer == 0)
        {
            chara1P[count[0]].GetComponent<Live2D_AnimController>().Anim_Hit();
        }
        else
        {
            chara2P[count[1]].GetComponent<Live2D_AnimController>().Anim_Hit();
        }
    }

    public void Chara_Anim_Win(int usePlayer)
    {

        if (usePlayer == 0)
        {
            chara1P[count[0]].GetComponent<Live2D_AnimController>().Anim_Win();
        }
        else
        {
            chara2P[count[1]].GetComponent<Live2D_AnimController>().Anim_Win();
        }
    }


    //public void CharaPosReset(int usePlayer)
    //{
    //    if (usePlayer == 0)
    //    {
    //        chara1P[count[0]].transform.position -= new Vector3(
    //            chara1P[count[0]].transform.position.x, 0, 0);
    //    }
    //    else
    //    {
    //        chara2P[count[1]].transform.position -= new Vector3(
    //            chara1P[count[0]].transform.position.x, 0, 0);
    //    }
    //}
}
