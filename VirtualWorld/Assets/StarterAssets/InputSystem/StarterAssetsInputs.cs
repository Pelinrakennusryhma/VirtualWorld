using Characters;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Quests;
using System;
using UnityEngine;
using UnityEngine.Events;
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

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM

        private void Start()
        {
            PlayerEvents.Instance.EventGameStateChanged.AddListener(OnGameStateChanged);

            LockCursor();
        }

        void OnGameStateChanged(GAME_STATE newState)
        {
            switch (newState)
            {
                case GAME_STATE.FREE:
                    LockCursor();
                    break;
                case GAME_STATE.MENU:
                    UnlockCursor();
                    ZeroInputs();
                    break;
                case GAME_STATE.TABLET:
                    UnlockCursor();
                    ZeroInputs();
                    break;
                case GAME_STATE.DIALOG:
                    UnlockCursor();
                    ZeroInputs();
                    break;
                case GAME_STATE.MINIGAME:
                    UnlockCursor();
                    ZeroInputs();
                    break;
                default:
                    break;
            }
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
            TabletInput(false);
		}
		public void OnMove(InputAction.CallbackContext value)
		{
			switch (CharacterManager.Instance.gameState)
			{
				case GAME_STATE.FREE:
                    MoveInput(value.ReadValue<Vector2>());
                    break;
				case GAME_STATE.MENU:
					break;
				case GAME_STATE.TABLET:
					break;
                case GAME_STATE.DIALOG:
                    break;
				default:
					break;
			}
		}

		public void OnLook(InputAction.CallbackContext value)
		{
            switch (CharacterManager.Instance.gameState)
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
                case GAME_STATE.DIALOG:
                    break;
                default:
                    break;
            }
		}

		public void OnJump(InputAction.CallbackContext value)
		{
            switch (CharacterManager.Instance.gameState)
            {
                case GAME_STATE.FREE:
                    JumpInput(value.performed);
                    break;
                case GAME_STATE.MENU:
                    break;
                case GAME_STATE.TABLET:
                    break;
                case GAME_STATE.DIALOG:
                    break;
                default:
                    break;
            }
        }

		public void OnSprint(InputAction.CallbackContext value)
		{
            switch (CharacterManager.Instance.gameState)
            {
                case GAME_STATE.FREE:
                    SprintInput(value.performed);
                    break;
                case GAME_STATE.MENU:
                    break;
                case GAME_STATE.TABLET:
                    break;
                case GAME_STATE.DIALOG:
                    break;
                default:
                    break;
            }
        }

		public void OnInteract(InputAction.CallbackContext value)
		{	
            switch (CharacterManager.Instance.gameState)
            {
                case GAME_STATE.FREE:
                    InteractInput(value.performed);
                    break;
                case GAME_STATE.MENU:
                    break;
                case GAME_STATE.TABLET:
                    break;
                case GAME_STATE.DIALOG:
                    break;
                default:
                    break;
            }
        }

        public void OnTablet(InputAction.CallbackContext value)
        {
            if (value.action.WasPerformedThisFrame())
            {
                switch (CharacterManager.Instance.gameState)
                {
                    case GAME_STATE.FREE:
                        ZeroInputs();
                        CharacterManager.Instance.SetGameState(GAME_STATE.TABLET);
                        PlayerEvents.Instance.CallEventOpenTabletPressed();
                        break;
                    case GAME_STATE.MENU:
                        break;
                    case GAME_STATE.TABLET:
                        PlayerEvents.Instance.CallEventCloseTabletPressed();
                        break;
                    case GAME_STATE.DIALOG:
                        break;
                    default:
                        break;
                }
            }
        }

        public void OnAction1(InputAction.CallbackContext value)
		{
            switch (CharacterManager.Instance.gameState)
            {
                case GAME_STATE.FREE:
                    Action1Input(value.performed);
                    break;
                case GAME_STATE.MENU:
                    break;
                case GAME_STATE.TABLET:
                    break;
                case GAME_STATE.DIALOG:
                    break;
                default:
                    break;
            }
        }

		public void OnMenu(InputAction.CallbackContext value)
		{
			if(value.action.WasPerformedThisFrame())
			{
                switch (CharacterManager.Instance.gameState)
                {
                    case GAME_STATE.FREE:
                        CharacterManager.Instance.SetGameState(GAME_STATE.MENU);
                        break;
                    case GAME_STATE.MENU:
                        CharacterManager.Instance.SetGameState(GAME_STATE.FREE);
                        break;
                    case GAME_STATE.TABLET:
                        PlayerEvents.Instance.CallEventCloseTabletPressed();
                        break;
                    case GAME_STATE.DIALOG:
                        CharacterManager.Instance.SetGameState(GAME_STATE.FREE);
                        break;
                    default:
                        break;
                }
            }
		}

        public void OnToggleFocusedQuest(InputAction.CallbackContext value)
        {
            if (value.action.WasPerformedThisFrame())
            {
                switch (CharacterManager.Instance.gameState)
                {
                    case GAME_STATE.FREE:
                        QuestManager.Instance.ToggleFocusedQuest();
                        break;
                    case GAME_STATE.MENU:
                        break;
                    case GAME_STATE.TABLET:
                        break;
                    case GAME_STATE.DIALOG:
                        break;
                    default:
                        break;
                }
            }
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