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
        private void OnDisable()
        {
            InstanceFinder.SceneManager.OnLoadEnd -= OnSceneLoaded;
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
            NetworkSceneInit init = null;
            Scene loadedScene = args.LoadedScenes[0];
            foreach (GameObject rootGO in loadedScene.GetRootGameObjects())
            {
                init = rootGO.GetComponent<NetworkSceneInit>();

                if (init != null)
                {
                    ownedController.SetPosAndRot(init.PlayerCharacterSpawnSpot.position, init.PlayerCharacterSpawnSpot.rotation);
                    PlayerEvents.Instance.CallEventSceneLoadEnded();
                    PlayerEvents.Instance.CallEventInformationReceived($"Entered {loadedScene.name}.");
                    break;
                }
            }
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

    }
}
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed