using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBlips : MonoBehaviour
{
    public GameObject GreenBlip;
    public GameObject YellowBlip;

    // Sets blips active or inactive
    public void SetMapBlip(bool activateGreenBlip,
                           bool activateYellowBlip)
    {
        if (activateGreenBlip)
        {
            GreenBlip.SetActive(true);
        }

        else
        {
            GreenBlip.SetActive(false);
        }

        if (activateYellowBlip)
        {
            YellowBlip.SetActive(true);
        }

        else
        {
            YellowBlip.SetActive(false);
        }
    }
}
