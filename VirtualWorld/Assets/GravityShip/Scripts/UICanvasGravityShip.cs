using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UICanvasGravityShip : MonoBehaviour
{
    public TextMeshProUGUI TextMesh;

    public void SetDeaths(int amount)
    {
        TextMesh.text = "Deaths: " + amount.ToString();
    }
}
