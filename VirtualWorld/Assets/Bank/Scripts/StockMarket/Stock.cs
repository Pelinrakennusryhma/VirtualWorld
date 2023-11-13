using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stock
{
    public string symbol;
    public string companyName;
    public string industry;
    public double basePrice;
    public string marketCap;
    public Item inventoryItem;

    public Stock(string symbol, 
                 string companyName, 
                 string industry, 
                 double basePrice, 
                 string marketCap,
                 Item inventoryItem)
    {
        this.symbol = symbol;
        this.companyName = companyName;
        this.industry = industry;
        this.basePrice = basePrice;
        this.marketCap = marketCap;
        this.inventoryItem = inventoryItem;
    }
}
