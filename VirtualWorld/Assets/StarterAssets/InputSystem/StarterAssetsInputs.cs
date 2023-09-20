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
		public bool sprint;
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
			ClearInteractInput();
			Action1Input(false);
			MenuInput(false);
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
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if (cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnInteract(InputValue value)
		{
			InteractInput(value.isPressed);
		}

        public void OnTablet(InputValue value)
        {
            //Debug.Log("On tablet called " + Time.time);
            TabletInput(value.isPressed);
        }

        public void OnAction1(InputValue value)
		{
			Action1Input(value.isPressed);
		}

		public void OnMenu(InputValue value)
		{
			MenuInput(value.isPressed);
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
			sprint = newSprintState;
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

		private void OnApplicationFocus(bool hasFocus)
		{
			// SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}

}