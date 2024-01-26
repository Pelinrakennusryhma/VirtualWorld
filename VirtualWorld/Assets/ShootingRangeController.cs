using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ShootingRangeController : MonoBehaviour
{    
    private static ShootingRangeController instance;
    public static ShootingRangeController Instance { get => instance; 
                                                     private set => instance = value; }

    [SerializeField] private ShootingRangeReadySetGoPrompt readySetGoPrompt;
    public ShootingRangeReadySetGoPrompt ReadySetGoPrompt { get => readySetGoPrompt; 
                                                             private set => readySetGoPrompt = value; }
        
    [SerializeField] private ShootingRangeTimer timer;
    public ShootingRangeTimer Timer { get => timer; 
                                       private set => timer = value; }
    
    [SerializeField] private ShootingRangeTargetUI targetUI;
    public ShootingRangeTargetUI TargetUI { get => targetUI; set => targetUI = value; }


    [SerializeField] private bool isAGameplayScene;

    //private const string beginnerSceneName = "BeginnerCourseShootingRange";
    //private const string intermediateSceneName = "IntermediateCourseShootingRange";
    //private const string expertSceneName = "ExpertCourseShootingRange";

    private bool allTargetsHaveBeenDestroyed;

    private ShootingRangeTargetTracker targetTracker;

    private bool areOptionsShowing;

    private float defaultShadowDistance;
    private int originalShadowCascadeCount;
    private float originalNormalBias;
    private float originalDepthBias;

    private void Awake()
    {
        targetTracker = FindObjectOfType<ShootingRangeTargetTracker>();
        Instance = this;


        //if (Instance != null)
        //{
        //    Destroy(gameObject);
        //}

        //else
        //{
        //    Instance = this;
        //    // No need to use don't destroy on load on this one, because all the 
        //    // scenes have their own controller and references.

        //    SetShadowDistance();
        //}
    }

    private void Start()
    {
        allTargetsHaveBeenDestroyed = false;

        Timer.OnHide();
        ReadySetGoPrompt.OnHide();

        //bool isAGamePlayScene = false;

        //string sceneName = SceneManager.GetActiveScene().name;

        //if (sceneName.Equals(beginnerSceneName)
        //    || sceneName.Equals(intermediateSceneName)
        //    || sceneName.Equals(expertSceneName))
        //{
        //    isAGamePlayScene = true;

        //    Debug.Log("Is a game play scene");
        //}

        //else
        //{
        //    Debug.Log("Is not a gameplay scene. Active scene is " + SceneManager.GetActiveScene().name);
        //}
        

        if (isAGameplayScene) 
        {
            StartReadySetGoPrompt();
        }
    }

    private void OnDestroy()
    {
        //RevertShadownDistance();
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
            }

            if (Timer.TimerUsRunning)
            {            
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
            }

            if (Timer.TimerUsRunning)
            {
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
            allTargetsAreDestroyed = targetTracker.CheckIfTargetsHaveBeenDestroyed(out int destroyedTargets,
                                                                                   out int aliveTargets,
                                                                                   out int totalTargets);

            targetUI.UpdateTargetAmounts(destroyedTargets, 
                                         aliveTargets, 
                                         totalTargets);
        }

        return allTargetsAreDestroyed;
    }

    //private void SetShadowDistance()
    //{
    //     return;

    //    UniversalRenderPipelineAsset urp = (UniversalRenderPipelineAsset)GraphicsSettings.currentRenderPipeline;
    //    defaultShadowDistance = urp.shadowDistance;
    //    urp.shadowDistance = 500;
        
    //    //originalShadowCascadeCount = urp.shadowCascadeCount;
    //    //urp.shadowCascadeCount = 1;


    //    //originalNormalBias = urp.shadowNormalBias; // Default is currently 1
    //    //urp.shadowNormalBias = 0;
    //    //originalDepthBias = urp.shadowDepthBias; // Default is currently 1
    //    //urp.shadowDepthBias = 0;

    //    //Debug.Log("Shadow distance is " + urp.shadowDistance + " shadow cascade count is " + urp.shadowCascadeCount);
    //}

    //private void RevertShadownDistance()
    //{
    //     return;

    //    UniversalRenderPipelineAsset urp = (UniversalRenderPipelineAsset)GraphicsSettings.currentRenderPipeline;
    //    urp.shadowDistance = defaultShadowDistance;

    //    //urp.shadowCascadeCount = originalShadowCascadeCount;
    //    //urp.shadowNormalBias = originalNormalBias;
    //    //urp.shadowDepthBias = originalDepthBias;
    //}
}
