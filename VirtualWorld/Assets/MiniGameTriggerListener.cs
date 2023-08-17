using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameTriggerListener : MonoBehaviour
{
    public ArcadeCabinetTrigger.CabinetType CurrentCabinet;

    public void OnEnteredArcadeCabinetTrigger(ArcadeCabinetTrigger.CabinetType cabinet)
    {
        CurrentCabinet = cabinet;
    }

    public void OnExitArcadeCabinetTrigger()
    {
        CurrentCabinet = ArcadeCabinetTrigger.CabinetType.None;
    }

    private void Update()
    {
        // REplace this with new input system

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (CurrentCabinet == ArcadeCabinetTrigger.CabinetType.TabletopInvaders) 
            {
                AdditiveSceneLauncher.Instance.SetScene(1);
            }

            else if (CurrentCabinet == ArcadeCabinetTrigger.CabinetType.GravityShip)
            {
                AdditiveSceneLauncher.Instance.SetScene(2);
            }
        }
    }
}
