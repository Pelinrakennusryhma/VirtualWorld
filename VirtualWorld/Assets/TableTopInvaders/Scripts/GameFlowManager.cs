using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scenes;

public class GameFlowManager : MonoBehaviour
{

    public enum GameMode
    {
        None = 0,
        Easy = 1,
        Medium = 2,
        Hard = 3,
        IronMan = 4,
        TimeAttack = 5
    }

    public static bool OnBonusLevel; 

    public static GameMode CurrentGameMode;

    public static GameFlowManager Instance;
    public static bool RelaunchingCustomScene = false;
    public static RespawnToPreviousPositions.PinType[] SavedPins;
    public static bool PlacingPins;
    public static bool WaitingForStartTimer;

    public static int CurrentLevel;
    public static bool CurrentLevelIsCustomPositionsLevel;

    public delegate void GameStarted();
    public event GameStarted OnGameStarted;

    public static bool LaunchingNormalScene;

    public static PinController CurrentPinController;
    public static PinPhaser CurrentPinPhaser;
    public static EndlessPinPhaser CurrentEndlessPinPhaser;

    public int TargetScore = 200;
    public SoundManagerTabletopInvaders SoundManager;

    public static int MaxLevel = 29;

    public NewMiniGameInputs Inputs;

    // Start is called before the first frame update
    public void Awake()
    {
        MaxLevel = 29;

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //Debug.Log("Created GameFlowManager");
            SavedPins = new RespawnToPreviousPositions.PinType[25];
            TimerCountdown.secondsLeft = 60;

            for (int i = 0; i < SavedPins.Length; i++)
            {
                SavedPins[i] = RespawnToPreviousPositions.PinType.None;
            }

            SoundManager = GetComponentInChildren<SoundManagerTabletopInvaders>();
        }

