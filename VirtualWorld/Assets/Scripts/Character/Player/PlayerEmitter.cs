using StarterAssets;
using UI;
using FishNet;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using Scenes;
using FishNet.Object;
using FishNet.Component.Animating;
using System.Collections.Generic;
using FishNet.Object.Synchronizing;
using FishNet.Connection;

namespace Characters
{
    public class PlayerEmitter : NetworkBehaviour
    {
        [SerializeField] StarterAssetsInputs inputs;
        [SerializeField] PlayerInput playerInput;

        [SerializeField] ThirdPersonController controller;

        [SerializeField] CinemachineVirtualCamera _cinemachineVirtualCamera;
        [SerializeField] List<GameObject> ownedObjects;

        [SyncVar]
        public int ClientID;

        private void Awake()
        {
            foreach (GameObject gameObject in ownedObjects)
            {
                gameObject.SetActive(false);
            }



            //Debug.LogError("Player emitter awoke. Scene is " + gameObject.scene.name);
            //UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, UnityEngine.SceneManagement.SceneManager.GetSceneByName("Playground"));
            //DoStartClientThings();

            //Debug.LogError("Player emitter moved. Scene is " + gameObject.scene.name);
        }

        public int GetClientID()
        {
            return ClientID;
        }

        private void OnDestroy()
        {
            //Debug.LogError("Player emitter got destroyed. Scene is " + gameObject.scene.name);
        }
        public override void OnStartClient()
        {
            base.OnStartClient();
            DoStartClientThings();

            SetClientId(CharacterManager.Instance.ClientId);


        }

        [ServerRpc]
        public void SetClientId(int id)
        {
            ClientID = id;
            //Debug.LogError("Set player emitter client id to " + ClientID);
        }

        private void DoStartClientThings()
        {

            // if not our own character notify the minigame loading system about a new gameobject being instantiated
            if (!IsOwner)
            {
                SceneLoader.Instance.NewMainSceneObjectAdded(gameObject);
                return;
            }

            // owned character is made priority for camera
            _cinemachineVirtualCamera.Priority = 100;

            UIManager.Instance.SetPlayerCharacter(gameObject);
            CharacterManager.Instance?.SetOwnedCharacter(gameObject, controller);
            foreach (GameObject gameObject in ownedObjects)
            {
                gameObject.SetActive(true);
            }

            controller.shouldAnimate = true;

            EnableNetworkedControls();
        }

        void EnableNetworkedControls()
        {
            if (IsClient && IsOwner)
            {
                inputs.enabled = true;
                playerInput.enabled = true;
            }
        }

        public void OnSceneLoaded(NetworkConnection connection,
                                  UnityEngine.SceneManagement.Scene scene,
                                  string sceneName)
        {
            UnityEngine.SceneManagement.Scene activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            Debug.LogError("Should spawn a first person controller. Active scene is " + activeScene.name);

            CharacterManager.Instance.OnShouldSpawnAFirstPersonController(connection, scene, this, sceneName);
        }
    }
}
