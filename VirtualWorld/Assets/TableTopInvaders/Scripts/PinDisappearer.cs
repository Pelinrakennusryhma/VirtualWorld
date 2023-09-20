using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PinDisappearer : MonoBehaviour
{
    public keilaappear Plate;

    public void PlacePinOnAPlate(keilaappear plate)
    {
        Plate = plate;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RemovePin();
        }
    }

    public void RemovePin()
    {
        Plate.RemovePin();
    }
}
