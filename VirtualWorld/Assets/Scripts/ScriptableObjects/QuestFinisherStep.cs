using Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Quests
{

    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Quests/QuestFinisherStep", order = 2)]
    public class QuestFinisherStep : QuestStep
    {
        public NPCData questFinisher;
        public string dialogTitle;
        [TextArea(3, 30)]
        public string dialogText;
    }
}


