using Characters;
using FishNet;
using FishNet.Managing.Scened;
using FishNet.Object;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Networking
{
    public class NetworkSceneInit : MonoBehaviour
    {
        [field: SerializeField] public Transform PlayerCharacterSpawnSpot { get; private set; }
    }
}

