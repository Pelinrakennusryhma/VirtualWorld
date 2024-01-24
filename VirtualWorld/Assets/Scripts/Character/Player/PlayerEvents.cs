using BackendConnection;
using Items;
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
        #region Player Character - movement etc.
        public UnityEvent EventPlayerLanded;

        public void CallEventPlayerLanded()
        {
            EventPlayerLanded.Invoke();
        }
        #endregion

        #region Character Data - backend data etc.
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

        public UnityEvent<List<InventoryItem>> EventInventoryChanged;

        public void CallEventInventoryChanged(List<InventoryItem> items)
        {
            EventInventoryChanged.Invoke(items);
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

        public UnityEvent<NPC, Quest> EventDialogOpened;
        public void CallEventDialogOpened(NPC npc, Quest quest = null)
        {
            EventDialogOpened.Invoke(npc, quest);
            CharacterManagerNonNetworked.Instance.SetGameState(GAME_STATE.DIALOG);
        }

        public UnityEvent EventDialogClosed;
        public void CallEventDialogClosed()
        {
            EventDialogClosed.Invoke();
            CharacterManagerNonNetworked.Instance.SetGameState(GAME_STATE.FREE);
        }

        #endregion

        #region Interaction
        public UnityEvent<I_Interactable, GameObject> EventInteractableDetected;
        
        public void CallEventInteractableDetected(I_Interactable interactable, GameObject interactableGO)
        {
            EventInteractableDetected.Invoke(interactable, interactableGO);
        }

        public UnityEvent EventInteractionStarted;

        public void CallEventInteractionStarted()
        {
            EventInteractionStarted.Invoke();
        }

        public UnityEvent<I_Interactable, GameObject> EventInteractionEnded;
        public void CallEventInteractionEnded(I_Interactable interactable, GameObject interactableGO)
        {
            EventInteractionEnded.Invoke(interactable, interactableGO);
        }

        public UnityEvent EventInteractableLost;
        public void CallEventInteractableLost()
        {
            EventInteractableLost.Invoke();
        }
        #endregion

        #region Quests

        public UnityEvent<Quest> EventQuestCompleted;

        public void CallEventQuestCompleted(Quest quest)
        {
            EventQuestCompleted.Invoke(quest);
        }

        public UnityEvent<QuestStep, int> EventQuestStepProgressed;

        public void CallEventQuestStepProgressed(QuestStep step, int byAmount)
        {
            Debug.Log("Invoked for step: " + step.name);
            EventQuestStepProgressed.Invoke(step, byAmount);
        }

        public UnityEvent<QuestStep> EventQuestStepCompleted;

        public void CallEventQuestStepCompleted(QuestStep step)
        {
            EventQuestStepCompleted.Invoke(step);
        }

        public UnityEvent<ActiveQuest> EventFocusedQuestUpdated;

        public void CallEventFocusedQuestUpdated(ActiveQuest quest)
        {
            EventFocusedQuestUpdated.Invoke(quest);
        }

        public UnityEvent<ActiveQuest> EventActiveQuestUpdated;

        public void CallEventActiveQuestUpdated(ActiveQuest quest)
        {
            EventActiveQuestUpdated.Invoke(quest);
        }

        public UnityEvent<ActiveQuestStep> EventActiveQuestStepUpdated;

        public void CallEventActiveQuestStepUpdated(ActiveQuestStep step)
        {
            EventActiveQuestStepUpdated.Invoke(step);
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
