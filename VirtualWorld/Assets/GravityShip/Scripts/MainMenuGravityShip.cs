using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuGravityShip : MonoBehaviour
{
    public void GoToShipSelection()
    {
        GameManagerGravityShip.Instance.SoundManager.PlayUIClick();
        GameManagerGravityShip.Instance.GoToShipSelection();
    }

    public void StartLevelOne()
    {
        GameManagerGravityShip.Instance.SoundManager.PlayUIClick();
        GameManagerGravityShip.Instance.StartNewGame();
    }

    public void StartLevelOneWitShipOne()
    {
        GameManagerGravityShip.Instance.SoundManager.PlayUIClick();
        PlayerControllerGravityShip.SetShipType(PlayerControllerGravityShip.ShipType.ShipOne);
        GameManagerGravityShip.Instance.StartNewGame();
    }

    public void StartLevelOneWithShipTwo()
    {
        GameManagerGravityShip.Instance.SoundManager.PlayUIClick();
        PlayerControllerGravityShip.SetShipType(PlayerControllerGravityShip.ShipType.ShipTwo);
        GameManagerGravityShip.Instance.StartNewGame();
    }

    public void GoToCredits()
    {
        GameManagerGravityShip.Instance.SoundManager.PlayUIClick();
        GameManagerGravityShip.Instance.GoToCredits();
    }

    public void GoToMainMenu()
    {
        GameManagerGravityShip.Instance.SoundManager.PlayUIClick();
        GameManagerGravityShip.Instance.GoToTitleScreen();
    }

    public void QuitGame()
    {
        GameManagerGravityShip.Instance.SoundManager.PlayUIClick();
        //Application.Quit();

        MiniGameLauncher.Instance.GoBackToPlayground(true);
    }



}
