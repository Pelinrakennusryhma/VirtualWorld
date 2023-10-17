using Authentication;
using BackendConnection;
using Characters;
using FishNet.Object;
using FishNet.Connection;
using Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
namespace Quests
{
    public class QuestManager : NetworkBehaviour
    {
        public static QuestManager Instance { get; private set; }
        List<ActiveQuest> activeQuests = new List<ActiveQuest>();
        List<Quest> completedQuests = new List<Quest>();
        public ActiveQuest FocusedQuest { get; private set; }

        // this should be added to settings for player to toggle
        public bool autoFocusQuest = true;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            PlayerEvents.Instance.EventQuestCompleted.AddListener(OnQuestCompleted);
            PlayerEvents.Instance.EventActiveQuestUpdated.AddListener(OnActiveQuestUpdated);
            PlayerEvents.Instance.EventActiveQuestStepUpdated.AddListener(OnActiveQuestStepUpdated);

            PlayerEvents.Instance.EventCharacterDataSet.AddListener(OnCharacterDataLoaded);
        }

        void OnCharacterDataLoaded(CharacterData data)
        {
            foreach (ActiveQuestData questData in data.quests.activeQuests)
            {
                ActiveQuest loadedQuest = new ActiveQuest(questData);
            }
        }

        void OnQuestCompleted(Quest quest)
        {
            foreach (ActiveQuest activeQuest in activeQuests)
            {
                if (activeQuest.Quest == quest)
                {
                    RemoveActiveQuest(activeQuest);
                    completedQuests.Add(quest);
                    PlayerEvents.Instance.CallEventInformationReceived($"Completed Quest \"{quest.title}\"");

                    if (activeQuest == FocusedQuest)
                    {
                        ResetFocusedQuest();
                    }

                    break;
                }
            }
        }

        void OnActiveQuestUpdated(ActiveQuest quest)
        {
            if (quest == FocusedQuest)
            {
                PlayerEvents.Instance.CallEventFocusedQuestUpdated(quest);
            }
        }

        void OnActiveQuestStepUpdated(ActiveQuestStep step)
        {
            if (FocusedQuest != null && step == FocusedQuest.CurrentStep)
            {
                PlayerEvents.Instance.CallEventFocusedQuestUpdated(FocusedQuest);
            }
        }

        public void AcceptQuest(Quest quest)
        {
            ActiveQuest activeQuest = new ActiveQuest(quest);
            PlayerEvents.Instance.CallEventInformationReceived($"Started Quest \"{quest.title}\"");

            if (autoFocusQuest)
            {
                SetFocusedQuest(activeQuest);
            }

            AddActiveQuest(activeQuest);
        }

        /// <summary>
        /// Checks whether the quest should be shown as an acceptable one, 
        /// e.g. it's not already completed or picked up and prerequisite has been completed.
        /// </summary>
        public bool CanAcceptQuest(Quest quest)
        {
            bool canAccept = true;

            if (activeQuests.Find(q => q.Quest == quest) != null || completedQuests.Contains(quest))
            {
                canAccept = false;
            }

            if (quest.preRequisiteQuest != null && !completedQuests.Contains(quest.preRequisiteQuest))
            {
                canAccept = false;
            }

            return canAccept;
        }

        public bool IsOnQuestStep(QuestStep step)
        {
            foreach (ActiveQuest activeQuest in activeQuests)
            {
                if (activeQuest.CurrentStep.QuestStep == step)
                {
                    return true;
                }
            }
            return false;
        }

        public void ProgressStep(QuestStep step, int byAmount)
        {
            PlayerEvents.Instance.CallEventQuestStepProgressed(step, byAmount);
        }

        public void ClearQuests()
        {
            activeQuests.Clear();
            completedQuests.Clear();
            ResetFocusedQuest();
        }

        void SetFocusedQuest(ActiveQuest quest)
        {
            FocusedQuest = quest;
            PlayerEvents.Instance.CallEventFocusedQuestUpdated(quest);
        }

        void ResetFocusedQuest()
        {
            FocusedQuest = null;
            PlayerEvents.Instance.CallEventFocusedQuestUpdated(null);
        }

        ActiveQuestData CreateActiveQuestData(ActiveQuest quest)
        {
            return new ActiveQuestData(quest.Quest.name, quest.CurrentStepId, quest.CurrentStep.completedObjectives);
        }

        #region Methods for API interactions - ADD Active Quest
        void AddActiveQuest(ActiveQuest newQuest)
        {
            activeQuests.Add(newQuest);
            ActiveQuestData data = CreateActiveQuestData(newQuest);
            AddActiveQuestServerRpc(LocalConnection, UserSession.Instance.LoggedUserData.id, data);
        }

        [ServerRpc(RequireOwnership = false)]
        void AddActiveQuestServerRpc(NetworkConnection conn, string userId, ActiveQuestData data)
        {
            APICalls_Server.Instance.AddActiveQuest(conn, userId, data, AddActiveQuestTargetRpc);
        }

        [TargetRpc]
        public void AddActiveQuestTargetRpc(NetworkConnection conn, ActiveQuestData quest)
        {
            Debug.Log("client got notified about saved quest: " + quest.id);
        }
        #endregion

        #region Methods for API interactions - Remove Active Quest
        void RemoveActiveQuest(ActiveQuest quest)
        {
            activeQuests.Remove(quest);
            ActiveQuestData data = CreateActiveQuestData(quest);
            RemoveActiveQuestServerRpc(LocalConnection, UserSession.Instance.LoggedUserData.id, data);
        }
        [ServerRpc(RequireOwnership = false)]
        void RemoveActiveQuestServerRpc(NetworkConnection conn, string userId, ActiveQuestData data)
        {
            APICalls_Server.Instance.RemoveActiveQuest(conn, userId, data);
        }
        #endregion

    }
}
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
