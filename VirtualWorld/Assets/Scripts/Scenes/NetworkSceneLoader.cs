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

namespace Scenes
{
    public class NetworkSceneLoader : NetworkBehaviour
    {
        public static NetworkSceneLoader Instance { get; private set; }
        [SerializeField] string mainSceneName;
        [SerializeField] List<string> otherSceneNames;

        Dictionary<string, Scene> scenesLoaded = new();

        ScenePicker mainScenePicker;
        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                mainScenePicker = GetComponent<ScenePicker>();
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
            SceneManager.LoadGlobalScenes(mainSld);

            // Preload other scenes on server only so later on clients can enter those.
            SceneLoadData otherSld = new SceneLoadData(otherSceneNames);
            SceneManager.LoadConnectionScenes(otherSld);

            // Unload launch scene as it's no longer needed
            SceneManager.UnloadConnectionScenes(new SceneUnloadData(launchSceneName));
            Debug.Log("--- SERVER INIT END ---");
        }

        public void MoveToNetworkScene(NetworkConnection conn, string sceneToLoadName)
        {
            string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            MoveToNetworkSceneServerRpc(conn, sceneToLoadName, currentSceneName, CreateMovedNetworkObjects());
        }

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
            Scene newSceneRef = scenesLoaded[sceneToLoadName];
            Scene oldSceneRef = scenesLoaded[sceneToUnloadName];
            SceneLoadData sld = new(newSceneRef) { MovedNetworkObjects = movedNetworkObjects};
            SceneUnloadData sud = new(oldSceneRef);

            SceneManager.LoadConnectionScenes(conn, sld);

            SceneManager.UnloadConnectionScenes(conn, sud);
        }

        public void RegisterScenes(SceneLoadEndEventArgs args)
        {
            foreach (Scene scene in args.LoadedScenes)
            {
                scenesLoaded.Add(scene.name, scene);
            }

            Utils.DumpToConsole(scenesLoaded);
        }

        public void OnDisable()
        {
            SceneManager.OnLoadEnd -= RegisterScenes;
        }
    }
}

