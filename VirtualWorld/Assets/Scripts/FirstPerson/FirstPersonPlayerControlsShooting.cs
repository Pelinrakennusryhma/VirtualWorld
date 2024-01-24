using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Scenes;

public class FirstPersonPlayerControlsShooting : MonoBehaviour
{

    public float Horizontal;
    public float Vertical;
    public float MouseDeltaX;
    public float MouseDeltaY;

    public bool JumpDownPressed;
    public bool CrouchDown;
    public bool RunDown;
    public bool Fire1Down;
    public bool OptionsDown;
    public bool InteractDown;
    public bool Alpha1Down;
    public bool Alpha2Down;
    public bool Alpha3Down;
    public bool Alpha4Down;
    public bool Alpha5Down;
    public bool Alpha6Down;
    public bool Alpha7Down;
    public bool Alpha8Down;
    public bool Alpha9Down;
    public bool Alpha0Down;


    private PlayerInput playerInput;

    private OptionsShooting options;

    public void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Debug.Log("Should hide and lock cursor");

        playerInput = GetComponent<PlayerInput>();
        //playerInput.SwitchCurrentActionMap("FirstPersonControls");
        //Debug.Log("Current action map is " + playerInput.currentActionMap.ToString());
        //UICanvas.gameObject.SetActive(true);


        OptionsShooting.OnLaunch();        
        options = FindObjectOfType<OptionsShooting>(true);
        options.OnGameStarted();


        //optionScreen.gameObject.SetActive(false);

        //if (GameManager.Instance.CurrentSceneType == GameManager.TypeOfScene.AsteroidField) 
        //{
        //    GameEvents.Instance.EventPlayerLeftAsteroid.AddListener(OnLeaveAsteroid);
        //    Debug.Log("Listener added to on leftasteroid" + Time.time);
        //}

