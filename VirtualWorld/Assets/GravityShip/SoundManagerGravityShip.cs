using Audio;
using GravityShip;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerGravityShip : MonoBehaviour
{
    public void PlayUIClick()
    {
        AudioManager.Instance.PlayOneShot(FMODEventsGravityShip.Instance.UIClick, Vector3.zero);
    }

    public void PlayDeathSound()
    {
        AudioManager.Instance.PlayOneShot(FMODEventsGravityShip.Instance.Explosion, Vector3.zero);
    }

    public void PlayOnBoost()
    {
        AudioManager.Instance.PlayOneShot(FMODEventsGravityShip.Instance.Boost, Vector3.zero);
    }

    public void PlayOnPulsar()
    {
        AudioManager.Instance.PlayOneShot(FMODEventsGravityShip.Instance.Pulsar, Vector3.zero);
    }
}
