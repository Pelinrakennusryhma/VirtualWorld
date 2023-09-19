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

        BigInteger previousMoney;

        void Awake()
        {
            if (CharacterManager.Instance != null)
            {
                CharacterManager.Instance.EventCharacterDataSet.AddListener(OnCharacterDataSet);
            }
        }

        void OnCharacterDataSet(CharacterData data)
        {
            BigInteger amountMoney = 0;
            foreach (InventoryItem item in data.inventory.items)
            {
                if(item.name == "money")
                {
                    amountMoney = item.amount;
                    break;
                }
            }

            moneyText.text = $"{amountMoney} {currencyIcon}";

            if (previousMoney != amountMoney)
            {
                moneyTextFlasher.FlashText();
            }

            previousMoney = amountMoney;
        }
    }
}
