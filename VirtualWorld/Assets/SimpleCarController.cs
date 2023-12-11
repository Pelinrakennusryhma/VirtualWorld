using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Authentication;
using Characters;
using FishNet.Component.Transforming;

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

    [SyncVar]
    public int DriverPlayerClientId;

    [SyncVar]
    public string PlayerIdUserSession;

    [SyncVar]
    public bool HasADriver;

    [SyncVar]
    public float TimeSpentOnRoof;


    public bool IsGoingInReverse;

    public Rigidbody Rigidbody;

    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have


    public float maxMotorTorqueInReverse;

    public AnimationCurve BrakeInputVelocityReduceCurve;
    public AnimationCurve AccelerationCurve;

    public GameObject CarGraphics;


    public Vector3 lastPos;


    public Vector3 predictedPos;


    public float TimeSinceLastFixedUpdate;
    public float FixedUpdateLenght;

    public float TimeOfLastFixedUpdate;

    // Just a test to see what values are on the server
    //-------------------------------------------------
    [SyncVar]
    public Vector3 serverLastPos;

    [SyncVar]
    public Vector3 serverPredictedPos;

    [SyncVar]
    public float serverTimeOfLastFixedUpdate;

    [SyncVar]
    public float serverTimeSinceLastFixedUpdate;

    [SyncVar]
    public float serverLastKnownVelocityMagnitude;

    [SyncVar]
    public Vector3 serverLastKnownVelocity;


    //------------------------------------------------


    public Vector3 CurrentCarGraphicsVelocity;
    private Vector3 LastCarGraphicsPosition;

    private Vector3 CurrentGraphicsForwardVelocity;
    private Vector3 LastCarGraphicsForward;


    private Vector3 CurrentRigidbodyPosition;

    public NetworkTransform NetworkTransformComponent;

    [SyncVar]
    public bool IsCurrentlyColliding;

    public bool brakeAbruptly;

    public float timeSpentInBrakingAbrubtly;

    private float YOffsetFromGroundToMakeTheCarNotFallThroughGroundAtHighSpeeds;

    private float lastValidYPos;


    public float LastKnownVelocityMagnitude;
    public Vector3 LastKnownVelocity;

    public GameObject CarGraphicsParent;


    public float LastKnownHorizontalInput;


    public float LastKnownVerticalInput;

    private Vector3[] lastTenKnownVelocities = new Vector3[10];
    private int runningVelocityIndex;

    public void OnPlayerEnteredCar(string playerId, int clientId)
    {
        this.PlayerIdUserSession = playerId;
        DriverPlayerClientId = clientId;
        HasADriver = true;
        CarGraphics.transform.parent = null;
        //Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
    }

    //[ServerRpc(RequireOwnership = false)]
    public void OnPlayerExitedCar(string playerId)
    {
        this.PlayerIdUserSession = "";
        this.DriverPlayerClientId = -1;
        HasADriver = false;

        if (CarGraphicsParent != null) 
        {
            CarGraphics.transform.parent = CarGraphicsParent.transform;
        }

        else
        {
            CarGraphics.transform.parent = transform;
        }

        SendInputToServerServerRpc(Vector2.zero);
        ResetForceAppDistance();
    }

    public void Awake()
    {
        NetworkTransformComponent = GetComponent<NetworkTransform>();
        //NetworkTransformComponent.SetSynchronizedProperties(SynchronizedProperty.);

        //Rigidbody.transform.position = CurrentRigidbodyPosition;

        lastPos = transform.position;
        predictedPos = transform.position;
        LastCarGraphicsPosition = transform.position;
        LastCarGraphicsForward = CarGraphics.transform.forward;
        CarGraphics.transform.position = transform.position;
        //Rigidbody.interpolation = RigidbodyInterpolation.None;

        maxMotorTorque = 888;

        
        Vector3 centerOfMass = Rigidbody.centerOfMass;
        Vector3 adjustedMass = new Vector3(centerOfMass.x, 
                                           centerOfMass.y - 1.5f,
                                           centerOfMass.z + 0.55f);
        Rigidbody.automaticCenterOfMass = false;
        Rigidbody.centerOfMass = adjustedMass;

        //axleInfos[0].leftWheel.suspensionDistance = 0.3f; // Default is 0.3f
        //axleInfos[0].rightWheel.suspensionDistance = 0.3f; // Default is 0.3f
        //axleInfos[1].leftWheel.suspensionDistance = 0.3f; // Default is 0.3f
        //axleInfos[1].rightWheel.suspensionDistance = 0.3f; // Default is 0.3f

        axleInfos[0].leftWheel.wheelDampingRate = 0.25f; // Default is 0.25f
        axleInfos[0].rightWheel.wheelDampingRate = 0.25f; // Default is 0.25f
        axleInfos[1].leftWheel.wheelDampingRate = 0.4f; // Default is 0.25f
        axleInfos[1].rightWheel.wheelDampingRate = 0.4f; // Default is 0.25f

        axleInfos[0].leftWheel.suspensionDistance = 0.3f; // Default is 0.3f
        axleInfos[0].rightWheel.suspensionDistance = 0.3f; // Default is 0.3f
        axleInfos[1].leftWheel.suspensionDistance = 0.2f; // Default is 0.3f
        axleInfos[1].rightWheel.suspensionDistance = 0.2f; // Default is 0.3f

        //axleInfos[0].leftWheel.wheelDampingRate = 0.2f; // Default is 0.25f
        //axleInfos[0].rightWheel.wheelDampingRate = 0.2f; // Default is 0.25f
        //axleInfos[1].leftWheel.wheelDampingRate = 0.35f; // Default is 0.25f
        //axleInfos[1].rightWheel.wheelDampingRate = 0.35f; // Default is 0.25f

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


        ResetForceAppDistance();
    }

    public void Start()
    {
        //LastCarGraphicsPosition = transform.position;
        //LastCarGraphicsForward = CarGraphics.transform.forward;
        //CarGraphics.transform.position = transform.position;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        LastCarGraphicsPosition = transform.position;
        LastCarGraphicsForward = CarGraphics.transform.forward;
        CarGraphics.transform.position = transform.position;
    }

    [ServerRpc]
    public void GetStartPosServerRpc()
    {

    }

    public void FixedUpdate()
    {

        bool isCarDriver = false;

        //if (UserSession.Instance == null 
        //    || UserSession.Instance.LoggedUserData.id == null)
        //{
        //    Debug.LogError("User session things are null");
        //}

        //else if (PlayerIdUserSession.Equals(UserSession.Instance.LoggedUserData.id))
        //{
        //    isCarDriver = true;
        //    //Debug.Log("We are the car driver. Execute " + Time.time);
        //}

        if (HasADriver 
            && CharacterManager.Instance.ClientId == DriverPlayerClientId)
        {
            isCarDriver = true;
        }
        

        if (isCarDriver)
        {
            //SendInputToServerServerRpc(new Vector2(LastKnownVerticalInput, 
            //                                       LastKnownHorizontalInput));
            //DriveServerRpc(new Vector2(LastKnownVerticalInput,
            //               LastKnownHorizontalInput));

            //DriveServerRpc(new Vector2(Input.GetAxis("Vertical"),
            //                           Input.GetAxis("Horizontal")));
        }

        if (IsServer
            && HasADriver)
        {
            Drive();
        }

        OnFixedUpdate();


        if (HasADriver)
        {
            //Debug.Log("car velocity mag at fixed update is " + Rigidbody.velocity.magnitude);
        }

        //LastKnownVelocityMagnitude = Rigidbody.velocity.magnitude;


        // Ground check: Check if we have fallen through ground.


    }

    #region NewDriveServerRpcs


    [ServerRpc(RequireOwnership = false)]
    public void SendInputToServerServerRpc(Vector2 input)
    {
        LastKnownVerticalInput = input.x;
        LastKnownHorizontalInput = input.y;
    }

    public void Drive()
    {
        Vector2 input = new Vector2(LastKnownVerticalInput,
                                    LastKnownHorizontalInput);

        //Debug.Log("Driving " + Time.time + " input is x:" + input.x + " y: " + input.y);

        Vector3 rigidbodyForward = Rigidbody.transform.forward;
        Vector3 velocity = Rigidbody.velocity;
        float rigidbodyVelocityMagnitude = velocity.magnitude;
        float angleBetweenVelocityAndForward = Vector3.Angle(rigidbodyForward, velocity);


        //Debug.Log("Angle of velocity " + angleBetweenVelocityAndForward);

        bool isBraking = false;

        float motor = maxMotorTorque * input.x;
        float steering = maxSteeringAngle * input.y;

        float brakeRate = rigidbodyVelocityMagnitude / 20.0f;
        brakeRate = Mathf.Clamp(brakeRate, 0, 1.0f);

        float brakeMultiplier = 40; // The values probably should be between 10 and 200, depending how snappy of a brake you want. But this may be a shitty implementation anyways


        if (rigidbodyVelocityMagnitude < 5.0f)
        {
            float motorAdd = AccelerationCurve.Evaluate(Mathf.Clamp(rigidbodyVelocityMagnitude / 5.0f,
                                                                    0,
                                                                    1.0f));
            motor += motorAdd * maxMotorTorque * input.x * 3;
            ResetForceAppDistance();
        }

        else
        {
            //axleInfos[0].leftWheel.forceAppPointDistance = -0.1f; // Default is 0f
            //axleInfos[0].rightWheel.forceAppPointDistance = -0.1f; // Default is 0f
            //axleInfos[1].leftWheel.forceAppPointDistance = -0.15f; // Default is 0f
            //axleInfos[1].rightWheel.forceAppPointDistance = -0.15f; // Default is 0f
        }

        if (brakeAbruptly)
        {
            timeSpentInBrakingAbrubtly += Time.deltaTime;

            if (timeSpentInBrakingAbrubtly >= 1.0f)
            {
                brakeAbruptly = false;
            }
        }


        //Debug.Log("Brakerate is " + brakeRate);

        IsGoingInReverse = false;

        if (input.x < -0.1f && angleBetweenVelocityAndForward < 90.0f
            || input.x > 0.1f && angleBetweenVelocityAndForward > 90.0f
            || brakeAbruptly)
        {
            //brakeMultiplier = 50000.0f;
            //motor = maxMotorTorque * brakeMultiplier * input.x;
            motor = 0;
            float amountToBrake = BrakeInputVelocityReduceCurve.Evaluate(brakeRate);
            //amountToBrake = 1.0f;
            float newVelo = rigidbodyVelocityMagnitude - (amountToBrake * Time.fixedDeltaTime * brakeMultiplier);
            newVelo = Mathf.Clamp(newVelo, 0, 300.0f);
            Rigidbody.velocity = serverLastKnownVelocity.normalized * newVelo;
            //Rigidbody.velocity = velocity.normalized * newVelo;
        }

        else if (input.x < 0)
        {
            float multiplier = 1;

            if (rigidbodyVelocityMagnitude < 5.0f)
            {
                multiplier = 5;
            }
            motor = maxMotorTorqueInReverse * multiplier * input.x;

        }

        if (angleBetweenVelocityAndForward > 90
            && rigidbodyVelocityMagnitude > 1.0f)
        {
            IsGoingInReverse = true;
        }

        else
        {
            IsGoingInReverse = false;
        }



        //if (input.x > 0.1f && angleBetweenVelocityAndForward > 90.0f)
        //{
        //    // brakeMultiplier = 50000.0f;
        //    //motor = maxMotorTorque * brakeMultiplier * input.x;
        //    motor = 0;
        //    float amountToBrake = BrakeInputVelocityReduceCurve.Evaluate(brakeRate);
        //    //amountToBrake = 1.0f;
        //    Rigidbody.velocity = velocity.normalized * (rigidbodyVelocityMagnitude - amountToBrake * Time.fixedDeltaTime * brakeMultiplier);
        //}

        bool freeRoll = false;
        float leftMotor = 0;
        float rightMotor = 0;

        if (input.x < 0.1f && input.x > -0.1f)
        {
            //Debug.Log("Braking. A free roll");
            motor = 0;
            steering = 44 * input.y;

            if (input.y > 0.1f)
            {
                leftMotor = maxMotorTorque * Mathf.PI;
            }

            else if (input.y < -0.1f)
            {
                rightMotor = maxMotorTorque * Mathf.PI;
            }
        }

        // If we ended up on our roof
        if (Vector3.Angle(Rigidbody.transform.up, Vector3.up) >= 90.0f)
        {
            TimeSpentOnRoof += Time.fixedDeltaTime;

            Vector3 previousForward = Rigidbody.transform.forward;

            if (TimeSpentOnRoof >= 6.0f)
            {
                Rigidbody.transform.rotation = Quaternion.LookRotation(previousForward, Vector3.up);
                TimeSpentOnRoof = 0;
            }
        }

        else
        {
            TimeSpentOnRoof = 0;
        }

        freeRoll = false;

        //Debug.Log("Rigidbody velocity is " + rigidbodyVelocityMagnitude);

        if (!freeRoll)
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

                    //Debug.Log("Left front torque is " + axleInfo.leftWheel.motorTorque);
                }
            }
        }

        else
        {

            axleInfos[0].leftWheel.motorTorque = leftMotor;
            axleInfos[0].rightWheel.motorTorque = rightMotor;
        }

        float distanceToGround;


        RaycastHit[] hits = Physics.RaycastAll(transform.position + (Vector3.up * 1000),
                                               Vector3.down * 2000);

        bool hitGround = false;
        float distanceToYPos = 1000000;

        float smallestDistanceToYPos = 100000;

        for (int i = 0; i < hits.Length; i++)
        {
            //Debug.Log("We are hitting object with tag " + hits[i].collider.gameObject.tag + " at time " + Time.time + " object name is " + hits[i].collider.gameObject.name + " at time " + Time.time);

            if (hits[i].collider.gameObject.CompareTag("Ground"))
            {
                distanceToYPos = Rigidbody.transform.position.y - hits[i].point.y;
                //Debug.Log("We are hitting the ground. Y pos is " + hits[i].point.y + Time.time);
                hitGround = true;

                if (distanceToYPos < smallestDistanceToYPos)
                {
                    smallestDistanceToYPos = distanceToYPos;
                }
            }
        }

        distanceToYPos = smallestDistanceToYPos;

        bool positionIsWithinTolerance = false;

        positionIsWithinTolerance = true;


        if (distanceToYPos > 1.3f)
        {
            positionIsWithinTolerance = false;
            Debug.Log("Distance to y pos is not within tolerance. Breaking. Distance to y pos is " + distanceToYPos + " at time " + Time.time);
            //Debug.Break();
        }

        else
        {
            positionIsWithinTolerance = true;
        }

        if (hitGround
            && positionIsWithinTolerance)
        {
            lastValidYPos = Rigidbody.transform.position.y;
            //Debug.Log("We are  hitting the ground " + Time.time);
        }

        else
        {
            Rigidbody.transform.position = new Vector3(Rigidbody.transform.position.x,
                                                       lastValidYPos,
                                                       Rigidbody.transform.position.z);
            //Debug.Log("We are NOT hitting the ground " + Time.time);
        }

        //Debug.Log("Distance to y pos is " + distanceToYPos);

        //Debug.Log("Rigidbody distance from ground is " + distanceToGround);
    }

    #endregion

    #region OldDriveServerRpc

    //[ServerRpc(RequireOwnership = false)]
    //public void DriveServerRpc(Vector2 input)
    //{
    //    Debug.Log("Driving " + Time.time + " input is x:" + input.x + " y: " + input.y);

    //    Vector3 rigidbodyForward = Rigidbody.transform.forward;
    //    Vector3 velocity = Rigidbody.velocity;
    //    float rigidbodyVelocityMagnitude = velocity.magnitude;
    //    float angleBetweenVelocityAndForward = Vector3.Angle(rigidbodyForward, velocity);


    //    //Debug.Log("Angle of velocity " + angleBetweenVelocityAndForward);

    //    bool isBraking = false;

    //    float motor = maxMotorTorque * input.x;
    //    float steering = maxSteeringAngle * input.y;

    //    float brakeRate = rigidbodyVelocityMagnitude / 20.0f;
    //    brakeRate = Mathf.Clamp(brakeRate, 0, 1.0f);

    //    float brakeMultiplier = 40; // The values probably should be between 10 and 200, depending how snappy of a brake you want. But this may be a shitty implementation anyways


    //    if (rigidbodyVelocityMagnitude < 5.0f)
    //    {
    //        float motorAdd = AccelerationCurve.Evaluate(Mathf.Clamp(rigidbodyVelocityMagnitude / 5.0f,
    //                                                                0,
    //                                                                1.0f));
    //        motor += motorAdd * maxMotorTorque * input.x * 3;
    //        ResetForceAppDistance();
    //    }

    //    else
    //    {
    //        //axleInfos[0].leftWheel.forceAppPointDistance = -0.1f; // Default is 0f
    //        //axleInfos[0].rightWheel.forceAppPointDistance = -0.1f; // Default is 0f
    //        //axleInfos[1].leftWheel.forceAppPointDistance = -0.15f; // Default is 0f
    //        //axleInfos[1].rightWheel.forceAppPointDistance = -0.15f; // Default is 0f
    //    }

    //    if (brakeAbruptly)
    //    {
    //        timeSpentInBrakingAbrubtly += Time.deltaTime;

    //        if (timeSpentInBrakingAbrubtly >= 1.0f)
    //        {
    //            brakeAbruptly = false;
    //        }
    //    }


    //    //Debug.Log("Brakerate is " + brakeRate);

    //    IsGoingInReverse = false;

    //    if (input.x < -0.1f && angleBetweenVelocityAndForward < 90.0f
    //        || input.x > 0.1f && angleBetweenVelocityAndForward > 90.0f
    //        || brakeAbruptly)
    //    {
    //        //brakeMultiplier = 50000.0f;
    //        //motor = maxMotorTorque * brakeMultiplier * input.x;
    //        motor = 0;
    //        float amountToBrake = BrakeInputVelocityReduceCurve.Evaluate(brakeRate);
    //        //amountToBrake = 1.0f;
    //        float newVelo = rigidbodyVelocityMagnitude - (amountToBrake * Time.fixedDeltaTime * brakeMultiplier);
    //        newVelo = Mathf.Clamp(newVelo, 0, 300.0f);
    //        Rigidbody.velocity = velocity.normalized * newVelo;
    //    }

    //    else if (input.x < 0)
    //    {
    //        float multiplier = 1;

    //        if (rigidbodyVelocityMagnitude < 5.0f) 
    //        {
    //            multiplier = 5;
    //        }
    //        motor = maxMotorTorqueInReverse * multiplier * input.x;

    //    }

    //    if (angleBetweenVelocityAndForward > 90
    //        && rigidbodyVelocityMagnitude > 1.0f)
    //    {
    //        IsGoingInReverse = true;
    //    }

    //    else
    //    {
    //        IsGoingInReverse = false;
    //    }



    //    //if (input.x > 0.1f && angleBetweenVelocityAndForward > 90.0f)
    //    //{
    //    //    // brakeMultiplier = 50000.0f;
    //    //    //motor = maxMotorTorque * brakeMultiplier * input.x;
    //    //    motor = 0;
    //    //    float amountToBrake = BrakeInputVelocityReduceCurve.Evaluate(brakeRate);
    //    //    //amountToBrake = 1.0f;
    //    //    Rigidbody.velocity = velocity.normalized * (rigidbodyVelocityMagnitude - amountToBrake * Time.fixedDeltaTime * brakeMultiplier);
    //    //}

    //    bool freeRoll = false;
    //    float leftMotor = 0;
    //    float rightMotor = 0;

    //    if (input.x < 0.1f && input.x > -0.1f)
    //    {
    //        //Debug.Log("Braking. A free roll");
    //        motor = 0;
    //        steering = 44 * input.y;

    //        if (input.y > 0.1f)
    //        {
    //            leftMotor = maxMotorTorque * Mathf.PI;
    //        }

    //        else if (input.y < -0.1f)
    //        {
    //            rightMotor = maxMotorTorque * Mathf.PI;
    //        }
    //    }

    //    // If we ended up on our roof
    //    if (Vector3.Angle(Rigidbody.transform.up, Vector3.up) >= 90.0f)
    //    {
    //        TimeSpentOnRoof += Time.fixedDeltaTime;

    //        Vector3 previousForward = Rigidbody.transform.forward;

    //        if (TimeSpentOnRoof >= 6.0f)
    //        {
    //            Rigidbody.transform.rotation = Quaternion.LookRotation(previousForward, Vector3.up);
    //            TimeSpentOnRoof = 0;
    //        }
    //    }

    //    else
    //    {
    //        TimeSpentOnRoof = 0;
    //    }

    //    freeRoll = false;

    //    //Debug.Log("Rigidbody velocity is " + rigidbodyVelocityMagnitude);

    //    if (!freeRoll)
    //    {
    //        foreach (AxleInfo axleInfo in axleInfos)
    //        {
    //            if (axleInfo.steering)
    //            {
    //                axleInfo.leftWheel.steerAngle = steering;
    //                axleInfo.rightWheel.steerAngle = steering;
    //            }
    //            if (axleInfo.motor)
    //            {
    //                axleInfo.leftWheel.motorTorque = motor;
    //                axleInfo.rightWheel.motorTorque = motor;

    //                //Debug.Log("Left front torque is " + axleInfo.leftWheel.motorTorque);
    //            }
    //        }
    //    }

    //    else
    //    {

    //        axleInfos[0].leftWheel.motorTorque = leftMotor;
    //        axleInfos[0].rightWheel.motorTorque = rightMotor;
    //    }

    //    float distanceToGround;


    //    RaycastHit[] hits = Physics.RaycastAll(transform.position + (Vector3.up * 1000),
    //                                           Vector3.down * 2000);

    //    bool hitGround = false;
    //    float distanceToYPos = 1000000;

    //    float smallestDistanceToYPos = 100000;

    //    for (int i = 0; i < hits.Length; i++)
    //    {
    //        //Debug.Log("We are hitting object with tag " + hits[i].collider.gameObject.tag + " at time " + Time.time + " object name is " + hits[i].collider.gameObject.name + " at time " + Time.time);

    //        if (hits[i].collider.gameObject.CompareTag("Ground"))
    //        {
    //            distanceToYPos = Rigidbody.transform.position.y - hits[i].point.y;
    //            //Debug.Log("We are hitting the ground. Y pos is " + hits[i].point.y + Time.time);
    //            hitGround = true;

    //            if (distanceToYPos < smallestDistanceToYPos)
    //            {
    //                smallestDistanceToYPos = distanceToYPos;
    //            }
    //        }
    //    }

    //    distanceToYPos = smallestDistanceToYPos;

    //    bool positionIsWithinTolerance = false;

    //    positionIsWithinTolerance = true;


    //    if (distanceToYPos > 1.3f )
    //    {
    //        positionIsWithinTolerance = false;
    //        Debug.Log("Distance to y pos is not within tolerance. Breaking. Distance to y pos is " + distanceToYPos + " at time " + Time.time);
    //        //Debug.Break();
    //    }

    //    else
    //    {
    //        positionIsWithinTolerance = true;
    //    }

    //    if (hitGround
    //        && positionIsWithinTolerance)
    //    {
    //        lastValidYPos = Rigidbody.transform.position.y;
    //        //Debug.Log("We are  hitting the ground " + Time.time);
    //    }

    //    else
    //    {
    //        Rigidbody.transform.position = new Vector3(Rigidbody.transform.position.x,
    //                                                   lastValidYPos,
    //                                                   Rigidbody.transform.position.z);
    //        //Debug.Log("We are NOT hitting the ground " + Time.time);
    //    }

    //    //Debug.Log("Distance to y pos is " + distanceToYPos);

    //    //Debug.Log("Rigidbody distance from ground is " + distanceToGround);
    //}

    #endregion

    public void OnFixedUpdate()
    {
        FixedUpdateLenght = Time.fixedDeltaTime;
        TimeOfLastFixedUpdate = Time.time;
        lastPos = transform.position;
        predictedPos = transform.position + Rigidbody.velocity * FixedUpdateLenght;
        CurrentRigidbodyPosition = Rigidbody.transform.position;
        LastKnownVelocity = Rigidbody.velocity;
        LastKnownVelocityMagnitude = LastKnownVelocity.magnitude;

        if (IsServer)
        {
            serverLastPos = transform.position;
            serverPredictedPos = transform.position + Rigidbody.velocity * FixedUpdateLenght;
            serverTimeOfLastFixedUpdate = Time.time;
            serverLastKnownVelocity = Rigidbody.velocity;
            serverLastKnownVelocityMagnitude = serverLastKnownVelocity.magnitude;
        }
    }

    private void ResetForceAppDistance()
    {
        axleInfos[0].leftWheel.forceAppPointDistance = 0f; // Default is 0f
        axleInfos[0].rightWheel.forceAppPointDistance = 0f; // Default is 0f
        axleInfos[1].leftWheel.forceAppPointDistance = 0f; // Default is 0f
        axleInfos[1].rightWheel.forceAppPointDistance = 0f; // Default is 0f
    }

    public void UpdateInput(Vector2 input)
    {
        LastKnownVerticalInput = input.x;
        LastKnownHorizontalInput = input.y;
        SendInputToServerServerRpc(new Vector2(LastKnownVerticalInput,
                                               LastKnownHorizontalInput));
    }

    private void Update()
    {
        if (IsServer)
        {
            serverTimeSinceLastFixedUpdate = Time.time - serverTimeOfLastFixedUpdate;
        }

        bool isCarDriver = false;

        if (HasADriver
            && CharacterManager.Instance.ClientId == DriverPlayerClientId)
        {
            isCarDriver = true;
        }


        //if (isCarDriver)
        //{
        //    LastKnownVerticalInput = Input.GetAxisRaw("Vertical");
        //    LastKnownHorizontalInput = Input.GetAxisRaw("Horizontal");

        //    SendInputToServerServerRpc(new Vector2(LastKnownVerticalInput,
        //                               LastKnownHorizontalInput));
        //}

        //lastTenKnownVelocities[runningVelocityIndex] = Rigidbody.velocity;
        lastTenKnownVelocities[runningVelocityIndex] = serverLastKnownVelocity;


        runningVelocityIndex++;

        if (runningVelocityIndex >= lastTenKnownVelocities.Length)
        {
            runningVelocityIndex = 0;
        }

        float average = 0;

        for (int i =0; i < lastTenKnownVelocities.Length; i++)
        {
            average += lastTenKnownVelocities[i].magnitude;
        }

        average /= lastTenKnownVelocities.Length;

        //Debug.Log("Average velocity is " + average);

        if (true)
        {
            float timePassedSinceLastFixedUpdate = Time.time - TimeOfLastFixedUpdate;

            float lerpAmount = timePassedSinceLastFixedUpdate / FixedUpdateLenght;
            //CarGraphics.transform.position = Vector3.Lerp(lastPos, predictedPos, lerpAmount);

            //Vector3 lerpedPos = Vector3.Lerp(lastPos, predictedPos, lerpAmount);


            float serverTimePassedLerpAmount = serverTimeSinceLastFixedUpdate / FixedUpdateLenght;

            Vector3 lerpedPos = Vector3.zero;

            lerpedPos = DoSimpleLerpWithAverageVelocity(average, lerpAmount);

            //if (CarGraphicsParent == null) 
            //{
            //    CarGraphics.transform.position = Rigidbody.transform.position;
            //}

            //else
            //{
            //    CarGraphics.transform.position = CarGraphicsParent.transform.position;
            //}

            //if (HasADriver) 
            //{
            //    Debug.Log("Last known velocity magnitude is " + LastKnownVelocityMagnitude + " at time " + Time.time);
            //}

            if (true)
            {
                //Debug.Log("Server velocity is " + serverLastKnownVelocity + " client fixed update velocity is " + LastKnownVelocity);

                //CarGraphics.transform.position =


                //Vector3 velocityPredictedPos = transform.position + Rigidbody.velocity * Time.deltaTime;

                //Vector3 velocityPredictedPos = LastCarGraphicsPosition + Rigidbody.velocity.normalized * average * Time.deltaTime;


                // Last good(yish) one.
                //----------------------------------------------------
                //lerpedPos = DoJerkyishMovement(average, lerpAmount);
                //----------------------------------------------------

                //CarGraphics.transform.position = Vector3.Lerp(CarGraphics.transform.position,
                //                                              CarGraphicsParent.transform.position,
                //                                              Time.deltaTime * 5.0f);

                //CarGraphics.transform.position = Vector3.SmoothDamp(LastCarGraphicsPosition,
                //                                    velocityPredictedPos,
                //                                    ref CurrentCarGraphicsVelocity,
                //                                    0.02f,
                //                                    3600,
                //                                    Time.deltaTime);
                //CarGraphics.transform.position = Vector3.Lerp(LastCarGraphicsPosition, Rigidbody.transform.position, Time.deltaTime * 2.0f);


                if (HasADriver)
                {
                    //Debug.Log("car velocity mag at late update is " + Rigidbody.velocity.magnitude);
                }
            }

            else
            {
                CarGraphics.transform.position = Vector3.SmoothDamp(LastCarGraphicsPosition,
                                                                    lerpedPos,
                                                                    ref CurrentCarGraphicsVelocity,
                                                                    0.2f,
                                                                    3600,
                                                                    Time.deltaTime);
            }


            //CarGraphics.transform.position = Vector3.Lerp(lastPos, predictedPos, lerpAmount);

            //CarGraphics.transform.position = Vector3.SmoothDamp(LastCarGraphicsPosition,
            //                                        Rigidbody.transform.position,
            //                                        ref CurrentCarGraphicsVelocity,
            //                                        0.3f,
            //                                        100,
            //                                        Time.deltaTime);

            //CarGraphics.transform.position = Vector3.SmoothDamp(LastCarGraphicsPosition,
            //                                        predictedPos,
            //                                        ref CurrentCarGraphicsVelocity,
            //                                        0.1f,
            //                                        100,
            //                                        Time.deltaTime);



            //if (HasADriver)
            //{
            //    Debug.Log("Lerpamount is " + lerpAmount + " at time " + Time.time + " time passed since last fixed update is " + timePassedSinceLastFixedUpdate + " time of last fixed update is " + TimeOfLastFixedUpdate + " current time is " + Time.time);
            //}


            //CarGraphics.transform.position = Vector3.Lerp(CarGraphics.transform.position, transform.position, 66.0f * Time.deltaTime);


            // Good values otherwise, but the collisions are way off visually.
            //CarGraphics.transform.position = Vector3.SmoothDamp(LastCarGraphicsPosition,
            //                                                    transform.position,
            //                                                    ref CurrentCarGraphicsVelocity,
            //                                                    0.3f,
            //                                                    30.0f,
            //                                                    Time.deltaTime);

            //CarGraphics.transform.position = Vector3.SmoothDamp(LastCarGraphicsPosition,
            //                                            transform.position,
            //                                            ref CurrentCarGraphicsVelocity,
            //                                            0.3f,
            //                                            300.0f,
            //                                            Time.deltaTime);


            //CarGraphics.transform.position = transform.position;
            LastCarGraphicsPosition = CarGraphics.transform.position;


            //CarGraphics.transform.forward = Vector3.SmoothDamp(-LastCarGraphicsForward,
            //                                                   transform.forward,
            //                                                   ref CurrentGraphicsForwardVelocity,
            //                                                   0.1f,
            //                                                   300.0f,
            //                                                   Time.deltaTime);

        }

        CarGraphics.transform.forward = Vector3.SmoothDamp(-LastCarGraphicsForward,
                                                           transform.forward,
                                                           ref CurrentGraphicsForwardVelocity,
                                                           0.1f,
                                                           3000.0f,
                                                           Time.deltaTime);

        CarGraphics.transform.forward = -CarGraphics.transform.forward;
        LastCarGraphicsForward = CarGraphics.transform.forward;

        //CurrentRigidbodyPosition = Rigidbody.transform.position;
    }

    private Vector3 DoSimpleLerpWithAverageVelocity(float average, float lerpAmount)
    {
        Vector3 predPos = lastPos + serverLastKnownVelocity.normalized * average * Time.deltaTime;
        //Vector3 lerpedPos = Vector3.Lerp(lastPos, predPos, serverTimePassedLerpAmount);

        //Vector3 predPos = serverLastPos + serverLastKnownVelocity * Time.deltaTime;
        //Vector3 lerpedPos = Vector3.Lerp(serverLastPos, predPos, serverTimePassedLerpAmount);
        //Vector3 lerpedPos = Vector3.Lerp(serverLastPos, predPos, lerpAmount);

        Vector3 lerpedPos = Vector3.Lerp(lastPos, predPos, lerpAmount);

        //Vector3 predPos = serverLastPos + serverLastKnownVelocity * Time.deltaTime;
        //Vector3 lerpedPos = Vector3.Lerp(serverLastPos, predPos, lerpAmount);

        //CarGraphics.transform.position = lerpedPos;

        //CarGraphics.transform.position = Vector3.SmoothDamp(LastCarGraphicsPosition,
        //                                    lerpedPos,
        //                                    ref CurrentCarGraphicsVelocity,
        //                                    0.22f,
        //                                    3600,
        //                                    Time.deltaTime);

        Vector3 intermediatePos = Vector3.SmoothDamp(LastCarGraphicsPosition,
                                    lerpedPos,
                                    ref CurrentCarGraphicsVelocity,
                                    0.12f,
                                    3600,
                                    Time.deltaTime);

        //CarGraphics.transform.position = Vector3.Lerp(LastCarGraphicsPosition, intermediatePos, Time.deltaTime * 6.0f);
        //CarGraphics.transform.position = lerpedPos;

        if (CarGraphicsParent != null) {
            CarGraphics.transform.position = Vector3.SmoothDamp(LastCarGraphicsPosition,
                                                                CarGraphicsParent.transform.position,
                                                                ref CurrentCarGraphicsVelocity,
                                                                0.1f,
                                                                3600,
                                                                Time.deltaTime);
        }

        else
        {
            CarGraphics.transform.position = Vector3.SmoothDamp(LastCarGraphicsPosition,
                                                    lerpedPos,
                                                    ref CurrentCarGraphicsVelocity,
                                                    0.1f,
                                                    3600,
                                                    Time.deltaTime);
        }

        return lerpedPos;
    }

    private Vector3 DoJerkyishMovement(float average, float lerpAmount)
    {
        Vector3 lerpedPos = Vector3.Lerp(lastPos, lastPos + Rigidbody.velocity.normalized * average * Time.deltaTime, lerpAmount);

        //CarGraphics.transform.position = lerpedPos;

        CarGraphics.transform.position = Vector3.SmoothDamp(LastCarGraphicsPosition,
                                                            lerpedPos,
                                                            ref CurrentCarGraphicsVelocity,
                                                            0.06f,
                                                            3600,
                                                            Time.deltaTime);
        return lerpedPos;
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
}