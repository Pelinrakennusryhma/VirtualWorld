using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAudio : MonoBehaviour
{
    public AudioSource AudioSource;

    public AudioClip HitWall1;
    public AudioClip HitWall2;
    public AudioClip HitWall3;

    private float originalPitch;

    private void Awake()
    {
        originalPitch = AudioSource.pitch;
    }

    public void HitWall(float velocity)
    {
        AudioClip clip = null;


        if (velocity > 12)
        {
            clip = HitWall1;
        }

        else if (velocity > 9)
        {
            clip = HitWall2;
        }

        else if (velocity > 6.0f)
        {
            clip = HitWall3;
        }

        if (clip != null)
        {
            AudioSource.pitch = originalPitch + Random.Range(-0.3f, 0.3f);
            AudioSource.PlayOneShot(clip);
        }
    }
}
