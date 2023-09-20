using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankUI : MonoBehaviour
{
    public TextMeshProUGUI TextMeshProUGUI;

    private void Start()
    {
        if (GameManagerGravityShip.Deaths == 0) 
        {
            TextMeshProUGUI.text = "RANK: GOD";
        }

        else if (GameManagerGravityShip.Deaths <= 10)
        {
            TextMeshProUGUI.text = "RANK: EXPERT";
        }

        else if (GameManagerGravityShip.Deaths <= 20)
        {
            TextMeshProUGUI.text = "RANK: INTERMEDIATE";
        }

        else if (GameManagerGravityShip.Deaths <= 30)
        {
            TextMeshProUGUI.text = "RANK: NOVICE";
        }

        else
        {
            TextMeshProUGUI.text = "RANK: ROOKIE";
        }
    }
}
