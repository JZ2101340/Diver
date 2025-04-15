using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endless : MonoBehaviour
{
    public float speed;
    public Renderer renderObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        renderObject.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0f);
    }
}
