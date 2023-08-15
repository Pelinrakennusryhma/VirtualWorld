using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameModeInfo : MonoBehaviour
{
    public TextMeshProUGUI TextMeshProUGUI;

    public void SetClarifier(GameFlowManager.GameMode mode)
    {
        string text = "";

        switch (mode)
        {
            case GameFlowManager.GameMode.None:
                text = "";
                break;
            case GameFlowManager.GameMode.Easy:
                text = "ROOKIE MODE \n Pickups are a plenty \n +75 TIME per level. +20 Ammo per cannon. \n Time and ammo is saved to next level";
                break;
            case GameFlowManager.GameMode.Medium:
                text = "NOVICE MODE \n Pickups are scarce \n +60 TIME per level. +15 Ammo per cannon. \n Time and ammo is saved to next level";
                break;
            case GameFlowManager.GameMode.Hard:
                text = "VETERAN MODE \n No pickups \n +60 TIME per level. +15 Ammo per cannon. \n Time and ammo is saved to next level";
                break;
            case GameFlowManager.GameMode.IronMan:
                text = "IRON MAN \n No pickups \n 60 TIME per level. 15 Ammo per cannon. \n Time and ammo is not saved to next level";
                break;
            case GameFlowManager.GameMode.TimeAttack:
                text = "TIME ATTACK \n Reach 200 points in 120 seconds\n infinite ammo";
                break;
            default:
                text = "";
                break;
        }

        TextMeshProUGUI.text = text;
    }
}
