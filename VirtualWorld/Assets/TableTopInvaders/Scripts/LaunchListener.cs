using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchListener : MonoBehaviour
{
    void Awake()
    {
        GameFlowManager.LaunchingNormalScene = true; 
    }

    void Start()
    {
        GameFlowManager.NormalSceneIsLaunched();
    }
}
