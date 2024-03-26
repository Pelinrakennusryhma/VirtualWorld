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

namespace Characters
{
    public class PlayerEmitter : NetworkBehaviour
    {
        [SerializeField] StarterAssetsInputs inputs;
        [SerializeField] PlayerInput playerInput;

        [SerializeField] ThirdPersonController controller;

        [SerializeField] CinemachineVirtualCamera _cinemachineVirtualCamera;
        [SerializeField] List<GameObject> ownedObjects;

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

        private void OnDestroy()
        {
            Debug.LogError("Player emitter got destroyed. Scene is " + gameObject.scene.name);
        }
        public override void OnStartClient()
        {
            base.OnStartClient();
            DoStartClientThings();
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
    }
}
