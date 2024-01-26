using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum WeaponType
    {
        None = 0,
        LaserGun = 1,
        MeleeWeapon = 2
    }

    public WeaponType TypeOfWeapon;

    public virtual void OnFire1Down()
    {
        //Debug.LogWarning("Fire 1 called on weapon base");
    }
}