        else
        {
            Destroy(gameObject);
            //Debug.Log("Destroyed extra GameFlowManager");
        }
    }

    public static void FinishSettingPinPositions()
    {
        PlacingPins = false;
        //Debug.Log("Pins in place.");
        SavePinPositionsToJSON();
        //SavePinPositionsToPlayerPrefs();

        CurrentPinController = FindObjectOfType<PinController>();
        CurrentPinController.FetchAllPins();

        if (Instance.OnGameStarted != null)
        {
            Instance.OnGameStarted();
        }
    }

    public static void NormalSceneIsLaunched()
    {
        LaunchingNormalScene = true;
        CurrentLevelIsCustomPositionsLevel = false;


        CurrentEndlessPinPhaser = FindObjectOfType<EndlessPinPhaser>();

        if (CurrentEndlessPinPhaser != null)
        {
            CurrentEndlessPinPhaser.Initialize();
            CurrentEndlessPinPhaser.OnLevelStart();
        }

        else 
        {
            CurrentPinPhaser = FindObjectOfType<PinPhaser>();

            if (CurrentPinPhaser == null)
            {
                CurrentPinController = FindObjectOfType<PinController>();
                CurrentPinController.FetchAllPins();
            }

            else
            {
                CurrentPinPhaser.Initialize();
                CurrentPinPhaser.OnLevelStart();
            }
        }

        if (Instance.OnGameStarted != null)
        {
            Instance.OnGameStarted();
        }
    }

    public static void RelaunchCustomPositionsScene()
    {
        CurrentLevelIsCustomPositionsLevel = true;
        LoadPinPositionsFromJSON();

        PlacingPins = true;
        RelaunchingCustomScene = true;
        //Debug.Log("Relaunching custom positions scene");
    }

    public static void OnRelaunchedCustomPositionsScene()
    {      
        RelaunchingCustomScene = false;
        //Debug.Log("Done relaunching custom scene");
    }

    // Called when any other scene is loaded other than custom positions scene.
    public static void LaunchCustomPositionsScene()
    {
        CurrentLevelIsCustomPositionsLevel = true;
        PlacingPins = true;

        for (int i = 0; i < SavedPins.Length; i++)
        {
            SavedPins[i] = RespawnToPreviousPositions.PinType.None;
        }

        LoadPinPositionsFromJSON();



        RelaunchingCustomScene = false;
        //Debug.Log("Launching custom positions Scene");
    }

    public RespawnToPreviousPositions.PinType GetPinTypeWithID(int id)
    {
       // Debug.Log("Getting pin type of " + SavedPins[id].ToString());

        return SavedPins[id]; // Ei tehdä error-tarkasteluja tässä tällä kertaa.
    }

    public static void SavePinPosition(int pos, RespawnToPreviousPositions.PinType pinType)
    {
        //Debug.Log("Saving pos " + pos + " " + pinType.ToString());
        SavedPins[pos] = pinType;
    }

    public static void SavePinPositionsToJSON()
    {
        if (SavedPins == null
            || SavedPins.Length <= 0)
        {
            Debug.Log("No saved pins");
        }

        else
        {
            SaveData data = new SaveData();
            data.SavedPins = SavedPins;
            string savedPins = JsonUtility.ToJson(data);
            System.IO.File.WriteAllText(Application.persistentDataPath + "/PinData.json", savedPins);
            //Debug.Log("We have saved pins");
        }


    }

    public static void LoadPinPositionsFromJSON()
    {
        if (System.IO.File.Exists(Application.persistentDataPath + "/PinData.json"))
        {
            string saved = System.IO.File.ReadAllText(Application.persistentDataPath + "/PinData.json");

            SaveData data = JsonUtility.FromJson<SaveData>(saved);

            RespawnToPreviousPositions.PinType[] savedPins = data.SavedPins;

            if (savedPins == null
                || savedPins.Length <= 0)
            {
                //Debug.Log("No pins to LOAD " + Time.time);
            }

            else
            {
                SavedPins = savedPins;
                //Debug.Log("We LOADed pins");
            }
        }
    }

    public static void SavePinPositionsToPlayerPrefs()
    {
        for (int i = 0; i < SavedPins.Length; i++)
        {
            RespawnToPreviousPositions.PinType pinType = SavedPins[i];

            // We are going to do this manually, if someone later messes with the enum with more pin types for an example
            int savedType = -1;

            switch (pinType)
            {
                case RespawnToPreviousPositions.PinType.None:
                    savedType = -1;
                    break;
                case RespawnToPreviousPositions.PinType.Minus2:
                    savedType = 0;
                    break;
                case RespawnToPreviousPositions.PinType.Minus5:
                    savedType = 1;
                    break;
                case RespawnToPreviousPositions.PinType.Plus1:
                    savedType = 2;
                    break;
                case RespawnToPreviousPositions.PinType.Plus2:
                    savedType = 3;
                    break;
                case RespawnToPreviousPositions.PinType.Plus5:
                    savedType = 4;
                    break;
                case RespawnToPreviousPositions.PinType.Plus10:
                    savedType = 5;
                    break;
                default:
                    savedType = -1;
                    break;
            }

            PlayerPrefs.SetInt("Pin " + i.ToString(), savedType);
            //Debug.Log(" Saved a pin pos " + " Pin " + i.ToString() + " " + savedType.ToString());
        }

        PlayerPrefs.Save();
        //Debug.Log("Should save playerprefs to disk ");
    }
    public static void LoadPinsFromPlayerPrefs()
    {

        for (int i = 0; i < SavedPins.Length; i++)
        {
            int savedType = -1;
            RespawnToPreviousPositions.PinType pinType = RespawnToPreviousPositions.PinType.None;

            if (PlayerPrefs.HasKey("Pin " + i.ToString()))
            {

                savedType = PlayerPrefs.GetInt("Pin " + i.ToString());
                //Debug.Log("Had a key " + "Pin " + i.ToString() + " " + savedType.ToString());
            }

            switch (savedType)
            {
                case -1:
                    pinType = RespawnToPreviousPositions.PinType.None;
                    break;

                case 0:
                    pinType = RespawnToPreviousPositions.PinType.Minus2;
                    break;

                case 1:
                    pinType = RespawnToPreviousPositions.PinType.Minus5;
                    break;

                case 2:
                    pinType = RespawnToPreviousPositions.PinType.Plus1;
                    break;

                case 3:
                    pinType = RespawnToPreviousPositions.PinType.Plus2;
                    break;

                case 4:
                    pinType = RespawnToPreviousPositions.PinType.Plus5;
                    break;

                case 5:
                    pinType = RespawnToPreviousPositions.PinType.Plus10;
                    break;
            }

            SavedPins[i] = pinType;

        }
    }

    public void OnTimerRanOut()
    {
        if (CurrentLevelIsCustomPositionsLevel)
        {
            TimerRanOutOnCustomPositionsScene();
        }

        else
        {
            if (CurrentEndlessPinPhaser != null)
            {
                if (CurrentGameMode == GameMode.TimeAttack) 
                {
                    CurrentEndlessPinPhaser.OnTimerRanOut();
                }

                else
                {
                    TimeRanOutOnLastScene();
                }
            }

            else
            {
                TimerRanOutOnNormalScene();
            }

        }
    }

    public void OnReadyToCloseTimeAttackSceneWin()
    {
        //SceneManager.LoadScene("GameOver_TimeAttackWin");

        LoadSceneForMiniGameMode("GameOver_TimeAttackWin");

        Debug.Log("Ready to close infinite scene");
    }

    public void OnReadyToCloseTimeAttackSceneLose()
    {
        //SceneManager.LoadScene("GameOver_TimeAttackLose");
        LoadSceneForMiniGameMode("GameOver_TimeAttackLose");
        Debug.Log("Ready to close infinite scene");
    }

    public void TimerRanOutOnCustomPositionsScene()
    {

        //Debug.Log("Timer ran out on custom positions scene");        
        //SceneManager.LoadScene("GameOver_CustomLose");
        LoadSceneForMiniGameMode("GameOver_CustomLose");
    }

    public void TimerRanOutOnNormalScene()
    {
        //Debug.Log("Timer ran out on normal scene");

        if (CurrentLevel <= MaxLevel) 
        {
            //SceneManager.LoadScene("GameOver_Lose");
            LoadSceneForMiniGameMode("GameOver_Lose");
        }

        else
        {
            TimeRanOutOnLastScene();
        }
    }

    public void TimeRanOutOnLastScene()
    {
        //SceneManager.LoadScene("GameOver_LastLevelCompleted");
        LoadSceneForMiniGameMode("GameOver_LastLevelCompleted");
    }

    public void OnReachedLastScene()
    {
        //SceneManager.LoadScene("GameOver_LastLevelReached");
        LoadSceneForMiniGameMode("GameOver_LastLevelReached");
    }

    public void AllPositivePinsToppledOverOnNormalScene()
    {
        //Debug.Log("All positive pins toppled over NORMAL scene. Current level is " + CurrentLevel);
        
        if (CurrentLevel <= MaxLevel) 
        {
            //SceneManager.LoadScene("GameOver_Win");
            LoadSceneForMiniGameMode("GameOver_Win");
        }

        else
        {
            OnReachedLastScene();
        }
    }

    public void AllPositivePinsToppledOverOnCustomScene()
    {
        //Debug.Log("All positive pins toppled over CUSTOM scene.");
        //SceneManager.LoadScene("GameOver_CustomWin");
        LoadSceneForMiniGameMode("GameOver_CustomWin");
    }

    public void AllPinsToppledOver()
    {
        if (CurrentLevelIsCustomPositionsLevel)
        {
            AllPositivePinsToppledOverOnCustomScene();
        }

        else
        {
            AllPositivePinsToppledOverOnNormalScene();
        }
    }

    public void LaunchNextLevel(int overrideLevel = -1)
    {
        if (overrideLevel >= 0)
        {
            CurrentLevel = overrideLevel;
        }

        //Debug.Log("SAved time is " + TimerCountdown.SavedTime);

        if (CurrentLevel > MaxLevel)
        {
            // Go to last level to convert time to points

            if (CurrentGameMode == GameMode.IronMan) 
            {
                TimerCountdown.secondsLeft = 0;
                TimerCountdown.AddTime(TimerCountdown.SavedTime);
            }

            OnBonusLevel = true;
            VasenTykki.kuuliaVasemmassaTykissä = 2147483646;
            OikeaTykki.kuuliaOikeassaTykissä = 2147483646;
            GoToEndlessRandom();
            return;
        }

        else
        {
            OnBonusLevel = false;
        }

        TimerCountdown.SaveTime();
        AddTimeAndAmmo();

        string levelName = "Level0";

        switch (CurrentLevel)
        {
            case 0:
                levelName = "Level0";
                break;

            case 1:
                levelName = "Level1";
                break;

            case 2:
                levelName = "Level2";
                break;

            case 3:
                levelName = "Level3";
                break;

            case 4:
                levelName = "Level4";
                break;

            case 5:
                levelName = "Level5";
                break;

            case 6:
                levelName = "Level6";
                break;

            case 7:
                levelName = "Level7";
                break;

            case 8:
                levelName = "Level8";
                break;

            case 9:
                levelName = "Level9";
                break;

            case 10:
                levelName = "Level10";
                break;

            case 11:
                levelName = "Level11";
                break;

            case 12:
                levelName = "Level12";
                break;

            case 13:
                levelName = "Level13";
                break;

            case 14:
                levelName = "Level14";
                break;

            case 15:
                levelName = "Level15";
                break;

            case 16:
                levelName = "Level16";
                break;

            case 17:
                levelName = "Level17";
                break;

            case 18:
                levelName = "Level18";
                break;

            case 19:
                levelName = "Level19";
                break;

            case 20:
                levelName = "Level20";
                break;

            case 21:
                levelName = "Level21";
                break;

            case 22:
                levelName = "Level22";
                break;

            case 23:
                levelName = "Level23";
                break;

            case 24:
                levelName = "Level24";
                break;

            case 25:
                levelName = "Level25";
                break;

            case 26:
                levelName = "Level26";
                break;

            case 27:
                levelName = "Level27";
                break;

            case 28:
                levelName = "Level28";
                break;

            case 29:
                levelName = "Level29";
                break;

            default:
                levelName = "Level0";
                break;
        }

        LoadSceneForMiniGameMode(levelName);
        CurrentLevel++;
    }

    private static void AddTimeAndAmmo()
    {
        switch (CurrentGameMode)
        {
            case GameMode.None:
                break;
            case GameMode.Easy:

                TimerCountdown.AddTime(75);

                VasenTykki.kuuliaVasemmassaTykissä += 20;
                OikeaTykki.kuuliaOikeassaTykissä += 20;
                break;

            case GameMode.Medium:
                TimerCountdown.AddTime(60);

                VasenTykki.kuuliaVasemmassaTykissä += 15;
                OikeaTykki.kuuliaOikeassaTykissä += 15;
                break;

            case GameMode.Hard:
                TimerCountdown.AddTime(60);

                VasenTykki.kuuliaVasemmassaTykissä += 15;
                OikeaTykki.kuuliaOikeassaTykissä += 15;
                break;

            case GameMode.IronMan:


                TimerCountdown.secondsLeft = 0;
                TimerCountdown.AddTime(60);

                VasenTykki.kuuliaVasemmassaTykissä = 15;
                OikeaTykki.kuuliaOikeassaTykissä = 15;
                break;

            case GameMode.TimeAttack:
                break;

            default:
                break;
        }


    }

    public void GoToGameModeMenu()
    {
        LoadSceneForMiniGameMode("GameModeMenu");
    }

    public void LaunchNewGame()
    {
        //TimerCountdown.AddTime(60);

        TimerCountdown.secondsLeft = 0;
        TimerCountdown.SavedTime = 0;
        PisteLaskuri.pisteet = 0;
        VasenTykki.kuuliaVasemmassaTykissä = 0;
        OikeaTykki.kuuliaOikeassaTykissä = 0;

        LaunchNextLevel(0);
    }

    public void LaunchCustomPositionsLevel()
    {
        TimerCountdown.secondsLeft = 0;
        TimerCountdown.AddTime(60);
        PisteLaskuri.pisteet = 0;
        VasenTykki.kuuliaVasemmassaTykissä = 10;
        OikeaTykki.kuuliaOikeassaTykissä = 10;

        //SceneManager.LoadScene("OmaValintaPeli");
        LoadSceneForMiniGameMode("OmaValintaPeli");
    }

    public void GoToCredits()
    {
        //SceneManager.LoadScene("Credits");
        LoadSceneForMiniGameMode("Credits");
    }

    public void GoToMainMenu()
    {
        bool alreadyInMenu = false;

        int sceneCount = SceneManager.sceneCount;

        //Scene[] scenes = SceneManager.GetAllScenes();

        for (int i = 0; i < sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name.Equals("Menu"))
            {
                alreadyInMenu = true;
            }
        }
        //SceneManager.LoadScene("Menu");

        if (!alreadyInMenu) 
        {
            LoadSceneForMiniGameMode("Menu");
        }

        else
        {
            Debug.Log("Already in menu");
        }
    }

    public void GoToEndlessRandom()
    {
        //SceneManager.LoadScene("LevelEndlessRandom");
        LoadSceneForMiniGameMode("LevelEndlessRandom");
    }

    public void StartTimeAttack()
    {
        //Debug.Log("Starting time attack");
        PisteLaskuri.pisteet = 0;
        VasenTykki.kuuliaVasemmassaTykissä = 2147483646;
        OikeaTykki.kuuliaOikeassaTykissä = 2147483646;
        TimerCountdown.secondsLeft = 0;
        TimerCountdown.AddTime(120);
        GoToEndlessRandom();
    }

    public static void SetGameMode(GameMode mode)
    {
        CurrentGameMode = mode;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    //Debug.Log("Skip level " + (CurrentLevel - 1).ToString());
        //    LaunchNextLevel();
        //}

        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    GoToMainMenu();
        //}

        //if (Inputs.escOptions)
        //{
        //    Debug.Log("Pressed esc");
        //    GoToMainMenu();
        //    Inputs.ClearEscOptions();
        //}

        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    TimerCountdown.secondsLeft -= 100;
        //}
        //Debug.Log("Current game mode is " + CurrentGameMode.ToString());
    }

    public void LoadSceneForMiniGameMode(string sceneName)
    {
        //SceneLoader.Instance.UnloadScene();
        //SceneLoader.Instance.LoadSceneByName(sceneName, new SceneLoadParams(transform.position, transform.rotation, ScenePackMode.ALL));

        SceneLoader.Instance.SwitchSubScenes(sceneName);


        //SceneLoader.Instance.LoadSceneByName(sceneName, new SceneLoadParams(ScenePackMode.ALL, null));

        //MiniGameLauncher.Instance.UnloadActiveScene();
        //MiniGameLauncher.Instance.SaveActiveSceneName(sceneName);
        //SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }
}
