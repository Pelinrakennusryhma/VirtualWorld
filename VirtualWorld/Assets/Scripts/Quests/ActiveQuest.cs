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

        public ActiveQuest(Quest quest)
        {
            Quest = quest;
            _currentStepId = 0;
            CurrentStep = new ActiveQuestStep(Quest.steps[_currentStepId]);

            PlayerEvents.Instance.EventQuestStepCompleted.AddListener(OnStepComplete);
        }

        public ActiveQuest(ActiveQuestData questData)
        {
            Quest = null; // need to find quest from some sort of quest db
            _currentStepId = questData.step;
            CurrentStep = new ActiveQuestStep(Quest.steps[_currentStepId]);
            CurrentStep.completedObjectives = questData.stepProgress;

            //PlayerEvents.Instance.EventQuestStepCompleted.AddListener(OnStepComplete);
        }

        void OnStepComplete(QuestStep step)
        {
            if(step == CurrentStep.QuestStep)
            {
                _currentStepId++;

                if (_currentStepId >= Quest.steps.Count)
                {
                    PlayerEvents.Instance.CallEventQuestCompleted(Quest);
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