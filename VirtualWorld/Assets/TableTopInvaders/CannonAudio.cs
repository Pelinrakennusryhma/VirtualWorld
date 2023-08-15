using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonAudio : MonoBehaviour
{
    public AudioSource AudioSource;

    public AudioClip Fire1;

    public float originalPitch;

    public void Awake()
    {
        originalPitch = AudioSource.pitch;
    }

    public void Fire()
    {
        AudioSource.pitch = originalPitch + Random.Range(-0.2f, 0.2f);
        AudioSource.PlayOneShot(Fire1);
    }
}
