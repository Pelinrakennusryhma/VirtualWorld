using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PisteLaskuri : MonoBehaviour
{
    // Luodaan Text muuttuja pisteetText
    public Text pisteetText;
    // Luodaan staattinen int muuttuja pisteet
    public static int pisteet;

    public InGameUI inGameUI;

    // Start is called before the first frame update
    void Start()
    {
        inGameUI = FindObjectOfType<InGameUI>();

        // alustetaan pisteet arvoon 0
        //pisteet = 0;
        inGameUI.SetPoints(pisteet);

    }

    // Update is called once per frame
    void Update()
    {
        inGameUI.SetPoints(pisteet);

        if (GameFlowManager.CurrentGameMode == GameFlowManager.GameMode.TimeAttack
            && GameFlowManager.CurrentEndlessPinPhaser != null
            && !GameFlowManager.CurrentEndlessPinPhaser.WaitingToEnd
            && pisteet >= GameFlowManager.Instance.TargetScore)
        {
            GameFlowManager.CurrentEndlessPinPhaser.OnPhasesFinished();
            Debug.Log("Reached target score of" + GameFlowManager.Instance.TargetScore + " with points " + pisteet.ToString());
        }

        //// Näytetään näytöllä pisteet
        //pisteetText.text = "Pisteet: " + pisteet;

        //// Jos pisteet on maksimi 22, niin siirrytään GameOver menuun
        //if(pisteet == 22) {
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //}
    }
}
