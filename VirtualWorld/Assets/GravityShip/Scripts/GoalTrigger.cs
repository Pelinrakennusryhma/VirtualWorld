using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManagerGravityShip.Instance.OnlevelComplete();
            GameManagerGravityShip.Instance.GoToNextLevel();
            
        }
    }
}
