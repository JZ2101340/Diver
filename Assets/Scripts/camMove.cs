using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camMove : MonoBehaviour
{
    public float camSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(camSpeed * Time.deltaTime, 0, 0);
    }
}
