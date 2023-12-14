using Authentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;
using UnityEngine.Events;
using WorldObjects;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Animations;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;

namespace Vehicles
{
    public class CarManager : NetworkBehaviour, I_Interactable
    {
        private const string EnterCarPrompt = "Enter Car";
        private const string CarAlreadyHasADriverPrompt = "Car Has a Driver";

        [field: SerializeReference]
        public string DetectionMessage { get; set; }
        public bool IsActive => true;
        public Vector3 DetectionMessageOffSet { get => Vector3.zero; }

        #region SyncVars

        [SyncVar]
        private bool HasADriver;

        [SyncVar]
        private int DriverPlayerClientId;

        #endregion

        [SerializeField] private Camera DedicatedCarCamera;

        [SerializeField] private GameObject ExitPos;

        [SerializeField] private GameObject ForwardCameraFocusPoint;
        [SerializeField] private GameObject ForwardCameraOffsetPosTemp;
        [SerializeField] private GameObject ReverseCameraFocusPoint;
        [SerializeField] private GameObject ReverseCameraOffsetPosTemp;

        private float ForwardOffsetLenghtY;
        private float ForwardOffsetLenghtZ;
        private float ReverseOffsetLenghtY;
        private float ReverseOffsetLenghtZ;

        private Vector3 CurrentVelocity;
        private Vector3 CurrentRotationalVelocity;
        private Vector3 LastForward;

        private SimpleCarController SimpleCarController;
        
        private PlayerInput PlayerInput;
        private CarInput CarInput;


        private void Start()
        {
            DetectionMessage = EnterCarPrompt;

            if (IsServer) 
            {
                DriverPlayerClientId = -1;
            }

            SimpleCarController = GetComponent<SimpleCarController>();
            PlayerInput = GetComponentInChildren<PlayerInput>(true);
            CarInput = GetComponentInChildren<CarInput>(true);

            PlayerInput.enabled = false;
            CarInput.enabled = false;

            ForwardOffsetLenghtZ = transform.position.z - ForwardCameraOffsetPosTemp.transform.position.z;
            ForwardOffsetLenghtY = transform.position.y - ForwardCameraOffsetPosTemp.transform.position.y;
            ReverseOffsetLenghtZ = transform.position.z - ReverseCameraOffsetPosTemp.transform.position.z;
            ReverseOffsetLenghtY = transform.position.y - ReverseCameraOffsetPosTemp.transform.position.y;

            DedicatedCarCamera.gameObject.SetActive(false);
            DedicatedCarCamera.transform.position = ForwardCameraOffsetPosTemp.transform.position;

            Vector3 toCar = ForwardCameraFocusPoint.transform.position - DedicatedCarCamera.transform.position;
            Quaternion lookRot = Quaternion.LookRotation(toCar, Vector3.up);

            DedicatedCarCamera.transform.rotation = lookRot;
        }

        private void Update()
        {
            if (HasADriver
                && CharacterManager.Instance.ClientId == DriverPlayerClientId)
            {
                if (CarInput.interact)
                {
                    ExitCar();
                    CarInput.ClearInteractInput();
                    CarInput.ZeroInputs();
                }

                SimpleCarController.UpdateInput(new Vector2(CarInput.move.x,
                                                            CarInput.move.y));
            }
        }

        private void LateUpdate()
        {
            if (HasADriver
                 && CharacterManager.Instance.ClientId == DriverPlayerClientId)
            {         
                if (SimpleCarController.IsGoingInReverse)
                {
                    DoReverseCameraThings();
                }

                else
                {
                    DoForwardCameraThings();
                }
            }
        }

        #region CameraMovements

        private void DoForwardCameraThings()
        {
            Vector3 offsetPos;
            bool inFrontOfCar = false;

            float magnitudeBetweenFocusPointAndCar = (ForwardCameraFocusPoint.transform.position
                                                      - SimpleCarController.CarGraphics.transform.position).magnitude;

            float magnitudeBetweenFocusPointAndCamera = (ForwardCameraFocusPoint.transform.position
                                                         - DedicatedCarCamera.transform.position).magnitude;

            if (magnitudeBetweenFocusPointAndCamera < magnitudeBetweenFocusPointAndCar)
            {
                inFrontOfCar = true;
            }

            if (inFrontOfCar)
            {

                offsetPos = SimpleCarController.CarGraphics.transform.position 
                            + (SimpleCarController.CarGraphics.transform.forward.normalized 
                               * ForwardOffsetLenghtZ) 
                            + (Vector3.up * -ForwardOffsetLenghtY);

                DedicatedCarCamera.transform.position = Vector3.SmoothDamp(DedicatedCarCamera.transform.position,
                                                                       offsetPos,
                                                                       ref CurrentVelocity,
                                                                       0.45f,
                                                                       155.0f,
                                                                       Time.deltaTime);

                Vector3 toCar = SimpleCarController.transform.position - DedicatedCarCamera.transform.position;
                Quaternion lookRot = Quaternion.LookRotation(toCar, Vector3.up);

                DedicatedCarCamera.transform.rotation = Quaternion.Slerp(DedicatedCarCamera.transform.rotation,
                                                                         lookRot,
                                                                         Time.deltaTime * 3.0f);


            }

            else
            {
                offsetPos = SimpleCarController.CarGraphics.transform.position 
                            + (SimpleCarController.CarGraphics.transform.forward.normalized 
                               * ForwardOffsetLenghtZ) 
                            + (Vector3.up * -ForwardOffsetLenghtY);

                DedicatedCarCamera.transform.position = Vector3.SmoothDamp(DedicatedCarCamera.transform.position,
                                                                           offsetPos,
                                                                           ref CurrentVelocity,
                                                                           0.25f,
                                                                           155.0f,
                                                                           Time.deltaTime);

                Vector3 toCar = ForwardCameraFocusPoint.transform.position - DedicatedCarCamera.transform.position;


                Vector3 newForward = Vector3.SmoothDamp(LastForward,
                                                        toCar,
                                                        ref CurrentRotationalVelocity,
                                                        0.15f,
                                                        155.0f,
                                                        Time.deltaTime);

                DedicatedCarCamera.transform.rotation = Quaternion.LookRotation(newForward, Vector3.up);
                LastForward = DedicatedCarCamera.transform.forward;
            }
        }

