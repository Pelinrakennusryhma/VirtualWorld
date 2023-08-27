using Authentication;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace UI
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] Character character;
        [SerializeField] TMP_Text moneyText;
        [SerializeField] TextFlasher moneyTextFlasher;
        [SerializeField] string currencyIcon = "€";

        int previousMoney;

        void Awake()
        {
            character = Character.Instance;
            character.EventInventoryChanged.AddListener(OnInventoryChanged);
        }

        void OnInventoryChanged(Inventory inventory)
        {
            moneyText.text = $"{inventory.money} {currencyIcon}";

            if(previousMoney != inventory.money)
            {
                moneyTextFlasher.FlashText();
            }

            previousMoney = inventory.money;
        }
    }
}
