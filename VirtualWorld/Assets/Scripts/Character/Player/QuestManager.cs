using Authentication;
using BackendConnection;
using Characters;
using FishNet.Object;
using FishNet.Connection;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
namespace Quests
{
    public class QuestManager : NetworkBehaviour
    {
        [SerializeField] QuestDatabase questDatabase;
        public static QuestManager Instance { get; private set; }
        public List<ActiveQuest> ActiveQuests { get; private set; } = new List<ActiveQuest>();
        public List<Quest> CompletedQuests { get; private set; } = new List<Quest>();
        public ActiveQuest FocusedQuest { get; private set; }

        // Stores all steps that get created with ActiveQuests.
        // Used for looking up and removing any listeners when quest gets abandoned
        // so that garbage collector can work.
        List<ActiveQuestStep> _createdSteps = new List<ActiveQuestStep>();

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
            questDatabase.Init();
            PlayerEvents.Instance.EventQuestCompleted.AddListener(OnQuestCompleted);
            PlayerEvents.Instance.EventActiveQuestUpdated.AddListener(OnActiveQuestUpdated);
            PlayerEvents.Instance.EventActiveQuestStepUpdated.AddListener(OnActiveQuestStepUpdated);

            PlayerEvents.Instance.EventCharacterDataSet.AddListener(OnCharacterDataLoaded);
        }

        void OnCharacterDataLoaded(CharacterData data)
        {
            foreach (ActiveQuestData questData in data.quests.activeQuests)
            {
                Quest quest = questDatabase.GetQuestById(questData.id);
                ActiveQuest loadedQuest = new ActiveQuest(quest, questData.step, questData.stepProgress);
                ActiveQuests.Add(loadedQuest);
            }

            foreach (CompletedQuestData questData in data.quests.completedQuests)
            {
                Quest quest = questDatabase.GetQuestById(questData.id);
                CompletedQuests.Add(quest);
            }

            foreach (ActiveQuest quest in ActiveQuests)
            {
                if(quest.Quest.name == data.quests.focusedQuest.id)
                {
                    FocusedQuest = quest;
                    PlayerEvents.Instance.CallEventFocusedQuestUpdated(FocusedQuest);
                }
            }
        }

        void OnQuestCompleted(Quest quest)
        {
            foreach (ActiveQuest activeQuest in ActiveQuests)
            {
                if (activeQuest.Quest == quest)
                {
                    bool resetFocused = activeQuest == FocusedQuest;
                    AddCompletedQuest(activeQuest, true, resetFocused);
                    PlayerEvents.Instance.CallEventInformationReceived($"Completed Quest \"{quest.title}\"");

                    break;
                }
            }

            if (autoFocusQuest)
            {
                if(ActiveQuests.Count > 0)
                {
                    SetFocusedQuest(ActiveQuests[0]);
                }
            }
        }

        void OnActiveQuestUpdated(ActiveQuest quest)
        {
            if (quest == FocusedQuest)
            {
                PlayerEvents.Instance.CallEventFocusedQuestUpdated(quest);
            }

            UpdateActiveQuest(quest);

        }

        void OnActiveQuestStepUpdated(ActiveQuestStep step)
        {
            if (FocusedQuest != null && step == FocusedQuest.CurrentStep)
            {
                PlayerEvents.Instance.CallEventFocusedQuestUpdated(FocusedQuest);
            }

            ActiveQuest questWithTheStep = FindActiveQuestWithActiveStep(step);

            if (questWithTheStep != null)
            {
                UpdateActiveQuest(questWithTheStep);
            }
        }

        ActiveQuest FindActiveQuestWithActiveStep(ActiveQuestStep step)
        {
            foreach (ActiveQuest quest in ActiveQuests)
            {
                if(quest.CurrentStep == step)
                {
                    return quest;
                }
            }
            return null;
        }

        public Dictionary<ActiveQuestStep, QuestFinisherStep> FindQuestStepsWithNPCFinisher(NPCData npcData)
        {
            Dictionary<ActiveQuestStep, QuestFinisherStep> result = new Dictionary<ActiveQuestStep, QuestFinisherStep>();
            foreach (ActiveQuest quest in ActiveQuests)
            {
                QuestFinisherStep finisherStep = quest.CurrentStep.QuestStep as QuestFinisherStep;
                if(finisherStep != null)
                {
                    if(finisherStep.questFinisher == npcData)
                    {
                        result.Add(quest.CurrentStep, finisherStep);
                    }
                }
            }
            return result;
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

            if (ActiveQuests.Find(q => q.Quest == quest) != null || CompletedQuests.Contains(quest))
            {
                canAccept = false;
            }

            if (quest.preRequisiteQuest != null && !CompletedQuests.Contains(quest.preRequisiteQuest))
            {
                canAccept = false;
            }

            return canAccept;
        }