        private void DoReverseCameraThings()
        {
            Vector3 offsetPos;

            offsetPos = SimpleCarController.CarGraphics.transform.position 
                        + (SimpleCarController.CarGraphics.transform.forward.normalized 
                           * ReverseOffsetLenghtZ) 
                        + (Vector3.up * -ReverseOffsetLenghtY);

            DedicatedCarCamera.transform.position = Vector3.Lerp(DedicatedCarCamera.transform.position,
                                                                 offsetPos,
                                                                 Time.deltaTime * 2.0f);

            bool behindCar = false;

            float magnitudeBetweenFocusPointAndCar = (ReverseCameraFocusPoint.transform.position
                                                      - SimpleCarController.CarGraphics.transform.position).magnitude;

            float magnitudeBetweenFocusPointAndCamera = (ReverseCameraFocusPoint.transform.position
                                                         - DedicatedCarCamera.transform.position).magnitude;

            if (magnitudeBetweenFocusPointAndCamera < magnitudeBetweenFocusPointAndCar)
            {
                behindCar = true;
            }

            if (behindCar)
            {
                Vector3 toCar = ReverseCameraFocusPoint.transform.position - DedicatedCarCamera.transform.position;
                Quaternion lookRot = Quaternion.LookRotation(toCar, Vector3.up);

                DedicatedCarCamera.transform.rotation = Quaternion.Slerp(DedicatedCarCamera.transform.rotation,
                                                                         lookRot,
                                                                         Time.deltaTime * 3.0f);
            }

            else
            {
                Vector3 toCar = SimpleCarController.CarGraphics.transform.position - DedicatedCarCamera.transform.position;
                Quaternion lookRot = Quaternion.LookRotation(toCar, Vector3.up);

                DedicatedCarCamera.transform.rotation = Quaternion.Slerp(DedicatedCarCamera.transform.rotation,
                                                                         lookRot,
                                                                         Time.deltaTime * 5.0f);

            }
        }

        #endregion

        public void Interact(UnityAction completionCallBackEvent)
        {
            if (!HasADriver)
            {
                PlayerEvents.Instance.CallEventInteractableLost();

                EnterCar();
            }
        }

        private void EnterCar()
        {
            OnPlayerEnteredCarServerRpc(CharacterManager.Instance.ClientId);

            CharacterManager.Instance.OwnedCharacter.GetComponent<AnimatedObjectDisabler>().Disable();
            CharacterManager.Instance.OwnedCharacter.transform.position = new Vector3(-3333, -3333, -3333);
            CharacterManager.Instance.SetInputsEnabled(false);

            PlayerInput.enabled = true;
            CarInput.enabled = true;

            DedicatedCarCamera.gameObject.SetActive(true);
            DedicatedCarCamera.transform.parent = null;
        }

        private void ExitCar()
        {                        
            OnPlayerExitedCarServerRpc();

            PlayerInput.enabled = false;
            CarInput.enabled = false;

            CharacterManager.Instance.OwnedCharacter.GetComponent<AnimatedObjectDisabler>().Enable();
            CharacterManager.Instance.SetInputsEnabled(true);

            Vector3 castOrigin = new Vector3(ExitPos.transform.position.x, 
                                             ExitPos.transform.position.y + 200, 
                                             ExitPos.transform.position.z);

            Physics.Raycast(castOrigin, Vector3.down, out RaycastHit hitInfo, 300);
            float yHit = hitInfo.point.y;

            CharacterController controller = CharacterManager.Instance.OwnedCharacter.GetComponent<CharacterController>();
            float yHeight = yHit + controller.center.y - controller.height / 2 + 0.1f;


            CharacterManager.Instance.OwnedCharacter.transform.position = new Vector3(ExitPos.transform.position.x,
                                                                                      yHeight,
                                                                                      ExitPos.transform.position.z);

            DedicatedCarCamera.gameObject.SetActive(false);
            DedicatedCarCamera.transform.parent = transform;
        }

        [ServerRpc(RequireOwnership = false)]
        public void OnPlayerEnteredCarServerRpc(int clientId)
        {
            HasADriver = true;
            DriverPlayerClientId = clientId;

            if (SimpleCarController != null)
            {
                SimpleCarController.OnPlayerEnteredCar(clientId);
            }

            ChangeDetectionMessageObserverRpc(CarAlreadyHasADriverPrompt);
        }

        [ServerRpc(RequireOwnership = false)]
        public void OnPlayerExitedCarServerRpc()
        {
            HasADriver = false;
            
            if (SimpleCarController != null)
            {
                SimpleCarController.OnPlayerExitedCar();
            }

            DriverPlayerClientId = -1;        
            
            ChangeDetectionMessageObserverRpc(EnterCarPrompt);
        }

        [ObserversRpc]
        public void ChangeDetectionMessageObserverRpc(string detectionMessage)
        {
            DetectionMessage = detectionMessage;
        }
    }
}
