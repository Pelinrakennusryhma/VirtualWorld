using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Quests
{
    [CreateAssetMenu(fileName = "QuestDatabase", menuName = "ScriptableObjects/Quests/QuestDatabase", order = 0)]
    public class QuestDatabase : ScriptableObject
    {
        [field: SerializeField] public List<Quest> AllQuests { get; private set; }
        Dictionary<string, Quest> allQuestsDictionary;

        public void Init()
        {
            ClearNulls();
            CreateDictionary();
        }

        public void AddQuest(Quest newQuest)
        {
            if (!AllQuests.Contains(newQuest))
            {
                AllQuests.Add(newQuest);
            }
        }

        void ClearNulls()
        {
            for (int i = AllQuests.Count - 1; i >= 0; i--)
            {
                if (AllQuests[i] == null)
                {
                    AllQuests.RemoveAt(i);
                } 
            }
        }

        void CreateDictionary()
        {
            allQuestsDictionary = new Dictionary<string, Quest>();

            foreach (Quest quest in AllQuests)
            {
                allQuestsDictionary.Add(quest.name, quest);
                Debug.Log("added: " + quest.name + quest.title);
            }
        }

        public Quest GetQuestById(string id)
        {
            return allQuestsDictionary[id];
        }
    }
}
