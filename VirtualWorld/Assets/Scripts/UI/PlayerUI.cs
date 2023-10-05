using Authentication;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Characters;
using BackendConnection;
using System.Numerics;

namespace UI
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] TMP_Text moneyText;
        [SerializeField] TextFlasher moneyTextFlasher;
        [SerializeField] string currencyIcon = "€";

        int previousMoney;

        void Start()
        {
            // skip on server
            if(PlayerEvents.Instance != null)
            {
                PlayerEvents.Instance.EventCharacterDataSet.AddListener(OnCharacterDataSet);
                PlayerEvents.Instance.EventMoneyAmountChanged.AddListener(OnMoneyAmountChanged);
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
    }
}
