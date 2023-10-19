#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Quests
{
    [CustomEditor(typeof(Quest), true)]
    public class QuestEditor : Editor
    {
        QuestDatabase questDatabase;
        bool isAddedToDB = false;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Quest quest = (Quest)target;

            questDatabase = (QuestDatabase)AssetDatabase.LoadAssetAtPath("Assets/Data/QuestData/QuestDatabase.asset", typeof(QuestDatabase));

            if (!isAddedToDB)
            {
                questDatabase.AddQuest(quest);
                isAddedToDB = true;
            }
        }
    }
}
#endif
