using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scenes;

public class BackToWorldTrigger : MonoBehaviour
{
    public bool IsAlreadyExiting;

    private void Awake()
    {
        IsAlreadyExiting = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!IsAlreadyExiting 
            && other.gameObject.CompareTag("Player"))
        {
            IsAlreadyExiting = true;
            SceneLoader.Instance.UnloadScene();
            //Debug.Log("On trigger enter called");
        }
    }
}
