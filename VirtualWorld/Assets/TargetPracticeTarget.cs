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

    public delegate void DeathHappened();
    public DeathHappened OnDeathHappened;

    private float forceMultiplier = 1.0f;

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

        RegisterToTakeForceOnHit();
    }

    public void RegisterToTakeForceOnHit()
    {
        Health.OnAddForceAtPosition -= AddForceAtPos;
        Health.OnAddForceAtPosition += AddForceAtPos;
    }

    public void SetForceMultiplier(float multiplier)
    {
        forceMultiplier = multiplier;
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

        if (OnDeathHappened != null)
        {
            OnDeathHappened();
        }

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
        Rigidbody.velocity = Vector3.zero;
        Rigidbody.angularVelocity = Vector3.zero;
    }

    public void SetKinematic()
    {
        for (int i = 0; i < MeshColliders.Length; i++)
        {
            MeshColliders[i].convex = true;
        }

        Rigidbody.isKinematic = true;
        Rigidbody.useGravity = false;
        Rigidbody.mass = 10;
    }

    public void AddForceAtPos(Vector3 hitPoint,
                              Vector3 force,
                              ForceMode forceMode)
    {
        SetUnderPhysics();

        //Debug.Log("Force to add is " + force);

        // Wrong order of parameters!!!
        //Rigidbody.AddForceAtPosition(hitPoint, 
        //                             force, 
        //                             forceMode);

        Rigidbody.AddForceAtPosition(force * forceMultiplier, 
                                     hitPoint,
                                     forceMode);
    }

}
