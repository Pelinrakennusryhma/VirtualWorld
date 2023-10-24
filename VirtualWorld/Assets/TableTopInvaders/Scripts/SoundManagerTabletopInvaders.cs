using Audio;
using System.Collections;
using System.Collections.Generic;
using TableTopInvaders;
using UnityEngine;

public class SoundManagerTabletopInvaders : MonoBehaviour
{

    public void PlayCountdown1()
    {
        AudioManager.Instance.PlayOneShot(FMODEventsTTI.Instance.Countdown123, Vector3.zero);
        //AudioSource2D.volume = 1.0f;
    }

    public void PlayCountdownGo()
    {
        AudioManager.Instance.PlayOneShot(FMODEventsTTI.Instance.CountdownGo, Vector3.zero);
        //AudioSource2D.volume = 1.0f;
    }

    public void PlayUIPress()
    {
        AudioManager.Instance.PlayOneShot(FMODEventsTTI.Instance.UIPress, Vector3.zero);
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
