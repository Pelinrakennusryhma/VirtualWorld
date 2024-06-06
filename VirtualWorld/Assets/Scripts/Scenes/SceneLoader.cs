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

        public void LoadScene(string scenePath, SceneLoadParams sceneLoadParams, bool bundled)
        {
            string sceneName = ParseSceneName(scenePath);

            // ALL_BUT_PLAYER essentially means loading a network scene because everything but player character is packed away.. not smooth.
            if (sceneLoadParams.scenePackMode == ScenePackMode.ALL_BUT_PLAYER)
            {
                NetworkSceneLoader.Instance.MoveToNetworkScene(InstanceFinder.ClientManager.Connection, sceneName, bundled);
            } else
            {
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
            CharacterManager.Instance.SetGameState(GAME_STATE.MINIGAME);
            PackScene(sceneLoadParams.scenePackMode);

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
            CharacterManager.Instance.SetGameState(GAME_STATE.FREE);
            AsyncOperation op = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            while (!op.isDone)
            {
                yield return null;
            }

            UnpackScene();

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(MainSceneName));
        }

        void PackScene(ScenePackMode scenePackMode)
        {
            if (scenePackMode == ScenePackMode.NONE)
            {
                return;
            }

            Scene activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

            if (scenePackMode == ScenePackMode.PLAYER_ONLY)
            {
                PackObject(CharacterManager.Instance.OwnedCharacter);
            }
            else
            {
                GameObject[] allObjects = activeScene.GetRootGameObjects();

                foreach (GameObject go in allObjects)
                {
                    if (scenePackMode == ScenePackMode.ALL_BUT_PLAYER && go == CharacterManager.Instance.OwnedCharacter)
                    {
                        // move character to the new scene here?
                    }
                    else
                    {
                        PackObject(go);
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

            // Animated NetworkObjects are disabled via script
            AnimatedObjectDisabler disabler = go.GetComponent<AnimatedObjectDisabler>();
            if (disabler != null)
            {
                disabler.Disable();
                return;
            }

            // Containers holding animated NetworkObjects
            AnimatedObjectContainer animatedObjectContainer = go.GetComponent<AnimatedObjectContainer>();
            if(animatedObjectContainer != null)
            {
                animatedObjectContainer.DisableChildren();
                return;
            }

            // Normal objects are simply disabled
            go.SetActive(false);

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

        public void MoveToMainScene(GameObject objectToMove)
        {
            SceneManager.MoveGameObjectToScene(objectToMove, SceneManager.GetSceneByName(MainSceneName));
        }
    }
}

