using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quests;
using TMPro;

namespace Dev
{
    public class DebugPanel : MonoBehaviour
    {
        [SerializeField] TMP_Text activeQuests;
        [SerializeField] TMP_Text completedQuests;
        [SerializeField] TMP_Text focusedQuest;
        public void ResetQuests()
        {
            QuestManagerNonNetworked.Instance.ClearQuests();
        }

        void Update()
        {
            if(QuestManagerNonNetworked.Instance == null)
            {
                return;
            }

            activeQuests.text = "Active:\n";
            foreach (ActiveQuest quest in QuestManagerNonNetworked.Instance.ActiveQuests)
            {
                activeQuests.text += quest.Quest.name;
            }

            completedQuests.text = "Completed:\n";
            foreach (Quest quest in QuestManagerNonNetworked.Instance.CompletedQuests)
            {
                completedQuests.text += quest.name;
            }

            if(QuestManagerNonNetworked.Instance.FocusedQuest != null)
            {
                focusedQuest.text = $"Focused: " +
                    $"\nstepId: {QuestManagerNonNetworked.Instance.FocusedQuest.CurrentStepId} " +
                    $"\nstepProgress: {QuestManagerNonNetworked.Instance.FocusedQuest.CurrentStep.completedObjectives}";
            } else
            {
                focusedQuest.text = "";
            }

        }
    }
}
