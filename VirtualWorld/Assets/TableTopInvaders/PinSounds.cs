using Audio;
using System.Collections;
using System.Collections.Generic;
using TableTopInvaders;
using UnityEngine;

public class PinSounds : MonoBehaviour
{
    public Pin Pin;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lattia"))
        {
            Pin.OnHitTheGround(other);
            AudioManager.Instance.PlayOneShot(FMODEventsTTI.Instance.ToppleOver, Vector3.zero);
        }
    }

    public void Spawn()
    {
        AudioManager.Instance.PlayOneShot(FMODEventsTTI.Instance.SpawnPin, Vector3.zero);
    }

    public void Die()
    {
        AudioManager.Instance.PlayOneShot(FMODEventsTTI.Instance.FadeOut, Vector3.zero);
    }

    public void BallHitPin(float velocity)
    {
        if (velocity > 10)
        {
            AudioManager.Instance.PlayOneShot(FMODEventsTTI.Instance.BallHit3, Vector3.zero);
        }

        else if (velocity > 7)
        {
            AudioManager.Instance.PlayOneShot(FMODEventsTTI.Instance.BallHit2, Vector3.zero);
        }

        else if(velocity > 3)
        {
            AudioManager.Instance.PlayOneShot(FMODEventsTTI.Instance.BallHit1, Vector3.zero);
        }
    }
}
