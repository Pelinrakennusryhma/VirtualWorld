using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Authentication;
using Characters;
using FishNet.Component.Transforming;

namespace Vehicles
{

    // Stolen some functionality from here: https://docs.unity3d.com/Manual/WheelColliderTutorial.html
    public class SimpleCarController : NetworkBehaviour
    {

        [System.Serializable]
        public class AxleInfo
        {
            public WheelCollider leftWheel;
            public WheelCollider rightWheel;
            public bool motor; // is this wheel attached to motor?
            public bool steering; // does this wheel apply steer angle?
        }

        [SerializeField] private GameObject carGraphics;
        public GameObject CarGraphics { get => carGraphics; 
                                        private set => carGraphics = value; }


        #region SyncVars

        [SyncVar]
        public bool IsGoingInReverse;

        [SyncVar]
        private int driverPlayerClientId;

        [SyncVar]
        private bool hasADriver;

        [SyncVar]
        private float timeSpentOnRoof;

        [SyncVar]
        private Vector3 serverLastKnownVelocity;


        #endregion

        public List<AxleInfo> axleInfos; // the information about each individual axle

        private float maxMotorTorque; // maximum torque the motor can apply to wheel
        private float maxSteeringAngle; // maximum steer angle the wheel can have
        private float maxMotorTorqueInReverse;

        [SerializeField] private AnimationCurve BrakeInputVelocityReduceCurve;
        [SerializeField] private AnimationCurve AccelerationCurve;


        [SerializeField] private float LastKnownHorizontalInput;

        [SerializeField] private float LastKnownVerticalInput;

        private Rigidbody Rigidbody;

        private Vector3 lastPos;        
        private float lastValidYPos;

        private float FixedUpdateLenght;
        private float TimeOfLastFixedUpdate;

        private Vector3 CurrentCarGraphicsVelocity;
        private Vector3 LastCarGraphicsPosition;

        private Vector3 CurrentGraphicsForwardVelocity;
        private Vector3 LastCarGraphicsForward;

        private bool brakeAbruptly;
        private float timeSpentInBrakingAbrubtly;

        private Vector3[] lastTenKnownVelocities = new Vector3[10];
        private int runningVelocityIndex;

        private Vector3 originalPos;
        private Quaternion originalRot;

        private float notAValidRotationTimer;

        public void Awake()
        {
            lastPos = transform.position;
            LastCarGraphicsPosition = transform.position;
            LastCarGraphicsForward = CarGraphics.transform.forward;
            CarGraphics.transform.position = transform.position;

            originalPos = transform.position;
            originalRot = transform.rotation;

            SetupRigidbodyAndAxleValues();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            LastCarGraphicsPosition = transform.position;
            LastCarGraphicsForward = CarGraphics.transform.forward;
            CarGraphics.transform.position = transform.position;
        }


        public void FixedUpdate()
        {
            if (IsServer
                && (hasADriver
                || brakeAbruptly))
            {
                Drive();
            }

            if (IsServer)
            {
                float angleBetweenRots = Vector3.Angle(Vector3.up, transform.up);

                if (angleBetweenRots < 1.0f)
                {
                    notAValidRotationTimer = 0;
                }

                else
                {
                    notAValidRotationTimer += Time.deltaTime;
                }

                if (notAValidRotationTimer >= 30.0f)
                {
                    notAValidRotationTimer = 0;
                    transform.position = originalPos;
                    transform.rotation = originalRot;
                    Rigidbody.velocity = Vector3.zero;
                    Debug.Log("Resetted pos " + Time.time);
                }
            }

            OnFixedUpdate();
        }

        private void Update()
        {
            float averageVelocityMagnitude = CalculateAverageVelocityMagnitude();

            DoPositionLerpAndSmoothDampWithAverageVelocity(averageVelocityMagnitude);

            LastCarGraphicsPosition = CarGraphics.transform.position;

            HandleCarGraphicsRotation();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (IsServer)
            {
                if (!collision.gameObject.CompareTag("Ground"))
                {
                    brakeAbruptly = true;
                    timeSpentInBrakingAbrubtly = 0;
                    Rigidbody.velocity = serverLastKnownVelocity;
                }
            }
        }

        public void OnPlayerEnteredCar(int clientId)
        {
            driverPlayerClientId = clientId;
            hasADriver = true;

            CarGraphics.transform.parent = null;
        }

        public void OnPlayerExitedCar()
        {
            driverPlayerClientId = -1;
            hasADriver = false;

            CarGraphics.transform.parent = transform;

            SetMotorAndSteering(0, 0);
            brakeAbruptly = true;
            timeSpentInBrakingAbrubtly = 0;

            SendInputToServerServerRpc(Vector2.zero);
        }

