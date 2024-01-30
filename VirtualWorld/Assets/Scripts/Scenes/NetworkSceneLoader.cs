using FishNet.Object;
using UnityEngine;
using FishNet.Managing.Scened;
using System.Collections.Generic;
using System.Linq;
using Dev;
using FishNet.Connection;
using UnityEngine.SceneManagement;
using FishNet;
using Characters;
using System;
using static FishNet.Managing.Scened.UnloadOptions;

namespace Scenes
{
    public class NetworkSceneLoader : NetworkBehaviour
    {
        public static NetworkSceneLoader Instance { get; private set; }
        [SerializeField] string mainSceneName;
        [SerializeField] List<string> otherSceneNames;

        ///<summary>
        ///Used for finding a scene reference by name. Populated whenever a scene is loaded on the server.
        ///</summary>
        Dictionary<string, Scene> scenesLoaded = new();

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            SceneManager.OnLoadEnd += RegisterScenes;
            LoadServerScenes();
        }

        void LoadServerScenes()
        {
            string launchSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            // Load main scene to be active on server and all clients.
            SceneLoadData mainSld = new SceneLoadData(mainSceneName);
            mainSld.Options.AutomaticallyUnload = false;
            SceneManager.LoadGlobalScenes(mainSld);

            // Preload other scenes on server only so later on clients can enter those.
            SceneLoadData otherSld = new SceneLoadData(otherSceneNames);
            otherSld.Options.AutomaticallyUnload = false;
            SceneManager.LoadConnectionScenes(otherSld);

            // Unload launch scene as it's no longer needed
            SceneManager.UnloadConnectionScenes(new SceneUnloadData(launchSceneName));
            Debug.Log("--- SERVER INIT END ---");
        }

        public void MoveToNetworkScene(NetworkConnection conn, string sceneToLoadName, bool bundled)
        {
            string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            if (bundled)
            {
                DownloadSceneAndMove(conn, sceneToLoadName, currentSceneName, CreateMovedNetworkObjects());
            } else
            {
                PlayerEvents.Instance.CallEventSceneLoadStarted(); // Load Ended is called from CharacterManager once character has been inited on the new scene.
                MoveToNetworkSceneServerRpc(conn, sceneToLoadName, currentSceneName, CreateMovedNetworkObjects());
            }


        }

        async void DownloadSceneAndMove(NetworkConnection conn, string sceneToLoadName, string sceneToUnloadName, NetworkObject[] movedNetworkObjects)
        {
            bool bundledSceneLoaded = await AssetBundleLoader.Instance.DownloadBundledScene(sceneToLoadName);

            if (bundledSceneLoaded)
            {
                Debug.Log("Succeeded to load scene from asset bundle.");
                PlayerEvents.Instance.CallEventSceneLoadStarted(); // Load Ended is called from CharacterManager once character has been inited on the new scene.
                MoveToNetworkSceneServerRpc(conn, sceneToLoadName, sceneToUnloadName, CreateMovedNetworkObjects());
            } else
            {
                Debug.LogError("Failed to load scene from asset bundle.");
            }

        }

        ///<summary>
        ///NetworkObjects that should be moved to another network scene.
        ///Currently an array of one object, the player character.
        ///</summary>
        NetworkObject[] CreateMovedNetworkObjects()
        {
            NetworkObject[] movedNetworkObjects = new NetworkObject[1];
            NetworkObject playerCharacter = CharacterManager.Instance.OwnedCharacter.GetComponent<NetworkObject>();
            movedNetworkObjects[0] = playerCharacter;
            return movedNetworkObjects;
        }

        [ServerRpc(RequireOwnership = false)]
        void MoveToNetworkSceneServerRpc(NetworkConnection conn, string sceneToLoadName, string sceneToUnloadName, NetworkObject[] movedNetworkObjects)
        {
            // These scenes are loaded on the server - find a reference to them by name.
            Scene newSceneRef = scenesLoaded[sceneToLoadName];
            Scene oldSceneRef = scenesLoaded[sceneToUnloadName];

            // Used for keeping scene alive on the server after last client unloads it.
            SceneLookupData activeScene = new SceneLookupData(newSceneRef);

            // Pass the old scene's name as params so we can use that to determine the teleport point in the new scene.
            byte[] oldSceneNameAsBytes = System.Text.Encoding.UTF8.GetBytes(sceneToUnloadName);

            SceneLoadData sld = new(newSceneRef) { 
                MovedNetworkObjects = movedNetworkObjects,
                Params = new LoadParams()
                {
                    ClientParams = oldSceneNameAsBytes,
                },
                PreferredActiveScene = activeScene
            };

            SceneUnloadData sud = new(oldSceneRef);
            sud.Options.Mode = ServerUnloadMode.KeepUnused;

            SceneManager.LoadConnectionScenes(conn, sld);
            SceneManager.UnloadConnectionScenes(conn, sud);
        }

        public void RegisterScenes(SceneLoadEndEventArgs args)
        {
            foreach (Scene scene in args.LoadedScenes)
            {
                scenesLoaded.Add(scene.name, scene);
            }
        }

        public void OnDisable()
        {
            SceneManager.OnLoadEnd -= RegisterScenes;
        }
    }
}

