using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnToPreviousPositions : MonoBehaviour
{
    public enum PinType
    {
        None = 0,
        Minus2 = 1,
        Minus5 = 2,
        Plus1 = 3,
        Plus2 = 4,
        Plus5 = 5,
        Plus10 = 6
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameManagerTabletopInvaders.instance != null)
        {
            Debug.Log("We have a gamemanager! Someone has launched custom scene.");
        }
    }


}
