using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonPlayerControllerShooting : MonoBehaviour
{
    public bool UseRealGravity;
    public float RealisticGravityJumpForce = 5.0f;
    public float UnrealisticGravityJumpForce = 5.0f;

    public float UnrealisticGravityAscendingGravity = 14.1f;
    public float UnrealisticGravityDescendingGravity = 28.2f;

    public float MaxWalkSpeed = 5.0f;
    public float MaxRunSpeed = 9.0f;
    public float MaxCrouchSpeed = 2.5f;

    public float AccelerationWalk = 48.0f;
    public float AccelerationRun = 96.0f;

    public Camera Camera;

    public CapsuleCollider StandingCapsuleCollider;
    public CapsuleCollider CrouchingCapsuleCollider;

    public Rigidbody Rigidbody;

    public Vector3 CrouchingCameraLocalPosition = Vector3.zero;

    private float yRot;
    private Vector3 movement;

    private bool IsOnAir;

    private bool SpaceWasPressedDuringLastUpdate;

    // Start is called before the first frame update
    private float groundedExtraTime;

    private bool isRunning;
    private bool isCrouchingButtonDown;

    private float jumpTimer;

    private bool isCrouching;

    private Vector3 cameraOriginalLocalPosition;

    private FirstPersonPlayerControlsShooting Controls;

    private Transform latestMovingPlatform;

    private bool canGetGrounded = true;

    void Awake()
    {
        Controls = GetComponent<FirstPersonPlayerControlsShooting>();

        if (UseRealGravity)
        {
            Rigidbody.useGravity = true;
        }

        else
        {
            Rigidbody.useGravity = false;
        }

        cameraOriginalLocalPosition = Camera.transform.localPosition;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OptionsShooting.IsShowingOptions)
        {
            return;
        }

        movement = new Vector3(Controls.Horizontal, 0, Controls.Vertical);

        Vector2 mouseInput = new Vector2(Controls.MouseDeltaX * 0.025f,
                                         Controls.MouseDeltaY * 0.025f);

        if (Controls.RunDown)
        {
            isRunning = true;
        }

        else
        {
            isRunning = false;
        }

        if (Controls.CrouchDown)
        {
            isCrouchingButtonDown = true;
        }

        else
        {
            isCrouchingButtonDown = false;
        }

        if (Controls.JumpDownPressed)
        {
            SpaceWasPressedDuringLastUpdate = true;
        }

        //float xRot;

        //MoveHead(mouseInput, out xRot);

        if (isCrouching)
        {
            Camera.transform.localPosition = Vector3.Lerp(Camera.transform.localPosition, 
                                                          CrouchingCameraLocalPosition,
                                                          9f * Time.deltaTime);
        }

        else
        {
            Camera.transform.localPosition = Vector3.Lerp(Camera.transform.localPosition,
                                                          cameraOriginalLocalPosition,
                                                          12f * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        if (OptionsShooting.IsShowingOptions
            || (ShootingRangeController.Instance != null 
                && ShootingRangeController.Instance.ReadySetGoPrompt.IsDisplayingReadySetGoPrompt))
        {
            return;
        }

        Vector2 mouseInput = new Vector2(Controls.MouseDeltaX * 0.025f,
                                 Controls.MouseDeltaY * 0.025f);

        float xRot;

        MoveHead(mouseInput, out xRot);
        MoveBody(movement);
    }

    private void MoveHead(Vector2 mouseInput, out float xRot)
    {


        float mouseSensitivity = OptionsShooting.MouseSensitivity;
        //Debug.Log("Mouse sensitivity is " + mouseSensitivity);
        xRot = mouseInput.x;
        float yRotation = mouseInput.y;

        if (!OptionsShooting.InvertMouse)
        {
            yRotation = -yRotation;
        }

        float newYRot = yRot + yRotation * mouseSensitivity;

        yRot = Mathf.Lerp(yRot, newYRot, 0.9f);

        if (yRot >= 89.999f)
        {
            yRot = 89.999f;
        }

        else if (yRot <= -89.999f)
        {
            yRot = -89.999f;
        }

        Camera.transform.localRotation = Quaternion.Euler(yRot, 0, 0);
        Quaternion previousRot = Rigidbody.transform.rotation;
        Rigidbody.transform.Rotate(0, xRot * mouseSensitivity, 0);
        Rigidbody.transform.rotation = Quaternion.Slerp(previousRot, Rigidbody.transform.rotation, 0.9f);
    }

    private void MoveBody(Vector3 movement)
    {
        //Debug.Log("Movement is x " + movement.x + " z " + movement.z);
        groundedExtraTime -= Time.deltaTime;

        bool isGrounded = false;
        bool hits = false;
        RaycastHit hit;

        hits = Physics.Raycast(Rigidbody.transform.position,
                               Vector3.down,
                               out hit,
                               10.0f);



        if (hits && hit.distance <= 1.001f)
        {
            isGrounded = true;
            groundedExtraTime = 0.2f;
        }

        else
        {
            isGrounded = false;
        }



        bool isOnASlope = false;
        Vector3 slopeNormal = Vector3.up;
        float slopeAngle = 0;

        if (Physics.Raycast(Rigidbody.transform.position,
                           Vector3.down,
                           out hit,
                           StandingCapsuleCollider.height / 2 + 0.1f))
        {
            float angle = Vector3.Angle(hit.normal, Vector3.up);

            if (angle <= 45.0f
                && angle != 0.0f)
            {
                isOnASlope = true;
                groundedExtraTime = 0.2f;
                slopeNormal = hit.normal;
                //Debug.Log("slope hit " + Time.time);
                slopeAngle = angle;
            }


        }

        else
        {
            //Debug.Log("Not hitting a slope " + Time.time);
        }

        //Debug.Log("Is on a slope " + isOnASlope + " " + Time.time);

        if (groundedExtraTime >= 0.0f)
        {
            isGrounded = true;
        }

        if (isOnASlope)
        {
            isGrounded = true;
            Rigidbody.useGravity = false;
            // Snap to slope

        }

        else
        {
            if (UseRealGravity) 
            {
                Rigidbody.useGravity = true;
            }
        }

        if (!canGetGrounded)
        {
            isGrounded = false;
            isOnASlope = false;
        }

        jumpTimer -= Time.deltaTime;

        // Check if can jump
        bool tryingToStandUp = false;

        if (isCrouchingButtonDown
            && isGrounded)
        {
            isCrouching = true;
            CrouchingCapsuleCollider.enabled = true;
            StandingCapsuleCollider.enabled = false;
            //Debug.Log("Crcouhing "  + Time.time);
        }

        else
        {
            if (isCrouching)
            {
                tryingToStandUp = true;
            }
            //Debug.Log("Not crouching " + Time.time);
        }

        bool canStandUp = true;

        bool hitCeiling = Physics.Raycast(transform.position,
                                          Vector3.up,
                                          StandingCapsuleCollider.height / 2 + StandingCapsuleCollider.radius);

        if (hitCeiling)
        {
            canStandUp = false;
        }

        if (tryingToStandUp
            && canStandUp)
        {
            isCrouching = false;
            CrouchingCapsuleCollider.enabled = false;
            StandingCapsuleCollider.enabled = true;
            //Debug.Log("Trying to stand up ");
        }

        if ((isGrounded || isOnASlope)
            && canGetGrounded
            && SpaceWasPressedDuringLastUpdate
            && jumpTimer <= 0)
        {
            groundedExtraTime = 0;

            if (!UseRealGravity) 
            {

                Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, UnrealisticGravityJumpForce, Rigidbody.velocity.z);
                //Debug.Log("Added false gravity on jump " + Time.time);
            }

            else
            {
                Rigidbody.AddForce(Vector3.up * RealisticGravityJumpForce, ForceMode.Impulse);
                //Debug.Log("Added real gravity on jump " + Time.time);
            }

            canGetGrounded = false;

            jumpTimer = 0.2f;

            //Debug.Log("We pressed jump. Rigidbody velo y is " + Rigidbody.velocity.y + " at time" + Time.time);
        }

        else
        {
            if (!isGrounded
                && !UseRealGravity)
            {
                if (Rigidbody.velocity.y >= 0)
                {
                    Rigidbody.velocity = new Vector3(Rigidbody.velocity.x,
                                         Rigidbody.velocity.y - UnrealisticGravityAscendingGravity * Time.deltaTime,
                                         Rigidbody.velocity.z);

                    //Debug.Log("Jumping ascending. Velo y is " + Rigidbody.velocity.y + " at time " + Time.time);
                }

                else
                {
                    Rigidbody.velocity = new Vector3(Rigidbody.velocity.x,
                    Rigidbody.velocity.y - UnrealisticGravityDescendingGravity * Time.deltaTime,
                    Rigidbody.velocity.z);

                }
            }
        }

        if (isOnASlope
            && jumpTimer <= 0)
        {
            //Debug.Log("On aslope, but not jumping " + Time.time);


            //Vector3 pos1 = transform.position + Vector3.up * CapsuleCollider.height;
            //Vector3 pos2 = transform.position + Vector3.down * CapsuleCollider.height;

            //hits = Physics.CapsuleCast(pos1, pos2, CapsuleCollider.radius, Vector3.down, out hit);

            //if (hits) 
            //{
            //    transform.position = new Vector3(transform.position.x,
            //                                     hit.point.y + (CapsuleCollider.height / 2) + CapsuleCollider.radius,
            //                                     transform.position.z);
            //}
        }



        SpaceWasPressedDuringLastUpdate = false;
        // How about air control?

        Vector3 moveForward = transform.forward * movement.z;
        Vector3 moveSideWays = transform.right * movement.x;
        Vector3 move = moveForward + moveSideWays;

        if (isGrounded) 
        {
            if (!isRunning) 
            {
                move *= AccelerationWalk * Time.deltaTime;
            }

            else
            {
                move *= AccelerationRun * Time.deltaTime;
            }
        }

        else
        {
            move *= 0.2f;
        }

        Vector3 xzVelo = new Vector3(Rigidbody.velocity.x, 0, Rigidbody.velocity.z);
        Vector3 clampedVelo;

        if (!isRunning
            && !isCrouching) 
        {
            clampedVelo = Vector3.ClampMagnitude(xzVelo + move, MaxWalkSpeed);
        }

        else if (isCrouching)
        {
            clampedVelo = Vector3.ClampMagnitude(xzVelo + move, MaxCrouchSpeed);
            //Debug.Log("Should clamp to max crouch speed");
        }

        else
        {
            clampedVelo = Vector3.ClampMagnitude(xzVelo + move, MaxRunSpeed);
        }

        if (move.magnitude <= 0.1f)
        {
            clampedVelo *= 0.931f;
        }

        float yVelo;

        if (Rigidbody.velocity.y < 0
            && !canGetGrounded)
        {
            canGetGrounded = true;
        }

        bool isJumping = false;

        if (SpaceWasPressedDuringLastUpdate
            || jumpTimer > 0
            || (Rigidbody.velocity.y > 0 && !canGetGrounded))
        {
            isJumping = true;

            //Debug.Log("We are jumping. Space was pressed " + SpaceWasPressedDuringLastUpdate + " jumptimer " + jumpTimer + " rb velo y " + Rigidbody.velocity.y + " at time " + Time.time);
        }

        if (((isGrounded 
            || isOnASlope) && canGetGrounded)
            && !SpaceWasPressedDuringLastUpdate
            && jumpTimer <= 0)
        {
            yVelo = 0;
        }

        else
        {
            yVelo = Rigidbody.velocity.y;
        }

        Vector3 velo = new Vector3(clampedVelo.x, yVelo, clampedVelo.z);

        if (isOnASlope
            && !isJumping)
        {
            float veloMag = velo.magnitude;
            //velo = OldAdjustDirectionToSlope(slopeNormal, velo);
            velo = NewAdjustDircetionToSlope(slopeNormal, move) * veloMag;
            velo = Vector3.ClampMagnitude(velo, MaxRunSpeed);
        }

        else
        {
            //Debug.Log("Check failed. Is on a slope " + isOnASlope + " is jumping " + isJumping + " y velo is " + yVelo + " at time " + Time.time);
        }

        Rigidbody.velocity = velo;

        //Debug.Log("Rigidbody velocity is " + Rigidbody.velocity);
    }

    private Vector3 NewAdjustDircetionToSlope(Vector3 slopeNormal,
                                              Vector3 movement)
    {
        return Vector3.ProjectOnPlane(movement, slopeNormal).normalized;
    }

    private Vector3 OldAdjustDirectionToSlope(Vector3 slopeNormal, Vector3 velo)
    {
        Quaternion slopeRotation = Quaternion.FromToRotation(Vector3.up, slopeNormal);

        float veloMag = velo.magnitude;

        Debug.Log("New velo sloped mag is " + (slopeRotation * velo.normalized).magnitude);

        velo = (slopeRotation * velo.normalized) * veloMag;


        //velo = slopeRotation * xzVelo;
        velo = Vector3.ClampMagnitude(velo, MaxRunSpeed);
        //Debug.Log("Sloped " + Time.time + " veloMag is " + veloMag + " velocity magnitude is " + velo.magnitude + " slope rotation is " + slopeRotation.eulerAngles);
        return velo;
    }

    public void ParentToMovingPlatform(Transform platform)
    {
        latestMovingPlatform = platform;
        transform.parent = latestMovingPlatform;

        //Debug.Log("Parented to moving platform");
    }

    public void UnparentFromMovingPlatform(Transform platform)
    {
        if (latestMovingPlatform == platform)
        {            
            latestMovingPlatform = null;
            transform.parent = null;
        }

        //Debug.Log("Unparented from moving platform");
    }
}
