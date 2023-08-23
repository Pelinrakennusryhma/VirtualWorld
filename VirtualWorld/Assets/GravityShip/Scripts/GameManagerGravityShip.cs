using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerGravityShip : MonoBehaviour
{
    public static int Deaths;
    public static GameManagerGravityShip Instance;

    public UICanvasGravityShip UICanvas;

    public bool RelaunchingTheSameScene;

    public int CurrentLevel;

    public SoundManagerGravityShip SoundManager;

    public string RelaunchLevelName;

    public string SceneToBeSetActive;
    public int FramesPassedSinceSettingTheScene;
    public bool WaitingToSetActiveScene;

    public void Awake()
    {
        if (Instance == null)
        {
            RelaunchingTheSameScene = false;
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
            OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UICanvas = FindObjectOfType<UICanvasGravityShip>();

        if (UICanvas != null) 
        {
            UICanvas.SetDeaths(Deaths);
        }

        // Hopefully we have only one light in the scene...


    }

    public void OnPlayerDeath()
    {
        RelaunchingTheSameScene = true;
        Deaths++;
    }

    public void OnlevelComplete()
    {
        RelaunchingTheSameScene = false;
    }

    public void GoToShipSelection()
    {
        //SceneManager.LoadScene("GravityShip_ShipSelectionScreen");
        LoadSceneForMiniGameMode("GravityShip_ShipSelectionScreen");
        //Debug.Log("Should go to ship selection screen");
    }

    public void StartNewGame()
    {
        CurrentLevel = 0;
        Deaths = 0;
        GoToNextLevel();
        //Debug.Log("Start new game");

    }

    public void GoToCredits()
    {
        //SceneManager.LoadScene("GravityShip_Credits");
        LoadSceneForMiniGameMode("GravityShip_Credits");
        //Debug.Log("Should go to credits");
    }

    public void GoToTitleScreen()
    {
        //SceneManager.LoadScene("GravityShip_TitleScreen");

        bool alreadyInMenu = false;

        int sceneCount = SceneManager.sceneCount;

        //Scene[] scenes = SceneManager.GetAllScenes();

        for (int i = 0; i < sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name.Equals("GravityShip_TitleScreen"))
            {
                alreadyInMenu = true;
            }
        }
        //SceneManager.LoadScene("Menu");

        if (!alreadyInMenu)
        {        
            LoadSceneForMiniGameMode("GravityShip_TitleScreen");
        }

        else
        {
            Debug.Log("Already in menu");
        }

    }

    public void GoToNextLevel()
    {
        CurrentLevel ++;

        string sceneName = "";

        switch (CurrentLevel)
        {
            case 1:
                sceneName = "GravityShip_Level1";
                //SceneManager.LoadScene("GravityShip_Level1");
                break;

            case 2:
                sceneName = "GravityShip_Level2";
                //SceneManager.LoadScene("GravityShip_Level2");
                break;

            case 3:
                sceneName = "GravityShip_Level3";
                //SceneManager.LoadScene("GravityShip_Level3");
                break;

            case 4:
                sceneName = "GravityShip_Level4";
                //SceneManager.LoadScene("GravityShip_Level4");
                break;


            case 5:
                sceneName = "GravityShip_Level5";
                //SceneManager.LoadScene("GravityShip_Level5");
                break;

            case 6:
                sceneName = "GravityShip_Level6";
                //SceneManager.LoadScene("GravityShip_Level6");
                break;

            case 7:
                sceneName = "GravityShip_Level7";
                //SceneManager.LoadScene("GravityShip_Level7");
                break;

            case 8:
                sceneName = "GravityShip_Level8";
                //SceneManager.LoadScene("GravityShip_Level8");
                break;

            case 9:
                sceneName = "GravityShip_Level9";
                //SceneManager.LoadScene("GravityShip_Level9");
                break;

            case 10:
                sceneName = "GravityShip_Level10";
                //SceneManager.LoadScene("GravityShip_Level10");
                break;

            case 11:
                sceneName = "GravityShip_Level11";
                //SceneManager.LoadScene("GravityShip_Level11");
                break;

            case 12:
                sceneName = "GravityShip_Level12";
                //SceneManager.LoadScene("GravityShip_Level12");
                break;

            case 13:
                sceneName = "GravityShip_Level13";
                //SceneManager.LoadScene("GravityShip_Level13");
                break;

            case 14:
                sceneName = "GravityShip_Level14";
                //SceneManager.LoadScene("GravityShip_Level14");
                break;

            case 15:
                sceneName = "GravityShip_Level15";
                //SceneManager.LoadScene("GravityShip_Level15");
                break;

            case 16:
                sceneName = "GravityShip_Level16";
                //SceneManager.LoadScene("GravityShip_Level16");
                break;

            case 17:
                sceneName = "GravityShip_Level17";
                //SceneManager.LoadScene("GravityShip_Level17");
                break;

            case 18:
                sceneName = "GravityShip_Level18";
                //SceneManager.LoadScene("GravityShip_Level18");
                break;

            case 19:
                sceneName = "GravityShip_Level19";
                //SceneManager.LoadScene("GravityShip_Level19");
                break;

            case 20:
                sceneName = "GravityShip_Level20";
                //SceneManager.LoadScene("GravityShip_Level20");
                break;

            case 21:
                sceneName = "GravityShip_Level21";
                //SceneManager.LoadScene("GravityShip_Level21");
                break;

            case 22:
                sceneName = "GravityShip_Level22";
                //SceneManager.LoadScene("GravityShip_Level22");
                break;

            case 23:
                sceneName = "GravityShip_Level23";
                //SceneManager.LoadScene("GravityShip_Level23");
                break;

            case 24:
                sceneName = "GravityShip_Level24";
                //SceneManager.LoadScene("GravityShip_Level24");
                break;

            case 25:
                sceneName = "GravityShip_Level25";
                //SceneManager.LoadScene("GravityShip_Level25");
                break;

            case 26:
                sceneName = "GravityShip_Level26";
                //SceneManager.LoadScene("GravityShip_Level26");
                break;

            case 27:
                sceneName = "GravityShip_Level27";
                //SceneManager.LoadScene("GravityShip_Level27");
                break;


            case 28:
                sceneName = "GravityShip_Level28";
                //SceneManager.LoadScene("GravityShip_Level28");
                break;

            case 29:
                sceneName = "GravityShip_Level29";
                //SceneManager.LoadScene("GravityShip_Level29");
                break;

            case 30:
                sceneName = "GravityShip_Level30";
                //SceneManager.LoadScene("GravityShip_Level30");
                break;
            default:
                sceneName = "GravityShip_GameCompleteScreen";
                //SceneManager.LoadScene("GravityShip_GameCompleteScreen");
                break;
        }

        RelaunchLevelName = sceneName;
        LoadSceneForMiniGameMode(sceneName);
    }

    public void LoadSceneForMiniGameMode(string sceneName)
    {
        Time.timeScale = 1.0f;
        MiniGameLauncher.Instance.UnloadActiveScene();
        MiniGameLauncher.Instance.SaveActiveSceneName(sceneName);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        FramesPassedSinceSettingTheScene = 0;
        SceneToBeSetActive = sceneName;
        WaitingToSetActiveScene = true;

    }

    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    GoToTitleScreen();
        //}

        if (WaitingToSetActiveScene)
        {
            FramesPassedSinceSettingTheScene++;

            if (FramesPassedSinceSettingTheScene >= 2)
            {
                //SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneToBeSetActive));
                WaitingToSetActiveScene = false;
            }
        }
    }

    public void OnShouldLoadPlayerDeadScene()
    {
        LoadSceneForMiniGameMode("GravityShip_YouDied");
    }

    public void RelaunchPreviousScene()
    {
        LoadSceneForMiniGameMode(RelaunchLevelName);
    }
}
