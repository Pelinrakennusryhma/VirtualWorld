using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

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
		public bool action1;
		public bool menu;

		[Header("Movement Settings")]
		public bool analogMovement;

#if !UNITY_IOS || !UNITY_ANDROID
		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
#endif

		//public override void OnNetworkSpawn()
		//{
		//	this.enabled = false;

		//	if (IsOwner)
		//	{
		//		this.enabled = true;
		//	}
		//}

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
			if(cursorInputForLook)
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

        public void OnAction1(InputValue value)
        {
			Action1Input(value.isPressed);
        }

		public void OnMenu(InputValue value)
		{
			MenuInput(value.isPressed);
		}
        public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
			//move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
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

        public void Action1Input(bool newAction1State)
        {
            action1 = newAction1State;
        }

		public void MenuInput(bool newMenuState)
		{
			menu = newMenuState;
		}

#if !UNITY_IOS || !UNITY_ANDROID || !UNITY_SERVER

        private void OnApplicationFocus(bool hasFocus)
		{
			//SetCursorState(cursorLocked);

			//if (IsOwner) 
			//{
   //             PlayerInput playerInput = GetComponent<PlayerInput>();
   //             playerInput.enabled = false;
   //             playerInput.enabled = true;
   //         }
			//SetCursorState(false);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

#endif

	}
	
}