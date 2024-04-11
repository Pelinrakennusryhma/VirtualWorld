using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTagInitialSpawnSpot : MonoBehaviour
{
    public void Awake()
    {

        return;
        StarterAssets.ThirdPersonController[] thirds = FindObjectsOfType<StarterAssets.ThirdPersonController>();

        for (int i = 0; i< thirds.Length; i++)
        {
            thirds[i].TryToSpawnToLaserTagSpot();
        }
    }

    public void GetPosAndRot(out Vector3 pos,
                             out Quaternion rot)
    {
        pos = transform.position;
        rot = transform.rotation;
    }
}
