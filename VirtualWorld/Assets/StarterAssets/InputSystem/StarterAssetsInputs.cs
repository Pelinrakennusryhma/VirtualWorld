using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using System;
using UnityEngine;
using UnityEngine.Events;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public enum GAME_STATE
	{
		FREE,
		MENU,
		TABLET
	}
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

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		public GAME_STATE gameState = GAME_STATE.FREE;

		public UnityEvent EventMenuPressed;
		public UnityEvent EventOpenTabletPressed;
		public UnityEvent<UnityAction> EventCloseTabletPressed;

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
		}
		public void OnMove(InputAction.CallbackContext value)
		{
			switch (gameState)
			{
				case GAME_STATE.FREE:
                    MoveInput(value.ReadValue<Vector2>());
                    break;
				case GAME_STATE.MENU:
					break;
				case GAME_STATE.TABLET:
					break;
				default:
					break;
			}
		}

		public void OnLook(InputAction.CallbackContext value)
		{
            switch (gameState)
            {
                case GAME_STATE.FREE:
                    if (cursorInputForLook)
                    {
                        LookInput(value.ReadValue<Vector2>());
                    }
                    break;
                case GAME_STATE.MENU:
                    break;
                case GAME_STATE.TABLET:
                    break;
                default:
                    break;
            }

		}

		public void OnJump(InputAction.CallbackContext value)
		{
            switch (gameState)
            {
                case GAME_STATE.FREE:
                    JumpInput(value.performed);
                    break;
                case GAME_STATE.MENU:
                    break;
                case GAME_STATE.TABLET:
                    break;
                default:
                    break;
            }
        }

		public void OnSprint(InputAction.CallbackContext value)
		{
            switch (gameState)
            {
                case GAME_STATE.FREE:
                    SprintInput(value.performed);
                    break;
                case GAME_STATE.MENU:
                    break;
                case GAME_STATE.TABLET:
                    break;
                default:
                    break;
            }
        }

		public void OnInteract(InputAction.CallbackContext value)
		{	
            switch (gameState)
            {
                case GAME_STATE.FREE:
                    InteractInput(value.performed);
                    break;
                case GAME_STATE.MENU:
                    break;
                case GAME_STATE.TABLET:
                    break;
                default:
                    break;
            }
        }

        public void OnTablet(InputAction.CallbackContext value)
        {
            if (value.action.WasPerformedThisFrame())
            {
                switch (gameState)
                {
                    case GAME_STATE.FREE:
                        ZeroInputs();
                        SetGameState(GAME_STATE.TABLET);
                        EventOpenTabletPressed.Invoke();
                        break;
                    case GAME_STATE.MENU:
                        break;
                    case GAME_STATE.TABLET: // callback function to set gamestate once tablet script is done zooming out
                        EventCloseTabletPressed.Invoke(() => SetGameState(GAME_STATE.FREE));
                        break;
                    default:
                        break;
                }
            }
        }

        public void OnAction1(InputAction.CallbackContext value)
		{
            switch (gameState)
            {
                case GAME_STATE.FREE:
                    Action1Input(value.performed);
                    break;
                case GAME_STATE.MENU:
                    break;
                case GAME_STATE.TABLET:
                    break;
                default:
                    break;
            }
        }

		public void OnMenu(InputAction.CallbackContext value)
		{
			if(value.action.WasPerformedThisFrame())
			{
                switch (gameState)
                {
                    case GAME_STATE.FREE:
                        ZeroInputs();
                        EventMenuPressed.Invoke();
                        SetGameState(GAME_STATE.MENU);
                        break;
                    case GAME_STATE.MENU:
                        EventMenuPressed.Invoke();
                        SetGameState(GAME_STATE.FREE);
                        break;
                    case GAME_STATE.TABLET:
                        EventCloseTabletPressed.Invoke(() => SetGameState(GAME_STATE.FREE));
                        break;
                    default:
                        break;
                }
            }
		}

        public void SetGameState(GAME_STATE newState)
        {
            gameState = newState;
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