        // These could and maybe even should be set in the editor,
        // but can't be bothered to do that since these values work
        // well enough for gour wheel, front wheel and rear wheel drives.
        private void SetupRigidbodyAndAxleValues()
        {
            Rigidbody = GetComponent<Rigidbody>();

            maxMotorTorque = 888;
            maxSteeringAngle = 36;
            maxMotorTorqueInReverse = 333;

            Vector3 centerOfMass = Rigidbody.centerOfMass;
            Vector3 adjustedMass = new Vector3(centerOfMass.x,
                                               centerOfMass.y - 1.5f,
                                               centerOfMass.z + 0.55f);
            Rigidbody.automaticCenterOfMass = false;
            Rigidbody.centerOfMass = adjustedMass;


            axleInfos[0].leftWheel.wheelDampingRate = 0.25f; // Default is 0.25f
            axleInfos[0].rightWheel.wheelDampingRate = 0.25f; // Default is 0.25f
            axleInfos[1].leftWheel.wheelDampingRate = 0.4f; // Default is 0.25f
            axleInfos[1].rightWheel.wheelDampingRate = 0.4f; // Default is 0.25f

            axleInfos[0].leftWheel.suspensionDistance = 0.3f; // Default is 0.3f
            axleInfos[0].rightWheel.suspensionDistance = 0.3f; // Default is 0.3f
            axleInfos[1].leftWheel.suspensionDistance = 0.2f; // Default is 0.3f
            axleInfos[1].rightWheel.suspensionDistance = 0.2f; // Default is 0.3f

            JointSpring frontSpring = new JointSpring();
            frontSpring.spring = 20000; // Default is 35000
            frontSpring.damper = 3500; // Default is 4500
            frontSpring.targetPosition = 0.5f; // Default is 0.5f
            JointSpring rearSpring = new JointSpring();
            rearSpring.spring = 50000; // Default is 35000. 
            rearSpring.damper = 4500; // Default is 4500
            rearSpring.targetPosition = 0.5f; // Default is 0.5f

            axleInfos[0].leftWheel.suspensionSpring = frontSpring;
            axleInfos[0].rightWheel.suspensionSpring = frontSpring;
            axleInfos[1].leftWheel.suspensionSpring = rearSpring;
            axleInfos[1].rightWheel.suspensionSpring = rearSpring;

            WheelFrictionCurve frontForwardFriction = new WheelFrictionCurve();
            frontForwardFriction.extremumSlip = 2.0f; // Default is 0.4f 
            frontForwardFriction.extremumValue = 3.0f; // Default is 1.0f
            frontForwardFriction.asymptoteSlip = 3.0f; // Default is 0.8f
            frontForwardFriction.asymptoteValue = 1.5f; // Default is 0.5f
            frontForwardFriction.stiffness = 1.0f; // Default is 1.0f

            WheelFrictionCurve rearForwardFriction = new WheelFrictionCurve();
            rearForwardFriction.extremumSlip = 2.0f; // Default is 0.4f 
            rearForwardFriction.extremumValue = 3.0f; // Default is 1.0f
            rearForwardFriction.asymptoteSlip = 3.0f; // Default is 0.8f
            rearForwardFriction.asymptoteValue = 1.5f; // Default is 0.5f
            rearForwardFriction.stiffness = 1.0f; // Default is 1.0f

            axleInfos[0].leftWheel.forwardFriction = frontForwardFriction;
            axleInfos[0].rightWheel.forwardFriction = frontForwardFriction;
            axleInfos[1].leftWheel.forwardFriction = rearForwardFriction;
            axleInfos[1].rightWheel.forwardFriction = rearForwardFriction;

            WheelFrictionCurve frontSidewaysFriction = new WheelFrictionCurve();
            frontSidewaysFriction.extremumSlip = 0.7f; // Default is 0.2f 
            frontSidewaysFriction.extremumValue = 3.0f; // Default is 1.0f
            frontSidewaysFriction.asymptoteSlip = 2.5f; // Default is 0.5f
            frontSidewaysFriction.asymptoteValue = 2.75f; // Default is 0.75f
            frontSidewaysFriction.stiffness = 1.0f; // Default is 1.0f

            WheelFrictionCurve rearSidewaysFriction = new WheelFrictionCurve();
            rearSidewaysFriction.extremumSlip = 0.3f; // Default is 0.2f 
            rearSidewaysFriction.extremumValue = 3.0f; // Default is 1.0f
            rearSidewaysFriction.asymptoteSlip = 2.5f; // Default is 0.5f
            rearSidewaysFriction.asymptoteValue = 2.75f; // Default is 0.75f
            rearSidewaysFriction.stiffness = 1.0f; // Default is 1.0f

            axleInfos[0].leftWheel.sidewaysFriction = frontSidewaysFriction;
            axleInfos[0].rightWheel.sidewaysFriction = frontSidewaysFriction;
            axleInfos[1].leftWheel.sidewaysFriction = rearSidewaysFriction;
            axleInfos[1].rightWheel.sidewaysFriction = rearSidewaysFriction;

            axleInfos[0].leftWheel.forceAppPointDistance = 0f; // Default is 0f
            axleInfos[0].rightWheel.forceAppPointDistance = 0f; // Default is 0f
            axleInfos[1].leftWheel.forceAppPointDistance = 0f; // Default is 0f
            axleInfos[1].rightWheel.forceAppPointDistance = 0f; // Default is 0f
        }


