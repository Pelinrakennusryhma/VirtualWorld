using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetter : MonoBehaviour
{
    public int Level;

    private void Start()
    {
        GameManagerGravityShip.Instance.CurrentLevel = Level;
    }
}
