using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaygroundScene : MonoBehaviour
{
    public void DisablePlayground()
    {
        gameObject.SetActive(false);
    }

    public void EnablePlayground()
    {
        gameObject.SetActive(true);
    }
}
