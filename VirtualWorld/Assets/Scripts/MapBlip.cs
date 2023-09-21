using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBlip : MonoBehaviour
{
    [SerializeField] GameObject blipParent;

    private void OnEnable()
    {
        blipParent.SetActive(true);
    }

    private void OnDisable()
    {
        blipParent.SetActive(false);
    }

    //public GameObject GreenBlip;
    //public GameObject YellowBlip;

    //public bool GreenBlipWasActive;
    //public bool YellowBlipWasActive;



    //public void DisableBlips()
    //{
    //    if (GreenBlip.gameObject.activeSelf)
    //    {
    //        GreenBlipWasActive = true;
    //    }

    //    else
    //    {
    //        GreenBlipWasActive = false;
    //    }

    //    if (YellowBlip.gameObject.activeSelf)
    //    {
    //        YellowBlipWasActive = true;
    //    }

    //    else
    //    {
    //        YellowBlipWasActive = false;
    //    }

    //    GreenBlip.SetActive(false);
    //    YellowBlip.SetActive(false);
    //}

    //public void ReEnableBlips()
    //{
    //    if (GreenBlipWasActive)
    //    {
    //        GreenBlip.SetActive(true);
    //    }

    //    if (YellowBlipWasActive)
    //    {
    //        YellowBlip.SetActive(true);
    //    }
    //}

    //public void EnableGreenBlip()
    //{
    //    GreenBlip.SetActive(true);
    //    YellowBlip.SetActive(false);
    //}

    //public void EnableYellowBlip()
    //{
    //    GreenBlip.SetActive(false);
    //    YellowBlip.SetActive(true);
    //}
}
