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

            Debug.Log("Character manager got destroyed. Scene is " + gameObject.scene.name);
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
                Debug.LogError("No loaded scenes");
                loadedScene = Scenes.NetworkSceneLoader.Instance.GetLatestLoaded();
            }

            else
            {
                loadedScene = args.LoadedScenes[0];
                foundAScene = true;
            }

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
            Debug.LogError("Should respawn player4");
            OnSceneLoaded(new SceneLoadEndEventArgs());
        }
    }
}
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed