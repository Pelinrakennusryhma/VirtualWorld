using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scenes;

public class TestSceneScript : MonoBehaviour
{
    [SerializeField] Button leaveButton;

    void Start()
    {
        if(SceneLoader.Instance != null)
        {
            leaveButton.onClick.AddListener(SceneLoader.Instance.UnloadScene);
        }
    }
}
