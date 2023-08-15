using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InGameUI : MonoBehaviour
{
    public TextMeshProUGUI Points;
    public TextMeshProUGUI LeftCannonBalls;
    public TextMeshProUGUI RightCannonBalls;
    public TextMeshProUGUI Time;

    public StartTimer StartTimer;

    public void Awake()
    {
        GameFlowManager.Instance.OnGameStarted -= OnGameStarted;
        GameFlowManager.Instance.OnGameStarted += OnGameStarted;

        if (GameFlowManager.CurrentGameMode == GameFlowManager.GameMode.TimeAttack
            || GameFlowManager.OnBonusLevel)
        {
            LeftCannonBalls.text = "\u221e";
            RightCannonBalls.text = "\u221e";
        }
    }

    public void OnDestroy()
    {
        GameFlowManager.Instance.OnGameStarted -= OnGameStarted;
    }

    public void OnGameStarted()
    {
        StartTimer.OnLevelLaunched();
    }

    public void SetPoints(int newPoints)
    {
        Points.text = newPoints.ToString();
    }

    public void SetLeftCannonBalls(int ballsRemaining)
    {
        if (GameFlowManager.CurrentGameMode == GameFlowManager.GameMode.TimeAttack
            || GameFlowManager.OnBonusLevel)
        {
            return;
        }

        LeftCannonBalls.text = ballsRemaining.ToString();
    }

    public void SetRightCannonBalls(int ballsRemaining)
    {
        if (GameFlowManager.CurrentGameMode == GameFlowManager.GameMode.TimeAttack
            || GameFlowManager.OnBonusLevel)
        {
            return;
        }

        RightCannonBalls.text = ballsRemaining.ToString();
    }

    public void SetTime(float time)
    {
        int wholeSeconds = (int) time;

        if (wholeSeconds <= 0)
        {
            wholeSeconds = 0;
        }

        Time.text = wholeSeconds.ToString();
    }

    private void Update()
    {
        
    }
}
