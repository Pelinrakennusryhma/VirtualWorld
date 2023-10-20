using BackendConnection;
using Characters;

namespace Quests
{
    public class ActiveQuest
    {
        public Quest Quest { get => _quest; private set => _quest = value; }
        private Quest _quest;
        public int CurrentStepId { get => _currentStepId; }
        int _currentStepId = 0;
        public ActiveQuestStep CurrentStep { get => _currentStep; private set => _currentStep = value; }
        private ActiveQuestStep _currentStep;

        public ActiveQuest(Quest quest, int step = 0, int stepProgress = 0)
        {
            Quest = quest;
            _currentStepId = step;
            CurrentStep = new ActiveQuestStep(Quest.steps[_currentStepId]);
            CurrentStep.completedObjectives = stepProgress;

            PlayerEvents.Instance.EventQuestStepCompleted.AddListener(OnStepComplete);
        }

        void OnStepComplete(QuestStep step)
        {
            if(step == CurrentStep.QuestStep)
            {
                _currentStepId++;

                if (_currentStepId >= Quest.steps.Count)
                {
                    PlayerEvents.Instance.CallEventQuestCompleted(Quest);
                    // object no longer needed - remove listener so object gets garbage collected
                    PlayerEvents.Instance.EventQuestStepCompleted.RemoveListener(OnStepComplete);
                }
                else
                {
                    CurrentStep = new ActiveQuestStep(Quest.steps[_currentStepId]);
                    PlayerEvents.Instance.CallEventActiveQuestUpdated(this);
                }
            }
        }
    }
}