using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scenes;

public class NonNetworkLauncher : MonoBehaviour
{
    [SerializeField] ScenePicker mainScenePicker;
    public void Awake()
    {
        Debug.Log("--- Non network init started ---");
        //InstanceFinder.ServerManager.OnServerConnectionState += OnServerStarted;
        //InstanceFinder.SceneManager.OnLoadEnd += SceneManager_OnLoadEnd;

        LoadMainScene();
    }

    //void OnServerStarted(ServerConnectionStateArgs args)
    //{
    //    if (args.ConnectionState == LocalConnectionState.Started)
    //    {
    //        LoadMainScene();
    //    }
    //}

    //private void SceneManager_OnLoadEnd(SceneLoadEndEventArgs args)
    //{
    //    if (!InstanceFinder.NetworkManager.IsHost && InstanceFinder.NetworkManager.IsClient)
    //    {
    //        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("NetworkLauncher");
    //    }
    //}

    void LoadMainScene()
    {
        string mainSceneName = mainScenePicker.GetSceneName();
        //SceneLoadData sld = new SceneLoadData(mainSceneName);
        //InstanceFinder.SceneManager.LoadGlobalScenes(sld);

        ////unload launch scene as it's no longer needed
        //InstanceFinder.SceneManager.UnloadConnectionScenes(new SceneUnloadData("NetworkLauncher"));

        UnityEngine.SceneManagement.SceneManager.LoadScene(mainSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
        Debug.Log("--- Main scene loaded ---");
    }
}
