using Dialog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quests
{

    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Quests/Quest", order = 1)]
    public class Quest : ScriptableObject
    {
        public string title;
        [TextArea(3, 30)]
        public string text;
        public List<QuestStep> steps;
    }
}

