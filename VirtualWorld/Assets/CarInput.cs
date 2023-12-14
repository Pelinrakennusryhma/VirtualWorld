using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Vehicles
{
    public class CarInput : MonoBehaviour
    {
        [Header("Car Input Values")]
        public Vector2 move;
        public bool interact;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM

        private void OnDisable()
        {
            ZeroInputs();
        }

        public void ZeroInputs()
        {
            MoveInput(Vector2.zero);
            InteractInput(false);
        }

        public void OnMove(InputAction.CallbackContext value)
        {
            MoveInput(value.ReadValue<Vector2>());
        }

        public void OnInteract(InputAction.CallbackContext value)
        {
            InteractInput(value.performed);
        }

#endif


        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void InteractInput(bool newInteractState)
        {
            interact = newInteractState;
        }

        public void ClearInteractInput()
        {
            interact = false;
        }

        public void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
