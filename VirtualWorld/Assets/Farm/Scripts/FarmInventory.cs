using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FarmInventory : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerCash;
    [SerializeField] private TextMeshProUGUI playerBank;
    [SerializeField] private TextMeshProUGUI playerDebt;
    [SerializeField] private FarmGameSystem gameSystem;

    private void OnEnable()
    {
        UpdateInventory();
    }
    public void UpdateInventory()
    {
        playerCash.text = gameSystem.playerMoney.ToString("C", gameSystem.culture);
        playerBank.text = gameSystem.bankMoney.ToString("C", gameSystem.culture);
        playerDebt.text = gameSystem.playerDebt.ToString("C", gameSystem.culture);
    }
}
