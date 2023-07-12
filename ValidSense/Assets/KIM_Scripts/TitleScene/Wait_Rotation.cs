using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait_Rotation : MonoBehaviour
{
    [SerializeField]
    GameObject entry_Wait1;

    [SerializeField]
    GameObject entry_Wait2;

    [SerializeField]
    float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        entry_Wait1.transform.Rotate(new Vector3(0, 0, speed * Time.deltaTime));
        entry_Wait2.transform.Rotate(new Vector3(0, 0, speed * Time.deltaTime));
    }
}
