using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TykinLiikeHiirellä : MonoBehaviour
{
    // Objecktin ja hiiren välisen paikan muuttuja mOffset
    private Vector3 mOffset;
    // Hiiren z koordinaatti kameran mukaan.
    private float mZCoord;

// Hiiren vasen nappi painettu
void OnMouseDown() {
    // Otetaan mZCoord muuttujaan peliobjectin z akseli kameran kokonäkymässä
    mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
    
    // Objecktin paikasta vähennetään hiiren paikka ja asetetaan mOffset muuttujaan
    mOffset = gameObject.transform.position - GetMouseWorldPos();
}

private Vector3 GetMouseWorldPos() {
    // Asetetaan pikselikordinaatti hiiren paikasta x Vector3:n mousePoint muuttujaan
    Vector3 mousePoint = new Vector3(Input.mousePosition.x, 0, 0);
    // Asetetaan mousePoint muuttujaan z kordinaatti mZCoord muuttujasta
    mousePoint.z = mZCoord;
    // Palautateaan kameran näyttöpaikaan asetettu hiiren paikka
    return Camera.main.ScreenToWorldPoint(mousePoint);
}

// Hiiren vasen nappi painettu ja hiirtä liikutettu
void OnMouseDrag() {
    // Jos hiiren x koordinaatti on suurempi kuin 18 ja hiiren x koordinaatti on pienempi kuin 46 toteuta if lause
    if(GetMouseWorldPos().x > 16 && GetMouseWorldPos().x < 42) {
        // Asetetaan hiiren paikka lisättynä mOffset
        transform.position = GetMouseWorldPos() + mOffset;
    }
    
}  
}
