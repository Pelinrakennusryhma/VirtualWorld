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
    public struct SceneLoadParams
    {
        public Vector3 origo;
        public Quaternion rotation;
        public bool nulled;

        public SceneLoadParams(Vector3 origo, Quaternion rotation)
        {
            this.origo = origo;
            nulled = false;
            this.rotation = rotation;
        }

        public SceneLoadParams(bool nulled)
        {
            this.nulled = nulled;
            origo = Vector3.zero;
            rotation = Quaternion.identity;
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

        [SerializeField] Transform inactiveSceneContainer;
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
            StartCoroutine(LoadAsyncScene(sceneName, true));
        }

        public void LoadSceneByName(string sceneName, bool unloadCurrent, SceneLoadParams sceneLoadParams)
        {
            StartCoroutine(LoadAsyncScene(sceneName, unloadCurrent));

            if(!sceneLoadParams.nulled)
            {
                this.sceneLoadParams = sceneLoadParams;
                Debug.Log("sceneloadparams exist!: " + sceneLoadParams.origo);
            }
        }

        public void UnloadScene()
        {
            StartCoroutine(UnloadAsyncScene(true));
        }

        IEnumerator LoadAsyncScene(string sceneName, bool pack)
        {
            if (pack)
            {
                PackScene();
            }
      
            AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            while (!async.isDone)
            {
                yield return null;
            }

            Scene subScene = SceneManager.GetSceneByName(sceneName);

            if (pack)
            {
                inactiveSceneContainer.gameObject.SetActive(false);
            }

            SceneManager.SetActiveScene(subScene);
        }

        IEnumerator UnloadAsyncScene(bool unpack)
        {
            AsyncOperation op = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            while (!op.isDone)
            {
                yield return null;
            }

            if (unpack)
            {
                inactiveSceneContainer.gameObject.SetActive(true);
                UnpackScene();
            }

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(mainSceneName));
        }

        void PackScene()
        {
            Scene activeScene = SceneManager.GetActiveScene();
            GameObject[] allObjects = activeScene.GetRootGameObjects();

            foreach (GameObject gameObject in allObjects)
            {
                cachedGameObjectList.Add(new CachedGameObject(gameObject, gameObject.activeSelf));
                gameObject.SetActive(false);
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

