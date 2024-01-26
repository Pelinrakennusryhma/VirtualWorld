using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageData", menuName = "ScriptableObjects/Damage/DamageResponse", order = 0)]
public class DamageResponse : ScriptableObject
{
    public int DamageFromLaserGunBody;
    public int DamageFromLaserGunHead;
    public int DamageFromLaserGunArm;
    public int DamageFromLaserGunLeg;
    public int DamageFromPlasmaCutterBody;
    public int DamageFromPlasmaCutterHead;
    public int DamageFromPlasmaCutterArm;
    public int DamageFromPlasmaCutterLeg;
}
