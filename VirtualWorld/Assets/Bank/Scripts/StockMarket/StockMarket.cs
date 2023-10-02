using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockMarket : MonoBehaviour
{
    [SerializeField] private GameObject layout;
    public StockDatabase stockDatabase;
    List<StockScript> ssList = new List<StockScript>();
    void Awake()
    {
        //Lisää kaikki osakkeet StockDatabasesta.
        foreach (Stock stock in stockDatabase.stocks)
        {
            Object prefab = Resources.Load("Prefabs/Stock");
            GameObject newItem = Instantiate(prefab, layout.transform) as GameObject;
            newItem.GetComponent<StockScript>().stock = stock;
            newItem.name = stock.symbol.ToString();
            ssList.Add(newItem.GetComponent<StockScript>());
        }
    }

    //Lisää kaikkiin osakkeisiin uuden hinnan.
    public void NextPrices()
    {
        foreach (StockScript ss in ssList)
        {
            ss.PatternSelect();
        }
    }
}
