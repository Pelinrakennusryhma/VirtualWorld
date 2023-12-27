using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ShootingRangeTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;

    private float elapsedTime;
    public float ElapsedTime { get => elapsedTime; 
                               private set => elapsedTime = value; }

    private bool timerIsRunning;

    public bool TimerUsRunning { get => timerIsRunning; 
                                  private set => timerIsRunning = value; }

    private bool isPaused;
    public bool IsPaused
    {
        get => isPaused;
        private set => isPaused = value;
    }    
    
    private bool timerHasStarted;

    public bool TimerHasStarted { get => timerHasStarted; 
                                  private set => timerHasStarted = value; }



    private void Awake()
    {
        textMesh.text = "0: 00: 00: 000";
        ElapsedTime = 0;
        TimerUsRunning = false;
    }

    private void Update()
    {
        if (!IsPaused 
            && TimerUsRunning)
        {
            ElapsedTime += Time.deltaTime;

            // https://stackoverflow.com/questions/463642/how-can-i-convert-seconds-into-hourminutessecondsmilliseconds-time
            TimeSpan timeSpan = TimeSpan.FromSeconds(ElapsedTime);
            string formatted = timeSpan.ToString(@"hh\:mm\:ss\:fff");

            textMesh.text = formatted;
        }
    }

    public void OnHide()
    {
        gameObject.SetActive(false);
    }

    public void OnShow()
    {
        gameObject.SetActive(true);
    }

    public void StartTimer()
    {
        TimerUsRunning = true;
        timerHasStarted = true;
    }

    public void StopTimer()
    {
        TimerUsRunning = false;
    }

    public void SetPaused()
    {
        IsPaused = true;
    }

    public void ResumeFromPause()
    {
        IsPaused = false;
    }
}
