using Authentication;
using Characters;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Netcode;
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
        public bool nulled;
        public ScenePackMode scenePackMode;

        public SceneLoadParams(Vector3 origo, Quaternion rotation, ScenePackMode scenePackMode)
        {
            this.origo = origo;
            nulled = false;
            this.rotation = rotation;
            this.scenePackMode = scenePackMode;
        }

        public SceneLoadParams(bool nulled)
        {
            this.nulled = nulled;
            origo = Vector3.zero;
            rotation = Quaternion.identity;
            scenePackMode = ScenePackMode.NONE;
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
    public class SceneLoader : NetworkBehaviour
    {
        public static SceneLoader Instance { get; private set; }
        [SerializeField] string mainSceneName;

        List<NetworkObject> cachedNetworkObjects = new List<NetworkObject>();

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
            mainSceneName = ParseSceneName(mainScenePath);
        }

        public void LoadScene(string scenePath)
        {
            this.sceneLoadParams = new SceneLoadParams(true);
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
            AsyncOperation op = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            while (!op.isDone)
            {
                yield return null;
            }

            UnpackScene();
            
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(mainSceneName));
        }

        void PackScene(ScenePackMode scenePackMode)
        {
            if(scenePackMode != ScenePackMode.NONE)
            {
                Scene activeScene = SceneManager.GetActiveScene();

                if(scenePackMode == ScenePackMode.PLAYER_ONLY)
                {
                    // make sure to only pick our own player
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    cachedGameObjectList.Add(new CachedGameObject(player, player.activeSelf));
                } else
                {
                    GameObject[] allObjects = activeScene.GetRootGameObjects();

                    foreach (GameObject gameObject in allObjects)
                    {
                        if (scenePackMode == ScenePackMode.ALL_BUT_PLAYER && gameObject.CompareTag("Player"))
                        {

                        } else
                        {
                            cachedGameObjectList.Add(new CachedGameObject(gameObject, gameObject.activeSelf));
                            gameObject.SetActive(false);
                        }
                    }
                }

            }
        }

        void UnpackScene()
        {
            foreach (CachedGameObject cachedGameObject in cachedGameObjectList)
            {
                cachedGameObject.gameObject.SetActive(cachedGameObject.isEnabled);
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

