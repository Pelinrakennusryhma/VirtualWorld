using Authentication;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] Character character;
    [SerializeField] TMP_Text moneyText;
    [SerializeField] string currencyIcon = "€";

    void Awake()
    {
        character = Character.Instance;
        character.EventInventoryChanged.AddListener(OnInventoryChanged);
    }

    void OnInventoryChanged(Inventory inventory)
    {
        moneyText.text = $"{inventory.money} {currencyIcon}";
    }
}
