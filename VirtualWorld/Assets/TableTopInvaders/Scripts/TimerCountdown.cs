using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerCountdown : MonoBehaviour
{
    public Text GameOver;
    public GameObject textDisplay;
    public static float secondsLeft = 0;
    public static float aikaaKulunut = 0;
    public bool takingAway = false;
    public static bool peliVoiAlkaa = false;

    public static float SavedTime;

    public InGameUI inGameUI;

    void Start() {
        peliVoiAlkaa = false;
        textDisplay.GetComponent<Text>().text = "Aikaa: " + secondsLeft;

        inGameUI = FindObjectOfType<InGameUI>();
        inGameUI.SetTime(secondsLeft);
    }

    void Update() 
    {
        // Timeri on toteutettu hieman kummallisesti. Miksi coroutine ja intit?
        //
        inGameUI.SetTime(secondsLeft);

        if (!peliVoiAlkaa)
        {
            return;
        }

        secondsLeft -= Time.deltaTime;
        aikaaKulunut += Time.deltaTime;


        if (secondsLeft <= 0)
        {
            peliVoiAlkaa = false;

            GameFlowManager.Instance.OnTimerRanOut();
            //SceneManager.LoadScene("GameOver"); // Lataa Scenen nimen mukaan.
        }


        //if(peliVoiAlkaa) {
        //    if(takingAway == false && secondsLeft > 0) {
        //        StartCoroutine(TimeTake());
        //    } else if (takingAway == false && secondsLeft <= 0){
        //        // GameOver.text = "GameOver";
        //        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Lataa Scenen numeropaikan mukaan
        //        SceneManager.LoadScene("GameOver"); // Lataa Scenen nimen mukaan.
        //    } 
        //}

        
    }

    IEnumerator TimeTake() {
        takingAway = true;
        yield return new WaitForSeconds(1);
        secondsLeft -= 1;
        aikaaKulunut++;
        textDisplay.GetComponent<Text>().text = "Aikaa: " + secondsLeft;
        takingAway = false;
    }

    public static void AddTime(float timeToAdd)
    {
        secondsLeft += timeToAdd;
        //Debug.Log("Added time " + timeToAdd);
    }

    public static void SaveTime()
    {
        SavedTime += secondsLeft;
        //Debug.Log("Saved time is " + SavedTime);
    }
}
