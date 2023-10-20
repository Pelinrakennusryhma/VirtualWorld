using Characters;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Quests
{
    public class ActiveQuestStep
    {
        public QuestStep QuestStep { get => _questStep; private set => _questStep = value; }
        private QuestStep _questStep;
        public int completedObjectives = 0;
        public bool Completed { get => Completed; private set => Completed = value; }

        public string CompletionStatus { get => $"{completedObjectives} / {QuestStep.requiredObjectives}"; }

        public ActiveQuestStep(QuestStep questStep)
        {
            QuestStep = questStep;
            completedObjectives = 0;

            PlayerEvents.Instance.EventQuestStepProgressed.AddListener(OnQuestStepProgressed);

            // Add to list of created quest steps so this one can
            // be cleaned when needed, e.g. when quest gets abandoned
            QuestManager.Instance.AddActiveQuestStep(this);

            UpdateStep();
        }

        void OnQuestStepProgressed(QuestStep step, int byAmount)
        {
            if(step == QuestStep)
            {
                Debug.Log("Progress in ActiveQuestStep: " + step.name);
                Advance(byAmount);
            }
        }

        public void Advance(int byAmount)
        {
            Debug.Log("Advancing quest step " + QuestStep.name);
            Debug.Log("CompletionStatus Pre: " + CompletionStatus);
            Debug.Log("ByAmount: " + byAmount);
            completedObjectives += byAmount;
            if(completedObjectives >= QuestStep.requiredObjectives)
            {
                Debug.Log("Completed.");
                CompleteStep();
            } else
            {
                Debug.Log("Updated.");
                UpdateStep();
            }
            Debug.Log("CompletionStatus Post: " + CompletionStatus);
        }

        void CompleteStep()
        {
            PlayerEvents.Instance.CallEventQuestStepCompleted(QuestStep);
            // object no longer needed - remove listener so object gets garbage collected
            Clean();
        }

        void UpdateStep()
        {
            PlayerEvents.Instance.CallEventActiveQuestStepUpdated(this);
        }

        public void Clean()
        {
            PlayerEvents.Instance.EventQuestStepProgressed.RemoveListener(OnQuestStepProgressed);
        }
        
    }
}
