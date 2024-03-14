using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scenes;

public class BackToWorldTrigger2 : MonoBehaviour
{
    public bool IsAlreadyExiting;

    private ScenePicker scenePicker;

    private void Awake()
    {
        IsAlreadyExiting = false;

        scenePicker = GetComponent<ScenePicker>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!IsAlreadyExiting
            && other.gameObject.CompareTag("Player"))
        {
            IsAlreadyExiting = true;
            SceneLoader.Instance.LoadScene(scenePicker.scenePath, new SceneLoadParams(ScenePackMode.ALL_BUT_PLAYER));
            //Debug.Log("On trigger enter called");
        }
    }
}
