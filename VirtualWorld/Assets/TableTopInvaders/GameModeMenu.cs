using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeMenu : MonoBehaviour
{
    public void OnEasyButtonPressed()
    {
        GameFlowManager.SetGameMode(GameFlowManager.GameMode.Easy);
        GameFlowManager.Instance.SoundManager.PlayUIPress();
        GameFlowManager.Instance.LaunchNewGame();
    }

    public void OnMediumButtonPressed()
    {
        GameFlowManager.SetGameMode(GameFlowManager.GameMode.Medium);
        GameFlowManager.Instance.SoundManager.PlayUIPress();
        GameFlowManager.Instance.LaunchNewGame();
    }

    public void OnHardButtonPressed()
    {
        GameFlowManager.SetGameMode(GameFlowManager.GameMode.Hard);
        GameFlowManager.Instance.SoundManager.PlayUIPress();
        GameFlowManager.Instance.LaunchNewGame();
    }

    public void OnIronManButtonPressed()
    {
        GameFlowManager.SetGameMode(GameFlowManager.GameMode.IronMan);
        GameFlowManager.Instance.SoundManager.PlayUIPress();
        GameFlowManager.Instance.LaunchNewGame();
    }

    public void OnTimeAttackButtonPressed()
    {
        GameFlowManager.SetGameMode(GameFlowManager.GameMode.TimeAttack);
        GameFlowManager.Instance.SoundManager.PlayUIPress();
        GameFlowManager.Instance.StartTimeAttack();
    }

    public void OnMainMenuButtonPressed()
    {
        GameFlowManager.Instance.SoundManager.PlayUIPress();
        GameFlowManager.Instance.GoToMainMenu();
    }
}