        //else
        //{
        //    Debug.LogWarning("We have a corresponding event for leaving an asteroid, but not one for planets or other future scenes");
        //}
    }



    public void OnHorizontal(InputAction.CallbackContext value)
    {
        Horizontal = value.ReadValue<float>();
        //Debug.Log("Pressed horizontal " + Horizontal);
    }

    public void OnVertical(InputAction.CallbackContext value)
    {
        Vertical = value.ReadValue<float>();
        //Debug.Log("Pressed vertical " + Vertical);
    }

    public void OnMouseMove(InputAction.CallbackContext value)
    {
        MouseDeltaX = value.ReadValue<Vector2>().x;
        MouseDeltaY = value.ReadValue<Vector2>().y;
        //MouseDeltaX = delta.x;
        //MouseDeltaY = delta.y;

        //Debug.Log("On mouse move " + value);
    }

    public void OnJumpPressed(InputAction.CallbackContext value)
    {
        float valueFloat = value.ReadValue<float>();

        if (valueFloat > 0)
        {
            JumpDownPressed = true;
            Debug.Log("Pressed jump ");
        }

        else
        {
            JumpDownPressed = false;
        }


        //Debug.Log("Pressed JUMP");
    }

    public void OnCrouchDown(InputAction.CallbackContext value)
    {
        float valueFloat = value.ReadValue<float>();

        if (valueFloat > 0)
        {
            CrouchDown = true;
        }

        else
        {
            CrouchDown = false;
        }
        //Debug.Log("Pressed CROUCH");
    }

    public void OnRunDown(InputAction.CallbackContext value)
    {
        float valueFloat = value.ReadValue<float>();

        if (valueFloat > 0)
        {
            RunDown = true;
        }

        else
        {
            RunDown = false;
        }

        //Debug.Log("Pressed RUN");
    }

    public void OnAlpha1Down(InputAction.CallbackContext value)
    {
        float valueFloat = value.ReadValue<float>();

        if (valueFloat > 0)
        {
           Alpha1Down = true;
        }

        else
        {
            Alpha1Down = false;
        }

        //Debug.Log("Pressed RUN");
    }

    public void OnAlpha2Down(InputAction.CallbackContext value)
    {
        float valueFloat = value.ReadValue<float>();

        if (valueFloat > 0)
        {
            Alpha2Down = true;
        }

        else
        {
            Alpha2Down = false;
        }

        //Debug.Log("Pressed RUN");
    }

    public void OnAlpha3Down(InputAction.CallbackContext value)
    {
        float valueFloat = value.ReadValue<float>();

        if (valueFloat > 0)
        {
            Alpha3Down = true;
        }

        else
        {
            Alpha3Down = false;
        }

        //Debug.Log("Pressed RUN");
    }

    public void OnAlpha4Down(InputAction.CallbackContext value)
    {
        float valueFloat = value.ReadValue<float>();

        if (valueFloat > 0)
        {
            Alpha4Down = true;
        }

        else
        {
            Alpha4Down = false;
        }

        //Debug.Log("Pressed RUN");
    }

    public void OnAlpha5Down(InputAction.CallbackContext value)
    {
        float valueFloat = value.ReadValue<float>();

        if (valueFloat > 0)
        {
            Alpha5Down = true;
        }

        else
        {
            Alpha5Down = false;
        }

        //Debug.Log("Pressed RUN");
    }

    public void OnAlpha6Down(InputAction.CallbackContext value)
    {
        float valueFloat = value.ReadValue<float>();

        if (valueFloat > 0)
        {
            Alpha6Down = true;
        }

        else
        {
            Alpha6Down = false;
        }

        //Debug.Log("Pressed RUN");
    }

    public void OnAlpha7Down(InputAction.CallbackContext value)
    {
        float valueFloat = value.ReadValue<float>();

        if (valueFloat > 0)
        {
            Alpha7Down = true;
        }

        else
        {
            Alpha7Down = false;
        }

        //Debug.Log("Pressed RUN");
    }
    public void OnAlpha8Down(InputAction.CallbackContext value)
    {
        float valueFloat = value.ReadValue<float>();

        if (valueFloat > 0)
        {
            Alpha8Down = true;
        }

        else
        {
            Alpha8Down = false;
        }

        //Debug.Log("Pressed RUN");
    }
    public void OnAlpha9Down(InputAction.CallbackContext value)
    {
        float valueFloat = value.ReadValue<float>();

        if (valueFloat > 0)
        {
            Alpha9Down = true;
        }

        else
        {
            Alpha9Down = false;
        }

        //Debug.Log("Pressed RUN");
    }
    public void OnAlpha0Down(InputAction.CallbackContext value)
    {
        float valueFloat = value.ReadValue<float>();

        if (valueFloat > 0)
        {
            Alpha0Down = true;
        }

        else
        {
            Alpha0Down = false;
        }

        //Debug.Log("Pressed RUN");
    }

    public void OnFire1Pressed(InputAction.CallbackContext value)
    {
        float valueFloat = value.ReadValue<float>();

        if (valueFloat > 0)
        {
            Fire1Down = true;
        }

        else
        {
            Fire1Down = false;
        }
        //Debug.Log("On fire 1 pressed");
    }

    public void OnInteractPressed(InputAction.CallbackContext value)
    {
        float valueFloat = value.ReadValue<float>();

        if (valueFloat > 0)
        {
            InteractDown = true;
        }

        else
        {
            InteractDown = false;
        }
        //Debug.Log("On fire 1 pressed");
    }

    //public void OnOptions(InputAction.CallbackContext value)
    //{
    //    float valueFloat = value.ReadValue<float>();

    //    if (valueFloat > 0)
    //    {
    //        OptionsDown = true;
    //    }

    //    else
    //    {
    //        OptionsDown = false;
    //    }

    //    if (GameManager.Instance != null)
    //    {
    //        GameManager.Instance.OnPausePressed();
    //    }

    //    //Debug.Log("On options pressed");
    //}

    public void OnLeave(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //GameEvents.Instance.CallEventPlayerTriedLeaving();
        }
    }

    public void OnOptions(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // GameManager.Instance.OnOptionsPressed();

            //SceneLoader.Instance.UnloadScene();
            
            if (OptionsShooting.IsShowingOptions)
            {
                options.OnBecomeHidden();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

            else
            {
                options.OnBecomeVisible();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            //Debug.Log("Options pressed");
        }
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        //if (context.performed)
        //{
        //   GameManager.Instance.OnInventoryPressed(context);
        // Debug.Log("On inventory pressed");
        //}

        if (context.performed) 
        {
            //GameManager.Instance.OnInventoryPressed();
            Debug.Log("Inventory pressed");
        }

        //Debug.Log("On invenotry pressed");
    }

    public void OnLeaveScene(InputAction.CallbackContext context)
    {
        //if (leavePlanetScene == null)
        //{
        //    //leavePlanetScene = FindObjectOfType<ButtonHeldAction>();
        //}

        //if (context.started)
        //{
        //    //leavePlanetScene.OnButtonPressed();
        //}
        //else if (context.canceled)
        //{
        //    //leavePlanetScene.OnButtonReleased();
        //}
    }

    public void LateUpdate()
    {
        ClearControls();
    }

    public void ClearControls()
    {
        //RunDown = false;
        //Fire1Down = false;
        OptionsDown = false;
        InteractDown = false;
        //CrouchDown = false;
        JumpDownPressed = false;
    }
}
