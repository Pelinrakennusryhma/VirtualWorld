using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using UnityEngine.SceneManagement;
using Scenes;
using FishNet.Object.Synchronizing;
using Characters;

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
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnActiveSceneChanged;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnActiveSceneChanged;

        CharacterManager.Instance.AssSceneMover(this);
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
        //Debug.Log("SCENE SYNC VAR BEING UPDATED. Scene sync var just got updated. The new value is " + next);

        if (SceneLoader.Instance.sceneLoadParams.scenePackMode == ScenePackMode.ALL
            && SceneLoader.Instance.WeAreInMiniScene)
        {
            Debug.LogError("Dont mess with packing right now, because everything should be packed away already");
            return;
        }


        SceneMover[] allMovers = FindObjectsOfType<SceneMover>(true);
        allMovers = CharacterManager.Instance.sciinMuuvers.ToArray();

        //if (IsOwner)
        //{
        //    return;
        //}

        Scene byName = UnityEngine.SceneManagement.SceneManager.GetSceneByName(next);

        if (byName.IsValid())
        {
            UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, byName);
            //Debug.LogError("WValid scene. Move gameobject");
        }

        else
        {
            //Debug.LogError("We have an invalid scene. Cant move gameobject");
        }

        for (int i = 0; i < allMovers.Length; i++) 
        {
            //Debug.LogError("Scene mover scene is " + allMovers[i].gameObject.scene.name);

            bool inTheSameSceneAsMainPlayer = false;

            if (allMovers[i].gameObject.scene.name.Equals(next))
            {
                inTheSameSceneAsMainPlayer = true;
            }


            if (inTheSameSceneAsMainPlayer)
            {
                SceneLoader.Instance.UnpackNonPlayerPlayer(gameObjects);

                //Debug.Log("Should unpack nonplayer player");
            }

            else
            {
                //SceneLoader.Instance.PackNonPlayerPlayer(GetComponentsInChildren<Transform>());

                //allMovers[i].gameObject.transform.position = new Vector3(-3333, -3333, -3333);

                //Debug.LogError("Should have moved to hevon vittu");
            }
        }




        incomingSceneName = next;
        incomingScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(next);
        //SceneLoader.Instance.PackNonPlayerPlayer(GetComponentsInChildren<Transform>());

        if (gameObject.scene.name.Equals("MovedObjectsHolder"))
        {
            //Debug.LogError("We ended up being in moved objects holder");
            gameObject.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(DelayedSceneMovement());
        }


        List<StarterAssets.ThirdPersonController> thirds = new List<StarterAssets.ThirdPersonController>();

        for (int i = 0; i < allMovers.Length; i++)
        {
            thirds.Add(allMovers[i].GetComponentInChildren<StarterAssets.ThirdPersonController>(true));
        }

        for (int i = 0; i < thirds.Count; i++)
        {
            thirds[i].MoveSlightly();
        }

        TellTheOwnerToMove();
        //SceneLoader.Instance.UnpackNonPlayerPlayer(gameObjects);
    }

    public void GoGetSceneFromServer(Scene scene, LoadSceneMode loadSceneMode)
    {
        GetSceneOnServer(scene, loadSceneMode);
    }

    [ServerRpc]
    public void GetSceneOnServer(Scene scene, LoadSceneMode loadSceneMode)
    {
        SceneWeShouldBeIn = gameObject.scene.name;

        //Debug.LogError("Server should update syncvar about which scene we should be in " + SceneWeShouldBeIn);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetSceneWeShouldBeIn(string sceneName)
    {
        SceneWeShouldBeIn = sceneName;

        //Debug.Log("Scene we should be in is " + SceneWeShouldBeIn);
    }

    public void SetSceneWeShouldBeInNonNetworked(string sceneName)
    {
        SceneWeShouldBeIn = sceneName;

        //Debug.Log("Scene we should be in is " + SceneWeShouldBeIn);
    }

    [ServerRpc]
    public void SetOwnerSceneMoverScene(string sceneName)
    {
        OwnerSceneMoverScene = sceneName;
    }

    private void DoTheActualSwitch(Scene scene, LoadSceneMode mode)
    {
        SwitchBySceneName();

        //Debug.Log("Loaded scene " + scene.name);

        int index = UnityEngine.SceneManagement.SceneManager.sceneCount;

        for (int i = 0; i < index; i++)
        {
            UnityEngine.SceneManagement.Scene scene2 = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
            //Debug.Log("Loaded scene at " + i + " is at the moment of scene loaded " + scene2.name );
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
            //Debug.LogError("Is owner is called ");

            SetOwnerSceneMoverScene(sceneToMoveTo.name);
        }

        //Debug.Log("Scene to move to is " + sceneNameAsString);

        int index = UnityEngine.SceneManagement.SceneManager.sceneCount;

        Scene sceneToLoad = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneNameAsString);

        for (int i = 0; i < index; i++)
        {
            UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
            //Debug.Log("Loaded scene at " + i + " is " + scene.name);
        }

        //Debug.Log("Active scene is " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

        incomingScene = sceneToMoveTo;

        //NetworkSceneLoader.Instance.ServerManager.Spawn(gameObject, connection, sceneToMoveTo);
        //UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, sceneToLoad);
        //Debug.Log("Moving to scene " + sceneToLoad.name + " network id is " + connection.ClientId + "gameobject name is " + gameObject.name + " active scene is " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);



        //UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, sceneToLoad);



        OwnerSceneMoverScene = sceneToMoveTo.name;
    }

    public void MoveCharacterToSceneSceen(
                                 UnityEngine.SceneManagement.Scene sceneToMoveTo,
                                 string sceneNameAsString)
    {
        //Debug.Log("Scene to move to is " + sceneNameAsString);

        int index = UnityEngine.SceneManagement.SceneManager.sceneCount;

        Scene sceneToLoad = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneNameAsString);

        for (int i = 0; i < index; i++)
        {
            UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
            //Debug.Log("Loaded scene at " + i + " is " + scene.name);
        }

        //Debug.Log("Active scene is " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

        incomingScene = sceneToMoveTo;

        //NetworkSceneLoader.Instance.ServerManager.Spawn(gameObject, connection, sceneToMoveTo);
        //UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        //Debug.Log("Moving to scene " + sceneToLoad.name + "gameobject name is " + gameObject.name);
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, sceneToLoad);


    }

    [TargetRpc]
    public void SaveSceneNameAsString(NetworkConnection connection, string sceneName)
    {
        incomingSceneName = sceneName;

        //Debug.Log("incoming scene name as string is " + sceneName);
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

    public void MakeSureTheObjectDoesNotGetDestroyed(GameObject objectToMove)
    {
        if (!IsServer) 
        {
            SceneLoader.Instance.PackNonPlayerPlayer(GetComponentsInChildren<Transform>());
        Scene mainScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName("Playground");
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(objectToMove, mainScene);

        //objectToMove.transform.position = new Vector3(-3333, -3333, -3333);

        //objectToMove.gameObject.SetActive(false);

        //Debug.LogError("Making sure the object does not get desotryed");
    }

    }

    [TargetRpc]
    public void PackNonPlayerPlayer(NetworkConnection connection)
    {
        if (!IsServer) { 
        SceneLoader.Instance.PackNonPlayerPlayer(GetComponentsInChildren<Transform>());
    }
    }

    [TargetRpc]

    public void UnpackNonPlayerPlayer(NetworkConnection connection)
    {
        if (!IsServer)
        {
            SceneLoader.Instance.UnpackNonPlayerPlayer(gameObjects);
        }
    }


    public void OnActiveSceneSet(NetworkConnection connection, Scene ownerScene)
    {
        //Debug.LogError("On active scene set. Owner scene name is " + ownerScene.name) ;

        if (gameObject.scene.name.Equals(ownerScene.name))
        {
            UnpackNonPlayerPlayer(connection);
        }

    }

    public void OnActiveSceneChanged(UnityEngine.SceneManagement.Scene scene,
                                     UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        //Debug.LogError("Evento of scene change is fired. SCENE NAME IS " + scene.name);

        if (string.IsNullOrEmpty(scene.name))
        {
            //Debug.LogError("Null or empty scene name. SCENE NAME IS " + scene.name);
            return;
        }



        if (!IsServer
            && !IsOwner) 
        {

            if (scene.IsValid()
                && gameObject.scene.name.Equals(scene.name))
            {
                SceneLoader.Instance.UnpackNonPlayerPlayer(gameObjects);
                //Debug.LogError("Names match. Unpack");
            }

            else
            {

                SceneLoader.Instance.PackNonPlayerPlayer(GetComponentsInChildren<Transform>());
                //UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, UnityEngine.SceneManagement.SceneManager.GetSceneByName("Playground"));
                //Debug.LogError("Names do not match. Pack");

                if (scene.IsValid())
                {
                    //UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, UnityEngine.SceneManagement.SceneManager.GetSceneByName("Playground"));
                }
            }
        }

        //UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, incomingScene);
        //Debug.LogError("The gameobject just moved to scene " + gameObject.scene.name);

        //UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, UnityEngine.SceneManagement.SceneManager.GetSceneByName(incomingSceneName));
    }

    public void DecideThisAndThat(UnityEngine.SceneManagement.Scene scene)
    {
        //OnActiveSceneChanged(scene, LoadSceneMode.Additive);

        if (string.IsNullOrEmpty(scene.name))
        {
            //Debug.LogError("Null or empty scene name");
            return;
        }

        if (scene.IsValid()
            && scene.name.Equals(incomingScene.name)) 
        {
            UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, UnityEngine.SceneManagement.SceneManager.GetSceneByName(scene.name));
        }

        else
        {
            UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, UnityEngine.SceneManagement.SceneManager.GetSceneByName("MovedObjectsHolder"));
            //Debug.LogError("Invalid scene. Moving to moved objects holder");
            StopAllCoroutines();
            gameObject.SetActive(true);
            StartCoroutine(DelayedSceneMovement());
        }

        if (scene.name.Equals(SceneWeShouldBeIn))
        {
            UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, scene);
        }

        OnActiveSceneChanged(scene, LoadSceneMode.Single);

        if (gameObject.scene.name.Equals(scene.name))
        {
            SceneLoader.Instance.UnpackNonPlayerPlayer(gameObjects);
            //Debug.LogError("Names match. Unpack");
        }

        else
        {

            SceneLoader.Instance.PackNonPlayerPlayer(GetComponentsInChildren<Transform>());
            //UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, UnityEngine.SceneManagement.SceneManager.GetSceneByName("Playground"));
            //Debug.LogError("Names do not match. Pack. Gameobject scene name is " + gameObject.scene.name + " scene name is " + scene.name);


        }

        if (gameObject.scene.name.Equals("MovedObjectsHolder"))
        {
            //Debug.LogError("We ended up being in MovedObjectsHolder. Do the delayed moving");
            gameObject.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(DelayedSceneMovement());
        }
        //Debug.LogError("Scene mover should do it's magic,");
    }

    public void OnDestroy()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= GoGetSceneFromServer;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnActiveSceneChanged;

        CharacterManager.Instance.DiliitSciinMuuver(this);
    }

    [TargetRpc]
    public void MoveYourAss(NetworkConnection connection, 
                            Scene scene)
    {
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, scene);

        Debug.LogError("Move your ass is called. Should move to scene " + scene.name);
    }

    private void StopTheFuckingCorouteines()
    {
        StopAllCoroutines();
        //Debug.LogError("Should have stopped coroutines");
    }

    public IEnumerator DelayedSceneMovement()
    {

        bool searchingForAValidScene = true;

        while (searchingForAValidScene) 
        {
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
            {
                //Debug.LogError("Currently loaded scene is " + UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).name);

                Scene valid;
                bool unassignedValid = true;
                if (!UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).name.Equals("MovedObjectsHolder")
                    && UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).IsValid()
                    && !string.IsNullOrEmpty(SceneWeShouldBeIn)
                    && UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).Equals(SceneWeShouldBeIn))
                {
                    valid = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
                    OnActiveSceneChanged(valid, LoadSceneMode.Single);
                    UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, valid);
                    searchingForAValidScene = false;

                    //Debug.LogError("OKAY, HERE WE ARE. A found non-moved objects holder scene is " + valid.name + " gameobject scene name is " + gameObject.scene.name);

                    if (gameObject.scene.name.Equals(valid.name))
                    {
                        SceneLoader.Instance.UnpackNonPlayerPlayer(gameObjects);
                        //Debug.LogError("Names match. Unpack");
                    }

                    else
                    {

                        SceneLoader.Instance.PackNonPlayerPlayer(GetComponentsInChildren<Transform>());
                        //UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, UnityEngine.SceneManagement.SceneManager.GetSceneByName("Playground"));
                        //Debug.LogError("Names do not match. Pack");


                    }

                    if (!searchingForAValidScene)
                    {
                        StopTheFuckingCorouteines();
                    }

                    unassignedValid = false;
                }

                if (unassignedValid)
                {
                    searchingForAValidScene = false;

                }
            }

            if (searchingForAValidScene) 
            {
                yield return new WaitForEndOfFrame();
            }

            else
            {
                //Debug.LogError("Should break");
                yield break;
            }
        }
    }

    [ServerRpc]
    public void TellTheOwnerToMove()
    {
        //Debug.LogError("Should tell the owner to move");


        foreach (KeyValuePair<int, NetworkConnection> kvp in ClientManager.Clients)
        {
            TellTheOwnerToMoveClientRpc(kvp.Value);
        }

    }

    public void TellTheOwnerToMoveClientRpc(NetworkConnection connection)
    {
        //Debug.LogError("Should tell the owner to move");

        StarterAssets.ThirdPersonController third = GetComponentInChildren<StarterAssets.ThirdPersonController>(true);
        third.MoveSlightly();
    }
}
