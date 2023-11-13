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
        InventoryHymisImplementation.Instance.GetMoneyThings(out double cash,
                                                             out double debt,
                                                             out double bank,
                                                             out double loan);

        playerCash.text = cash.ToString("C", InventoryHymisImplementation.Instance.gameSystem.culture);
        playerBank.text = bank.ToString("C", InventoryHymisImplementation.Instance.gameSystem.culture);
        playerDebt.text = debt.ToString("C", InventoryHymisImplementation.Instance.gameSystem.culture);

        //playerCash.text = gameSystem.playerMoney.ToString("C", gameSystem.culture);
        //playerBank.text = gameSystem.bankMoney.ToString("C", gameSystem.culture);
        //playerDebt.text = gameSystem.playerDebt.ToString("C", gameSystem.culture);
    }
}
