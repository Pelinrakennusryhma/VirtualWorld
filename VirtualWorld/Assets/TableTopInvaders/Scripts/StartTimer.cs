using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartTimer : MonoBehaviour
{
    public TextMeshProUGUI StartTimerText;

    public float TimeUntilStart;

    private int lastWholeNumber;

    public AnimationCurve PulseCurve;
    public AnimationCurve GoOutCurve;

    private float pulseTimer; 


    private bool WaitingForTimer;
    private Vector3 originalGameObjectScale;


    private void Awake()
    {
        StartTimerText.gameObject.SetActive(false);
        originalGameObjectScale = transform.localScale;
    }

    public void OnLevelLaunched()
    {
        lastWholeNumber = 4;
        TimeUntilStart = 4.0f;
        StartTimerText.text = 3.ToString();
        StartTimerText.gameObject.SetActive(true);
        WaitingForTimer = true;
        GameFlowManager.WaitingForStartTimer = true;
        TimerCountdown.peliVoiAlkaa = false;
        pulseTimer = 0.0f;
    }

    public void OnTimerFinished()
    {
        // Inform something that we are ready to start
        WaitingForTimer = false;
        StartTimerText.gameObject.SetActive(false);
        GameFlowManager.WaitingForStartTimer = false;
        TimerCountdown.peliVoiAlkaa = true;
    }

    private void Update()
    {
        if (!WaitingForTimer)
        {
            return;
        }

        pulseTimer += Time.deltaTime;
        TimeUntilStart -= Time.deltaTime;

        int wholeNumber = (int)TimeUntilStart;

        if (wholeNumber < lastWholeNumber)
        {
            lastWholeNumber = wholeNumber;
            pulseTimer = 0.0f;

            if (lastWholeNumber == 3)
            {
                StartTimerText.text = 3.ToString();
                GameFlowManager.Instance.SoundManager.PlayCountdown1();
                GameFlowManager.Instance.SoundManager.PlaySound("countdown THREE");
            }

            else if (lastWholeNumber == 2)
            {
                StartTimerText.text = 2.ToString();
                GameFlowManager.Instance.SoundManager.PlayCountdown1();
                GameFlowManager.Instance.SoundManager.PlaySound("countdown TWO");
            }

            else if (lastWholeNumber == 1)
            {
                StartTimerText.text = 1.ToString();
                GameFlowManager.Instance.SoundManager.PlayCountdown1();
                GameFlowManager.Instance.SoundManager.PlaySound("countdown ONE");
            }

            else if (lastWholeNumber <= 0)
            {
                StartTimerText.text = "GO";
                GameFlowManager.Instance.SoundManager.PlayCountdownGo();
                GameFlowManager.Instance.SoundManager.PlaySound("countdown GO");
                //Debug.Log("GO!");
            }
        }

        if (lastWholeNumber <= 0)
        {
            float valueAtCurve = GoOutCurve.Evaluate(pulseTimer);
            transform.localScale = new Vector3(originalGameObjectScale.x * valueAtCurve,
                                               originalGameObjectScale.y * valueAtCurve,
                                               originalGameObjectScale.z);
        }

        else
        {
            float valueAtCurve = PulseCurve.Evaluate(pulseTimer);
            transform.localScale = new Vector3(originalGameObjectScale.x * valueAtCurve,
                                               originalGameObjectScale.y * valueAtCurve,
                                               originalGameObjectScale.z);
        }

        if (TimeUntilStart <= -0.0f)
        {
            OnTimerFinished();
            //Debug.Log("GO!");
        }
    }
}
