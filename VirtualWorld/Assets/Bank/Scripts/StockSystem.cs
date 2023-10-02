using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockSystem : MonoBehaviour
{
    [SerializeField] public GameObject stockMarket;
    [SerializeField] public GameObject stockInventory;
    private StockDatabase stockDatabase;
    public Dictionary<Stock, int> playerStocks = new Dictionary<Stock, int>();
    public Dictionary<Stock, double> playerStocksCost = new Dictionary<Stock, double>();
    public bool stockMarketOpen = false;

    void Start()
    {
        stockDatabase = gameObject.GetComponent<StockDatabase>();

        foreach (Stock stock in stockDatabase.stocks)
        {
            playerStocks.Add(stock, 0);
            playerStocksCost.Add(stock, 0.0);
        }
    }

    //Lisää pelaajalle osakkeen. 
    public void AddStockToPlayer(Stock stock, int amount, double cost)
    {
        playerStocks[stock] += amount;
        playerStocksCost[stock] += cost;
    }

    //Poistaa pelaajalta osakkeen.
    public void RemoveStockFromPlayer(Stock stock, int amount, double value)
    {
        playerStocks[stock] -= amount;
        if (playerStocks[stock] == 0)
        {
            playerStocksCost[stock] = 0;
        }
        else
        {
            playerStocksCost[stock] -= value;
        }

    }
    public void OpenStockMarket()
    {
        stockMarket.SetActive(true);
        stockMarketOpen = true;
    }
    public void CloseStockMarket()
    {
        stockMarket.SetActive(false);
        stockMarketOpen = false;
    }
    public void OpenStockInventory()
    {
        stockInventory.SetActive(true);
        stockInventory.GetComponent<StockInventory>().UpdateInventory();
    }
    public void CloseStockInventory()
    {
        stockInventory.SetActive(false);
    }

}
