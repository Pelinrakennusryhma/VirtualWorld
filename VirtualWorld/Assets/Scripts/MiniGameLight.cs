using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameLight : MonoBehaviour
{
    public static MiniGameLight Instance;
    public Light Light;

    private void Awake()
    {
        Instance = this;
        Light = GetComponent<Light>();
        gameObject.SetActive(false);
    }

    public void TurnOnMiniGameLight(ArcadeCabinetTrigger.CabinetType cabinet)
    {

        if (cabinet == ArcadeCabinetTrigger.CabinetType.TabletopInvaders)
        {
            Light.intensity = 0.17f;
        }

        else if (cabinet == ArcadeCabinetTrigger.CabinetType.GravityShip)
        {
            Light.intensity = 1;
        }

        gameObject.SetActive(true);
    }

    public void TurnOffMiniGameLight()
    {
        gameObject.SetActive(false);
    }
}
