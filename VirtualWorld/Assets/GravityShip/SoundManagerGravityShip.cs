using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerGravityShip : MonoBehaviour
{
    public AudioSource AudioSource;
    public AudioClip UIClick;
    public AudioClip ExplosionClip;
    public AudioClip BoostClip;
    public AudioClip PulsarClip;

    public float OriginalAudioSourcePitch;

    public void Awake()
    {
        OriginalAudioSourcePitch = AudioSource.pitch;    
    }

    public void PlayUIClick()
    {
        AudioSource.PlayOneShot(UIClick, 1.0f);
    }

    public void PlayDeathSound()
    {
        AudioSource.pitch = OriginalAudioSourcePitch + Random.Range(-0.2f, 0.2f);
        AudioSource.PlayOneShot(ExplosionClip, 1.0f);
    }

    public void PlayOnBoost()
    {
        AudioSource.pitch = OriginalAudioSourcePitch + Random.Range(-0.2f, 0.2f);
        AudioSource.PlayOneShot(BoostClip, 0.4f);
    }

    public void PlayOnPulsar()
    {
        AudioSource.pitch = OriginalAudioSourcePitch + Random.Range(-0.2f, 0.2f);
        AudioSource.PlayOneShot(PulsarClip, 0.4f);
    }
}
