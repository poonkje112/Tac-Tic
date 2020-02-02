using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    // Start is called before the first frame update  
    public float speed = 1;
    public bool rotate = true;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (rotate)
            transform.eulerAngles += Vector3.forward * speed;
    }
}