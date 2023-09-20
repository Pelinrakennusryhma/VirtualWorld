using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelaunchListener : MonoBehaviour
{
    void Awake()
    {
        if (GameFlowManager.RelaunchingCustomScene)
        {
            GameFlowManager.OnRelaunchedCustomPositionsScene();
        }

        else
        {
            GameFlowManager.LaunchCustomPositionsScene();
        }
    }
}
