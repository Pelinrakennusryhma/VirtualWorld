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

        public GameObject PlayerPrefab;
        public static NetworkSceneLoader Instance { get; private set; }
        [SerializeField] string mainSceneName;
        [SerializeField] List<string> otherSceneNames;

        ///<summary>
        ///Used for finding a scene reference by name. Populated whenever a scene is loaded on the server.
        ///</summary>
        Dictionary<string, Scene> scenesLoaded = new();

        private NetworkConnection pendingConn;
        SceneUnloadData pendingSceneUnloadData;
        private string sceneToUnloadName;
        private Scene newSceneRef;


        public Scene LatestLoaded;

        private SceneUnloadData scneUnloadData;

        public NetworkConnection pendingConn2;



        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;

                //otherSceneNames = new List<string>();
              
                //otherSceneNames.Add("LobbyLaserTag");
                //otherSceneNames.Add("Map3LaserTag");
                //otherSceneNames.Add("Map2LaserTag");
                //otherSceneNames.Add("Map1LaserTag");                
                //otherSceneNames.Add("Farm"); 

            }
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            SceneManager.OnLoadEnd += RegisterScenes;
            SceneManager.OnLoadEnd -= OkayToStartUnloading;
            SceneManager.OnLoadEnd += OkayToStartUnloading;
            SceneManager.OnLoadEnd -= OnSceneLoadEnded;
            SceneManager.OnLoadEnd += OnSceneLoadEnded;


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

        public void MoveToNetworkScene(NetworkConnection conn, string sceneToLoadName)
        {
            string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            MoveToNetworkSceneServerRpc(conn, sceneToLoadName, currentSceneName, CreateMovedNetworkObjects());

            Debug.LogError("Moving to networked scene");
        }
        ///<summary>
        ///NetworkObjects that should be moved to another network scene.
        ///Currently an array of one object, the player character.
        ///</summary>
        NetworkObject[] CreateMovedNetworkObjects()
        {
            NetworkObject[] movedNetworkObjects = new NetworkObject[1];

            if (CharacterManager.Instance.OwnedCharacter != null) 
            {
                NetworkObject playerCharacter = CharacterManager.Instance.OwnedCharacter.GetComponent<NetworkObject>();
                movedNetworkObjects[0] = playerCharacter;
            }

            else
            {
                movedNetworkObjects = null;
            }

            return movedNetworkObjects;
        }

        [ServerRpc(RequireOwnership = false)]
        void MoveToNetworkSceneServerRpc(NetworkConnection conn, string sceneToLoadName, string sceneToUnloadName, NetworkObject[] movedNetworkObjects)
        {



            // These scenes are loaded on the server - find a reference to them by name.
            Scene newSceneRef = scenesLoaded[sceneToLoadName];
            Scene oldSceneRef = scenesLoaded[sceneToUnloadName];

            //Debug.LogError("New scene name is " + newSceneRef.name + " old scene name is " + oldSceneRef.name);

            // Used for keeping scene alive on the server after last client unloads it.
            SceneLookupData activeScene = new SceneLookupData(newSceneRef);

            // Pass the old scene's name as params so we can use that to determine the teleport point in the new scene.
            byte[] oldSceneNameAsBytes = System.Text.Encoding.UTF8.GetBytes(sceneToUnloadName);

            SceneLoadData sld = new(newSceneRef)
            {
                MovedNetworkObjects = movedNetworkObjects,
                Params = new LoadParams()
                {
                    ClientParams = oldSceneNameAsBytes,
                },
                PreferredActiveScene = activeScene
            };

            SceneUnloadData sud = new(oldSceneRef);
            sud.Options.Mode = ServerUnloadMode.KeepUnused;


            for (int i =0; i < movedNetworkObjects.Length; i++)
            {
                movedNetworkObjects[i].gameObject.GetComponent<SceneMover>().SetSceneWeShouldBeInNonNetworked(newSceneRef.name);
            }
            //for (int i = 0; i < movedNetworkObjects.Length; i++)
            //{
            //    movedNetworkObjects[i].GetComponent<StarterAssets.ThirdPersonController>().DontDestroyOnLoad();
            //}


            //SceneMover[] sceneMovers = FindObjectsOfType<SceneMover>();

            //for (int i = 0; i < sceneMovers.Length; i++)
            //{
            //    sceneMovers[i].MakeSureTheObjectDoesNotGetDestroyed(sceneMovers[i].gameObject);
            //}

            //SceneManager.UnloadConnectionScenes(conn, sud);
            if (sceneToLoadName.Equals(mainSceneName))
            {

                SceneManager.LoadConnectionScenes(conn, sld);
                UnpackMainSceneOnClient(conn, sceneToLoadName);

                //SpawnANewPlayerTArgetRpc(conn);
                //SpawnANewPlayer(conn, newSceneRef);


                //pendingConn = conn;
                //this.newSceneRef = newSceneRef;

                //for (int i = 0; i < movedNetworkObjects.Length; i++)
                //{
                //    UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(movedNetworkObjects[i].gameObject, newSceneRef);
                //}

                //SceneManager.UnloadConnectionScenes(conn, sud); 
                //conn = null;
            }

            else
            {
                SceneManager.LoadConnectionScenes(conn, sld);
            }



            //SceneManager.LoadConnectionScenes(conn, sld);

            //SceneManager.UnloadConnectionScenes(conn, sud); // The original place for this call. After load
            //UnloadSceneOnClient(conn, sceneToUnloadName);

            LatestLoaded = newSceneRef;

            scneUnloadData = sud;
            pendingConn = conn;
            pendingSceneUnloadData = sud;
            pendingConn2 = conn;

            this.sceneToUnloadName = sceneToUnloadName;

            //DoTheMoveToThingsAndStuff(conn);

            //UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneToUnloadName);

            //Debug.LogError("Moving to network scene server rpc completed");

            for (int i = 0; i < movedNetworkObjects.Length; i++)
            {
                UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(movedNetworkObjects[i].gameObject, UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneToLoadName));

                Scene correct = newSceneRef;
                gameObject.GetComponent<SceneMover>().MoveYourAss(pendingConn, correct);
                CharacterManager.DoSomethingHeinous(pendingConn, correct);
            }
        }

        private static void DoTheMoveToThingsAndStuff(NetworkConnection conn)
        {
            SceneMover[] sceneMovers = FindObjectsOfType<SceneMover>(true);





            for (int i = 0; i < sceneMovers.Length; i++)
            {

                UnityEngine.SceneManagement.Scene sceeen = sceneMovers[i].gameObject.scene;

                Debug.Log("At do the move things and stuff scene name is " + sceeen.name);

                //SpawnObject(sceneMovers[i].gameObject, conn, sceeen);

                sceneMovers[i].SetSceneWeShouldBeIn(sceeen.name);
                sceneMovers[i].SaveSceneNameAsString(conn, sceeen.name);
                sceneMovers[i].MoveCharacterToScene(conn, sceeen, sceeen.name);
                //sceneMovers[i].MoveCharacterToSceneSceen(sceeen, sceeen.name);

                sceneMovers[i].OnActiveSceneChanged();
            }
        }



        [TargetRpc]

        public void UnpackMainSceneOnClient(NetworkConnection target,
                                            string sceneToUnpack)
        {            
            //NetworkSceneLoader.Instance.MoveToNetworkScene(target, sceneToUnpack);
            SceneLoader.Instance.UnpackScene(sceneToUnpack);

            UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneToUnpack));



            Debug.Log("Unpacking main scene");

            //SceneLoader.Instance.MoveToMainScene(CharacterManager.Instance.OwnedCharacter.gameObject);
        }

        [TargetRpc]
        public void UnloadSceneOnClient(NetworkConnection target,
                                        string sceneToUnloadName)
        {

            //UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneToUnloadName);
            //return;

            Debug.LogError("Unloading scene on client. About to do packing to main scene");

            //if (!sceneToUnloadName.Equals(mainSceneName)) 
            //{
            //    UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneToUnloadName);
            //}

            //else
            //{
            //    SceneLoader.Instance.PackScene(sceneToUnloadName, ScenePackMode.ALL);
            //}

            if (!sceneToUnloadName.Equals(mainSceneName)) 
            {
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneToUnloadName);

                //SpawnANewPlayer(pendingConn, newSceneRef);
                Debug.LogError("Should unload scene");
            }

            else
            {
                SceneLoader.Instance.PackScene(sceneToUnloadName, ScenePackMode.ALL, true);

                Debug.Log("Packing main scene");
            }

            //SceneMover[] sceneMovers = FindObjectsOfType<SceneMover>(true);

            //for (int i  = 0; i < sceneMovers.Length; i++)
            //{
                
            //    UnityEngine.SceneManagement.Scene sceeen = LatestLoaded;
            //    sceneMovers[i].MoveCharacterToScene(sceeen);
            //}
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
            SceneManager.OnLoadEnd -= OkayToStartUnloading;
        }

        public void OkayToStartUnloading(SceneLoadEndEventArgs args)
        {
            Debug.LogError("About to do unloading");
            SceneMover[] sceneMovers = FindObjectsOfType<SceneMover>(true);



            for (int i = 0; i < sceneMovers.Length; i++)
            {
                if (sceneMovers[i].gameObject.scene.name.Equals(sceneToUnloadName)) 
                {
                    sceneMovers[i].MakeSureTheObjectDoesNotGetDestroyed(sceneMovers[i].gameObject);
                    sceneMovers[i].PackNonPlayerPlayer(pendingConn);
                }

                else
                {

                    //sceneMovers[i].UnpackNonPlayerPlayer(pendingConn);
                }
                Debug.Log("Scene mover gameobject scene is " + sceneMovers[i].gameObject.scene.name);
            }


            //if (pendingConn != null
            //    && pendingSceneUnloadData != null) 
            //{
            //    SceneManager.UnloadConnectionScenes(pendingConn, pendingSceneUnloadData);
            //}

            //pendingConn = null;
            //pendingSceneUnloadData = null;

            Debug.LogError("STARTED UNLOADING");

            if (pendingConn != null) 
            {
                UnloadSceneOnClient(pendingConn, sceneToUnloadName);
                Debug.LogError("should unload " + sceneToUnloadName);

                //if (!sceneToUnloadName.Equals(mainSceneName))
                //{
                //    SpawnANewPlayer(pendingConn, newSceneRef);
                //}

                //DoTheMoveToThingsAndStuff(pendingConn);
            }

            //SpawnANewPlayer(pendingConn, LatestLoaded);

            pendingConn = null;
            sceneToUnloadName = "";


        }

        private void OnSceneLoadEnded(SceneLoadEndEventArgs args)
        {
            DoTheMoveToThingsAndStuff(pendingConn2);

            Debug.LogError("On scene load ended at network scene loader");

            SceneMover[] sceneMovers = FindObjectsOfType<SceneMover>(true);

            for (int i = 0; i < sceneMovers.Length; i++) 
            {
                Debug.LogError("We have a scene mover at scene " + sceneMovers[i].gameObject.scene.name);
                sceneMovers[i].OnActiveSceneSet(pendingConn2, sceneMovers[i].gameObject.scene);
            }
        }


        public void MovePlayerToMainScene(NetworkObject[] movedObjects,
                                          string scene)
        {

        }

        [TargetRpc]
        public void SpawnANewPlayerTArgetRpc(NetworkConnection conn)
        {
            //Debug.LogError("Should spawn a new player");
           // CharacterManager.Instance.Respawn();
        }

        public void SpawnANewPlayer(NetworkConnection ownerConnection,
                                    Scene sceneToSpawnTo)
        {
            GameObject go = Instantiate(PlayerPrefab);
            //UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(go, sceneToSpawnTo);
            NetworkObject nob = go.GetComponent<NetworkObject>();
            //InstanceFinder.ServerManager.Spawn(go, ownerConnection, sceneToSpawnTo);

            Debug.LogError("Scene to spawn to is " + sceneToSpawnTo.name);
            Instance.ServerManager.Spawn(nob, ownerConnection, sceneToSpawnTo);
            OnRespawn(ownerConnection);
            Debug.LogError("spawning a new player. Go scene is " + go.gameObject.scene.name);
        }

        [TargetRpc]
        public void OnRespawn(NetworkConnection connection)
        {
            NetworkObjectSpawner.Instance.ReInit();
        }

        public Scene GetLatestLoaded()
        {
            return LatestLoaded;
        }


        public Scene GetCorrectScene(GameObject obj)
        {
            return obj.scene;
        }

        public static void SpawnObject(GameObject obj, NetworkConnection connection, Scene sceneToSpawnTo)
        {
            Instance.ServerManager.Spawn(obj, connection, sceneToSpawnTo);
        }
    }
}

