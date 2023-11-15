using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Transform cameraTransform;
    float mouseSensitivity = 250f;
    float headRotation = 0f;
    float headRotationLimit = 60f;

    Rigidbody playerRigidbody;
    public float playerSpeed = 1.5f;
    float sprintMultiplier = 2f;



    public float jumpHeight = 7f;
    public bool isGrounded;

    void Start()
    {
        cameraTransform = transform.GetChild(0).transform;
        playerRigidbody = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float x = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float y = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime * -1f;

        transform.Rotate(0f, x, 0f);

        headRotation += y;
        headRotation = Mathf.Clamp(headRotation, -headRotationLimit, headRotationLimit);
        cameraTransform.localEulerAngles = new Vector3(headRotation, 0f, 0f);


    if (isGrounded)
    {
       if (Input.GetButtonDown("Jump"))
       {
            Debug.Log("Hyppy");
            playerRigidbody.AddForce(Vector3.up * jumpHeight);
       }
    }

    }




 void OnCollisionEnter(Collision other)
 {
     if (other.gameObject.tag == "Ground")
     {
         isGrounded = true;
     }
    if (other.gameObject.tag == "Laava")
     {
         Debug.Log("KUOLIT");
     }
    if (other.gameObject.tag == "TippuvaKivi")
     {
         Debug.Log("KUOLIT");
     }
 }
 
 void OnCollisionExit(Collision other)
 {
     if (other.gameObject.tag == "Ground")
     {
         isGrounded = false;
     }
 }




    void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        float actualSpeed = playerSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            actualSpeed *= sprintMultiplier;
        }
        playerRigidbody.MovePosition(transform.position + move.normalized * actualSpeed * Time.deltaTime);
    }

}