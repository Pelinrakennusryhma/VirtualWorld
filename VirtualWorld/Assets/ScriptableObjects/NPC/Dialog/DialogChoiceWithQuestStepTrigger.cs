using Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialog
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Dialog/DialogChoiceWithQuestStepTrigger", order = 3)]
    public class DialogChoiceWithQuestStepTrigger : DialogChoiceSub
    {
        [Tooltip("Quest step to trigger completed upon clicking.")]
        public QuestStep questStep;
        [Tooltip("Quest step dialog which follows this one.")]
        public DialogChoiceWithQuestStepTrigger followupStep;
    }
}
