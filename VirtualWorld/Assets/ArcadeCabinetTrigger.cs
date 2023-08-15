using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeCabinetTrigger : MonoBehaviour
{
    public enum CabinetType
    {
        None = 0,
        TabletopInvaders = 1
    }

    public CabinetType Cabinet;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MiniGameTriggerListener listener = other.GetComponent<MiniGameTriggerListener>();
            listener.OnEnteredArcadeCabinetTrigger(Cabinet);
            Debug.Log("Player entered trigger area " + Time.time);

        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MiniGameTriggerListener listener = other.GetComponent<MiniGameTriggerListener>();
            listener.OnExitArcadeCabinetTrigger();
            Debug.Log("Player exited trigger area " + Time.time);

        }
    }
}
