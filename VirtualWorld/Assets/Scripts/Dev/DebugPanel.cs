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
            QuestManager.Instance.ClearQuests();
        }

        void Update()
        {
            if(QuestManager.Instance == null)
            {
                return;
            }

            activeQuests.text = "Active:\n";
            foreach (ActiveQuest quest in QuestManager.Instance.ActiveQuests)
            {
                activeQuests.text += quest.Quest.name;
            }

            completedQuests.text = "Completed:\n";
            foreach (Quest quest in QuestManager.Instance.CompletedQuests)
            {
                completedQuests.text += quest.name;
            }

            if(QuestManager.Instance.FocusedQuest != null)
            {
                focusedQuest.text = $"Focused: " +
                    $"\nstepId: {QuestManager.Instance.FocusedQuest.CurrentStepId} " +
                    $"\nstepProgress: {QuestManager.Instance.FocusedQuest.CurrentStep.completedObjectives}";
            } else
            {
                focusedQuest.text = "";
            }

        }
    }
}
