using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public bool IsPaused;
    public float NormalTimeScale;
    public Options OptionsScreen;



    public void Awake()
    {
        if (Instance == null)
        {
            //Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;

            Instance = this;
            DontDestroyOnLoad(gameObject);

            //OptionsScreen = FindObjectOfType<Options>(true);
            //OptionsScreen.gameObject.SetActive(false);
            //OptionsScreen.OnGameStarted();

            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        else
        {
            Destroy(gameObject);
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //OptionsScreen = FindObjectOfType<Options>(true);

        //if (OptionsScreen != null) 
        //{
        //    OptionsScreen.gameObject.SetActive(false);
        //    OptionsScreen.OnGameStarted();
        //}

        //else
        //{
        //    Debug.LogError("Options screen is null");
        //}

        //Debug.Log("Scene loaded, should shut down options");
    }

    // Update is called once per frame
    void Update()
    {
        //return;

        //if (Input.GetKeyDown(KeyCode.Escape)
        //    || Input.GetKeyDown(KeyCode.O))
        //{
        //    if (IsPaused) 
        //    {
        //        OnUnpause();
        //    }

        //    else
        //    {
        //        OnPause();
        //    }
        //}
    }

    //public void OnPause()
    //{
    //    OptionsScreen.OnBecomeVisible();
    //    OptionsScreen.gameObject.SetActive(true);
    //    Cursor.visible = true;
    //    Cursor.lockState = CursorLockMode.None;

    //    IsPaused = true;
    //    NormalTimeScale = Time.timeScale;
    //    Time.timeScale = 0;
    //    //Debug.Log("Should pause");
    //}

    //public void OnUnpause()
    //{
    //    OptionsScreen.OnBecomeHidden();
    //    OptionsScreen.gameObject.SetActive(false);
    //    Cursor.visible = false;
    //    Cursor.lockState = CursorLockMode.Locked;

    //    IsPaused = false;
    //    Time.timeScale = NormalTimeScale;
    //    //Debug.Log("Should unpause");
    //}

    public void SetMouseSensitivity(float sensitivity)
    {
        
    }

    public void SetMouseInvert(bool invert)
    {

    }
}
