using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTriggerGravityShip : MonoBehaviour
{
    public float ReloadTimer;

    public bool IsVisible;

    public void Awake()
    {
        ReloadTimer = -1;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ReloadTimer = 2.0f;
            GameManagerGravityShip.Instance.OnPlayerDeath();
            PlayerControllerGravityShip.Instance.OnPlayerDeath();

            if (!IsVisible) 
            {
                //UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
                GameManagerGravityShip.Instance.OnShouldLoadPlayerDeadScene();
            }
        }
    }

    private void Update()
    {
        if (ReloadTimer >= 0.0f)
        {
            ReloadTimer -= Time.deltaTime;

            if (ReloadTimer <= 0)
            {
                //UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
                GameManagerGravityShip.Instance.OnShouldLoadPlayerDeadScene();
            }
        }
    }
}
