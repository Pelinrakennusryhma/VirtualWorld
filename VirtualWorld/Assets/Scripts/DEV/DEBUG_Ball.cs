using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class DEBUG_Ball : NetworkBehaviour
{
    void Awake()
    {
        Debug.Log("--------- Playground Scene Loaded! ---------");
    }
}