        #region DriveThings

        public void UpdateInput(Vector2 input)
        {
            if (IsOwner) 
            {
                LastKnownHorizontalInput = input.x;
                LastKnownVerticalInput = input.y;

                SendInputToServerServerRpc(new Vector2(LastKnownHorizontalInput,
                                                       LastKnownVerticalInput));
            }
            
        }

        [ServerRpc(RequireOwnership = true)]
        public void SendInputToServerServerRpc(Vector2 input)
        {           
            LastKnownHorizontalInput = input.x;
            LastKnownVerticalInput = input.y;

            //Debug.Log("Server received input horizontal: " + LastKnownHorizontalInput + " vertical: " + LastKnownVerticalInput);
        }

        private void Drive()
        {
            //Debug.Log("Driving " + Time.time + " horizontal is " + LastKnownHorizontalInput + " vertical is " + LastKnownVerticalInput);

            Vector2 input = new Vector2(LastKnownHorizontalInput,
                                        LastKnownVerticalInput);


            Vector3 rigidbodyForward = Rigidbody.transform.forward;
            Vector3 velocity = Rigidbody.velocity;
            float rigidbodyVelocityMagnitude = velocity.magnitude;
            float angleBetweenVelocityAndForward = Vector3.Angle(rigidbodyForward, velocity);

            float motor = maxMotorTorque * input.y;
            float steering = maxSteeringAngle * input.x;

            float brakeRate = rigidbodyVelocityMagnitude / 20.0f;
            brakeRate = Mathf.Clamp(brakeRate, 0, 1.0f);

            float brakeMultiplier = 40; // The values probably should be between 10 and 200, depending how snappy of a brake you want. But this may be a shitty implementation anyways


            if (rigidbodyVelocityMagnitude < 5.0f)
            {
                float motorAdd = AccelerationCurve.Evaluate(Mathf.Clamp(rigidbodyVelocityMagnitude / 5.0f,
                                                                        0,
                                                                        1.0f));
                motor += motorAdd * maxMotorTorque * input.x * 3;
            }

            if (brakeAbruptly)
            {
                timeSpentInBrakingAbrubtly += Time.deltaTime;

                if (timeSpentInBrakingAbrubtly >= 1.0f)
                {
                    brakeAbruptly = false;
                    Rigidbody.velocity = Vector3.zero;
                }
            }

            if (input.y < -0.1f && angleBetweenVelocityAndForward < 90.0f
                || input.y > 0.1f && angleBetweenVelocityAndForward > 90.0f
                || brakeAbruptly)
            {
                motor = 0;

                float amountToBrake = BrakeInputVelocityReduceCurve.Evaluate(brakeRate);
                float newVelo = rigidbodyVelocityMagnitude - (amountToBrake * Time.fixedDeltaTime * brakeMultiplier);
                newVelo = Mathf.Clamp(newVelo, 0, 300.0f);
                Rigidbody.velocity = serverLastKnownVelocity.normalized * newVelo;
            }

            else if (input.y < 0)
            {
                float multiplier = 1;

                if (rigidbodyVelocityMagnitude < 5.0f)
                {
                    multiplier = 5;
                }

                motor = maxMotorTorqueInReverse * multiplier * input.y;

            }

            CheckIfWeAreGoingInReverse(rigidbodyVelocityMagnitude, 
                                       angleBetweenVelocityAndForward);

            if (input.y < 0.1f && input.y > -0.1f)
            {
                motor = 0;
                steering = 44 * input.x;
            }

            CheckIfWeAreOnOurRoof();

            SetMotorAndSteering(motor,
                                steering);

            CheckGroundHit();
        }

        private void CheckIfWeAreGoingInReverse(float rigidbodyVelocityMagnitude, 
                                                float angleBetweenVelocityAndForward)
        {
            if (angleBetweenVelocityAndForward > 90
                && rigidbodyVelocityMagnitude > 1.0f)
            {
                IsGoingInReverse = true;
            }

            else
            {
                IsGoingInReverse = false;
            }
        }

