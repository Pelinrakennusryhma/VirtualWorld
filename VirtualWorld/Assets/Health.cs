using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public enum DamageType
    {
        None = 0,
        LaserGun = 1,
        PlasmaCutter = 2
    }

    public DamageResponse DamageResponse;

    public int HitPoints;
    public int MaxHitPoints;

    public delegate void DamageTake(int damage);
    public DamageTake OnDamageTaken;

    public delegate void Die();
    public Die OnDie;

    public delegate void AddForceAtPosition(Vector3 hitPoint, Vector3 forceToAdd, ForceMode forceMode);
    public AddForceAtPosition OnAddForceAtPosition;

    public HitDetector[] HitDetectors;

    private void Awake()
    {
        HitDetectors = GetComponentsInChildren<HitDetector>(true);

        for (int i = 0; i < HitDetectors.Length; i++)
        {
            HitDetectors[i].Init(this);
        }
    }

    public void ResetHealth()
    {
        HitPoints = MaxHitPoints;
    }

    public void ResetHealth(int maxHitPoints)
    {
        MaxHitPoints = maxHitPoints;
        HitPoints = MaxHitPoints;
    }

    public void TakeDamage(int amount)
    {
        if(OnDamageTaken != null)
        {
            OnDamageTaken(amount);
        }

        OnTakeDamage(amount);
    }

    public void TakeDamage(DamageType damageType,
                           HitDetector.DetectorType detector)
    {
        int amount = 0;

        amount = CheckDamageAmount(damageType, 
                                   detector, 
                                   amount);

        TakeDamage(amount);
    }

    public void AddForceAtPos(Vector3 hitPoint,
                              Vector3 force,
                              ForceMode forceMode)
    {
        if (OnAddForceAtPosition != null)
        {
            OnAddForceAtPosition(hitPoint, force, forceMode);
        }
    }

    private void OnTakeDamage(int amount)
    {
        HitPoints -= amount;

        //Debug.Log("We took damage " + gameObject.name + " current amount of hit points is " + HitPoints);

        if (HitPoints <= 0)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        if(OnDie != null)
        {
            OnDie();
            //Debug.Log("We had a listener to OnDie()");
        }

        //Debug.Log("We died " + gameObject.name);
    }

    private int CheckDamageAmount(DamageType damageType,
                              HitDetector.DetectorType detector,
                              int amount)
    {
        if (damageType == DamageType.LaserGun)
        {
            if (detector == HitDetector.DetectorType.Body)
            {
                amount = DamageResponse.DamageFromLaserGunBody;
            }

            else if (detector == HitDetector.DetectorType.Head)
            {
                amount = DamageResponse.DamageFromLaserGunHead;
            }

            else if (detector == HitDetector.DetectorType.Leg)
            {
                amount = DamageResponse.DamageFromLaserGunLeg;
            }

            else if (detector == HitDetector.DetectorType.Arm)
            {
                amount = DamageResponse.DamageFromLaserGunArm;
            }

            else
            {
                amount = 0;
                Debug.LogWarning("We don't have a good hit detector. No damage is given");
            }

        }

        else if (damageType == DamageType.PlasmaCutter)
        {
            if (detector == HitDetector.DetectorType.Body)
            {
                amount = DamageResponse.DamageFromPlasmaCutterBody;
            }

            else if (detector == HitDetector.DetectorType.Head)
            {
                amount = DamageResponse.DamageFromPlasmaCutterHead;
            }

            else if (detector == HitDetector.DetectorType.Leg)
            {
                amount = DamageResponse.DamageFromPlasmaCutterLeg;
            }

            else if (detector == HitDetector.DetectorType.Arm)
            {
                amount = DamageResponse.DamageFromPlasmaCutterArm;
            }

            else
            {
                amount = 0;
                Debug.LogWarning("We don't have a good hit detector. No damage is given");
            }
        }

        return amount;
    }
}
