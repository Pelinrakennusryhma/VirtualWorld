using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathCountUI : MonoBehaviour
{
    public TextMeshProUGUI TextMesh;

    private void Awake()
    {
        TextMesh.text = "DEATHS: " + GameManagerGravityShip.Deaths.ToString();
    }
}
