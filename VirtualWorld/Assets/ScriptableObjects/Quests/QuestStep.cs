using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Quests
{

    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Quests/QuestStep", order = 1)]
    public class QuestStep : ScriptableObject
    {
        public string objectiveDescLong;
        public string objectiveDescShort;
        public int requiredObjectives;
        public int completedObjectives;
        public bool Completed { get => Completed; private set => Completed = value; }
        public UnityEvent<QuestStep> OnComplete;

        public string CompletionStatus { get => $"{objectiveDescShort} {completedObjectives} / {requiredObjectives}"; }

        public void Advance(int byAmount)
        {
            requiredObjectives++;
            Complete();
        }

        public void Complete()
        {
            Completed = true;
            OnComplete.Invoke(this);
        }
    }
}


