using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using UnityEngine.SceneManagement;
using Scenes;
using FishNet.Object.Synchronizing;

public class SceneMover : NetworkBehaviour
{

    private Scene incomingScene;

    private string incomingSceneName;

    [SyncVar(OnChange = nameof(OnSceneSyncVarBeingUpdated))]
    private string SceneWeShouldBeIn;


    List<Scenes.CachedGameObject> gameObjects = new List<CachedGameObject>();

    [SyncVar(OnChange = nameof(OnAnotherSceneSyncVarBeingUpdated))]
    private string OwnerSceneMoverScene;

    private void Awake()
    {
        Transform[] children = GetComponentsInChildren<Transform>();

        for (int i = 0; i < children.Length; i++)
        {
            gameObjects.Add(new CachedGameObject(children[i].gameObject, children[i].gameObject.activeSelf));
        }

        ///UnityEngine.SceneManagement.SceneManager.sceneLoaded -= DoTheActualSwitch;
        //UnityEngine.SceneManagement.SceneManager.sceneLoaded += DoTheActualSwitch;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= GoGetSceneFromServer;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += GoGetSceneFromServer;

    }

    private void OnAnotherSceneSyncVarBeingUpdated(string prev, string next, bool asServer) 
    {
        //if (OwnerSceneMoverScene.Equals(SceneWeShouldBeIn))
        //{
        //    Debug.Log("We are on the same scene as the owner");
        //    //return true;
        //}

        //else
        //{
        //    Debug.Log("We are NOT on the same scene as the owner");
        //    //return false;
        //}
    }

    private void OnSceneSyncVarBeingUpdated(string prev, string next, bool asServer)
    {
        Debug.Log("Scene sync var just got updated. The new value is " + next);


        Scene byName = UnityEngine.SceneManagement.SceneManager.GetSceneByName(next);

        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, byName);
        
        //bool inTheSameSceneAsMainPlayer = false;

        //inTheSameSceneAsMainPlayer = DetermineIfWeAreOnTheSameSceneAsMainPlayer();


        //if (inTheSameSceneAsMainPlayer)
        //{
        //    SceneLoader.Instance.UnpackNonPlayerPlayer(gameObjects);
        //}

        //else
        //{
        //    SceneLoader.Instance.PackNonPlayerPlayer(GetComponentsInChildren<Transform>());
        //}



        SceneLoader.Instance.UnpackNonPlayerPlayer(gameObjects);
    }

    public void GoGetSceneFromServer(Scene scene, LoadSceneMode loadSceneMode)
    {
        GetSceneOnServer(scene, loadSceneMode);
    }

    [ServerRpc]
    public void GetSceneOnServer(Scene scene, LoadSceneMode loadSceneMode)
    {
        SceneWeShouldBeIn = gameObject.scene.name;

        Debug.LogError("Server should update syncvar about which scene we should be in " + SceneWeShouldBeIn);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetSceneWeShouldBeIn(string sceneName)
    {
        SceneWeShouldBeIn = sceneName;

        Debug.Log("Scene we should be in is " + SceneWeShouldBeIn);
    }

    [ServerRpc]
    public void SetOwnerSceneMoverScene(string sceneName)
    {
        OwnerSceneMoverScene = sceneName;
    }

    private void DoTheActualSwitch(Scene scene, LoadSceneMode mode)
    {
        SwitchBySceneName();

        Debug.Log("Loaded scene " + scene.name);

        int index = UnityEngine.SceneManagement.SceneManager.sceneCount;

        for (int i = 0; i < index; i++)
        {
            UnityEngine.SceneManagement.Scene scene2 = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
            Debug.Log("Loaded scene at " + i + " is at the moment of scene loaded " + scene2.name );
        }


        Scene byName = UnityEngine.SceneManagement.SceneManager.GetSceneByName(incomingScene.name);

        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, byName);
    }

    private void SwitchBySceneName()
    {
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, UnityEngine.SceneManagement.SceneManager.GetSceneByName(incomingSceneName));
    }





    // Only called on the connections, not on all instances everywhere.
    // So, make it so, that all the instances get a call 

    [TargetRpc]
    public void MoveCharacterToScene(NetworkConnection connection, 
                                     UnityEngine.SceneManagement.Scene sceneToMoveTo,
                                     string sceneNameAsString)
    {
        if (IsOwner)
        {
            SetOwnerSceneMoverScene(sceneToMoveTo.name);
        }

        Debug.Log("Scene to move to is " + sceneNameAsString);

        int index = UnityEngine.SceneManagement.SceneManager.sceneCount;

        Scene sceneToLoad = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneNameAsString);

        for (int i = 0; i < index; i++)
        {
            UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
            Debug.Log("Loaded scene at " + i + " is " + scene.name);
        }

        Debug.Log("Active scene is " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

        incomingScene = sceneToMoveTo;

        //NetworkSceneLoader.Instance.ServerManager.Spawn(gameObject, connection, sceneToMoveTo);
        //UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        Debug.Log("Moving to scene " + sceneToLoad.name + " network id is " + connection.ClientId + "gameobject name is " + gameObject.name);
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, sceneToLoad);

        OwnerSceneMoverScene = sceneToMoveTo.name;
    }

    public void MoveCharacterToSceneSceen(
                                 UnityEngine.SceneManagement.Scene sceneToMoveTo,
                                 string sceneNameAsString)
    {
        Debug.Log("Scene to move to is " + sceneNameAsString);

        int index = UnityEngine.SceneManagement.SceneManager.sceneCount;

        Scene sceneToLoad = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneNameAsString);

        for (int i = 0; i < index; i++)
        {
            UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
            Debug.Log("Loaded scene at " + i + " is " + scene.name);
        }

        Debug.Log("Active scene is " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

        incomingScene = sceneToMoveTo;

        //NetworkSceneLoader.Instance.ServerManager.Spawn(gameObject, connection, sceneToMoveTo);
        //UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        Debug.Log("Moving to scene " + sceneToLoad.name + "gameobject name is " + gameObject.name);
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, sceneToLoad);


    }

    [TargetRpc]
    public void SaveSceneNameAsString(NetworkConnection connection, string sceneName)
    {
        incomingSceneName = sceneName;

        Debug.Log("incoming scene name as string is " + sceneName);
    }


    public void OnActiveSceneChanged()
    {

    }

    public bool DetermineIfWeAreOnTheSameSceneAsMainPlayer()
    {
        if (OwnerSceneMoverScene.Equals(SceneWeShouldBeIn))
        {
            return true;
        }

        else
        {
            return false;
        }
    }

}
