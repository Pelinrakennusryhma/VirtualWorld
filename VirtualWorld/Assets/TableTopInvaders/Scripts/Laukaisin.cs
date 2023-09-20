using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laukaisin : MonoBehaviour
{
private Vector3 mOffset;
private float mZCoord;
void OnMouseDown() {

    // objektin z akseli kameran näytössä
    mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
    // Store offset = gameobject world pos - mouse world pos
    // Objecktin paikasta vähennetään hiiren paikka ja asetetaan mOffset muuttujaan
    mOffset = gameObject.transform.position - GetMouseWorldPos();
}

private Vector3 GetMouseWorldPos() {
    // Asetetaan pikselikordinaatti hiiren paikasta y Vector3:n
    Vector3 mousePoint = new Vector3(0, Input.mousePosition.y, 0);
    // Asetetaan mousePoint muuttujaan z kordinaatti mZCoord muuttujasta
    mousePoint.z = mZCoord;
    // Palautateaan kameran näyttöpaikaan asetettu hiiren paikka
    return Camera.main.ScreenToWorldPoint(mousePoint);
}

void OnMouseDrag() {
        // Asetetaan hiiren paikka lisättynä mOffset
        transform.position = GetMouseWorldPos() + mOffset;
    
}
}
