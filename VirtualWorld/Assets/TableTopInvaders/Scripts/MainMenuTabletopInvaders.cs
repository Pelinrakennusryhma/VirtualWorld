using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuTabletopInvaders : MonoBehaviour
{

    public void PlayGame() {

        GameFlowManager.Instance.SoundManager.PlayUIPress();
        GameFlowManager.Instance.SoundManager.PlaySound("UI button pressed");
        // Aloitetaan peli (Eli ladataan Game Scene)
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        GameFlowManager.Instance.GoToGameModeMenu();
    }

    public void AsetaItseKeilatGame() {

            TimerCountdown.secondsLeft = 60;

        GameFlowManager.Instance.SoundManager.PlayUIPress();
        GameFlowManager.Instance.SoundManager.PlaySound("UI button pressed");
        // Aloitetaan peli (Eli ladataan Game Scene)
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
        GameFlowManager.Instance.LaunchCustomPositionsLevel();
    }

    public void GoToCredits()
    {
        GameFlowManager.Instance.SoundManager.PlayUIPress();
        GameFlowManager.Instance.SoundManager.PlaySound("UI button pressed");
        GameFlowManager.Instance.GoToCredits();
    }

    public void QuitGame() {
        GameFlowManager.Instance.SoundManager.PlayUIPress();
        GameFlowManager.Instance.SoundManager.PlaySound("UI button pressed");
        Debug.Log("QUIT!");
        // Poistutaan pelistä
        //Application.Quit();

        MiniGameLauncher.Instance.GoBackToPlayground(true);
    }
}
