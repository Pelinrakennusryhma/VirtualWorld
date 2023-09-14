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
        //[ClientRpc]
        //public void NewClientConnectedClientRpc(NetworkIdentity identity)
        //{
        //    // if playing minigame, handle any new characters getting instantiated
        //    ScenePackMode packMode = sceneLoadParams.scenePackMode;
        //    if (cachedGameObjectList.Count > 0 && (packMode == ScenePackMode.ALL || packMode == ScenePackMode.ALL_BUT_PLAYER))
        //    {
        //        AddNewCachedObject(identity.gameObject);
        //    }

        //}

        //void AddNewCachedObject(GameObject obj)
        //{
        //    cachedGameObjectList.Add(new CachedGameObject(obj, obj.activeSelf));
        //    SceneManager.MoveGameObjectToScene(obj, SceneManager.GetSceneByName(MainSceneName));

        //    if (obj.CompareTag("Player"))
        //    {
        //        PlayerEmitter playerEmitter = obj.GetComponent<PlayerEmitter>();
        //        playerEmitter.DisableCharacter();
        //    }
        //    else
        //    {
        //        obj.SetActive(false);
        //    }
        //}

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
                    GameObject player = Character.Instance.OwnedCharacter;
                    cachedGameObjectList.Add(new CachedGameObject(player, player.activeSelf));
                    player.SetActive(false);
                    //Character.Instance.PlayerEmitter.DisableCharacter();
                }
                else
                {
                    GameObject[] allObjects = activeScene.GetRootGameObjects();

                    foreach (GameObject gameObject in allObjects)
                    {
                        if (scenePackMode == ScenePackMode.ALL_BUT_PLAYER && gameObject == Character.Instance.OwnedCharacter)
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

    }
}

