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

    public Material SkyboxMat;
    public Material PlaygroundSkyBoxMat;

    public bool IsPlayingMinigame;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsServer
            && !IsHost)
        {
            //Debug.LogError("On network spawn called on checking if serve is playing minigame. Owner client id is " + OwnerClientId);
            //CheckIfServerIsPlayingMiniGameServerRpc(OwnerClientId);

        }
    }

    public void Start()
    {
        if (!IsServer
           && !IsHost)
        {
            //Debug.LogError("Start called on checking if serve is playing minigame. Owner client id is " + OwnerClientId);
            //CheckIfServerIsPlayingMiniGameServerRpc(OwnerClientId);

        }
    }

    public void StartUnloadingActiveScene()
    {
        FramesPassedSinceLoadRequest = 0;
    }

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

        if (WaitingToLoad)
        {
            return;

            FramesPassedSinceLoadRequest++;

            if (FramesPassedSinceLoadRequest >= 2)
            {
                Scene active = SceneManager.GetSceneByName(ActiveSceneName);
                SceneManager.SetActiveScene(active);
            }
        }
    }


    public void StartPlayingMiniGame()
    {
        IsPlayingMinigame = true;
    }

    public void GoBackToPlayground(bool unloadScene)
    {
        if (GameFlowManager.Instance != null)
        {
            DestroyImmediate(GameFlowManager.Instance.gameObject);
        }

        IsPlayingMinigame = false;
        Character.EnableCharacter();


        //PlayerCharacter[] allCharacters = FindObjectsOfType<PlayerCharacter>(true);

        //for (int i = 0; i < allCharacters.Length; i++)
        //{
        //    allCharacters[i].EnableCharacter();
        //}
        
        if (unloadScene) 
        {
            UnloadActiveScene();
        }
        FindObjectOfType<PlaygroundScene>(true).gameObject.SetActive(true);
        RenderSettings.skybox = PlaygroundSkyBoxMat;
        DynamicGI.UpdateEnvironment();
        //Scene active = SceneManager.GetSceneByName("Playground");
        //SceneManager.SetActiveScene(active);
        //SceneManager.LoadScene("Playground", LoadSceneMode.Single);
    }

    public void UnloadActiveScene()
    {        
        Debug.Log("Unloading scene " + ActiveSceneName);
        SceneManager.UnloadSceneAsync(ActiveSceneName);

    }

    public void SaveActiveSceneName(string sceneName)
    {
        ActiveSceneName = sceneName;
        WaitingToLoad = true;
        FramesPassedSinceLoadRequest = 0;
    }

    public void SetSkyboxToDefaultSkybox()
    {
        PlaygroundSkyBoxMat = RenderSettings.skybox;
        RenderSettings.skybox = SkyboxMat;
        DynamicGI.UpdateEnvironment();
    }

    [ServerRpc(RequireOwnership = false)]
    public void CheckIfServerIsPlayingMiniGameServerRpc(ulong clientId)
    {
        Debug.LogError("RPC called. client id is" + clientId);

        if ((IsServer 
            || IsHost)
            && IsPlayingMinigame)
        {
            Debug.LogError("Got through the if checks");
            AdditiveSceneLauncher.Instance.UnloadMiniGameSceneClientRpc(ActiveSceneName, clientId);            
        }
    }
}
