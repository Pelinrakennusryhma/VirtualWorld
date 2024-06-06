using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterParkFinishLineTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Finish line got triggered");
            RaftGameManager.Instance.OnFinishLineReached();
        }
    }
}
