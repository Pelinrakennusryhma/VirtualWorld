using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RaftControls : MonoBehaviour
{
    public float Horizontal;
    public float Vertical;
    public bool OptionsDown;

    private PlayerInput playerInput;

    public void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Debug.Log("Should hide and lock cursor");

        playerInput = GetComponent<PlayerInput>();
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        Horizontal = value.ReadValue<Vector2>().x;
        Vertical = value.ReadValue<Vector2>().y;
    }

    public void OnOptions(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OptionsDown = true;
        }

        else
        {
            OptionsDown = false;
        }
    }

    public void LateUpdate()
    {
        if (OptionsDown)
        {
            RaftGameManager.Instance.GoToLobby();
        }

        ClearControls();
    }

    public void ClearControls()
    {
        OptionsDown = false;
    }
}
