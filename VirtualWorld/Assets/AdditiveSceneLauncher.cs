using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class AdditiveSceneLauncher : NetworkBehaviour
{

    public PlayerCharacter character;
    public static AdditiveSceneLauncher Instance;

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {


        if (IsOwner) 
        {        
            character = GetComponentInChildren<PlayerCharacter>(true);
            MiniGameLauncher.Instance.SetCharacter(character);
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();


        if (!IsHost
            && !IsServer) 
        {        
            Debug.Log("Owner client id is " + OwnerClientId);

            MiniGameLauncher.Instance.CheckIfServerIsPlayingMiniGameServerRpc(OwnerClientId);
        }
    }

    [ClientRpc]
    public void UnloadMiniGameSceneClientRpc(string sceneName,
                                             ulong clientId)
    {

        if (!IsServer
            && !IsHost
            && clientId == OwnerClientId
            )
        {
            Debug.LogError("Cleint rpc called. Client id is " + clientId + " OwnerClientId is " + OwnerClientId);

            MiniGameLauncher.Instance.GoBackToPlayground(false);
            SceneManager.UnloadSceneAsync(sceneName);
            Debug.LogError("Should have unloaded scene async");
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (!IsOwner)
        {
            return;
        }

        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    //SetSceneServerRpc(1);
        //    SetScene(1);
        //}

        //else if (Input.GetKeyDown(KeyCode.P))
        //{
        //    //SetSceneServerRpc(2);
        //    SetScene(2);
        //}

        //else if (Input.GetKeyDown(KeyCode.I))
        //{
        //    //SetSceneServerRpc(3);
        //    SetScene(3);
        //}
    }

    [ServerRpc]
    public void SetSceneServerRpc(int scene)
    {
        if (scene == 1)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("Test1", LoadSceneMode.Additive);

        }

        else if (scene == 2)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("Test2", LoadSceneMode.Additive);
        }

        else if (scene == 3)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("Test3", LoadSceneMode.Additive);
        }
    }

    public void SetScene(int scene)
    {
        Debug.Log("Should set scene to " + scene);

        PlaygroundScene playground = FindObjectOfType<PlaygroundScene>(true);
        playground.DisablePlayground();

        character.DisableCharacter();
        //DontDestroyOnLoad(character.transform.parent);

        //MiniGameLauncher.Instance.SetSkyboxToDefaultSkybox();
        MiniGameLauncher.Instance.StartPlayingMiniGame();

        if (scene == 1)
        {
            MiniGameLauncher.Instance.SetupSceneForTableTopInvaders();

            MiniGameLauncher.Instance.SaveActiveSceneName("Menu");
            SceneManager.LoadScene("Menu", LoadSceneMode.Additive);

            Debug.Log("TAbletop invaders load called");
            //SceneManager.LoadScene("Menu", LoadSceneMode.Single);
            //SceneManager.SetActiveScene(SceneManager.GetSceneByName("Test1"));

        }

        else if (scene == 2)
        {
            MiniGameLauncher.Instance.SetSceneForGravityShip();
            MiniGameLauncher.Instance.SaveActiveSceneName("GravityShip_TitleScreen");
            SceneManager.LoadScene("GravityShip_TitleScreen", LoadSceneMode.Additive);

            Debug.Log("Gravity ship load called");
            //SceneManager.LoadScene("Menu", LoadSceneMode.Single);
            //SceneManager.SetActiveScene(SceneManager.GetSceneByName("Test2"));
        }

        else if (scene == 3)
        {            
            MiniGameLauncher.Instance.SaveActiveSceneName("Menu");
            SceneManager.LoadScene("Menu", LoadSceneMode.Additive);

            Debug.Log("Menu load called");
            //SceneManager.LoadScene("Menu", LoadSceneMode.Single);
            //SceneManager.SetActiveScene(SceneManager.GetSceneByName("Test3"));
        }
    }
}