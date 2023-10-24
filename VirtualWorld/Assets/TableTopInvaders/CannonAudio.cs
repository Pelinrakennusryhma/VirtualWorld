using Audio;
using System.Collections;
using System.Collections.Generic;
using TableTopInvaders;
using UnityEngine;

public class CannonAudio : MonoBehaviour
{
    public void Fire()
    {
        AudioManager.Instance.PlayOneShot(FMODEventsTTI.Instance.Fire1, Vector3.zero);
    }
}
