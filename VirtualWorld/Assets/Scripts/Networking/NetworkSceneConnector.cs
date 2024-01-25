using AYellowpaper.SerializedCollections;
using Characters;
using FishNet;
using FishNet.Managing.Scened;
using FishNet.Object;
using StarterAssets;
using UnityEngine;

namespace Networking
{
    public class NetworkSceneConnector : MonoBehaviour
    {
        [SerializedDictionary("Source scene", "Target transform")]
        [SerializeField] SerializedDictionary<string, Transform> entryPointsFromScenes;

        ///<summary>
        ///Returns a arrival spot for player character, based on the name of the scene where they are coming from.
        ///</summary>
        public Transform GetSpawnTransform(string sceneName)
        {
            return entryPointsFromScenes[sceneName];
        }
    }
}

