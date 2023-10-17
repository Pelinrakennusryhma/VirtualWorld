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

        // TODO: listen to EventActiveQuestStepUpdated for stepToTrigger to become active and enable/disable stuff based on that

        protected virtual void ProgressQuestStep()
        {
            int byAmount = progressAmount > 0 ? progressAmount : 1;
            PlayerEvents.Instance.CallEventQuestStepProgressed(stepToTrigger, byAmount);
        }
    }
}
