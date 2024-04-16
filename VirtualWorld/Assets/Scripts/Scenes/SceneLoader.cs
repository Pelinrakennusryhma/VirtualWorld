using Authentication;
using Characters;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using FishNet.Object;
using UnityEngine;
using UnityEngine.SceneManagement;
using StarterAssets;
using Animations;
using FishNet;

namespace Scenes
{
    #region Enums
    public enum ScenePackMode
    {
        ALL,
        NONE,
        PLAYER_ONLY,
        ALL_BUT_PLAYER // Network scene
    }
    #endregion

    #region Structs

    public struct SceneLoadParams
    {
        public Vector3 origo;
        public Quaternion rotation;
        public object sceneData; // any type of data that might need to be moved to a loaded soloscene
        public ScenePackMode scenePackMode;

        // constructor used if we care about the player's position in the world, E.g. when throwing dice in the world
        public SceneLoadParams(Vector3 origo, Quaternion rotation, ScenePackMode scenePackMode, object sceneData = null)
        {
            this.origo = origo;
            this.sceneData = sceneData;
            this.rotation = rotation;
            this.scenePackMode = scenePackMode;
        }

        public SceneLoadParams(ScenePackMode scenePackMode, object sceneData = null)
        {
            this.origo = Vector3.zero;
            this.sceneData = sceneData;
            this.rotation = Quaternion.identity;
            this.scenePackMode = scenePackMode;
        }
    }
    public struct CachedGameObject
    {
        public GameObject gameObject;
        public bool isEnabled;

        public CachedGameObject(GameObject go, bool isEnabled)
        {
            gameObject = go;
            this.isEnabled = isEnabled;
        }
    }

    struct CachedMonoBehaviour
    {
        public MonoBehaviour mb;
        public bool isEnabled;

        public CachedMonoBehaviour(MonoBehaviour mb, bool isEnabled)
        {
            this.mb = mb;
            this.isEnabled = isEnabled;
        }
    }

    struct CachedCollider
    {
        public Collider col;
        public bool isEnabled;

        public CachedCollider(Collider col, bool isEnabled)
        {
            this.col = col;
            this.isEnabled = isEnabled;
        }
    }

    #endregion

