using Audio;
using Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Configuration
{
    public class SingleplayerMode : MonoBehaviour
    {
        public static SingleplayerMode Instance { get; private set; }
        [SerializeField] ScenePicker mainScenePicker;
        [SerializeField] GameObject playerPrefab;
        [SerializeField] List<GameObject> objectsToDisable;

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                DisableNetworking();
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
        }

        private void Start()
        {
            StartSingleplayer();
        }

        void DisableNetworking()
        {
            foreach (GameObject gameObject in objectsToDisable)
            {
                DestroyImmediate(gameObject);
            }
        }

        void StartSingleplayer()
        {
            Debug.Log("Starting singleplayer mode");
            SceneManager.LoadScene(mainScenePicker.GetSceneName());
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if(scene.name == mainScenePicker.GetSceneName())
            {
                Debug.Log("Loaded main scene");
                GameObject playerObject = Instantiate(playerPrefab);
                playerObject.SetActive(true);
            }
        }
    }
}

