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
        [SyncVar]
        public bool HasADriver;

        [SyncVar]
        public string DriverPlayerId;


        [field: SerializeReference]
        public string DetectionMessage { get; set; }
        public bool IsActive => true;
        public Vector3 DetectionMessageOffSet { get => Vector3.zero; }

        public TestMover TestMover;

        public Camera DedicatedCarCamera;

        public Vector3 cameraOffset;

        public SimpleCarController SimpleCarController;

        public GameObject CameraOffsetPosTemp;

        public float OffsetLenghtZ;
        public float OffsetLenghtY;


        public GameObject CameraFocusPoint;

        public GameObject ReverseCameraFocusPoint;
        public GameObject ReverseCameraOffsetPosTemp;

        private float ReverseOffsetLenghtZ;
        private float ReverseOffsetLenghtY;


        private Vector3 CurrentVelocity;
        private Vector3 CurrentRotationalVelocity;
        private Vector3 LastForward;

        public GameObject ExitPos;


        private Vector3 previousOffsetPos;

        public PlayerInput PlayerInput;


        public CarInput CarInput;

        [SyncVar]
        public int DriverPlayerClientId;

        void Start()
        {
            DriverPlayerClientId = -1;

            PlayerInput = GetComponentInChildren<PlayerInput>(true);
            CarInput = GetComponentInChildren<CarInput>(true);

            PlayerInput.enabled = false;
            CarInput.enabled = false;

            TestMover = GetComponent<TestMover>();
            cameraOffset = transform.position - DedicatedCarCamera.transform.position;
            DedicatedCarCamera.gameObject.SetActive(false);

            //OffsetLenghtZ = transform.position.z - DedicatedCarCamera.transform.position.z;
            //OffsetLenghtY = transform.position.y - DedicatedCarCamera.transform.position.y;

            OffsetLenghtZ = transform.position.z - CameraOffsetPosTemp.transform.position.z;
            OffsetLenghtY = transform.position.y - CameraOffsetPosTemp.transform.position.y;

            DedicatedCarCamera.transform.position = CameraOffsetPosTemp.transform.position;
            Vector3 toCar = CameraFocusPoint.transform.position - DedicatedCarCamera.transform.position;
            Quaternion lookRot = Quaternion.LookRotation(toCar, Vector3.up);

            DedicatedCarCamera.transform.rotation = lookRot;

            ReverseOffsetLenghtZ = transform.position.z - ReverseCameraOffsetPosTemp.transform.position.z;
            ReverseOffsetLenghtY = transform.position.y - ReverseCameraOffsetPosTemp.transform.position.y;
        }

        public void Interact(string playerId, UnityAction dummy)
        {
            PlayerEvents.Instance.CallEventInteractableLost();

            Debug.Log("Should enter car");
            EnterCar(playerId);
        }

        public void EnterCar(string playerId)
        {
            if (HasADriver)
            {
                Debug.LogWarning("The car already has a driver. Should not enter");
            }

            else
            {
                Debug.LogWarning("CAr doesn't have a driver. Can enter.");            
                OnPlayerEnteredCarServerRpc(playerId, CharacterManager.Instance.ClientId);
                CharacterManager.Instance.OwnedCharacter.GetComponent<AnimatedObjectDisabler>().Disable();
                CharacterManager.Instance.OwnedCharacter.transform.position = new Vector3(-3333, -3333, -3333);

                // We should probably swap to a dedicated car camera here
                //DedicatedCarCamera.transform.position = CameraOffsetPosTemp.transform.position;
                //Vector3 toFocusPoint = CameraFocusPoint.transform.position - DedicatedCarCamera.transform.position;
                //DedicatedCarCamera.transform.rotation = Quaternion.LookRotation(toFocusPoint, Vector3.up);


                CharacterManager.Instance.SetInputsEnabled(false);

                PlayerInput.enabled = true;
                CarInput.enabled = true;
                
                DedicatedCarCamera.gameObject.SetActive(true);
                DedicatedCarCamera.transform.parent = null;
                

            }
        }

        public void ExitCar()
        {            
            PlayerInput.enabled = false;
            CarInput.enabled = false;



            CharacterManager.Instance.SetInputsEnabled(true);

            OnPlayerExitedCarServerRpc();
            Debug.Log("On exit car called");

            CharacterManager.Instance.OwnedCharacter.GetComponent<AnimatedObjectDisabler>().Enable();

            Vector3 castOrigin = ExitPos.transform.position;

            castOrigin = new Vector3(castOrigin.x, castOrigin.y + 200, castOrigin.z);

 
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

        private void Update()
        {

            //Debug.Log(gameObject.name + " Driver player client id is " + DriverPlayerClientId);

            if (HasADriver
                &&  CharacterManager.Instance.ClientId == DriverPlayerClientId)
            {

                //Debug.Log("Driver player client id is " + DriverPlayerClientId);
                //Debug.Log("CarInput y is " + CarInput.move.y + " x is " + CarInput.move.x);

                //if (Input.GetKeyDown(KeyCode.E))
                //{
                //    ExitCar();
                //}

                //SimpleCarController.UpdateInput(new Vector2(Input.GetAxisRaw("Vertical"),
                //                                            Input.GetAxisRaw("Horizontal")));


                if (CarInput.interact)
                {
                    ExitCar();
                    CarInput.ClearInteractInput();
                    CarInput.ZeroInputs();
                }

                SimpleCarController.UpdateInput(new Vector2(CarInput.move.y,
                                                            CarInput.move.x));
            }
        }

        private void LateUpdate()
        {

            SimpleCarController.IsGoingInReverse = false;

            if (HasADriver
                 && CharacterManager.Instance.ClientId == DriverPlayerClientId)
            {
                //DedicatedCarCamera.transform.position = transform.position - cameraOffset;

                Vector3 offsetPos = Vector3.zero;

                //if (SimpleCarController.LastKnownVelocityMagnitude <= 1.0f)
                //{
                //    DedicatedCarCamera.transform.position = CameraOffsetPosTemp.transform.position;
                //    Vector3 toFocuPoint = CameraFocusPoint.transform.position - DedicatedCarCamera.transform.position; 
                //    DedicatedCarCamera.transform.rotation = Quaternion.LookRotation(toFocuPoint, Vector3.up);
                //}

                //else
                {
                    if (SimpleCarController.IsGoingInReverse)
                    {
                        offsetPos = SimpleCarController.CarGraphics.transform.position + (SimpleCarController.CarGraphics.transform.forward.normalized * ReverseOffsetLenghtZ) + (Vector3.up * -ReverseOffsetLenghtY);

                        //Debug.Log("Offset pos y is " + offsetPos.y);

                        DedicatedCarCamera.transform.position = Vector3.Lerp(DedicatedCarCamera.transform.position,
                                                                             offsetPos,
                                                                             Time.deltaTime * 2.0f);



                        // Comment/delete this out when ready
                        //DedicatedCarCamera.transform.position = offsetPos;



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


                        //Vector3 currentForward = DedicatedCarCamera.transform.forward;

                        //Vector3 newForward = Vector3.SmoothDamp(LastForward,
                        //                                                          toCar,
                        //                                                          ref CurrentRotationalVelocity,
                        //                                                          0.01f,
                        //                                                          155.0f,
                        //                                                          Time.deltaTime);
                        //DedicatedCarCamera.transform.rotation = Quaternion.LookRotation(newForward, Vector3.up);

                    }

                    else
                    {
                        //if (SimpleCarController.LastKnownVelocityMagnitude < 2.0f)
                        //{
                        //    offsetPos = SimpleCarController.CarGraphics.transform.position + (SimpleCarController.CarGraphics.transform.forward.normalized * OffsetLenghtZ) + (Vector3.up * -OffsetLenghtY);

                        //    DedicatedCarCamera.transform.position = Vector3.Lerp(DedicatedCarCamera.transform.position,
                        //                                                         offsetPos,
                        //                                                         Time.deltaTime * 1.0f);

                        //    Vector3 toCar = CameraFocusPoint.transform.position - DedicatedCarCamera.transform.position;
                        //    Quaternion lookRot = Quaternion.LookRotation(toCar, Vector3.up);

                        //    DedicatedCarCamera.transform.rotation = Quaternion.Slerp(DedicatedCarCamera.transform.rotation,
                        //                                                             lookRot,
                        //                                                             Time.deltaTime * 1.0f);

                        //}

                        //else 
                        {
                            bool inFrontOfCar = false;

                            float magnitudeBetweenFocusPointAndCar = (CameraFocusPoint.transform.position
                                                                      - SimpleCarController.CarGraphics.transform.position).magnitude;

                            float magnitudeBetweenFocusPointAndCamera = (CameraFocusPoint.transform.position
                                                                         - DedicatedCarCamera.transform.position).magnitude;

                            if (magnitudeBetweenFocusPointAndCamera < magnitudeBetweenFocusPointAndCar)
                            {
                                inFrontOfCar = true;
                            }

                            if (inFrontOfCar)
                            {

                                offsetPos = SimpleCarController.CarGraphics.transform.position + (SimpleCarController.CarGraphics.transform.forward.normalized * OffsetLenghtZ) + (Vector3.up * -OffsetLenghtY);

                                //offsetPos = Vector3.Lerp(previousOffsetPos, offsetPos, Time.deltaTime * 0.2f);

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
                                //Debug.Log("In front of car");
                                //Debug.Break();

                                previousOffsetPos = offsetPos;
                            }

                            else 
                            {



                                offsetPos = SimpleCarController.CarGraphics.transform.position + (SimpleCarController.CarGraphics.transform.forward.normalized * OffsetLenghtZ) + (Vector3.up * -OffsetLenghtY);

                                DedicatedCarCamera.transform.position = Vector3.SmoothDamp(DedicatedCarCamera.transform.position,
                                                                                       offsetPos,
                                                                                       ref CurrentVelocity,
                                                                                       0.25f,
                                                                                       155.0f,
                                                                                       Time.deltaTime);

                                Vector3 toCar = CameraFocusPoint.transform.position - DedicatedCarCamera.transform.position;
                                //Quaternion lookRot = Quaternion.LookRotation(toCar, Vector3.up);

                                //DedicatedCarCamera.transform.rotation = Quaternion.Slerp(DedicatedCarCamera.transform.rotation,
                                //                                                         lookRot,
                                //                                                         Time.deltaTime * 3.0f);

                                Vector3 currentForward = DedicatedCarCamera.transform.forward;

                                Vector3 newForward = Vector3.SmoothDamp(LastForward,
                                                                                          toCar,
                                                                                          ref CurrentRotationalVelocity,
                                                                                          0.15f,
                                                                                          155.0f,
                                                                                          Time.deltaTime);
                                DedicatedCarCamera.transform.rotation = Quaternion.LookRotation(newForward, Vector3.up);
                            }
                        }
                    }
                }

                //DedicatedCarCamera.transform.position = Vector3.Lerp(DedicatedCarCamera.transform.position,
                //                                                     offsetPos,
                //                                                     Time.deltaTime * 5.0f);



                //DedicatedCarCamera.transform.rotation = CameraOffsetPosTemp.transform.rotation;
                //DedicatedCarCamera.transform.LookAt(transform.position, Vector3.up);

                //Vector3 toCar = SimpleCarController.CarGraphics.transform.position - DedicatedCarCamera.transform.position;
                //Quaternion lookRot = Quaternion.LookRotation(toCar, Vector3.up);



                //Vector3 toCar = CameraFocusPoint.transform.position - DedicatedCarCamera.transform.position;
                ////Quaternion lookRot = Quaternion.LookRotation(toCar, Vector3.up);

                ////DedicatedCarCamera.transform.rotation = Quaternion.Slerp(DedicatedCarCamera.transform.rotation,
                ////                                                         lookRot,
                ////                                                         Time.deltaTime * 3.0f);

                //Vector3 currentForward = DedicatedCarCamera.transform.forward;

                //Vector3 newForward = Vector3.SmoothDamp(LastForward,
                //                                                          toCar,
                //                                                          ref CurrentRotationalVelocity,
                //                                                          0.15f,
                //                                                          155.0f,
                //                                                          Time.deltaTime);
                //DedicatedCarCamera.transform.rotation = Quaternion.LookRotation(newForward, Vector3.up);

                LastForward = DedicatedCarCamera.transform.forward;
            }
        }

        [ServerRpc(RequireOwnership = false)]

        public void OnPlayerEnteredCarServerRpc(string playerId, int clientId)
        {
            HasADriver = true;
            DriverPlayerId = playerId;
            DriverPlayerClientId = clientId;

            if (TestMover != null) 
            {
                TestMover.OnPlayerEnteredCar(playerId);
            }

            if (SimpleCarController != null)
            {
                SimpleCarController.OnPlayerEnteredCar(playerId, clientId);
            }
        }

        [ServerRpc(RequireOwnership = false)]

        public void OnPlayerExitedCarServerRpc()
        {
            HasADriver = false;
            
            if (TestMover != null)
            {
                TestMover.OnPlayerExitedCar(DriverPlayerId);
            }

            if (SimpleCarController != null)
            {
                SimpleCarController.OnPlayerExitedCar(DriverPlayerId);
            }

            DriverPlayerId = "";
            DriverPlayerClientId = -1;
        }   

        
    }
}
