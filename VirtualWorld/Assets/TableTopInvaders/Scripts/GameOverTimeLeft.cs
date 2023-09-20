using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverTimeLeft : MonoBehaviour
{
    private TextMeshProUGUI TextMesh;

    public void Awake()
    {
        TextMesh = GetComponent<TextMeshProUGUI>();

        int seconds = (int) TimerCountdown.secondsLeft;

        TextMesh.text = seconds.ToString();
    }
}
