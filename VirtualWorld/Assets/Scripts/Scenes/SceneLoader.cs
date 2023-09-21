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

namespace Scenes
{
    public enum ScenePackMode
    {
        ALL,
        NONE,
        PLAYER_ONLY,
        ALL_BUT_PLAYER
    }

    public struct SceneLoadParams
    {
        public Vector3 origo;
        public Quaternion rotation;
        public object sceneData;
        public ScenePackMode scenePackMode;

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
    struct CachedGameObject
    {
        public GameObject gameObject;
        public bool isEnabled;

        public CachedGameObject(GameObject go, bool isEnabled)
        {
            gameObject = go;
            this.isEnabled = isEnabled;
        }
    }

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

            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;

            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }
        public void NewMainSceneObjectAdded(GameObject playerGO)
        {
            // if playing minigame, handle any new characters getting instantiated
            Debug.Log("new client!");

            if (InSoloScene)
            {
                AddNewCachedObject(playerGO);
            }

        }

        void AddNewCachedObject(GameObject obj)
        {
            SceneManager.MoveGameObjectToScene(obj, SceneManager.GetSceneByName(MainSceneName));
            ScenePackMode packMode = sceneLoadParams.scenePackMode;
            Debug.Log("new client is moved to main scene");

            if(packMode == ScenePackMode.ALL || packMode == ScenePackMode.ALL_BUT_PLAYER)
            {
                Debug.Log("new client is disabled");
                cachedGameObjectList.Add(new CachedGameObject(obj, obj.activeSelf));
                obj.SetActive(false);
            }

        }

        public void LoadScene(string scenePath, SceneLoadParams sceneLoadParams)
        {
            this.sceneLoadParams = sceneLoadParams;
            string sceneName = ParseSceneName(scenePath);
            StartCoroutine(LoadAsyncScene(sceneName, sceneLoadParams));
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
            PackScene(sceneLoadParams.scenePackMode);

            AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            while (!async.isDone)
            {
                yield return null;
            }

            Scene subScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);

            UnityEngine.SceneManagement.SceneManager.SetActiveScene(subScene);
        }

        IEnumerator UnloadAsyncScene()
        {
            AsyncOperation op = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene());

            while (!op.isDone)
            {
                yield return null;
            }

            UnpackScene();

            UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager.GetSceneByName(MainSceneName));
        }

        void PackScene(ScenePackMode scenePackMode)
        {
            if (scenePackMode != ScenePackMode.NONE)
            {
                Scene activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

                if (scenePackMode == ScenePackMode.PLAYER_ONLY)
                {
                    GameObject player = CharacterManager.Instance.OwnedCharacter;
                    cachedGameObjectList.Add(new CachedGameObject(player, player.activeSelf));
                    player.SetActive(false);
                    //Character.Instance.PlayerEmitter.DisableCharacter();
                }
                else
                {
                    GameObject[] allObjects = activeScene.GetRootGameObjects();

                    foreach (GameObject gameObject in allObjects)
                    {
                        if (scenePackMode == ScenePackMode.ALL_BUT_PLAYER && gameObject == CharacterManager.Instance.OwnedCharacter)
                        {
                            // move character to the new scene here?
                        }
                        else
                        {
                            if (gameObject.CompareTag("Player"))
                            {
                                //PlayerEmitter playerEmitter = gameObject.GetComponent<PlayerEmitter>();
                                cachedGameObjectList.Add(new CachedGameObject(gameObject, gameObject.activeSelf));
                                //playerEmitter.DisableCharacter();
                                gameObject.SetActive(false);
                            }
                            else
                            {
                                cachedGameObjectList.Add(new CachedGameObject(gameObject, gameObject.activeSelf));
                                gameObject.SetActive(false);
                            }
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
                    if (cachedGameObject.gameObject.CompareTag("Player"))
                    {
                        PlayerEmitter playerEmitter = cachedGameObject.gameObject.GetComponent<PlayerEmitter>();
                        //playerEmitter.EnableCharacter();
                        cachedGameObject.gameObject.SetActive(true);
                    }
                    else
                    {
                        cachedGameObject.gameObject.SetActive(cachedGameObject.isEnabled);
                    }

                }
            }

            cachedGameObjectList.Clear();
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
        public void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            Debug.Log("On scene loaded " + scene.name);
        }

        public void OnSceneUnloaded(Scene scene)
        {
            Debug.Log("Unloaded scene " + scene.name);

            if (scene.name.Equals(MainSceneName))
            {
                Debug.Log("We just unloaded main scene. This is not cool.");
            }
        }

    }
}

