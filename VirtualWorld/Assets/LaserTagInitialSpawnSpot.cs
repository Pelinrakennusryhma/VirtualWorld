using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTagInitialSpawnSpot : MonoBehaviour
{
    public void GetPosAndRot(out Vector3 pos,
                             out Quaternion rot)
    {
        pos = transform.position;
        rot = transform.rotation;
    }
}
