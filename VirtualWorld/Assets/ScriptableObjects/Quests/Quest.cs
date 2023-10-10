using Dialog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quests
{

    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Quests/Quest", order = 1)]
    public class Quest : DialogChoiceWithTitle
    {
        public List<QuestStep> steps;

        int currentStepId = 0;
        public QuestStep CurrentStep { get => CurrentStep; private set => CurrentStep = value; }

        public void Init()
        {
            currentStepId = 0;
            CurrentStep = steps[currentStepId];
            CurrentStep.OnComplete.AddListener(OnStepComplete);
        }

        void OnStepComplete(QuestStep step)
        {
            step.OnComplete.RemoveListener(OnStepComplete);

            currentStepId++;

            if (currentStepId >= steps.Count)
            {
                //completed
            } else
            {
                CurrentStep = steps[currentStepId];
            }
        }
    }
}

