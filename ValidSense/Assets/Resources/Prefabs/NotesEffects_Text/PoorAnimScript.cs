using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoorAnimScript : MonoBehaviour
{
    [SerializeField]
    private float animTime;

    [SerializeField]
    private List<Texture2D> textures;

    private RawImage rawImage;

    private float updateTime;

    // Start is called before the first frame update
    void Start()
    {
        rawImage = GetComponent<RawImage>();


        StartCoroutine(PoorAnim());
    }

    private IEnumerator PoorAnim()
    {
        updateTime = animTime / textures.Count;

        for (int i = 0; i < textures.Count; i++)
        {
            yield return new WaitForSeconds(updateTime);
            rawImage.texture = textures[i]; 
        }
    }
}
