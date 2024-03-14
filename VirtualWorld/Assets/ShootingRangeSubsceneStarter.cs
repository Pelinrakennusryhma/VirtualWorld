using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scenes;
using UnityEngine.SceneManagement;

public class ShootingRangeSubsceneStarter : MonoBehaviour
{
    private bool IsAlreadySwitchingScenes = false;

    private ScenePicker scenePicker;

    private void Awake()
    {
        IsAlreadySwitchingScenes = false;
        scenePicker = GetComponent<ScenePicker>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsAlreadySwitchingScenes
            && other.gameObject.CompareTag("Player"))
        {
            Debug.LogError("About to switch subscenes");

            IsAlreadySwitchingScenes = true;

            if (SceneLoader.Instance != null) 
            {
                SceneLoader.Instance.SwitchSubScenes(scenePicker.GetSceneName());
            }

            else
            {
                // To make it so (for now at least while the initial development is ongoing)
                // that we can launch the scenes in editor, without having to open a networked scene
                SceneManager.LoadScene(scenePicker.GetSceneName());
            }
        }
    }
}
