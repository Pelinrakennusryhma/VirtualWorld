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

            activeQuests.text = "";
            foreach (ActiveQuest quest in QuestManager.Instance.ActiveQuests)
            {
                activeQuests.text += quest.Quest.name;
            }

            completedQuests.text = "";
            foreach (Quest quest in QuestManager.Instance.CompletedQuests)
            {
                activeQuests.text += quest.name;
            }
        }
    }
}
