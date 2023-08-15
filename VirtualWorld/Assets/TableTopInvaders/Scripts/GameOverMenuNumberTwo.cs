using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenuNumberTwo : MonoBehaviour
{
    public void LaunchNextLevel()
    {
        GameFlowManager.Instance.SoundManager.PlayUIPress();
        GameFlowManager.Instance.SoundManager.PlaySound("UI button pressed");
        GameFlowManager.Instance.LaunchNextLevel();
    }

    public void NewGame()
    {
        GameFlowManager.Instance.SoundManager.PlayUIPress();
        GameFlowManager.Instance.SoundManager.PlaySound("UI button pressed");
        GameFlowManager.Instance.LaunchNewGame();
    }

    public void GoToMainMenu()
    {
        GameFlowManager.Instance.SoundManager.PlayUIPress();
        GameFlowManager.Instance.SoundManager.PlaySound("UI button pressed");
        GameFlowManager.Instance.GoToMainMenu();
    }

    public void RelaunchCustomPositionsScene()
    {
        GameFlowManager.Instance.SoundManager.PlayUIPress();
        GameFlowManager.Instance.SoundManager.PlaySound("UI button pressed");
        GameFlowManager.Instance.LaunchCustomPositionsLevel();
    }

    public void RelaunchTimeAttackScene()
    {
        GameFlowManager.Instance.SoundManager.PlayUIPress();
        GameFlowManager.Instance.SoundManager.PlaySound("UI button pressed");
        GameFlowManager.Instance.StartTimeAttack();
    }
}
