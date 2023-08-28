using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StarterAssets;

public class MiniGameTriggerListener : MonoBehaviour
{
    public ArcadeCabinetTrigger.CabinetType CurrentCabinet;
    public StarterAssetsInputs Inputs;
    public MiniGamePrompt MiniGamePrompt;
    public AdditiveSceneLauncher AdditiveSceneLauncher;

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

        if (Inputs.interact)
        {
            Inputs.ClearInteractInput();

            if (CurrentCabinet == ArcadeCabinetTrigger.CabinetType.TabletopInvaders) 
            {
                AdditiveSceneLauncher.SetScene(1);
            }

            else if (CurrentCabinet == ArcadeCabinetTrigger.CabinetType.GravityShip)
            {
                AdditiveSceneLauncher.SetScene(2);
            }
        }
    }
}
