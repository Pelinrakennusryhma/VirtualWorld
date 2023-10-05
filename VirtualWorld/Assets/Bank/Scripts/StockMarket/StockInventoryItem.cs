using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class StockInventoryItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stockSymbol;
    [SerializeField] private TextMeshProUGUI stockCompanyName;
    [SerializeField] private TextMeshProUGUI stockOwned;
    [SerializeField] private TextMeshProUGUI stockProfitLoss;
    [SerializeField] private TextMeshProUGUI stockValue;
    public double lastPrice;
    public Stock stock;
    public BankGameSystem gameSystem;
    public StockSystem stockSystem;
    public void SetText(double price, int owned)
    {
        gameSystem = GameObject.Find("Canvas").GetComponent<BankGameSystem>();
        stockSystem = GameObject.Find("Canvas").GetComponent<StockSystem>();
        stockSymbol.text = stock.symbol;
        stockCompanyName.text = stock.companyName;
        stockOwned.text = owned.ToString();

        if (stockSystem.stockMarketOpen)
        {
            lastPrice = GameObject.Find("StockMarket/Scroll/View/Layout/" + stock.symbol).GetComponent<StockScript>().priceHistory.Last();
            double currentPrice = lastPrice * stockSystem.playerStocks[stock];
            double paidPrice = stockSystem.playerStocksCost[stock];
            stockProfitLoss.text = ((paidPrice - currentPrice) / paidPrice).ToString("P");
            stockValue.text = lastPrice.ToString("C", gameSystem.culture);
        }
        else
        {
            stockProfitLoss.text = "–%";
            stockValue.text = "–";
        }
    }
}
