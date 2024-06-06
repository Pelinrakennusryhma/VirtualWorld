using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterParkStopAreaTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("We entered stop area");

            RaftGameManager.Instance.OnEnterStopArea();
        }
    }
}
