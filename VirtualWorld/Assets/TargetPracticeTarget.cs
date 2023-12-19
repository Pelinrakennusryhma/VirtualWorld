using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPracticeTarget : MonoBehaviour
{
    private bool isDead;
    public bool IsDead { get => isDead; 
                         private set => isDead = value; }

    private Health Health;

    private Rigidbody Rigidbody;

    private MeshCollider[] MeshColliders;



    private void Awake()
    {
        IsDead = false;

        Health = GetComponent<Health>();
        Health.ResetHealth(1);

        Health.OnDie -= OnDeath;
        Health.OnDie += OnDeath;
        Health.OnDamageTaken -= TakingDamage;
        Health.OnDamageTaken += TakingDamage;

        Rigidbody = GetComponent<Rigidbody>();
        MeshColliders = GetComponentsInChildren<MeshCollider>();

        SetUnderPhysics();
    }

    public void TakingDamage(int damage)
    {
        //Debug.Log("Target practice target knows we are taking damage: " + damage);

        if (Health.HitPoints - damage <= 1)
        {
            SetUnderPhysics();
        }
    }

    public void OnDeath()
    {
        IsDead = true;

        //Destroy(gameObject);
    }

    public void SetUnderPhysics()
    {
        for (int i = 0; i < MeshColliders.Length; i++)
        {
            MeshColliders[i].convex = true;
        }

        Rigidbody.isKinematic = false;
        Rigidbody.useGravity = true;
        Rigidbody.mass = 10;
    }
}
