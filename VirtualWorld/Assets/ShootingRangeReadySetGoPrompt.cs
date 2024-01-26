using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShootingRangeReadySetGoPrompt : MonoBehaviour
{    
    public delegate void PromptFinished();
    public PromptFinished OnReadySetGoPromptFinished;

    private const string ReadyString = "READY";
    private const string SetString = "SET";
    private const string GoString = "GO";

    [SerializeField] private TextMeshProUGUI prompt;

    [SerializeField] private AnimationCurve scaleCurve;

    private bool isDisplayingReadySetGoPrompt;
    public bool IsDisplayingReadySetGoPrompt { get => isDisplayingReadySetGoPrompt; 
                                               private set => isDisplayingReadySetGoPrompt = value; }    
    
    private bool isPaused;
    public bool IsPaused { get => isPaused; 
                           private set => isPaused = value; }

    private float timePassedInPhase;
    private float promptLength = 3.0f;
    private int promptPhase = -1;
    private float timePassedDisplayingPrompt;

    private Vector3 originalPromptScale;



    private void Awake()
    {
        originalPromptScale = prompt.transform.localScale;
    }

    private void Update()
    {
        if (!IsPaused
            && IsDisplayingReadySetGoPrompt)
        {
            timePassedInPhase += Time.deltaTime;

            if (promptPhase <= 0)
            {
                promptPhase = 1;
                prompt.text = ReadyString;
                timePassedInPhase = 0f;
            }

            else if (promptPhase == 1
                     && timePassedDisplayingPrompt
                     >= promptLength * (1f / 3f))
            {
                promptPhase = 2;
                prompt.text = SetString;
                timePassedInPhase = 0f;
            }

            else if (promptPhase == 2
                     && timePassedDisplayingPrompt
                     >= promptLength * (2f / 3f))
            {
                promptPhase = 3;
                prompt.text = GoString;
                timePassedInPhase = 0f;
            }

            else if (promptPhase == 3
                     && timePassedDisplayingPrompt
                     >= promptLength)
            {
                OnPromptFinished();
            }

            float ratio = timePassedInPhase / (promptLength / 3f);
            float scaleMultiplier = scaleCurve.Evaluate(ratio);
            prompt.gameObject.transform.localScale = new Vector3(originalPromptScale.x * scaleMultiplier,
                                                                 originalPromptScale.y * scaleMultiplier,
                                                                 originalPromptScale.z * scaleMultiplier);

            timePassedDisplayingPrompt += Time.deltaTime;
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

    public void SetPaused()
    {
        IsPaused = true;
    }

    public void ResumeFromPause()
    {
        IsPaused = false;
    }

    public void StartPrompt()
    {
        IsDisplayingReadySetGoPrompt = true;
        promptPhase = 0;
        timePassedDisplayingPrompt = 0f;
        timePassedInPhase = 0f;
        prompt.transform.localScale = Vector3.zero;
    }

    private void OnPromptFinished()
    {
        IsDisplayingReadySetGoPrompt = false;

        if (OnReadySetGoPromptFinished != null)
        {
            OnReadySetGoPromptFinished();
            //Debug.Log("We fired an event about ready, set, go prompt finished");
        }
    }
}
