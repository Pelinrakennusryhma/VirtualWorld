using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverTimeElapsed : MonoBehaviour
{
    private TextMeshProUGUI TextMesh;

    public void Awake()
    {
        TextMesh = GetComponent<TextMeshProUGUI>();

        int seconds = Mathf.RoundToInt(TimerCountdown.aikaaKulunut);

        TextMesh.text = seconds.ToString();
    }
}
