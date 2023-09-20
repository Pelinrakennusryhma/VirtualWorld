using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverPoints : MonoBehaviour
{
    private TextMeshProUGUI TextMesh;

    public void Awake()
    {
        TextMesh = GetComponent<TextMeshProUGUI>();
        TextMesh.text = PisteLaskuri.pisteet.ToString();
    }
}
