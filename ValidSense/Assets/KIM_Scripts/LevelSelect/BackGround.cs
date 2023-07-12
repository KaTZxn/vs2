using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGround : MonoBehaviour
{
    [SerializeField]
    JsonReader json;

    [SerializeField]
    SpriteRenderer overTexuter_Renderer;

    [SerializeField]
    Animator overTexture_Animator;

    SpriteRenderer spriteRenderer;

    [SerializeField]
    List<Sprite> firstSprite;

    [SerializeField]
    List<Sprite> secondSprite;

    public int songNum;

    // Start is called before the first frame update
    void Start()
    {
        json = GameObject.Find("Json").GetComponent<JsonReader>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        songNum = json._songList.songdata.songnum;

        spriteRenderer.sprite = firstSprite[songNum];

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Z))
        {
            NextSprite();
        }
        
    }

    public void NextSprite()
    {
        spriteRenderer.sortingOrder = 10;
        //spriteRenderer.sprite = secondSprite[songNum];
        overTexuter_Renderer.sprite = secondSprite[songNum];
        overTexture_Animator.SetTrigger("OverTexture_FadeIn");
    }
}
