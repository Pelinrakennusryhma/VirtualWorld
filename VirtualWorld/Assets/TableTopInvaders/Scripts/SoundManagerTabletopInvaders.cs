using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerTabletopInvaders : MonoBehaviour
{
    public AudioSource AudioSource2D;

    public AudioClip Countdown123;
    public AudioClip CountdownGo;
    public AudioClip UIPress;

    public void PlayCountdown1()
    {
        AudioSource2D.PlayOneShot(Countdown123, 0.2f);
        //AudioSource2D.volume = 1.0f;
    }

    public void PlayCountdownGo()
    {
        AudioSource2D.PlayOneShot(CountdownGo, 0.2f);
        //AudioSource2D.volume = 1.0f;
    }

    public void PlayUIPress()
    {

        AudioSource2D.PlayOneShot(UIPress, 0.2f);
    }

    public void PlaySound(string text)
    {
        //Debug.Log("Play sound " + text + " " + Time.time);
    }

    public void StopSound(string text)
    {
        //Debug.Log("Stop sound " + text + Time.time);
    }

    public void Update()
    {
        // The camera will change between scenes. So we just move this like this every frame. Whatever.

        if (Camera.main != null) 
        {
            transform.position = Camera.main.transform.position;
        }
    }
}
