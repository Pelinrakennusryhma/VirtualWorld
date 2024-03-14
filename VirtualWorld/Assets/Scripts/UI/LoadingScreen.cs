using Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class LoadingScreen : MonoBehaviour
    {
        GameObject loadingScreenPanel;
        // Start is called before the first frame update
        void Start()
        {
            loadingScreenPanel = transform.GetChild(0).gameObject;
            PlayerEvents.Instance.EventSceneLoadStarted.AddListener(OnSceneLoadStarted);
            PlayerEvents.Instance.EventSceneLoadEnded.AddListener(OnSceneLoadEnded);
        }

        void OnSceneLoadStarted()
        {
            //loadingScreenPanel.SetActive(true);
            loadingScreenPanel.SetActive(false);
        }

        void OnSceneLoadEnded()
        {
            loadingScreenPanel.SetActive(false);
        }
    }
}
