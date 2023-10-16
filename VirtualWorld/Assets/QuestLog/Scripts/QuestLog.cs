using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestLog : MonoBehaviour
{
    public GameObject QuestPrefab;

    [SerializeField] private Transform QuestList;
    [SerializeField] private Transform abandonConfirmation;
    [SerializeField] private Button ShowOnMapButton;
    [SerializeField] private Button CompleteButton;
    [SerializeField] private Button AbandonButton;
    [SerializeField] private Button PinQuestButton;
    [SerializeField] private GameObject pinnedQuests;
    public QuestDatabase questDatabase;
    public List<PlayerQuest> playerQuests = new List<PlayerQuest>();
    public QuestListQuest selectedQuestListQuest;
    void Start()
    {
        //
        //Testausta varten lisää pelaajalle kaikki questit. Poistettava myöhemmin
        //
        foreach (Quest quest in questDatabase.quests)
        {
            PlayerQuest playerQuest = new PlayerQuest();
            playerQuest.quest = quest;
            playerQuests.Add(playerQuest);
        }
    }

    public void ShowQuestsByCategory(string category)
    {
        foreach(Transform transform in QuestList.GetComponentInChildren<Transform>())
        {
            Destroy(transform.gameObject);
        }
        foreach(PlayerQuest playerQuest in playerQuests)
        {
            Quest quest = playerQuest.quest;
            if(quest.category == category)
            {
                //UnityEngine.Object prefab = Resources.Load("Prefabs/Quest");
                //GameObject newItem = Instantiate(prefab, QuestList) as GameObject;

                GameObject newItem = Instantiate(QuestPrefab, QuestList);
                QuestListQuest questListQuest = newItem.GetComponent<QuestListQuest>();
                questListQuest.InitializeQuest(quest.name);
                questListQuest.quest = quest;
            }
        }
    }

    public void NextQuestStep(int id)
    {
        Quest quest = questDatabase.GetQuest(id);
        if (PlayerQuestCheck(quest) && quest.steps.Count - 1 > GetPlayerQuest(quest).step)
        {
            PlayerQuest playerQuest = GetPlayerQuest(quest);
            playerQuest.step++;
            if (selectedQuestListQuest != null && selectedQuestListQuest.quest == quest)
            {
                selectedQuestListQuest.AddQuestStep(playerQuest.step);
            }
            playerQuest.progress = 0;
        }
    }

    public void AbandonQuest()
    {

        SetButtonsInactive();
        abandonConfirmation.gameObject.SetActive(false);
        Quest quest = selectedQuestListQuest.AbandonQuest();
        if (pinnedQuests.transform.Find(quest.name))
        {
            pinnedQuests.transform.Find(quest.name).GetComponent<PinnedQuest>().UnpinQuest();
        }
        playerQuests.Remove(GetPlayerQuest(quest));
        Destroy(selectedQuestListQuest.gameObject);
        selectedQuestListQuest = null;
    }
    public void AbandonConfirmationCheck()
    {
        abandonConfirmation.gameObject.SetActive(true);
        abandonConfirmation.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Are you sure you want to abandon\n" + selectedQuestListQuest.quest.name + "?";
    }
    public void AbandonConfirmationNo()
    {
        abandonConfirmation.gameObject.SetActive(false);
    }

    public void SetButtonsActive()
    {
        ShowOnMapButton.interactable = true;
        AbandonButton.interactable = true;
        PinQuestButton.interactable = true;
    }
    public void SetButtonsInactive()
    {
        ShowOnMapButton.interactable = false;
        AbandonButton.interactable = false;
        PinQuestButton.interactable = false;
    }
    public void PinQuest()
    {
        pinnedQuests.SetActive(true);
        selectedQuestListQuest.PinQuest();
    }
    public void CompleteQuest()
    {
        throw new NotImplementedException();
    }
    public void ShowOnMap()
    {
        throw new NotImplementedException();
    }
    public PlayerQuest GetPlayerQuest(Quest quest)
    {
        return playerQuests.Find(playerQuest => playerQuest.quest == quest);
    }
    private bool PlayerQuestCheck(Quest quest)
    {
        return playerQuests.Any(playerQuest => playerQuest.quest == quest);
    }



    //Tällä hetkellä testaus nappia varten. Lisää 'amount' quest progressiin. Vaihdettaessa true falseksi asettaisi määrän lisäämisen sijaan.
    public void SetCurrentQuestProgress(int amount)
    {
        selectedQuestListQuest.SetQuestProgress(amount, true);
    }

}
