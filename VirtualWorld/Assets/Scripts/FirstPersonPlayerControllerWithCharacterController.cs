using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonPlayerControllerWithCharacterController : MonoBehaviour
{
    public bool UseRealGravity;
    public float RealisticGravityJumpForce = 5.0f;
    public float UnrealisticGravityJumpForce = 5.0f;

    public float UnrealisticGravityAscendingGravity = 14.1f;
    public float UnrealisticGravityDescendingGravity = 28.2f;

    public float MaxWalkSpeed = 5.0f;
    public float MaxRunSpeed = 9.0f;

    public float AccelerationWalk = 48.0f;
    public float AccelerationRun = 96.0f;

    public Camera Camera;

    public CapsuleCollider CapsuleCollider;

    public Rigidbody Rigidbody;

    private float yRot;
    private Vector3 movement;

    private bool IsOnAir;

    private bool SpaceWasPressedDuringLastUpdate;

    // Start is called before the first frame update
    private float groundedExtraTime;

    private bool isRunning;

    void Awake()
    {
        if (UseRealGravity)
        {
            Rigidbody.useGravity = true;
        }

        else
        {
            Rigidbody.useGravity = false;
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        movement = new Vector3(Input.GetAxisRaw("Horizontal"),
                               0, Input.GetAxisRaw("Vertical"));

        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"),
                                         Input.GetAxisRaw("Mouse Y"));

        if (Input.GetButton("Fire3"))
        {
            isRunning = true;
        }

        else
        {
            isRunning = false;
        }

        float mouseSensitivity, xRot;

        MoveHead(mouseInput, out mouseSensitivity, out xRot);

        if (Input.GetButtonDown("Jump"))
        {
            SpaceWasPressedDuringLastUpdate = true;
        }

        //MoveBody(movement);
    }

    private void FixedUpdate()
    {
        MoveBody(movement);
    }

    private void MoveHead(Vector2 mouseInput, out float mouseSensitivity, out float xRot)
    {
        mouseSensitivity = 2.0f;
        bool invertMouse = true;

        xRot = mouseInput.x;
        float yRotation = mouseInput.y;

        if (!invertMouse)
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

        Vector3 point1 = CapsuleCollider.transform.position + Vector3.up * (CapsuleCollider.height / 2);
        Vector3 point2 = CapsuleCollider.transform.position + Vector3.down * (CapsuleCollider.height / 2);

        //hits = Physics.CapsuleCast(point1,
        //                           point2,
        //                           CapsuleCollider.radius,
        //                           Vector3.down * 10.0f,
        //                           out hit);

        hits = Physics.Raycast(Rigidbody.transform.position,
                               Vector3.down,
                               out hit,
                               10.0f);

        if (hits)
        {
            Debug.Log("Normal of hit is " + hit.normal);
        }

        if (hits && hit.distance <= 1.001f)
        {
            isGrounded = true;
            groundedExtraTime = 0.2f;
        }

        else
        {
            isGrounded = false;
        }

        if (groundedExtraTime >= 0.0f)
        {
            isGrounded = true;
        }

        bool isOnASlope = false;

        if(Physics.Raycast(Rigidbody.transform.position,
                           Vector3.down,
                           out hit,
                           CapsuleCollider.height / 2 + 0.3f))
        {
            float angle = Vector3.Angle(hit.normal, Vector3.up);

            if (angle <= 45.0f)
            {
                isOnASlope = true;
            }
        }

        Debug.Log("Is on a slope " + isOnASlope + " " + Time.time);

        if (isGrounded
            && SpaceWasPressedDuringLastUpdate)
        {
            groundedExtraTime = 0;

            if (!UseRealGravity)
            {
                Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, UnrealisticGravityJumpForce, Rigidbody.velocity.z);
            }

            else
            {
                Rigidbody.AddForce(Vector3.up * RealisticGravityJumpForce, ForceMode.Impulse);
            }
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
                }

                else
                {
                    Rigidbody.velocity = new Vector3(Rigidbody.velocity.x,
                    Rigidbody.velocity.y - UnrealisticGravityDescendingGravity * Time.deltaTime,
                    Rigidbody.velocity.z);

                }
            }
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

        if (!isRunning)
        {
            clampedVelo = Vector3.ClampMagnitude(xzVelo + move, MaxWalkSpeed);
        }

        else
        {
            clampedVelo = Vector3.ClampMagnitude(xzVelo + move, MaxRunSpeed);
        }

        if (move.magnitude <= 0.1f)
        {
            clampedVelo *= 0.931f;
        }

        float veloY;

        if (isGrounded || isOnASlope)
        {
            veloY = 0;
        }

        else 
        { 
            veloY = Rigidbody.velocity.y; 
        }

        Rigidbody.velocity = new Vector3(clampedVelo.x, veloY, clampedVelo.z);
    }
}
