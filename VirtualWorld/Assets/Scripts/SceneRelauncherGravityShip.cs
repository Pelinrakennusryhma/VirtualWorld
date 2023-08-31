using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneRelauncherGravityShip : MonoBehaviour
{
    // Start is called before the first frame update
    private float Timer;

    void Start()
    {
        Timer = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;

        if (Timer <= 0)
        {
            GameManagerGravityShip.Instance.RelaunchPreviousScene();
        }
    }
}
