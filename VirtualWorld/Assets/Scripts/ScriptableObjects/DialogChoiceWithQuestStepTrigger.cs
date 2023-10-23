using Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialog
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Dialog/DialogChoiceWithQuestStepTrigger", order = 3)]
    public class DialogChoiceWithQuestStepTrigger : DialogChoiceSub
    {
        [Tooltip("Quest step required for this dialog to show up and to trigger completed upon clicking.")]
        public QuestStep questStep;
    }
}
