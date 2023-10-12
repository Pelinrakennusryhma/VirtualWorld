using Characters;
using Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    List<ActiveQuest> activeQuests = new List<ActiveQuest>();
    List<Quest> completedQuests = new List<Quest>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
    }

    private void Start()
    {
        PlayerEvents.Instance.EventQuestCompleted.AddListener(OnQuestCompleted);
    }

    void OnQuestCompleted(Quest quest)
    {
        foreach (ActiveQuest activeQuest in activeQuests)
        {
            if(activeQuest.Quest == quest)
            {
                Debug.Log("removing quest?");
                activeQuests.Remove(activeQuest);
                completedQuests.Add(quest);
                PlayerEvents.Instance.CallEventInformationReceived($"Completed Quest \"{quest.title}\"");
                break;
            }
        }
    }

    public void AcceptQuest(Quest quest)
    {
        ActiveQuest activeQuest = new ActiveQuest(quest);
        activeQuests.Add(activeQuest);
        PlayerEvents.Instance.CallEventInformationReceived($"Started Quest \"{quest.title}\"");
    }

    public bool CanAcceptQuest(Quest quest)
    {
        bool canAccept = true;

        if (activeQuests.Find(q => q.Quest == quest) != null || completedQuests.Contains(quest))
        {
            canAccept = false;
        }

        return canAccept;
    }

    public bool IsOnQuestStep(QuestStep step)
    {
        foreach (ActiveQuest activeQuest in activeQuests)
        {
            if(activeQuest.CurrentStep.QuestStep == step)
            {
                return true;
            }
        }
        return false;
    }

    public void UpdateStep(QuestStep step, int byAmount)
    {
        PlayerEvents.Instance.CallEventQuestStepUpdated(step, byAmount);
    }

    public void CompleteStep(QuestStep step)
    {
        Debug.Log("completed step: " + step.objectiveDescLong);
    }
}
