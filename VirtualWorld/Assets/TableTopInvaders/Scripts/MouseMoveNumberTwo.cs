using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMoveNumberTwo : MonoBehaviour
{
    private Plane plane;
    private Vector3 lastMousePos;
    private bool MouseWasUsedLast;
    private Vector3 targetPos;
    private Quaternion targetRot;
    private bool keyboardWasUsedLast;

    private void Awake()
    {
        plane = new Plane(Vector3.up, Vector3.up * 6.64f); // Nostetaan tasoa putken korkeudelle, niin perspektiivi ei vääristä hiiren paikkaa niin pahasti.
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 lastPos = transform.position;

        bool keyboardWasUsedThisFrame = true;


        float horizontal = Input.GetAxis("Horizontal");
        // Otetaan horizontal muuttujaan näppäinten A ja D / nuoli vasen ja nuoli oikea arvo
        float vertical = Input.GetAxis("Vertical");





        bool keyboardWasUsed = false;

        if (horizontal != 0.0f
            || vertical != 0.0f)
        {
            keyboardWasUsed = true;
            keyboardWasUsedThisFrame = true;
            keyboardWasUsedLast = true;
        }

        else
        {
            keyboardWasUsedThisFrame = false;
        }

        float differenceBetweenMousePositions = 0;

        differenceBetweenMousePositions = (Input.mousePosition - lastMousePos).magnitude;

        lastMousePos = Input.mousePosition;

        if (differenceBetweenMousePositions > 0)
        {

            keyboardWasUsedLast = false;

            if (!keyboardWasUsedThisFrame)
            {
                MouseWasUsedLast = true;
            }
        }


        targetPos = transform.position;
        targetRot = transform.rotation;

        if (!keyboardWasUsedThisFrame
            && !keyboardWasUsed
            && !keyboardWasUsedLast
            && MouseWasUsedLast)  
        {
            MoveCannonWithMouse(out targetPos, out targetRot);
        }    
        
        if (targetPos.x < 16)
        {
            targetPos = new Vector3(16, transform.position.y, transform.position.z);
        }

        else if (targetPos.x > 42)
        {
            targetPos = new Vector3(42, transform.position.y, transform.position.z);
        }

        transform.position = Vector3.Lerp(transform.position, targetPos, 25.10f * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 25.10f * Time.deltaTime);
        

    }

    private void MoveCannonWithMouse(out Vector3 targetPos,
                                     out Quaternion targetRot)
    {
        targetPos = transform.position;

        // LIIKE
        //https://gamedevbeginner.com/how-to-convert-the-mouse-position-to-world-space-in-unity-2d-3d/#screen_to_world_3d
        // Mallia otettu tuolta.

        float distance;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 worldPosition = Vector3.zero;

        if (plane.Raycast(ray, out distance))
        {
            worldPosition = ray.GetPoint(distance);
        }

        targetPos = new Vector3(worldPosition.x, transform.position.y, transform.position.z);

        // VANHA YRITELMÄ LIIKKEELLE, joka tuskin toimii mukavasti, jos esim. kameran perspektiiviä tai paikkaa muuttaa, kun on kovin hard coodattua

        //float cameraZHeight = Camera.main.transform.position.z - 34.07f; // Convert to use a depth plane...
        //Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -cameraZHeight)); // z täytyy asettaa positiiviseksi, muuten ei toimi


        //mousePosition = new Vector3(mousePosition.x, mousePosition.y, 1.0f);
        ////Debug.Log("x " + Input.mousePosition.x + " y " + Input.mousePosition.y + " z " + Input.mousePosition.z);
        ////Debug.Log("Mouse position " + Time.time + " x " + mousePosition.x + " y " + mousePosition.y + " z " + mousePosition.z);

        //transform.position = new Vector3(mousePosition.x, transform.position.y, transform.position.z);

        // ROTAATIO
        float zOffsetFromCannonOrigin = worldPosition.z - transform.position.z + 4.0f;

        if (zOffsetFromCannonOrigin > 7.0f)
        {
            zOffsetFromCannonOrigin = 7.0f;
        }

        if (zOffsetFromCannonOrigin < -7.0f)
        {
            zOffsetFromCannonOrigin = -7.0f;
        }

        Vector3 eulerRotation = transform.rotation.eulerAngles;

        // Tämä ei toimi yhdessä vanhan näppis-skriptin kanssa
        targetRot = Quaternion.Euler(zOffsetFromCannonOrigin, eulerRotation.y, eulerRotation.z);

        //Debug.Log("Z offset from origing " + zOffsetFromCannonOrigin);
    }
}
