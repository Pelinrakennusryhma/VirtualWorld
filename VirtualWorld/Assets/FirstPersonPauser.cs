using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonPauser : MonoBehaviour
{
    public Options OptionsScreen;

    public bool IsPaused;

    void Awake()
    {
        
        OptionsScreen.gameObject.SetActive(false);
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        OptionsScreen.OnGameStarted();
        OnUnpause();
    }


    public void OnPause()
    {
        OptionsScreen.OnBecomeVisible();
        OptionsScreen.gameObject.SetActive(true);
        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;

        IsPaused = true;
        //NormalTimeScale = Time.timeScale;
        //Time.timeScale = 0;
        //Debug.Log("Should pause");
    }

    public void OnUnpause()
    {
        OptionsScreen.OnBecomeHidden();
        OptionsScreen.gameObject.SetActive(false);
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;

        IsPaused = false;
        //Time.timeScale = NormalTimeScale;
        //Debug.Log("Should unpause");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)
            || Input.GetKeyDown(KeyCode.O))
        {
            if (IsPaused)
            {
                OnUnpause();
            }

            else
            {
                OnPause();
            }
        }
    }
}
