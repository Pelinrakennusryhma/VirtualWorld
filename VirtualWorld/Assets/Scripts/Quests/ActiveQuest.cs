using Characters;
using Quests;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace Quests
{
    public class ActiveQuest
    {
        public Quest Quest { get => _quest; private set => _quest = value; }
        private Quest _quest;
        int currentStepId = 0;
        public ActiveQuestStep CurrentStep { get => _currentStep; private set => _currentStep = value; }
        private ActiveQuestStep _currentStep;

        public ActiveQuest(Quest quest)
        {
            Quest = quest;
            currentStepId = 0;
            CurrentStep = new ActiveQuestStep(Quest.steps[currentStepId]);

            PlayerEvents.Instance.EventQuestStepCompleted.AddListener(OnStepComplete);
        }

        void OnStepComplete(QuestStep step)
        {
            if(step == CurrentStep.QuestStep)
            {
                currentStepId++;

                if (currentStepId >= Quest.steps.Count)
                {
                    PlayerEvents.Instance.CallEventQuestCompleted(Quest);
                }
                else
                {
                    CurrentStep = new ActiveQuestStep(Quest.steps[currentStepId]);
                }
            }

        }
    }
}