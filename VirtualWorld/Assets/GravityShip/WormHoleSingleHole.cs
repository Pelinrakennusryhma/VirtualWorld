using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormHoleSingleHole : MonoBehaviour
{

    public bool CanEnterHole;
    private bool WaitingForHoleExit;

    public WormHoleGravityShip WormHoleController;

    public void Init(WormHoleGravityShip wormHoleController)
    {
        WormHoleController = wormHoleController;
        CanEnterHole = true;

        //Debug.Log("Initted hole " + gameObject.name.ToString());
    }

    public void WarpToThisHole()
    {
        CanEnterHole = false;
    }



    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("WArps warps");

        if (other.gameObject.CompareTag("Player")
            && CanEnterHole)
        {
            Vector3 offset = other.gameObject.transform.position - transform.position;
            CanEnterHole = true;
            WormHoleController.WarpToOtherHole(this,
                                               offset);

        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")
            && WaitingForHoleExit)
        {
            CanEnterHole = true;
        }
    }

    public void StartWaitingForHoleAreaExit()
    {
        CanEnterHole = false;
        WaitingForHoleExit = true;
    }
}
