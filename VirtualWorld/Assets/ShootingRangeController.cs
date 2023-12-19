using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ShootingRangeController : MonoBehaviour
{    
    private static ShootingRangeController instance;
    public static ShootingRangeController Instance { get => instance; 
                                                     private set => instance = value; }

    [SerializeField] private ShootingRangeReadySetGoPrompt readySetGoPrompt;
    public ShootingRangeReadySetGoPrompt ReadySetGoPrompt { get => readySetGoPrompt; 
                                                             private set => readySetGoPrompt = value; }


    [SerializeField] private ShootingRangeTimer Timer;

    private const string beginnerSceneName = "BeginnerCourseShootingRange";
    private const string intermediateSceneName = "IntermediateCourseShootingRange";
    private const string expertSceneName = "ExpertCourseShootingRange";

    private bool allTargetsHaveBeenDestroyed;

    private ShootingRangeTargetTracker targetTracker;

    private bool areOptionsShowing;

    private void Awake()
    {
        targetTracker = FindObjectOfType<ShootingRangeTargetTracker>();

        if (Instance != null)
        {
            Destroy(gameObject);
        }

        else
        {
            Instance = this;
            // No need to use don't destroy on load on this one, because all the 
            // scenes have their own controller and references.
        }
    }

    private void Start()
    {
        allTargetsHaveBeenDestroyed = false;

        Timer.OnHide();
        ReadySetGoPrompt.OnHide();

        bool isAGamePlayScene = false;

        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName.Equals(beginnerSceneName)
            || sceneName.Equals(intermediateSceneName)
            || sceneName.Equals(expertSceneName))
        {
            isAGamePlayScene = true;
        }

        if (isAGamePlayScene) 
        {
            StartReadySetGoPrompt();
        }
    }

    private void OnDestroy()
    {
        ReadySetGoPrompt.OnReadySetGoPromptFinished -= OnReadySetGoPromptFinished;
    }

    private void Update()
    {
        allTargetsHaveBeenDestroyed = CheckTargets();

        if (allTargetsHaveBeenDestroyed)
        {
            StopTimer();
        }

        if (!areOptionsShowing
            && OptionsShooting.IsShowingOptions)
        {         
            if (Timer.TimerHasStarted)
            {            
                Timer.OnHide();
                Timer.SetPaused();
            }

            else if (ReadySetGoPrompt.IsDisplayingReadySetGoPrompt)
            {            
                ReadySetGoPrompt.OnHide();
                ReadySetGoPrompt.SetPaused();
            }
            
            areOptionsShowing = true;
        }

        else if (areOptionsShowing
                 && !OptionsShooting.IsShowingOptions)
        {
            if (Timer.TimerHasStarted)
            {
                Timer.OnShow();
                Timer.ResumeFromPause();
            }

            else if (ReadySetGoPrompt.IsDisplayingReadySetGoPrompt)
            {
                ReadySetGoPrompt.OnShow();
                ReadySetGoPrompt.ResumeFromPause();
            }

            areOptionsShowing = false;
        }
    }

    private void StartReadySetGoPrompt()
    {        
        Timer.OnHide();

        ReadySetGoPrompt.OnReadySetGoPromptFinished -= OnReadySetGoPromptFinished;
        ReadySetGoPrompt.OnReadySetGoPromptFinished += OnReadySetGoPromptFinished;

        ReadySetGoPrompt.OnShow();
        ReadySetGoPrompt.StartPrompt();
    }

    private void OnReadySetGoPromptFinished()
    {
        ReadySetGoPrompt.OnHide();
        Timer.OnShow();
        Timer.StartTimer();
    }

    private void StopTimer()
    {
        Timer.StopTimer();
    }

    private bool CheckTargets()
    {
        bool allTargetsAreDestroyed = false;

        if (targetTracker != null)
        {
            allTargetsAreDestroyed = targetTracker.CheckIfTargetsHaveBeenDestroyed();
        }

        return allTargetsAreDestroyed;
    }
}
