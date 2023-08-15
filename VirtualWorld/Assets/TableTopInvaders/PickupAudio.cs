using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAudio : MonoBehaviour
{
    public AudioSource AudioSource;
    public AudioClip ExplosionClip;

    public AudioClip SpawnClip;

    public float originalPitch;

    private float explosionVolume = 1.0f;
    private float spawnVolume = 0.25f;

    public void Awake()
    {
        originalPitch = AudioSource.pitch;
    }

    public void Spawn()
    {
        AudioSource.pitch = originalPitch + Random.Range(-0.2f, 0.2f);
        AudioSource.PlayOneShot(SpawnClip, spawnVolume);
    }

    public void Explode()
    {
        AudioSource.pitch = originalPitch + Random.Range(-0.2f, 0.2f);
        AudioSource.PlayOneShot(ExplosionClip, explosionVolume);
    }
}
