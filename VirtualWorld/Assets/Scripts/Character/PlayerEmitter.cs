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

        // stuff to disable when loading a minigamescene
        //[SerializeField] GameObject geometry;
        //[SerializeField] GameObject detector;
        //[SerializeField] GameObject playerActions;
        //[SerializeField] CharacterController characterController;
        //[SerializeField] Animator animator;
        //[SerializeField] NetworkAnimator nAnimator;


        public override void OnStartClient()
        {
            base.OnStartClient();

            if (!IsOwner)
            {
                return;
            }

            //Character.Instance.SetPlayerGameObject(this, gameObject);
            UIManager.Instance.SetPlayerCharacter(gameObject);

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

        //public void EnableCharacter()
        //{
        //    geometry.SetActive(true);
        //    detector.SetActive(true);
        //    inputs.enabled = true;
        //    playerActions.SetActive(true);
        //    characterController.enabled = true;
        //    animator.enabled = true;
        //    controller.enabled = true;
        //    nAnimator.enabled = true;
        //    if (IsClient && IsOwner)
        //    {
        //        playerInput.enabled = true;
        //    }
        //}

        //public void DisableCharacter()
        //{
        //    playerActions.SetActive(false);
        //    geometry.SetActive(false);
        //    detector.SetActive(false);
        //    inputs.enabled = false;
        //    playerInput.enabled = false;
        //    characterController.enabled = false;
        //    animator.enabled = false;
        //    controller.enabled = false;
        //    nAnimator.enabled = false;
        //}
    }
}
