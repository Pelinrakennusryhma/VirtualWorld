using UnityEngine;
using Authentication;
using UnityEngine.Events;
using BackendConnection;
using FishNet.Object;
using Dev;
using UI;
using FishNet.Connection;
using System.Collections.Generic;
using Items;
using StarterAssets;
using FishNet;
using FishNet.Managing.Scened;
using Networking;
using UnityEngine.SceneManagement;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
namespace Characters
{
    public class CharacterManager : NetworkBehaviour
    {
        public static CharacterManager Instance { get; private set; }
        public GAME_STATE gameState = GAME_STATE.FREE;
        [field: SerializeField] public GameObject OwnedCharacter { get; private set; }
        private ThirdPersonController ownedController;

        [SerializeField] CharacterData characterData;
        [SerializeField] public PlayerEmitter PlayerEmitter { get; private set; }

        // Antti's addition to help determine who is driving a car.
        public int ClientId;

        public List<SceneMover> sceneMovers = new List<SceneMover>();


        public List<SceneMover> sciinMuuvers = new List<SceneMover>();

        public SceneMover OwnerSceneMover;


        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }
        private void OnDestroy()
        {
            Instance = null;

            //Debug.Log("Character manager got destroyed. Scene is " + gameObject.scene.name);
        }

        public void OnREspawn()
        {
            Instance = this;
        }

        private void OnDisable()
        {
            if (IsOwner)
            {
                InstanceFinder.SceneManager.OnLoadEnd -= OnSceneLoaded;
            }
        }

        public void SetGameState(GAME_STATE newState)
        {
            Instance.gameState = newState;
            PlayerEvents.Instance.CallEventGameStateChanged(newState);
        }

        public void SetOwnedCharacter(GameObject obj, ThirdPersonController controller)
        {
            InstanceFinder.SceneManager.OnLoadEnd += OnSceneLoaded;
            OwnedCharacter = obj;
            ownedController = controller;
        }

        void OnSceneLoaded(SceneLoadEndEventArgs args)
        {
            //UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, Scenes.NetworkSceneLoader.Instance.GetCorrectScene(gameObject));


            


            NetworkSceneConnector connector = null;
            Scene loadedScene;
            bool foundAScene = false;

            if (args.LoadedScenes == null|| args.LoadedScenes.Length == 0)
            {
                //Debug.LogError("No loaded scenes");
                loadedScene = Scenes.NetworkSceneLoader.Instance.GetLatestLoaded();
            }

            else
            {
                loadedScene = args.LoadedScenes[0];
                foundAScene = true;
            }

            List<Scene> allScenes = new List<Scene>();

            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
            {
                allScenes.Add(UnityEngine.SceneManagement.SceneManager.GetSceneAt(i));
            }


            for (int k = 0; k < allScenes.Count; k++)
            {


                GameObject[] rootObjects = allScenes[k].GetRootGameObjects();

                for (int i = 0; i < rootObjects.Length; i++)
                {
                    SceneMover[] movers = rootObjects[i].GetComponentsInChildren<SceneMover>(true);


                    for (int j = 0; j < movers.Length; j++)
                    {
                        sceneMovers.Add(movers[j]);
                    }

                }
            }

            //Debug.LogError("Found Scene movers length is " + sceneMovers.Count);
            sceneMovers.Clear();

            LogSceneMovers();
            //Debug.LogError("ABOUT TO INFROM SCENE MOVERS. Scene movers length is " + sciinMuuvers.Count);
            InformSceneMovers(loadedScene);
            //Debug.LogError("SHOULD HAVE INFORMED SCENE MOVERS");

            if (foundAScene) 
            {
                foreach (GameObject rootGO in loadedScene.GetRootGameObjects())
                {
                    connector = rootGO.GetComponent<NetworkSceneConnector>();

                    if (connector != null)
                    {
                        break;
                    }
                }

                if (connector != null)
                {
                    string prevSceneName = System.Text.Encoding.UTF8.GetString(args.QueueData.SceneLoadData.Params.ClientParams);

                    Transform spawnPos = connector.GetSpawnTransform(prevSceneName);

                    ownedController.SetPosAndRot(spawnPos.position, spawnPos.rotation);
                } else
                {
                    // maybe some type of backup to move player to like Vector3.zero or whatever in case there is no connector
                }
            }

            PlayerEvents.Instance.CallEventSceneLoadEnded();
            PlayerEvents.Instance.CallEventInformationReceived($"Entered {loadedScene.name}");

            OnSpawn();
        }

        // Disable and enable inputs depending on if we are driving a car.
        public void SetInputsEnabled(bool isEnabled)
        {
            OwnedCharacter.GetComponentInChildren<UnityEngine.InputSystem.PlayerInput>(true).enabled = isEnabled;
            OwnedCharacter.GetComponentInChildren<StarterAssets.StarterAssetsInputs>(true).enabled = isEnabled;
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            ClientId = LocalConnection.ClientId;

            GetCharacterDataServerRpc(LocalConnection, UserSession.Instance.LoggedUserData.id);
        }

        [ServerRpc(RequireOwnership = false)]
        public void GetCharacterDataServerRpc(NetworkConnection conn, string id)
        {
            APICalls_Server.Instance.GetCharacterData(conn, id, AcceptCharacterData);
        }

        [TargetRpc]
        void AcceptCharacterData(NetworkConnection conn, CharacterData characterData)
        {
            Utils.DumpToConsole(characterData);
            PlayerEvents.Instance.CallEventCharacterDataSet(characterData);
        }

        public void Respawn()
        {
            //Debug.LogError("Should respawn player4");
            OnSceneLoaded(new SceneLoadEndEventArgs());
        }

        public void AssSceneMover(SceneMover over)
        {
            sciinMuuvers.Add(over);
            //Debug.LogError("About to DELETE a scene mover");
            LogSceneMovers();


        }

        public void DiliitSciinMuuver(SceneMover overAndOut)
        {
            sciinMuuvers.Remove(overAndOut);
        }

        private void LogSceneMovers()
        {
            for (int i = 0; i < sciinMuuvers.Count; i++)
            {
                if (sciinMuuvers[i].gameObject != null) 
                {
                    //Debug.LogError("Scene mover at " + i + " is " + sciinMuuvers[i].gameObject.name);
                }

                else
                {
                    //Debug.LogError("Null sciin muuver");
                }
            }

           // Debug.LogError("Scene mover count is " + sciinMuuvers.Count);
        }

        private void InformSceneMovers(Scene scene)
        {
            //Debug.LogError("About to inform scene movers of the scene "+ scene.name  );

            for (int i = 0; i < sciinMuuvers.Count; i++)
            {
                //Debug.LogError("Informing scene mover at " + i + " that is in the scene " + sciinMuuvers[i].gameObject.scene.name);
                sciinMuuvers[i].DecideThisAndThat(scene);
            }
        }

        public void SetOwnerSceneMover(SceneMover mover)
        {
            OwnerSceneMover = mover;
        }


        public static void DoSomethingHeinous(NetworkConnection connection, Scene scene)
        {
            Instance.OwnerSceneMover.MoveYourAss(connection, scene);
        }

        public void OnSpawn()
        {
            //Debug.LogError("OnSpawn called " + Time.time);
            ownedController.MoveSlightly();

            for (int i = 0; i< sciinMuuvers.Count; i++)
            {
                sciinMuuvers[i].GetComponentInChildren<ThirdPersonController>(true).MoveSlightly();
            }
        }
    }
}
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed