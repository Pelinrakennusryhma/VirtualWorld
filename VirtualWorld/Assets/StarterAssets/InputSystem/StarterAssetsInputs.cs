using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif
using Unity.Netcode;

namespace StarterAssets
{
	public class StarterAssetsInputs : NetworkBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool interact;
		public bool action1;

		[Header("Movement Settings")]
		public bool analogMovement;

#if !UNITY_IOS || !UNITY_ANDROID
		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
#endif


		public override void OnNetworkSpawn()
		{
			this.enabled = false;

			if (IsOwner)
			{
				this.enabled = true;
			}
		}

		private void LateUpdate()
		{
			ClearInteractInput();
			Action1Input(false);
		}

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
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
#else
	// old input sys if we do decide to have it (most likely wont)...
#endif


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

#if !UNITY_IOS || !UNITY_ANDROID

        private void OnApplicationFocus(bool hasFocus)
		{
			//SetCursorState(cursorLocked);

			if (IsOwner) 
			{
                PlayerInput playerInput = GetComponent<PlayerInput>();
                playerInput.enabled = false;
                playerInput.enabled = true;
            }
			SetCursorState(false);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

#endif

	}
	
}