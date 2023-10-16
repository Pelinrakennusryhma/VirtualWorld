using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestListQuest : MonoBehaviour
{
    public GameObject QuestStepPrefab;
    public GameObject PinnedQuestPrefab;

    public Quest quest;
    private Button completeButton;
    private Transform questStepLayout;
    private QuestLog questLog;
    private Transform pinnedQuests;
    private TextMeshProUGUI questProgress;
    private int currentStep;
    private PinnedQuest pinnedQuestScript;
    public void InitializeQuest(string questName)
    {
        completeButton = GameObject.Find("QuestButtons/CompleteButton").GetComponent<Button>();
        completeButton.interactable = false;
        gameObject.name = questName;
        questStepLayout = GameObject.Find("QuestInfo/Scroll/View/Layout").transform;
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = questName;
        questLog = GameObject.Find("QuestLog").GetComponent<QuestLog>();
    }

    public void SelectQuest()
    {
        completeButton.interactable = false;
        questLog.SetButtonsActive();
        foreach (Transform transform in gameObject.transform.parent.transform.GetComponentInChildren<Transform>())
        {
            transform.GetComponent<Image>().color = new Color(0.757f, 0.757f, 0.757f);
        }
        gameObject.GetComponent<Image>().color = Color.white;
        questLog.selectedQuestListQuest = gameObject.GetComponent<QuestListQuest>();
        GameObject.Find("QuestName/QuestNameText").GetComponent<TextMeshProUGUI>().text = quest.name;

        //Tyhjentää vanhat askeleet
        foreach (Transform stepTransform in questStepLayout.GetComponentsInChildren<Transform>())
        {
            if(stepTransform.name != "Layout")
            {
                Destroy(stepTransform.gameObject);
            }
        }

        //Lisää uudet askeleet
        for (int i = 0; i <= questLog.GetPlayerQuest(quest).step;)
        {
            if (quest.steps.Count > i)
            {
                AddQuestStep(i);
            }
            i++;
        }
    }

    //Lisää uuden askeleen questiin. stepNumber on kuinka monennes askel questista.
    public void AddQuestStep(int stepNumber)
    {
        //Object prefab = Resources.Load("Prefabs/QuestStep");
        //GameObject newItem = Instantiate(prefab, questStepLayout) as GameObject;

        GameObject newItem = Instantiate(QuestStepPrefab, questStepLayout);

        newItem.transform.Find("QuestStepText").GetComponent<TextMeshProUGUI>().text = quest.steps.ElementAt(stepNumber).Key;
        questProgress = newItem.transform.Find("QuestProgressText").GetComponent<TextMeshProUGUI>();
        currentStep = stepNumber;
        UpdateQuestProgress();

        //Tekee complete napista painettavan jos quest on valmis
        if (quest.steps.Count <= questLog.GetPlayerQuest(quest).step + 1)
        {
            completeButton.interactable = true;
        }

        //Muuttaa PinnedQuestin uuteen steppiin
        if(GameObject.Find("PinnedQuests/" + quest.name))
        {
            GameObject.Find("PinnedQuests/" + quest.name).GetComponent<PinnedQuest>().SetQuestObjective(quest.steps.ElementAt(stepNumber).Key);
        }
    }

    //Kutsutaan QuestLog.cs kautta
    public Quest AbandonQuest()
    {

        foreach (Transform transform in gameObject.transform.parent.transform.GetComponentInChildren<Transform>())
        {
            transform.GetComponent<Image>().color = new Color(0.757f, 0.757f, 0.757f);
        }
        GameObject.Find("QuestName/QuestNameText").GetComponent<TextMeshProUGUI>().text = "";
        foreach (Transform stepTransform in questStepLayout.GetComponentsInChildren<Transform>())
        {
            if (stepTransform.name != "Layout")
            {
                Destroy(stepTransform.gameObject);
            }
        }



        return quest;
    }
    public void PinQuest()
    {
        int maxPinnedQuests = 5;
        pinnedQuests = GameObject.Find("PinnedQuests").transform;
        pinnedQuests.gameObject.SetActive(true);
        if (pinnedQuests.childCount < maxPinnedQuests &&!pinnedQuests.Find(quest.name))
        {
            //Object prefab = Resources.Load("Prefabs/PinnedQuest");
            //GameObject newItem = Instantiate(prefab, pinnedQuests) as GameObject;
            GameObject newItem = Instantiate(PinnedQuestPrefab, pinnedQuests);

            newItem.name = quest.name;
            pinnedQuestScript = newItem.GetComponent<PinnedQuest>();
            pinnedQuestScript.SetQuestName(quest.name);
            pinnedQuestScript.SetQuestObjective(quest.steps.ElementAt(questLog.GetPlayerQuest(quest).step).Key);
        }
        UpdateQuestProgress();
    }
    public void UpdateQuestProgress()
    {
        if(quest.steps.ElementAt(currentStep).Value == 0)
        {
            questProgress.text = "";
        }
        else
        {
            questProgress.text = questLog.GetPlayerQuest(quest).progress + "/" + quest.steps.ElementAt(currentStep).Value;
        }
        PlayerQuest playerQuest = questLog.GetPlayerQuest(quest);
        SetPinnedQuestProgress(playerQuest.progress, playerQuest.quest.steps.ElementAt(currentStep).Value);
    }

    //addToCurrentAmount = 
    //true = Lisää vanhaan amount määrän.
    //false = Asettaa progressin amountiksi
    public void SetQuestProgress(int amount, bool addToCurrentAmount)
    {
        PlayerQuest playerQuest = questLog.GetPlayerQuest(quest);
        if (addToCurrentAmount)
        {
            playerQuest.progress += amount;
        }
        else
        {
            playerQuest.progress = amount;
        }
        UpdateQuestProgress();
        if(playerQuest.progress >= playerQuest.quest.steps.ElementAt(currentStep).Value)
        {
            questLog.NextQuestStep(quest.id);
            
        }
        UpdateQuestProgress();
    }

    public void SetPinnedQuestProgress(int current, int max)
    {
        if (pinnedQuestScript != null)
        {
            pinnedQuestScript.SetQuestProgress(current, max);
        }
    }
}
