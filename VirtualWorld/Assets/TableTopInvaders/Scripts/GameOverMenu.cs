using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void NewGame() {
        // Jos ajanlaskenta (secondsLeft) on 0, niin asetetaaan ajanlaskenta (secondsLeft) arvoon 10

        TimerCountdown.secondsLeft = 60;
        
        // Siirrytään Game sceneen
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); 
    }
    public void MainMenu() {
        // Siirrytään Menu sceneen
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }

    public void SamatPaikat() {

        GameFlowManager.RelaunchCustomPositionsScene();

        TimerCountdown.secondsLeft = 60;
        
        SceneManager.LoadScene("OmaValintaPeli");
    }

    public void QuitGame() {
        Debug.Log("QUIT!");
        // Suljetaan peli
        Application.Quit();
    }
}
