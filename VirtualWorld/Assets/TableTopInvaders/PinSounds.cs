using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinSounds : MonoBehaviour
{
    public Pin Pin;

    public AudioSource AudioSource;

    public AudioClip Spawn1;
    public AudioClip Spawn2;
    public AudioClip Spawn3;
    public AudioClip Spawn4;
    public AudioClip Spawn5;

    public AudioClip FadeOut1;
    public AudioClip FadeOut2;
    public AudioClip FadeOut3;
    public AudioClip FadeOut4;
    public AudioClip FadeOut5;

    public AudioClip BallHit1;
    public AudioClip BallHit2;
    public AudioClip BallHit3;

    public AudioClip ToppleOver;
    public float originalAudioSourcePitch;

    public void Awake()
    {
        originalAudioSourcePitch = AudioSource.pitch;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lattia"))
        {
            Pin.OnHitTheGround(other);
            AudioSource.pitch = originalAudioSourcePitch + Random.Range(-0.1f, 0.1f);
            AudioSource.PlayOneShot(ToppleOver);
   
            GameFlowManager.Instance.SoundManager.PlaySound("Pin hit the ground ");
        }
    }

    public void Spawn()
    {
        int rand = Random.Range(0, 5);

        AudioClip clip;

        if (rand == 0)
        {
            clip = Spawn1;
        }

        else if (rand == 1)
        {
            clip = Spawn2;
        }

        else if (rand == 2)
        {
            clip = Spawn3;
        }

        else if (rand == 3)
        {
            clip = Spawn4;
        }

        else
        {
            clip = Spawn5;
        }

        AudioSource.pitch = originalAudioSourcePitch + Random.Range(-0.1f, 0.1f);
        AudioSource.PlayOneShot(clip);
    }

    public void Die()
    {
        int rand = Random.Range(0, 5);

        AudioClip clip;

        if (rand == 0)
        {
            clip = FadeOut1;
        }

        else if (rand == 1)
        {
            clip = FadeOut2;
        }

        else if (rand == 2)
        {
            clip = FadeOut3;
        }

        else if (rand == 3)
        {
            clip = FadeOut4;
        }

        else
        {
            clip = FadeOut5;
        }


        AudioSource.pitch = originalAudioSourcePitch + Random.Range(-0.1f, 0.1f);
        AudioSource.PlayOneShot(clip);
    }

    public void BallHitPin(float velocity)
    {
        AudioClip clip = null;

        if (velocity > 10)
        {
            clip = BallHit3;
        }

        else if (velocity > 7)
        {
            clip = BallHit2;
        }

        else if(velocity > 3)
        {
            clip = BallHit1;
        }

        if (clip != null)
        {
            AudioSource.pitch = originalAudioSourcePitch + Random.Range(-0.2f, 0.2f);
            AudioSource.PlayOneShot(clip);
        }
    }
}
