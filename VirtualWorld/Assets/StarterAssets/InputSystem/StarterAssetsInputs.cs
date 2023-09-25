using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		[field:SerializeField] public bool Sprint { get; private set; }
		public bool interact;
        public bool tablet;
        public bool action1;
		public bool menu;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM

		private void LateUpdate()
		{
			//ClearInteractInput();
			//Action1Input(false);
			//MenuInput(false);
		}

		private void OnDisable()
		{
			ZeroInputs();
		}

		public void ZeroInputs()
		{
			MoveInput(Vector2.zero);
			LookInput(Vector2.zero);
			JumpInput(false);
			SprintInput(false);
			InteractInput(false);
			Action1Input(false);
			MenuInput(false);
		}
		public void OnMove(InputAction.CallbackContext value)
		{
			MoveInput(value.ReadValue<Vector2>());
		}

		public void OnLook(InputAction.CallbackContext value)
		{
			if (cursorInputForLook)
			{
				LookInput(value.ReadValue<Vector2>());
			}
		}

		public void OnJump(InputAction.CallbackContext value)
		{
			JumpInput(value.performed);
		}

		public void OnSprint(InputAction.CallbackContext value)
		{
			Debug.Log("performed: " + value.performed);
			SprintInput(value.performed);
		}

		public void OnInteract(InputAction.CallbackContext value)
		{
			InteractInput(value.performed);
		}

        public void OnTablet(InputAction.CallbackContext value)
        {
            //Debug.Log("On tablet called " + Time.time);
            TabletInput(value.performed);
        }

        public void OnAction1(InputAction.CallbackContext value)
		{
			Action1Input(value.performed);
		}

		public void OnMenu(InputAction.CallbackContext value)
		{
			MenuInput(value.performed);
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		}

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			Sprint = newSprintState;
		}

		public void InteractInput(bool newInteractState)
		{
			interact = newInteractState;
		}

		public void ClearInteractInput()
		{
			interact = false;
		}

		public void ClearExecuteInputs()
		{
			action1 = false;
		}

        public void TabletInput(bool newTabletState)
        {
            tablet = newTabletState;
        }

        public void ClearTabletInput()
        {
            tablet = false;
        }

        public void Action1Input(bool newAction1State)
		{
			action1 = newAction1State;
		}

		public void MenuInput(bool newMenuState)
		{
			menu = newMenuState;
		}
#if UNITY_WEBGL

		private void Start()
		{
            LockCursor();
        }
#endif

        private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
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