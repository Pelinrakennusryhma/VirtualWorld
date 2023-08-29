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
            StartCoroutine(LoadAsyncScene(scenePath));
        }

        public void UnloadScene()
        {
            StartCoroutine(UnloadAsyncScene());
        }

        IEnumerator LoadAsyncScene(string scenePath)
        {
            string sceneName = ParseSceneName(scenePath);

            PackScene();
            
            AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            while (!async.isDone)
            {
                yield return null;
            }

            Scene subScene = SceneManager.GetSceneByName(sceneName);

            inactiveSceneContainer.gameObject.SetActive(false);

            SceneManager.SetActiveScene(subScene);
        }

        IEnumerator UnloadAsyncScene()
        {
            AsyncOperation op = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            while (!op.isDone)
            {
                yield return null;
            }

            inactiveSceneContainer.gameObject.SetActive(true);
            UnpackScene();

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

            //inactiveSceneContainer = new GameObject("Inactive Scene Container").transform;

            //foreach (GameObject go in allObjects)
            //{
            //    NetworkObject nObj = go.GetComponent<NetworkObject>();
            //    if (nObj != null)
            //    {
            //        cachedNetworkObjects.Add(nObj);
            //        nObj.gameObject.SetActive(false);
            //    } else
            //    {
            //        go.transform.SetParent(inactiveSceneContainer, false);
            //    }
            //}

        }

        void UnpackScene()
        {
            foreach (CachedGameObject cachedGameObject in cachedGameObjectList)
            {
                cachedGameObject.gameObject.SetActive(cachedGameObject.isEnabled);
            }

            //inactiveSceneContainer.DetachChildren();

            //foreach (NetworkObject networkObject in cachedNetworkObjects)
            //{
            //    networkObject.gameObject.SetActive(true);
            //}

            //Destroy(inactiveSceneContainer.gameObject);
            //cachedNetworkObjects.Clear();
        }

        string ParseSceneName(string scenePath)
        {
            string[] scenePathSplit = scenePath.Split('/');
            string sceneName = scenePathSplit[scenePathSplit.Length - 1].Split('.')[0];

            return sceneName;
        }

    }
}

