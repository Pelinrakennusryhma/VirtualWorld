using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormHoleGravityShip : MonoBehaviour
{
    public WormHoleSingleHole WormHole1;
    public WormHoleSingleHole WormHole2;

    public void Awake()
    {
        WormHole1.Init(this);
        WormHole2.Init(this);
    }

    public void WarpToOtherHole(WormHoleSingleHole warpFrom,
                                Vector3 offset)
    {
        //Debug.Log("WARP! " + warpFrom.gameObject.name);

        Vector3 targetPos = Vector3.zero;

        if (warpFrom.Equals(WormHole1))
        {
            targetPos = WormHole2.transform.position;
            WormHole2.StartWaitingForHoleAreaExit();
        }

        else if (warpFrom.Equals(WormHole2))
        {
            targetPos = WormHole1.transform.position;
            WormHole1.StartWaitingForHoleAreaExit();
        }

        else
        {
            Debug.LogError("Unknown hole. WTF?");
        }

        PlayerControllerGravityShip.Instance.WarpWithWormHole(targetPos + offset);
    }
}
