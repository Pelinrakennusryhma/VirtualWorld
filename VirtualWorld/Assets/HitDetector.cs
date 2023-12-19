using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetector : MonoBehaviour
{
    public enum DetectorType
    {
        None = 0,
        Body = 1,
        Head = 2,
        Leg = 3,
        Arm = 4
    }

    public DetectorType detectorType;

    private Health OwnerHealth;

    public void Init(Health owner)
    {
        OwnerHealth = owner;
    }

    public void OnHit(int damageAmount)
    {
        OwnerHealth.TakeDamage(damageAmount);
    }

    public void OnHit(Health.DamageType damageType)
    {
        OwnerHealth.TakeDamage(damageType, detectorType);
    }

    public void AddForceToPos(Vector3 position,
                              Vector3 force,
                              ForceMode forceMode)
    {
        OwnerHealth.AddForceAtPos(position,
                                  force,
                                  forceMode);
    }
}
