using Characters;
using Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    public List<Quest> allQuests = new List<Quest>();

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

    public void AcceptQuest(Quest quest)
    {
        allQuests.Add(quest);
        PlayerEvents.Instance.CallEventInformationReceived($"Started Quest \"{quest.title}\"");
    }
}
