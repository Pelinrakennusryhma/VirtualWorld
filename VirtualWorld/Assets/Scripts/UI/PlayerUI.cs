using Authentication;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Characters;
using BackendConnection;
using System.Numerics;
using Quests;

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

        int previousMoney;

        void Start()
        {
            focusedQuestContainer.SetActive(false);
            // skip on server.. somehow? should PlayerEvents be instantiated from the player prefab or something?
            if(PlayerEvents.Instance != null)
            {
                PlayerEvents.Instance.EventCharacterDataSet.AddListener(OnCharacterDataSet);
                PlayerEvents.Instance.EventMoneyAmountChanged.AddListener(OnMoneyAmountChanged);
                PlayerEvents.Instance.EventFocusedQuestUpdated.AddListener(OnFocusedQuestUpdated);
            }
        }

        void OnMoneyAmountChanged(InventoryItem moneyItem)
        {
            UpdateMoney(moneyItem.amount);
        }

        void OnCharacterDataSet(CharacterData data)
        {
            int amountMoney = 0;
            foreach (InventoryItem item in data.inventory.items)
            {
                if (item.name == "money")
                {
                    amountMoney = item.amount;
                    break;
                }
            }

            UpdateMoney(amountMoney);
        }

        void UpdateMoney(int newAmount)
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
                focusedQuestProgress.text = quest.CurrentStep.CompletionStatus;
            }
        }
    }
}
