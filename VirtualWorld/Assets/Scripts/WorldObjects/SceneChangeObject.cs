using Authentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scenes;
using UnityEngine.Events;
using Unity.Netcode;

namespace WorldObjects
{
    [RequireComponent(typeof(ScenePicker))]
    public class SceneChangeObject : MonoBehaviour, I_Interactable
    {
        [field: SerializeReference]
        public string DetectionMessage { get; set; }

        ScenePicker scenePicker;

        void Start()
        {
            scenePicker = GetComponent<ScenePicker>();
        }

        public void Interact(string playerId, UnityAction callback)
        {
            callback?.Invoke();
            SceneLoader.Instance.LoadScene(scenePicker.scenePath, new SceneLoadParams(ScenePackMode.ALL, "ShowWorlds"));
        }
    }
}

