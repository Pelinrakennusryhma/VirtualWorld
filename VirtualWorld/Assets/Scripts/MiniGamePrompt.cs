using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiniGamePrompt : MonoBehaviour
{

    public TextMeshProUGUI TextMeshPro;

    public void Awake()
    {
        TextMeshPro = GetComponent<TextMeshProUGUI>();
        OnExitMiniGameTrigger();
    }

    public void OnEnterMinigameTrigger(ArcadeCabinetTrigger.CabinetType cabinet)
    {
        gameObject.SetActive(true);

        if (cabinet == ArcadeCabinetTrigger.CabinetType.TabletopInvaders)
        {
            TextMeshPro.text = "PRESS E TO PLAY\nTABLETOP INVADERS";
        }

        else if (cabinet == ArcadeCabinetTrigger.CabinetType.GravityShip)
        {
            TextMeshPro.text = "PRESS E TO PLAY\nGRAVITY SHIP";
        }
    }

    public void OnExitMiniGameTrigger()
    {
        gameObject.SetActive(false);
    }
}
