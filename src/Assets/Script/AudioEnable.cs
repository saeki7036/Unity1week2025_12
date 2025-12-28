using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEnable : MonoBehaviour
{
    [SerializeField] AudioSource audio;
    // Update is called once per frame
    void OnEnable() 
    {
        audio.Play();
    }
}
