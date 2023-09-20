using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToppleOverAudio : MonoBehaviour
{
    public AudioSource AudioSource;
    public AudioClip ToppleoverClip;

    private float originalPitch;

    public void Awake()
    {
        originalPitch = AudioSource.pitch;    
    }

    public void Spawn()
    {
        AudioSource.pitch = originalPitch + Random.Range(-0.3f, 0.3f);
        AudioSource.PlayOneShot(ToppleoverClip);
        //AudioSource.pitch = originalPitch;
    }
}
