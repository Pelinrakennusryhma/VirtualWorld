using Authentication;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Characters;
using BackendConnection;
using System.Numerics;
using Quests;
using Items;

namespace UI
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] TMP_Text moneyText;
        [SerializeField] TextFlasher moneyTextFlasher;
        [SerializeField] string currencyIcon = "€";

        [Header("Focused Quest")]
        [SerializeField] GameObject focusedQuestContainer;
        [SerializeField] TMP_Text focusedQuestTitle;
        [SerializeField] TMP_Text focusedQuestObjective;
        [SerializeField] TMP_Text focusedQuestProgress;
        [SerializeField] TextFlasher focusedQuestFlasher;

        double previousMoney;

        void Start()
        {
            focusedQuestContainer.SetActive(false);
            // skip on server.. somehow? should PlayerEvents be instantiated from the player prefab or something?
            if(PlayerEvents.Instance != null)
            {
                PlayerEvents.Instance.EventMoneyAmountChanged.AddListener(OnMoneyAmountChanged);
                PlayerEvents.Instance.EventFocusedQuestUpdated.AddListener(OnFocusedQuestUpdated);
            }
        }

        void OnMoneyAmountChanged(InventoryItem moneyItem)
        {
            UpdateMoney(moneyItem.amount);
        }

        //void OnCharacterDataSet(CharacterData data)
        //{
        //    int amountMoney = 0;
        //    foreach (InventoryItem item in data.inventory.items)
        //    {
        //        if (item.name == "Credit")
        //        {
        //            amountMoney = item.amount;
        //            break;
        //        }
        //    }

        //    UpdateMoney(amountMoney);
        //}

        void UpdateMoney(double newAmount)
        {
            moneyText.text = $"{newAmount} {currencyIcon}";

            if (previousMoney != newAmount)
            {
                moneyTextFlasher.FlashText();
            }

            previousMoney = newAmount;
        }

        void OnFocusedQuestUpdated(ActiveQuest quest)
        {
            if(quest == null)
            {
                focusedQuestContainer.SetActive(false);
            } 
            else
            {
                focusedQuestContainer.SetActive(true);

                focusedQuestTitle.text = quest.Quest.title;
                focusedQuestObjective.text = quest.CurrentStep.QuestStep.objectiveDescShort;

                // only show objectives that require more than one, e.g. never 0/1 progression status
                if(quest.CurrentStep.QuestStep.requiredObjectives > 1)
                {
                    focusedQuestProgress.text = quest.CurrentStep.CompletionStatus;
                    focusedQuestProgress.gameObject.SetActive(true);

                    // flash the text only when progress updates, not when the status initially shows up
                    // because that looks awkward
                    if(quest.CurrentStep.completedObjectives > 0)
                    {
                        focusedQuestFlasher.FlashText();
                    }

                }
                else
                {
                    focusedQuestProgress.gameObject.SetActive(false);
                }
            }
        }
    }
}
