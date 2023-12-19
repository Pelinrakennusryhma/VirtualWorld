using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BodyPartsDamageTester : MonoBehaviour
{

    public Health Health;
    public VisualEffect Explosion;

    public MeshRenderer[] AllMeshes;
    public Collider[] Colliders;

    private float DisableTimer;

    private bool isDead;

    public void Awake()
    {
        isDead = false;

        Health = GetComponent<Health>();
        Health.ResetHealth(10);
        Health.OnDie -= OnDeath;
        Health.OnDie += OnDeath;

        AllMeshes = GetComponentsInChildren<MeshRenderer>();
        Colliders = GetComponentsInChildren<Collider>();

        Explosion.Stop();
        DisableTimer = -1;
    }

    public void OnDeath()
    {
        if (!isDead) 
        {
            isDead = true;
            Explosion.playRate = 2.0f;
            Explosion.Play();
            DisableTimer = 0.6f;

            //Debug.Log("We should be dead and all ");
        }
    }

    private void DisableObjects()
    {
        for (int i = 0; i < AllMeshes.Length; i++)
        {
            AllMeshes[i].enabled = false;
        }

        for (int i = 0; i < Colliders.Length; i++)
        {
            Colliders[i].enabled = false;
        }
    }

    private void Update()
    {
        if (DisableTimer > 0)
        {
            DisableTimer -= Time.deltaTime;

            if (DisableTimer <= 0)
            {
                DisableObjects();
                DisableTimer = -1;
            }
        }
    }
}
