using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Quests
{

    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Quests/QuestStep", order = 1)]
    public class QuestStep : ScriptableObject
    {
        public string objectiveDescLong;
        public string objectiveDescShort;
        public int requiredObjectives;
    }
}