    [RequireComponent(typeof(ScenePicker))]
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }
        [SerializeField] public string MainSceneName { get; private set; }

        List<CachedGameObject> cachedGameObjectList = new List<CachedGameObject>();

        public SceneLoadParams sceneLoadParams;

        bool InSoloScene { get { return cachedGameObjectList.Count > 0; } }

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        void Start()
        {
            string mainScenePath = GetComponent<ScenePicker>().scenePath;
            MainSceneName = ParseSceneName(mainScenePath);
        }

        public void NewMainSceneObjectAdded(GameObject gameObject)
        {
            // if playing minigame, handle any new objects getting instantiated
            if (InSoloScene)
            {
                AddNewCachedObject(gameObject);
            }

        }

        void AddNewCachedObject(GameObject obj)
        {
            SceneManager.MoveGameObjectToScene(obj, SceneManager.GetSceneByName(MainSceneName));
            ScenePackMode packMode = sceneLoadParams.scenePackMode;

            // in most cases we want to disable the game object, but not when throwing dice out in the main scene for example
            if(packMode == ScenePackMode.ALL || packMode == ScenePackMode.ALL_BUT_PLAYER)
            {
                cachedGameObjectList.Add(new CachedGameObject(obj, obj.activeSelf));

                AnimatedObjectDisabler disabler = obj.GetComponent<AnimatedObjectDisabler>();

                if(disabler != null)
                {
                    disabler.Disable();
                } else
                {
                    obj.SetActive(false);
                }
            }
        }

        public void LoadScene(string scenePath, SceneLoadParams sceneLoadParams)
        {

            Debug.LogError("Loading scene at path " + scenePath);

            for(int paska = 0; paska < CharacterManager.Instance.sciinMuuvers.Count; paska++)
            {
                UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(CharacterManager.Instance.sciinMuuvers[paska].gameObject, UnityEngine.SceneManagement.SceneManager.GetSceneByName("Playground"));
            }

            string sceneName = ParseSceneName(scenePath);

            // ALL_BUT_PLAYER essentially means loading a network scene because everything but player character is packed away.. not smooth.
            if (sceneLoadParams.scenePackMode == ScenePackMode.ALL_BUT_PLAYER)
            {
                PlayerEvents.Instance.CallEventSceneLoadStarted(); // Load Ended is called from CharacterManager once character has been inited on the new scene.

                if (NonNetworkRecognizer.Instance == null) 
                {
                    //Debug.LogError("About to move to the networked scene");
                    NetworkSceneLoader.Instance.MoveToNetworkScene(InstanceFinder.ClientManager.Connection, sceneName);
                }

                else
                {
                    //Debug.Log("Do the non-networked scene loading thingie");
                    MoveToNonNetworkedScene(sceneName);
                }
            } else
            {

                Debug.LogError("Scene to load name is " + sceneName);
                this.sceneLoadParams = sceneLoadParams;
                StartCoroutine(LoadAsyncScene(sceneName, sceneLoadParams));
            }
        }

        public void LoadSceneByName(string sceneName, SceneLoadParams sceneLoadParams)
        {
            this.sceneLoadParams = sceneLoadParams;
            StartCoroutine(LoadAsyncScene(sceneName, sceneLoadParams));
        }

        public void UnloadScene()
        {
            StartCoroutine(UnloadAsyncScene());
        }

        IEnumerator LoadAsyncScene(string sceneName, SceneLoadParams sceneLoadParams)
        {
            if (CharacterManager.Instance != null) 
            {
                CharacterManager.Instance.SetGameState(GAME_STATE.MINIGAME);
            }

            else
            {
                CharacterManagerNonNetworked.Instance.SetGameState(GAME_STATE.MINIGAME);
            }
            PackScene(null, sceneLoadParams.scenePackMode, false);

            AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            while (!async.isDone)
            {
                yield return null;
            }

            Scene subScene = SceneManager.GetSceneByName(sceneName);

            SceneManager.SetActiveScene(subScene);
        }

        IEnumerator UnloadAsyncScene()
        {
            if (CharacterManager.Instance != null) 
            {
                CharacterManager.Instance.SetGameState(GAME_STATE.FREE);
            }

            else
            {
                CharacterManagerNonNetworked.Instance.SetGameState(GAME_STATE.FREE);
            }

            AsyncOperation op = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            while (!op.isDone)
            {
                yield return null;
            }

            UnpackScene();

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(MainSceneName));
        }

        public void PackScene(string sceneName,
                              ScenePackMode scenePackMode,
                              bool keepPlayersUnpacked)
        {
            PackScene(scenePackMode, 
                      UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName), 
                      keepPlayersUnpacked);
        }

        public void UnpackScene(string sceneName)
        {
            UnpackScene();
        }

        void PackScene(ScenePackMode scenePackMode,
                       Scene sceneToPack,
                       bool keepPlayersUnpacked)
        {
            if (scenePackMode == ScenePackMode.NONE)
            {
                return;
            }

            Scene activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

            Debug.LogError("Active scene is " + activeScene.name + " active scene is valid " + activeScene.IsValid());

            if (sceneToPack != null)
            {
                Debug.LogError("Set active scene to scene to pack. Active scene name is "+ activeScene.name);
                
                if (!string.IsNullOrEmpty(sceneToPack.name)) 
                {
                    activeScene = sceneToPack;                
                    Debug.LogError("Set active scene to scene to pack . Active scene name is " + activeScene.name);
                }

            }

            if (scenePackMode == ScenePackMode.PLAYER_ONLY)
            {
                if (!keepPlayersUnpacked) 
                {
                    if (CharacterManager.Instance != null)
                    {
                        PackObject(CharacterManager.Instance.OwnedCharacter);
                    }

                    else
                    {
                        PackObject(CharacterManagerNonNetworked.Instance.OwnedCharacter);
                    }
                }
            }
            else
            {
                Debug.LogError("Active scene is " + activeScene.name + " active scene is valid " + activeScene.IsValid()+ ". About to get root gameobjects");


                GameObject[] allObjects = activeScene.GetRootGameObjects();

                foreach (GameObject go in allObjects)
                {
                    Debug.LogError("scene pack mode is " + scenePackMode.ToString());

                    if (scenePackMode == ScenePackMode.ALL_BUT_PLAYER && go == CharacterManager.Instance.OwnedCharacter)
                    {
                        // move character to the new scene here?
                    }
                    else
                    {

                        Debug.LogError("Packing object " + go.name);
                        bool shouldPack = true;

                        if (go.GetComponent<PlayerEmitter>())
                        {
                            shouldPack = false;
                        }

                        shouldPack = true;

                        if (shouldPack) 
                        {
                            PackObject(go);
                        }
                    }
                }
            }
        }

        void UnpackScene()
        {
            foreach (CachedGameObject cachedGameObject in cachedGameObjectList)
            {
                if (cachedGameObject.gameObject != null)
                {
                    UnpackObject(cachedGameObject);
                }
            }

            cachedGameObjectList.Clear();
        }

        void PackObject(GameObject go)
        {
            cachedGameObjectList.Add(new CachedGameObject(go, go.activeSelf));

            bool dontDiasbleGO = false;

            // Animated NetworkObjects are disabled via script
            AnimatedObjectDisabler disabler = go.GetComponent<AnimatedObjectDisabler>();
            if (disabler != null)
            {
                disabler.Disable();

                Debug.LogError("Disabled an animated object");
                dontDiasbleGO = true;
                //return;
            }

            // Containers holding animated NetworkObjects
            AnimatedObjectContainer animatedObjectContainer = go.GetComponent<AnimatedObjectContainer>();
            if(animatedObjectContainer != null)
            {
                animatedObjectContainer.DisableChildren();
                Debug.LogError("Disabled an animated object container");
                dontDiasbleGO = true;
                //return;
            }


            // Normal objects are simply disabled
            if (!dontDiasbleGO) 
            {
                go.SetActive(false);
                Debug.LogError("Set GO to inactive");
            }

        }

        void UnpackObject(CachedGameObject cachedGameObject)
        {
            AnimatedObjectDisabler disabler = cachedGameObject.gameObject.GetComponent<AnimatedObjectDisabler>();
            if (disabler != null)
            {
                disabler.Enable();
                return;
            }

            AnimatedObjectContainer animatedObjectContainer = cachedGameObject.gameObject.GetComponent<AnimatedObjectContainer>();
            if (animatedObjectContainer != null)
            {
                animatedObjectContainer.EnableChildren();
                return;
            }

            cachedGameObject.gameObject.SetActive(cachedGameObject.isEnabled);

        }

        string ParseSceneName(string scenePath)
        {
            string[] scenePathSplit = scenePath.Split('/');
            string sceneName = scenePathSplit[scenePathSplit.Length - 1].Split('.')[0];

            return sceneName;
        }

        // For the purposes of changing the subscene without messing with the packed main scene.
        public void SwitchSubScenes(string incomingSceneName)
        {
            StartCoroutine(LoadNewSubSceneAndUnloadOldSubScene(incomingSceneName));
        }

        IEnumerator LoadNewSubSceneAndUnloadOldSubScene(string incomingSceneName)
        {
            Scene oldSubScene = SceneManager.GetActiveScene();

            // Disable objects, so they won't mess up with FindObjectsOfType when a new scene is loaded
            // Gravity ship does this at the start of every level.
            GameObject[] oldRootObjects = oldSubScene.GetRootGameObjects();

            Camera oldMainCamera = Camera.main;

            for (int i = 0; i < oldRootObjects.Length; i++)
            {
                oldRootObjects[i].gameObject.SetActive(false);
            }

            // But keep the main camera enabled, so we don't get a brief flash of default skybox
            oldMainCamera.gameObject.SetActive(true);

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(incomingSceneName, LoadSceneMode.Additive);

            while (!loadOperation.isDone)
            {
                yield return null;
            }

            Scene newSubScene = SceneManager.GetSceneByName(incomingSceneName);

            oldMainCamera.gameObject.SetActive(false);

            SceneManager.SetActiveScene(newSubScene);
            SceneManager.UnloadSceneAsync(oldSubScene);
        }

        private void MoveToNonNetworkedScene(string newSceneName)
        {
            Debug.Log("Moving to non-networked scene " + newSceneName);

            string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            GameObject characterToMove = CharacterManagerNonNetworked.Instance.OwnedCharacter;

            //UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(newSceneName);
            //Scene newScene = SceneManager.GetSceneByName(newSceneName);
            //SceneManager.MoveGameObjectToScene(characterToMove, newScene);
            //UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currentSceneName);

            StartCoroutine(DoTheSwitch(newSceneName, 
                                       currentSceneName, 
                                       characterToMove));
        }

        private IEnumerator DoTheSwitch(string newScene, 
                                        string oldScene, 
                                        GameObject objectToMove)
        {
            Debug.Log("Started coroutine of scene switching");


            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);

            //SceneManager.LoadScene(newScene, LoadSceneMode.Single);

            //yield return null;

            while (!loadOperation.isDone)
            {
                yield return null;
            }

            Scene newSceneScene = SceneManager.GetSceneByName(newScene);
            SceneManager.MoveGameObjectToScene(objectToMove, newSceneScene);

            Networking.NetworkSceneConnector connector = null;

            foreach (GameObject rootGO in newSceneScene.GetRootGameObjects())
            {
                connector = rootGO.GetComponent<Networking.NetworkSceneConnector>();

                if (connector != null)
                {
                    Debug.Log("Found a connector " + connector.gameObject.name);
                    break;
                }
            }

            Transform spawnPoint = objectToMove.transform;

            if (connector != null) 
            {
                spawnPoint = connector.GetSpawnTransform(oldScene);
            }



            objectToMove.transform.position = spawnPoint.position;
            objectToMove.transform.rotation = spawnPoint.rotation;


            SceneManager.SetActiveScene(newSceneScene);
            SceneManager.UnloadSceneAsync(oldScene);

            PlayerEvents.Instance.CallEventSceneLoadEnded();

            //GameObject newCharacter = Instantiate(NonNetworkObjectSpawner.Instance.playerPrefab);

            //newCharacter.transform.position = spawnPoint.position;
            //newCharacter.transform.rotation = spawnPoint.rotation;
        }

        public void MoveToMainScene(GameObject objectToMove)
        {
            SceneManager.MoveGameObjectToScene(objectToMove, SceneManager.GetSceneByName(MainSceneName));
        }

        public void UnpackNonPlayerPlayer(List<CachedGameObject> objects)
        {
            //Debug.LogError("Unpacking non player player");

            for (int i = 0; i < objects.Count; i++) {

               // Debug.Log("Iterating through object " + i);

                ThirdPersonController controller = objects[i].gameObject.GetComponentInChildren<ThirdPersonController>(true);

                if (controller != null) 
                {
                    controller.enabled = true;
                    //Debug.LogError("Enabled third person controller");
                }

                AnimatedObjectDisabler disabler = objects[i].gameObject.GetComponent<AnimatedObjectDisabler>();
                if (disabler != null)
                {
                    disabler.Enable();

                }

                AnimatedObjectContainer animatedObjectContainer = objects[i].gameObject.GetComponent<AnimatedObjectContainer>();
                if (animatedObjectContainer != null)
                {
                    animatedObjectContainer.EnableChildren();

                }

                objects[i].gameObject.SetActive(objects[i].isEnabled);

               // Debug.Log("Gameobject " + objects[i].gameObject + " is enabled " + objects[i].isEnabled);
            }
        }

        public List<CachedGameObject> PackNonPlayerPlayer(Transform[] objs)
        {
            for (int i = 0; i < objs.Length; i++) 
            {
                //Debug.LogError("PACKING NON PLAYER PLAYER. Objs[i] scene is  " + objs[i].gameObject.scene.name);
            }

            List<CachedGameObject> cahced = new List<CachedGameObject>();


            for (int i  = 0; i < objs.Length; i++)
            {
                cahced.Add(new CachedGameObject(objs[i].gameObject, objs[i].gameObject.activeSelf));


                // Animated NetworkObjects are disabled via script
                AnimatedObjectDisabler disabler = objs[i].GetComponent<AnimatedObjectDisabler>();
                if (disabler != null)
                {
                    disabler.Disable();
                    //return;
                }

                // Containers holding animated NetworkObjects
                AnimatedObjectContainer animatedObjectContainer = objs[i].GetComponent<AnimatedObjectContainer>();
                if (animatedObjectContainer != null)
                {
                    animatedObjectContainer.DisableChildren();
                    //return;
                }

                // Normal objects are simply disabled
                objs[i].gameObject.SetActive(false);

                //Debug.LogError("Game object " + objs[i].gameObject.name + " is enabled " + objs[i].gameObject.activeSelf);
            }



            return cahced;
        }
    }
}

