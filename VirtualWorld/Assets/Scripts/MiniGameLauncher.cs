using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class MiniGameLauncher : NetworkBehaviour
{
    public static MiniGameLauncher Instance;
    public PlayerCharacter Character;

    public string ActiveSceneName;

    public int FramesPassedSinceLoadRequest;
    public bool WaitingToLoad;

    public Material TabletopInvadersSkyboxMat;
    public Material GravityShipSkyboxMat;
    public Material PlaygroundSkyBoxMat;

    public LightingSettings LightingSettingsGravityShip;
    public LightingSettings LightingSettingsPlayground;
    public LightingSettings LightingSettingsTabletopInvaders;

    public bool IsPlayingMinigame;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);      
    }

    //public void StartUnloadingActiveScene()
    //{
    //    FramesPassedSinceLoadRequest = 0;
    //}

    public void SetCharacter(PlayerCharacter character)
    {
        Character = character;
    }

    private void Update()       
    {
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    GoBackToPlayground(true);
        //}

        //if (WaitingToLoad)
        //{
        //    return;

        //    FramesPassedSinceLoadRequest++;

        //    if (FramesPassedSinceLoadRequest >= 2)
        //    {
        //        Scene active = SceneManager.GetSceneByName(ActiveSceneName);
        //        SceneManager.SetActiveScene(active);
        //    }
        //}
    }


    public void StartPlayingMiniGame()
    {
        //MapBlip[] blips = FindObjectsOfType<MapBlip>();

        //for (int i = 0; i < blips.Length; i++)
        //{
        //    blips[i].DisableBlips();
        //}

        //PlayerEnablerDisabler[] players = FindObjectsOfType<PlayerEnablerDisabler>(true);

        //for (int i = 0; i < players.Length; i++)
        //{
        //    players[i].DisablePlayer();
        //}

        //PlayerCharacter[] allPlayers = FindObjectsOfType<PlayerCharacter>(true);

        //for (int i = 0; i < allPlayers.Length; i++)
        //{
        //    allPlayers[i].DisableCharacter();
        //}


        IsPlayingMinigame = true;
    }

    public void StopPlayingMinigame()
    {
        if (GameFlowManager.Instance != null)
        {
            DestroyImmediate(GameFlowManager.Instance.gameObject);
        }

        if (GameManagerGravityShip.Instance != null)
        {
            DestroyImmediate(GameManagerGravityShip.Instance.gameObject);
        }
    }

    //public void GoBackToPlayground(bool unloadScene)
    //{
    //    if (GameFlowManager.Instance != null)
    //    {
    //        DestroyImmediate(GameFlowManager.Instance.gameObject);
    //    }

    //    if (GameManagerGravityShip.Instance != null)
    //    {
    //        DestroyImmediate(GameManagerGravityShip.Instance.gameObject);
    //    }

    //    IsPlayingMinigame = false;


    //    Character.EnableCharacter();


    //    //PlayerCharacter[] allCharacters = FindObjectsOfType<PlayerCharacter>(true);

    //    //for (int i = 0; i < allCharacters.Length; i++)
    //    //{
    //    //    allCharacters[i].EnableCharacter();
    //    //}

    //    if (unloadScene) 
    //    {
    //        UnloadActiveScene();
    //    }

    //    FindObjectOfType<PlaygroundScene>(true).gameObject.SetActive(true);
    //    RenderSettings.skybox = PlaygroundSkyBoxMat;
    //    DynamicGI.UpdateEnvironment();

    //    //MapBlip[] blips = FindObjectsOfType<MapBlip>();

    //    //for (int i = 0; i < blips.Length; i++)
    //    //{
    //    //    blips[i].ReEnableBlips();
    //    //}

    //    //PlayerCharacter[] allPlayers = FindObjectsOfType<PlayerCharacter>(true);

    //    //for (int i = 0; i < allPlayers.Length; i++)
    //    //{
    //    //    allPlayers[i].EnableCharacter();
    //    //}
    //    //Scene active = SceneManager.GetSceneByName("Playground");
    //    //SceneManager.SetActiveScene(active);
    //    //SceneManager.LoadScene("Playground", LoadSceneMode.Single);
    //}

    //public void UnloadActiveScene()
    //{        
    //    Debug.Log("Unloading scene " + ActiveSceneName);
    //    if(ActiveSceneName != "")
    //    {
    //        SceneManager.UnloadSceneAsync(ActiveSceneName);
    //    }
    //}

    public void SaveActiveSceneName(string sceneName)
    {
        ActiveSceneName = sceneName;
        WaitingToLoad = true;
        FramesPassedSinceLoadRequest = 0;
    }

    //public void SetupSceneForTableTopInvaders()
    //{
    //    MiniGameLight.Instance.TurnOnMiniGameLight(ArcadeCabinetTrigger.CabinetType.TabletopInvaders);
    //    PlaygroundSkyBoxMat = RenderSettings.skybox;
    //    RenderSettings.skybox = TabletopInvadersSkyboxMat;
    //    DynamicGI.UpdateEnvironment(); // Do we need to even call this. Does it have an effect on anything?
    //}

    //public void SetSceneForGravityShip()
    //{
    //    MiniGameLight.Instance.TurnOnMiniGameLight(ArcadeCabinetTrigger.CabinetType.GravityShip);
    //    PlaygroundSkyBoxMat = RenderSettings.skybox;
    //    RenderSettings.skybox = GravityShipSkyboxMat;
    //    DynamicGI.UpdateEnvironment(); // Do we need to even call this. Does it have an effect on anything?
    //}    
    
    
    //public void SetupSceneForTableTopInvaders()
    //{
    //    MiniGameLight.Instance.TurnOnMiniGameLight(ArcadeCabinetTrigger.CabinetType.TabletopInvaders);
    //    PlaygroundSkyBoxMat = RenderSettings.skybox;
    //    RenderSettings.skybox = TabletopInvadersSkyboxMat;
    //    DynamicGI.UpdateEnvironment(); // Do we need to even call this. Does it have an effect on anything?
    //}

    //public void SetSceneForGravityShip()
    //{
    //    MiniGameLight.Instance.TurnOnMiniGameLight(ArcadeCabinetTrigger.CabinetType.GravityShip);
    //    PlaygroundSkyBoxMat = RenderSettings.skybox;
    //    RenderSettings.skybox = GravityShipSkyboxMat;
    //    DynamicGI.UpdateEnvironment(); // Do we need to even call this. Does it have an effect on anything?
    //}

    //[ServerRpc(RequireOwnership = false)]
    //public void CheckIfServerIsPlayingMiniGameServerRpc(ulong clientId)
    //{

    //    Debug.Log("RPC called. client id is" + clientId);

    //    if ((IsServer 
    //        || IsHost)
    //        && IsPlayingMinigame)
    //    {
    //        Debug.Log("Got through the if checks");

    //        string activeSceneName = SceneManager.GetActiveScene().name;
    //        Debug.Log("Activescene name is " + activeSceneName);
    //        AdditiveSceneLauncher.Instance.UnloadMiniGameSceneClientRpc(activeSceneName, clientId);            
    //    }
    //}
}
