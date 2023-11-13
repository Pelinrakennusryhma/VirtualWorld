using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using Scenes;

public class BankGameSystem : MonoBehaviour
{
    [SerializeField] public GameObject inventory;
    [SerializeField] private Bank bank;
    BankInventory inventoryScript;
    public double playerMoney = 1000;
    public double bankMoney = 0;
    public double playerDebt = 0;
    public double playerLoanedMoney = 0;

    public CultureInfo culture = CultureInfo.CreateSpecificCulture("fi-FI");

    private void Start()
    {
        inventoryScript = inventory.GetComponent<BankInventory>();
        //Muuttaa valuuttamerkkiä
        culture.NumberFormat.CurrencySymbol = "C";
        InventoryHymisImplementation.Instance.GetMoneyThings(out double cash,
                                                             out double debt,
                                                             out double bankBalance,
                                                             out double loanedMoney);
        playerMoney = cash;
        playerDebt = debt;
        bankMoney = bankBalance;
        playerLoanedMoney = loanedMoney;
        bank.InitBank(bankBalance);
    }

    //Lisää pelaajalle 'amount' määrän rahaa.
    public void AddMoneyToPlayer(double amount)
    {
        playerMoney += amount;
        inventoryScript.UpdateInventory();
        InventoryHymisImplementation.Instance.AddMoneyFromSubscene(amount);
    }

    //Poistaa pelaajalta 'amount' määrän rahaa
    public void RemoveMoneyFromPlayer(double amount)
    {
        playerMoney -= amount;
        inventoryScript.UpdateInventory();
        InventoryHymisImplementation.Instance.RemoveMoneyFromSubscene(amount);
    }

    //Lisää pelaajan pankkiin 'amount' määrän rahaa.
    public void AddMoneyToBank(double amount)
    {
        bankMoney += amount;
        inventoryScript.UpdateInventory();
        InventoryHymisImplementation.Instance.AddBankBalanceFromSubscene(amount);
    }

    //Poistaa pelaajan pankista 'amount' määrän rahaa.
    public void RemoveMoneyFromBank(double amount)
    {
        bankMoney -= amount;
        inventoryScript.UpdateInventory();
        InventoryHymisImplementation.Instance.RemoveBankBalanceFromSubscene(amount);
    }

    //Lisää pelaajalla 'amount' määrän velkaa.
    public void AddDebtToPlayer(double amount)
    {
        playerDebt += amount;
        playerLoanedMoney += amount;
        inventoryScript.UpdateInventory();
        InventoryHymisImplementation.Instance.AddDebtFromSubscene(amount);
    }

    //Poistaa pelaajalta 'amount' määrän velkaa
    public void RemoveDebtFromPlayer(double amount)
    {
        playerDebt -= amount;
        inventoryScript.UpdateInventory();
        InventoryHymisImplementation.Instance.RemoveDebtFromSubscene(amount);
    }

    //Lisää pelaajan velkaan 'amount' määrän korkoa.
    public void AddInterestToDebt(double amount)
    {
        playerDebt += playerLoanedMoney * amount;
        inventoryScript.UpdateInventory();
        InventoryHymisImplementation.Instance.AddDebtFromSubscene(playerDebt);

    }

    public void OpenInventory()
    {
        inventory.SetActive(true);
    }

    public void CloseInventory()
    {
        inventory.SetActive(false);
    }

    public void GoBackToWorld()
    {
        SceneLoader.Instance.UnloadScene();
    }
}
