using BackendConnection;
using Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WorldObjects;

namespace Characters
{
    public class PlayerEvents : MonoBehaviour
    {
        public static PlayerEvents Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        #region CharacterData
        public UnityEvent<CharacterData> EventCharacterDataSet;
        public void CallEventCharacterDataSet(CharacterData data)
        {
            EventCharacterDataSet.Invoke(data);
        }
        #endregion

        #region Inventory
        public UnityEvent<InventoryItem> EventMoneyAmountChanged;
        public void CallEventMoneyAmountChanged(InventoryItem inventoryItem)
        {
            EventMoneyAmountChanged.Invoke(inventoryItem);
        }

        #endregion

        #region GameState
        public UnityEvent<GAME_STATE> EventGameStateChanged;
        public void CallEventGameStateChanged(GAME_STATE newState)
        {
            EventGameStateChanged.Invoke(newState);
        }

        public UnityEvent EventOpenTabletPressed;
        public void CallEventOpenTabletPressed()
        {
            EventOpenTabletPressed.Invoke();
        }

        public UnityEvent EventCloseTabletPressed;
        public void CallEventCloseTabletPressed()
        {
            EventCloseTabletPressed.Invoke();
        }

        public UnityEvent<NPC> EventDialogOpened;
        public void CallEventDialogOpened(NPC npc)
        {
            EventDialogOpened.Invoke(npc);
            CharacterManager.Instance.SetGameState(GAME_STATE.DIALOG);
        }

        public UnityEvent EventDialogClosed;
        public void CallEventDialogClosed()
        {
            EventDialogClosed.Invoke();
            CharacterManager.Instance.SetGameState(GAME_STATE.FREE);
        }

        #endregion

        #region Interaction
        public UnityEvent<I_Interactable, GameObject> EventInteractableDetected;
        
        public void CallEventInteractableDetected(I_Interactable interactable, GameObject interactableGO)
        {
            EventInteractableDetected.Invoke(interactable, interactableGO);
        }
        public UnityEvent EventInteractableLost;
        public void CallEventInteractableLost()
        {
            EventInteractableLost.Invoke();
        }
        public UnityEvent EventInteractionStarted;
        public void CallEventInteractionStarted()
        {
            EventInteractionStarted.Invoke();
        }
        #endregion

        #region Quests

        public UnityEvent<Quest> EventQuestAccepted;

        public void CallEventQuestAccepted(Quest quest)
        {
            EventQuestAccepted.Invoke(quest);
        }
        #endregion

        #region UIDisplay
        public UnityEvent<string> EventInformationReceived;

        public void CallEventInformationReceived(string info)
        {
            EventInformationReceived.Invoke(info);
        }
        #endregion
    }
}
