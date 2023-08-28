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
    [RequireComponent(typeof(ScenePicker))]
    public class SceneLoader : NetworkBehaviour
    {
        public static SceneLoader Instance { get; private set; }
        [SerializeField] string mainSceneName;

        [SerializeField] Transform inactiveSceneContainer;
        List<NetworkObject> cachedNetworkObjects = new List<NetworkObject>();

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

            SceneManager.SetActiveScene(subScene);

            inactiveSceneContainer.gameObject.SetActive(false);
        }

        IEnumerator UnloadAsyncScene()
        {
            AsyncOperation op = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            while (!op.isDone)
            {
                yield return null;
            }

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(mainSceneName));
            UnpackScene();
        }

        void PackScene()
        {
            Scene activeScene = SceneManager.GetActiveScene();
            GameObject[] allObjects = activeScene.GetRootGameObjects();

            inactiveSceneContainer = new GameObject("Inactive Scene Container").transform;

            foreach (GameObject go in allObjects)
            {
                NetworkObject nObj = go.GetComponent<NetworkObject>();
                if (nObj != null)
                {
                    cachedNetworkObjects.Add(nObj);
                    nObj.gameObject.SetActive(false);
                } else
                {
                    go.transform.SetParent(inactiveSceneContainer, false);
                }
            }

        }

        void UnpackScene()
        {
            SceneManager.MoveGameObjectToScene(inactiveSceneContainer.gameObject, SceneManager.GetActiveScene());
            inactiveSceneContainer.DetachChildren();;
            inactiveSceneContainer.parent = transform;

            foreach (NetworkObject networkObject in cachedNetworkObjects)
            {
                networkObject.gameObject.SetActive(true);
            }

            Destroy(inactiveSceneContainer.gameObject);
            cachedNetworkObjects.Clear();
        }

        string ParseSceneName(string scenePath)
        {
            string[] scenePathSplit = scenePath.Split('/');
            string sceneName = scenePathSplit[scenePathSplit.Length - 1].Split('.')[0];

            return sceneName;
        }

    }
}

