using Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quests
{
    public abstract class QuestTrigger : MonoBehaviour
    {
        [SerializeField] protected QuestStep stepToTrigger;
        [SerializeField] int progressAmount = 1;

        protected virtual void ProgressQuestStep()
        {
            int byAmount = progressAmount > 0 ? progressAmount : 1;
            PlayerEvents.Instance.CallEventQuestStepProgressed(stepToTrigger, byAmount);
        }
    }
}
