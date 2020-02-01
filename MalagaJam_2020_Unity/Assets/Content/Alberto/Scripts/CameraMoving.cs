using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    public float speed = 0.3f;
    //public AudioSource audioSource = new AudioSource();

    //void Start()
    //{
    //    audioSource.clip = LevelSoundtrack;
    //    audioSource.Play();
    //}
    void Update()
    {
        var pos = transform.position;
        pos.y += Time.deltaTime * speed;
        transform.position = pos;
    }
}
