using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockDatabase : MonoBehaviour
{
    public List<Stock> stocks = new List<Stock>();

    void Awake()
    {
        BuildDatabase();
    }

    //Hakee stockin symbolin mukaan
    public Stock GetStock(string symbol)
    {
        return stocks.Find(stock => stock.symbol == symbol);
    }

    //Täsä voidaan lisätä uusia osakkeita. (symbol, companyName, industry, basePrice, marketCap)
    //Industry näkyy StockInfossa
    //basePrice on mistä hintaa alunperin alkaa. Vaikuttaa myös paljonko hinta nousee/laskee.
    //marketCap on joko 'Large', 'Mid', tai 'Small'. Vaikuttaa paljonko hinta nousee/laskee.
    void BuildDatabase()
    {
        stocks = new List<Stock> {
            new Stock("PEAR", "Pear Inc.", "Consumer Electronics", 170, "Large"),
            new Stock("GSFT", "Giantsoft Corporation", "Software - Infastructure", 310, "Large"),
            new Stock("EDSN", "Edison Inc.", "Automobile Manufacturers", 160, "Large"),
            new Stock("GMT", "GameStart Corp.", "Specialty Retail", 20, "Mid"),
        };
    }
}