        public bool IsOnQuestStep(QuestStep step)
        {
            foreach (ActiveQuest activeQuest in ActiveQuests)
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
            ActiveQuests.Clear();
            CompletedQuests.Clear();
            FocusedQuest = null;
            PlayerEvents.Instance.CallEventFocusedQuestUpdated(null);
            PlayerEvents.Instance.CallEventActiveQuestStepUpdated(null);
            RemoveAllActiveQuestSteps();
            ClearQuestData();
        }

        void SetFocusedQuest(ActiveQuest quest)
        {
            FocusedQuest = quest;
            PlayerEvents.Instance.CallEventFocusedQuestUpdated(quest);
            AddFocusedQuest(quest);
        }

        void ResetFocusedQuest()
        {
            FocusedQuest = null;
            PlayerEvents.Instance.CallEventFocusedQuestUpdated(null);
            AddFocusedQuest(null);
        }

        public void ToggleFocusedQuest()
        {
            if(ActiveQuests.Count > 1)
            {
                int currentIndex = ActiveQuests.FindIndex(q => q == FocusedQuest);
                int newIndex = currentIndex + 1;
                if(newIndex >= ActiveQuests.Count)
                {
                    newIndex = 0;
                }

                SetFocusedQuest(ActiveQuests[newIndex]);
            }
        }

        ActiveQuestData CreateActiveQuestData(ActiveQuest quest)
        {
            return new ActiveQuestData(quest.Quest.name, quest.CurrentStepId, quest.CurrentStep.completedObjectives);
        }

        #region ActiveQuestStep handling

        public void AddActiveQuestStep(ActiveQuestStep step)
        {
            _createdSteps.Add(step);
        }

        // Currently in no use but will be when quests can be abandoned.
        public void RemoveActiveQuestStep(ActiveQuestStep step)
        {
            ActiveQuestStep foundStep = _createdSteps.Find(s => s == step);
            foundStep.Clean();
            _createdSteps.Remove(step);
        }

        public void RemoveAllActiveQuestSteps()
        {
            foreach (ActiveQuestStep step in _createdSteps)
            {
                step.Clean();
            }

            _createdSteps.Clear();
        }

        #endregion

        #region Methods for API interactions - ADD Active Quest
        void AddActiveQuest(ActiveQuest newQuest)
        {
            ActiveQuests.Add(newQuest);
            ActiveQuestData data = CreateActiveQuestData(newQuest);
            AddActiveQuestServerRpc(LocalConnection, UserSession.Instance.LoggedUserData.id, data);
        }

        void UpdateActiveQuest(ActiveQuest quest)
        {
            ActiveQuestData data = CreateActiveQuestData(quest);
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
            // TODO: add lines to check and remove focused quest
            ActiveQuests.Remove(quest);
            ActiveQuestData data = CreateActiveQuestData(quest);
            RemoveActiveQuestServerRpc(LocalConnection, UserSession.Instance.LoggedUserData.id, data);
        }
        [ServerRpc(RequireOwnership = false)]
        void RemoveActiveQuestServerRpc(NetworkConnection conn, string userId, ActiveQuestData data)
        {
            APICalls_Server.Instance.RemoveActiveQuest(conn, userId, data);
        }
        #endregion

        #region Methods for API interactions - Add Completed Quest
        void AddCompletedQuest(ActiveQuest quest, bool deleteFromActives = true, bool resetFocused = true)
        {
            if (deleteFromActives)
            {
                ActiveQuests.Remove(quest);
            }
            if (resetFocused)
            {
                FocusedQuest = null;
                PlayerEvents.Instance.CallEventFocusedQuestUpdated(null);
            }
            CompletedQuests.Add(quest.Quest);
            CompletedQuestData data = new CompletedQuestData(quest.Quest.name, deleteFromActives, resetFocused);
            AddCompletedQuestServerRpc(LocalConnection, UserSession.Instance.LoggedUserData.id, data);

        }

        [ServerRpc(RequireOwnership = false)]
        void AddCompletedQuestServerRpc(NetworkConnection conn, string userId, CompletedQuestData data)
        {
            APICalls_Server.Instance.AddCompletedQuest(conn, userId, data);
        }
        #endregion

        #region Methods for API interactions - Set Focused Quest

        void AddFocusedQuest(ActiveQuest quest)
        {
            FocusedQuestData data = new FocusedQuestData(quest == null ? "" : quest.Quest.name);
            AddFocusedQuestServerRpc(LocalConnection, UserSession.Instance.LoggedUserData.id, data);
        }

        [ServerRpc(RequireOwnership = false)]
        void AddFocusedQuestServerRpc(NetworkConnection conn, string userId, FocusedQuestData data)
        {
            APICalls_Server.Instance.AddFocusedQuest(conn, userId, data);
        }

        #endregion

        #region Methods for API interactions - Clear All Quest Data

        void ClearQuestData()
        {
            ClearQuestDataServerRpc(LocalConnection, UserSession.Instance.LoggedUserData.id);
        }
        [ServerRpc(RequireOwnership = false)]
        void ClearQuestDataServerRpc(NetworkConnection conn, string userId)
        {
            APICalls_Server.Instance.ClearQuestData(conn, userId);
        }
        #endregion

    }
}
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
