using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TestHostClientServerStarter : MonoBehaviour
{
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        //NetworkSceneManager.ActiveSceneSynchronizationEnabled = false;

        NetworkManager.Singleton.SceneManager.LoadScene("Test1", UnityEngine.SceneManagement.LoadSceneMode.Single);        
        //NetworkManager.Singleton.SceneManager.ActiveSceneSynchronizationEnabled = false;
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
        NetworkManager.Singleton.SceneManager.LoadScene("Test1", UnityEngine.SceneManagement.LoadSceneMode.Single);
        //NetworkManager.Singleton.SceneManager.ActiveSceneSynchronizationEnabled = false;
    }
}
