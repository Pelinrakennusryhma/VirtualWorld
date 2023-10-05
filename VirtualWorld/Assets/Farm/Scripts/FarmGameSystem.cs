using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using Scenes;

public class FarmGameSystem : MonoBehaviour
{
    [SerializeField] public GameObject inventory;
    FarmInventory inventoryScript;
    public double playerMoney = 1000;
    public double bankMoney = 0;
    public double playerDebt = 0;
    public double playerLoanedMoney = 0;

    public CultureInfo culture = CultureInfo.CreateSpecificCulture("fi-FI");

    private void Start()
    {
        inventoryScript = inventory.GetComponent<FarmInventory>();
        //Muuttaa valuuttamerkkiä
        culture.NumberFormat.CurrencySymbol = "C";
    }

    //Lisää pelaajalle 'amount' määrän rahaa.
    public void AddMoneyToPlayer(double amount)
    {
        playerMoney += amount;
        inventoryScript.UpdateInventory();
    }

    //Poistaa pelaajalta 'amount' määrän rahaa
    public void RemoveMoneyFromPlayer(double amount)
    {
        playerMoney -= amount;
        inventoryScript.UpdateInventory();
    }

    //Lisää pelaajan pankkiin 'amount' määrän rahaa.
    public void AddMoneyToBank(double amount)
    {
        bankMoney += amount;
        inventoryScript.UpdateInventory();
    }

    //Poistaa pelaajan pankista 'amount' määrän rahaa.
    public void RemoveMoneyFromBank(double amount)
    {
        bankMoney -= amount;
        inventoryScript.UpdateInventory();
    }

    //Lisää pelaajalla 'amount' määrän velkaa.
    public void AddDebtToPlayer(double amount)
    {
        playerDebt += amount;
        playerLoanedMoney += amount;
        inventoryScript.UpdateInventory();
    }

    //Poistaa pelaajalta 'amount' määrän velkaa
    public void RemoveDebtFromPlayer(double amount)
    {
        playerDebt -= amount;
        inventoryScript.UpdateInventory();
    }

    //Lisää pelaajan velkaan 'amount' määrän korkoa.
    public void AddInterestToDebt(double amount)
    {
        playerDebt += playerLoanedMoney * amount;
        inventoryScript.UpdateInventory();
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
