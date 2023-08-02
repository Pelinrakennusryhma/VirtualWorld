using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TestHostClientServerStarter : MonoBehaviour
{
    //private string TestSceneName = "Test1";
    private string TestSceneName = "Playground";

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        //NetworkSceneManager.ActiveSceneSynchronizationEnabled = false;


        Debug.Log("Test scene name is " + TestSceneName);
        NetworkManager.Singleton.SceneManager.LoadScene(TestSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);        
        //NetworkManager.Singleton.SceneManager.ActiveSceneSynchronizationEnabled = false;
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
        NetworkManager.Singleton.SceneManager.LoadScene(TestSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
        //NetworkManager.Singleton.SceneManager.ActiveSceneSynchronizationEnabled = false;
    }
}
