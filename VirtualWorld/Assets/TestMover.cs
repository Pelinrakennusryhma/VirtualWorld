using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Authentication;
using Characters;


// Stolen things from: https://docs.unity.cn/560/Documentation/Manual/WheelColliderTutorial.html
public class TestMover : NetworkBehaviour
{

    // STOLEN SECTION --------------------------------------------------------------
    [System.Serializable]
    public class AxleInfo
    {
        public WheelCollider leftWheel;
        public WheelCollider rightWheel;
        public bool motor; // is this wheel attached to motor?
        public bool steering; // does this wheel apply steer angle?
    }

    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have
    // END STOLEN SECTION ---------------------------------------------------------


    private float maxMagnitude = 10.0f;
    private Vector3 velocity;
    private float timeUntilVelocityChange;
    private Vector3 startPos;
    private Rigidbody rigidBody;
    private float startY;

    [SyncVar]
    public string playerId;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        rigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //Debug.Log("Fixed update is called");

        bool isCarDriver = false;

        if (playerId.Equals(UserSession.Instance.LoggedUserData.id))
        {
            isCarDriver = true;
            Debug.Log("We are the car driver. Execute " + Time.time);
        }

        if (isCarDriver)
        {                
            RequestMovementServerRpc(new Vector2(Input.GetAxis("Vertical"),
                                                 Input.GetAxis("Horizontal")));

            //timeUntilVelocityChange -= Time.fixedDeltaTime;

            //if (timeUntilVelocityChange <= 0)
            //{
            //    timeUntilVelocityChange = 1.0f;
            //    velocity = new Vector3(Random.Range(-12.0f, 12.0f),
            //                           rigidBody.velocity.y,
            //                           Random.Range(-12.0f, 12.0f));

            //    //RequestVelocityChangeServerRpc(velocity);

            //}
        }
    }

    //[ServerRpc(RequireOwnership = false)]
    public void OnPlayerEnteredCar(string playerId)
    {
        this.playerId = playerId;
    }

    //[ServerRpc(RequireOwnership = false)]
    public void OnPlayerExitedCar(string playerId)
    {
        this.playerId = "";
    }

    [ServerRpc(RequireOwnership = false)]

    public void RequestMovementServerRpc(Vector2 input)
    {
        float motor = maxMotorTorque * input.x;
        float steering = maxSteeringAngle * input.y;

        Debug.Log("Motor is " + motor + " steering is " + steering);

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


    [ServerRpc(RequireOwnership = false)]

    public void RequestVelocityChangeServerRpc(Vector3 newVelocity)
    {
        Debug.Log("SErver rpc is calleed");

        rigidBody.velocity = newVelocity;

        Vector3 toStartPosXZ = startPos - transform.position;
        toStartPosXZ = new Vector3(toStartPosXZ.x, 0, toStartPosXZ.z);
        float magnitudeFromStartPos = toStartPosXZ.magnitude;

        //if (magnitudeFromStartPos >= maxMagnitude)
        //{
        //    transform.position = new Vector3(startPos.x, 0, startPos.z) 
        //                         + -new Vector3(toStartPosXZ.x,
        //                                                 transform.position.y,
        //                                                 toStartPosXZ.z)
        //                                                 * maxMagnitude;
        //}

        Debug.Log("Should update velocity");
    }

    public void LateUpdate()
    {
        bool isCarDriver = false;

        if (playerId.Equals(UserSession.Instance.LoggedUserData.id))
        {
            isCarDriver = true;
            Debug.Log("We are the car driver. Execute " + Time.time);
        }

        if (isCarDriver)
        {
            CharacterManager.Instance.OwnedCharacter.transform.position = transform.position;
            Debug.Log("Maybe we are doing heinous things here. Fix");
        }
    }
}
