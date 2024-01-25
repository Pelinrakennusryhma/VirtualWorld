using Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WorldObjects;

namespace Quests
{
    public class QuestTriggerLootable : QuestTrigger, I_Interactable
    {
        [field: SerializeReference] public string DetectionMessage { get; set; }
        public Vector3 DetectionMessageOffSet => Vector3.zero;

        [field: SerializeReference] public bool IsActive { get; private set; }

        Respawnable respawnable;

        void Start()
        {
            respawnable = transform.parent.GetComponent<Respawnable>();
            // this probably needs to be checked after loading quest progress data from the server?
            PlayerEvents.Instance.EventActiveQuestStepUpdated.AddListener(OnActiveQuestStepUpdated);
        }

        void OnActiveQuestStepUpdated(ActiveQuestStep step)
        {
            if (step == null)
            {
                IsActive = false;
                return;
            }

            if (step.QuestStep == stepToTrigger)
            {
                IsActive = true;
            }
            else
            {
                IsActive = false;
            }
        }

        public void Interact(UnityAction _)
        {
            ProgressQuestStep();
            respawnable.Despawn(this, gameObject);
        }
    }
}