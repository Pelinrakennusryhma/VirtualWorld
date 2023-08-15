using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TykinLiike : MonoBehaviour
{
// Alustetaan float tyyppinen muuttuja tykinLiikkumisNopeus ja annetaan sille arvoksi 20f;
public float tykinLiikkumisNopeus = 29.0f;

    // float smooth = 5.0f;

    Vector3 lastMousePos;
    float cannonAngle;

    private bool lastUsedKeyboard = true;

    private Quaternion lastKnownCannonRotation;


    private void Update()
{
    // Otetaan horizontal muuttujaan näppäinten A ja D / nuoli vasen ja nuoli oikea arvo
    var horizontal = Input.GetAxis("Horizontal");
        // Otetaan horizontal muuttujaan näppäinten A ja D / nuoli vasen ja nuoli oikea arvo
    var vertical = Input.GetAxis("Vertical");
        
    // Tehdään rajakysely, jossa verrataan tykin reunoja laudan seiniin ja näin estetään tykin läpi meneminen.
    if(transform.transform.position.x > 16 && transform.transform.position.x < 42) {
        transform.Translate(new Vector3(horizontal, 0, 0) * (tykinLiikkumisNopeus * Time.deltaTime));
    } else if (transform.transform.position.x > 16){
        transform.Translate(new Vector3(horizontal-1, 0, 0) * (tykinLiikkumisNopeus * Time.deltaTime));
    } else if (transform.transform.position.x < 42) {
        transform.Translate(new Vector3(horizontal+1, 0, 0) * (tykinLiikkumisNopeus * Time.deltaTime));
    }


        /*     // Rotate the cube by converting the angles into a quaternion.
            // Quaternion target = Quaternion.Euler(vertical, 0, 0);

            // Dampen towards the target rotation
            // transform.rotation = Quaternion.Slerp(target, transform.rotation,  Time.deltaTime * smooth); */


        bool mouseWasUsed = false;
        bool keyboardWasUsed = false;

        if (horizontal != 0.0f
            || vertical != 0.0f)
        {
            keyboardWasUsed = true;

            if (!lastUsedKeyboard)
            {
                lastUsedKeyboard = true;
                cannonAngle = 0; // Reset to zero rotation.
            }
        }

        if ((Input.mousePosition - lastMousePos).magnitude > 0)
        {
            mouseWasUsed = true;
            lastUsedKeyboard = false;
        }

        lastMousePos = Input.mousePosition;


        // Liikutetaan tykin kulmaa maksimissaan 7 astetta pysty akselilla

        if (keyboardWasUsed
            && !mouseWasUsed) 
        {

            cannonAngle = cannonAngle + vertical * 7.0f * Time.deltaTime * 5.0f;
            
            if (cannonAngle < -7.0f)
            {
                cannonAngle = -7.0f;
            }

            else if (cannonAngle > 7.0f)
            {
                cannonAngle = 7.0f;
            }

            transform.rotation = Quaternion.Euler(cannonAngle, 0, 0);        
        }

        else
        {

        }

        lastKnownCannonRotation = transform.rotation;
    }
}

