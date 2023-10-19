using Dialog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quests
{

    [CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/Quests/Quest", order = 1)]
    public class Quest : ScriptableObject
    {
        public string title;
        [TextArea(3, 30)]
        public string text;
        public List<QuestStep> steps;
        [Tooltip("Quest which must be completed in order for this one to show up")]
        public Quest preRequisiteQuest;
    }
}

