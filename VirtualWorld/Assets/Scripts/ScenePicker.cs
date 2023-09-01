using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes
{
    public class ScenePicker : MonoBehaviour
    {
        [SerializeField]
        public string scenePath;

        public string GetSceneName()
        {
            string[] scenePathSplit = scenePath.Split('/');
            string sceneName = scenePathSplit[scenePathSplit.Length - 1].Split('.')[0];

            return sceneName;
        }
    }
}

