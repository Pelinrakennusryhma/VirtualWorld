using Audio;
using System.Collections;
using System.Collections.Generic;
using TableTopInvaders;
using UnityEngine;

public class PickupAudio : MonoBehaviour
{
    public void Spawn()
    {
        AudioManager.Instance.PlayOneShot(FMODEventsTTI.Instance.SpawnPickup, Vector3.zero);
    }

    public void Explode()
    {
        AudioManager.Instance.PlayOneShot(FMODEventsTTI.Instance.Explosion, Vector3.zero);
    }
}
