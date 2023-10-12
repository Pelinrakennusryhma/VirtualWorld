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

        public string CompletionStatus { get => $"{QuestStep.objectiveDescShort} {completedObjectives} / {QuestStep.requiredObjectives}"; }

        public ActiveQuestStep(QuestStep questStep)
        {
            QuestStep = questStep;
            completedObjectives = 0;

            PlayerEvents.Instance.EventQuestStepUpdated.AddListener(OnQuestStepUpdated);
        }

        void OnQuestStepUpdated(QuestStep step, int byAmount)
        {
            if(step == QuestStep)
            {
                Advance(byAmount);
            }
        }

        public void Advance(int byAmount)
        {
            completedObjectives += byAmount;
            if(completedObjectives >= QuestStep.requiredObjectives)
            {
                CompleteStep();
            } else
            {
                UpdateStep();
            }
        }

        void CompleteStep()
        {
            Debug.Log("step completed here right???");
            PlayerEvents.Instance.CallEventQuestStepCompleted(QuestStep);
        }

        void UpdateStep()
        {
            //PlayerEvents.Instance.CallEventQuestEventUpdated(this);
        }
        
    }
}