        private void SetMotorAndSteering(float motor, 
                                         float steering)
        {
            foreach (AxleInfo axleInfo in axleInfos)
            {
                if (axleInfo.steering)
                {
                    axleInfo.leftWheel.steerAngle = steering;
                    axleInfo.rightWheel.steerAngle = steering;
                }

                if (axleInfo.motor)
                {
                    axleInfo.leftWheel.motorTorque = motor;
                    axleInfo.rightWheel.motorTorque = motor;
                }
            }
        }

        private void CheckIfWeAreOnOurRoof()
        {
            // If we ended up on our roof
            if (Vector3.Angle(Rigidbody.transform.up, Vector3.up) >= 90.0f)
            {
                timeSpentOnRoof += Time.fixedDeltaTime;

                Vector3 previousForward = Rigidbody.transform.forward;

                if (timeSpentOnRoof >= 6.0f)
                {
                    Rigidbody.transform.rotation = Quaternion.LookRotation(previousForward, Vector3.up);
                    timeSpentOnRoof = 0;
                }
            }

            else
            {
                timeSpentOnRoof = 0;
            }
        }

        private void CheckGroundHit()
        {
            RaycastHit[] hits = Physics.RaycastAll(transform.position + (Vector3.up * 1000),
                                                   Vector3.down * 2000);

            bool hitGround = false;

            float distanceToYPos;
            float smallestDistanceToYPos = 100000;

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.gameObject.CompareTag("Ground"))
                {
                    distanceToYPos = Rigidbody.transform.position.y - hits[i].point.y;
                    hitGround = true;

                    if (distanceToYPos < smallestDistanceToYPos)
                    {
                        smallestDistanceToYPos = distanceToYPos;
                    }
                }
            }

            distanceToYPos = smallestDistanceToYPos;

            bool positionIsWithinTolerance = true;

            if (distanceToYPos > 1.3f)
            {
                positionIsWithinTolerance = false;
            }

            if (hitGround
                && positionIsWithinTolerance)
            {
                lastValidYPos = Rigidbody.transform.position.y;
            }

            else
            {
                Rigidbody.transform.position = new Vector3(Rigidbody.transform.position.x,
                                                           lastValidYPos,
                                                           Rigidbody.transform.position.z);
            }
        }

        #endregion


        private void OnFixedUpdate()
        {
            FixedUpdateLenght = Time.fixedDeltaTime;
            TimeOfLastFixedUpdate = Time.time;
            lastPos = transform.position;

            if (IsServer)
            {
                serverLastKnownVelocity = Rigidbody.velocity;
            }
        }

        private void HandleCarGraphicsRotation()
        {
            CarGraphics.transform.forward = Vector3.SmoothDamp(-LastCarGraphicsForward,
                                                               transform.forward,
                                                               ref CurrentGraphicsForwardVelocity,
                                                               0.1f,
                                                               3000.0f,
                                                               Time.deltaTime);

            CarGraphics.transform.forward = -CarGraphics.transform.forward;
            LastCarGraphicsForward = CarGraphics.transform.forward;
        }

        // Perhaps this could be in the server fixed update?
        // Or perhaps not.
        private float CalculateAverageVelocityMagnitude()
        {
            float average = 0;
            lastTenKnownVelocities[runningVelocityIndex] = serverLastKnownVelocity;
            runningVelocityIndex++;

            if (runningVelocityIndex >= lastTenKnownVelocities.Length)
            {
                runningVelocityIndex = 0;
            }

            for (int i = 0; i < lastTenKnownVelocities.Length; i++)
            {
                average += lastTenKnownVelocities[i].magnitude;
            }

            average /= lastTenKnownVelocities.Length;

            return average;
        }

        private void DoPositionLerpAndSmoothDampWithAverageVelocity(float average)
        {
            float timePassedSinceLastFixedUpdate = Time.time - TimeOfLastFixedUpdate;
            float lerpAmount = timePassedSinceLastFixedUpdate / FixedUpdateLenght;

            Vector3 predPos = lastPos + serverLastKnownVelocity.normalized 
                              * average 
                              * Time.deltaTime;

            Vector3 lerpedPos = Vector3.Lerp(lastPos, predPos, lerpAmount);

            CarGraphics.transform.position = Vector3.SmoothDamp(LastCarGraphicsPosition,
                                                                lerpedPos,
                                                                ref CurrentCarGraphicsVelocity,
                                                                0.1f,
                                                                3600,
                                                                Time.deltaTime);
        }
    }
}