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

namespace Characters
{
    public class PlayerEmitter : NetworkBehaviour
    {
        [SerializeField] StarterAssetsInputs inputs;
        [SerializeField] PlayerInput playerInput;

        [SerializeField] ThirdPersonController controller;

        [SerializeField] Transform cameraFollowTarget;
        [SerializeField] CinemachineVirtualCamera _cinemachineVirtualCamera;

        public override void OnStartClient()
        {
            base.OnStartClient();

            // if not our own character notify the minigame loading system about a new gameobject being instantiated
            if (!IsOwner)
            {
                SceneLoader.Instance.NewMainSceneObjectAdded(gameObject);
                return;
            }

            // owned character is made priority for camera
            _cinemachineVirtualCamera.Priority = 100;

            UIManager.Instance.SetPlayerCharacter(gameObject);
            CharacterManager.Instance?.SetOwnedCharacter(gameObject);
            SceneLoader.Instance.SetInputs(GetComponent<StarterAssetsInputs>());

            controller.shouldAnimate = true;

            if (_cinemachineVirtualCamera == null)
            {
                _cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            }

            EnableNetworkedControls();
        }

        void EnableNetworkedControls()
        {
            if (IsClient && IsOwner)
            {
                inputs.enabled = true;
                playerInput.enabled = true;
                _cinemachineVirtualCamera.Follow = cameraFollowTarget;
            }
        }
    }
}
