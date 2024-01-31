using Authentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scenes;
using UnityEngine.Events;
using FishNet;

namespace WorldObjects
{
    [RequireComponent(typeof(ScenePicker))]
    public class SceneChangeObject : MonoBehaviour, I_Interactable
    {
        [field: SerializeReference]
        public string DetectionMessage { get; set; }
        public bool IsActive => true;
        public Vector3 DetectionMessageOffSet { get => Vector3.zero; private set => DetectionMessageOffSet = value; }

        [SerializeField] ScenePackMode scenePackMode = ScenePackMode.ALL;
        [Tooltip("Extra data that can be passed to scene loading")]
        [SerializeField] string sceneDataString;
        [SerializeField] bool bundled = false;
        ScenePicker scenePicker;

        void Start()
        {
            scenePicker = GetComponent<ScenePicker>();
        }

        public void Interact(UnityAction _)
        {
            SceneLoader.Instance.LoadScene(scenePicker.scenePath, new SceneLoadParams(scenePackMode, sceneDataString), bundled);
        }
    }
}

