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
        private CinemachineVirtualCamera _cinemachineVirtualCamera;
        bool controlsDisabled = false;

        public override void OnStartClient()
        {
            base.OnStartClient();

            // if not our own character, notify the minigame loading system about a new gameobject being instantiated
            if (!IsOwner)
            {
                SceneLoader.Instance.NewMainSceneObjectAdded(gameObject);
                return;
            }

            UIManager.Instance.SetPlayerCharacter(gameObject);
            CharacterManager.Instance.SetOwnedCharacter(gameObject);
            SceneLoader.Instance.SetInputs(GetComponent<StarterAssetsInputs>());

            controller.shouldAnimate = true;

            UIManager.Instance.EventMenuToggled.AddListener(TogglePlayerInputs);

            if (_cinemachineVirtualCamera == null)
            {
                _cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            }

            EnableNetworkedControls();
        }

        private void Update()
        {
            if (controlsDisabled)
            {
                inputs.ZeroInputs();
            }
        }

        void TogglePlayerInputs(bool menuEnabled)
        {
            controlsDisabled = menuEnabled;
            Debug.Log("Inputs enabled: " + !menuEnabled);
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
