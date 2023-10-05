using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class StockInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI symbol;
    [SerializeField] private TextMeshProUGUI companyName;
    [SerializeField] private TextMeshProUGUI industry;
    [SerializeField] private TextMeshProUGUI price;
    [SerializeField] private TextMeshProUGUI priceChange;
    [SerializeField] private TextMeshProUGUI pricePercentChange;
    [SerializeField] private TMP_InputField buyAmount;
    [SerializeField] private TMP_InputField sellAmount;
    [SerializeField] private TextMeshProUGUI owned;
    public Stock stock;
    private BankGameSystem gameSystem;
    private StockSystem stockSystem;
    public List<double> priceHistory;
    void Start()
    {
        gameSystem = GameObject.Find("Canvas").GetComponent<BankGameSystem>();
        stockSystem = GameObject.Find("Canvas").GetComponent<StockSystem>();
        symbol.text = stock.symbol;
        companyName.text = stock.companyName;
        industry.text = stock.industry;
        price.text = priceHistory.LastOrDefault().ToString("C", gameSystem.culture);

        //Vaihtaa tekstin värin joko vihreäksi tai punaiseksi riippuen onko hinta laskenut vai noussut.
        double priceChanged = (priceHistory.Last() - priceHistory[priceHistory.Count - 2]);
        priceChange.text = priceChanged.ToString("C", gameSystem.culture);
        pricePercentChange.text = (priceChanged / priceHistory.Last()).ToString("P");
        if (priceChanged == 0)
        {
            priceChange.color = new Color(0.2f, 0.2f, 0.2f, 1.0f);
            pricePercentChange.color = new Color(0.2f, 0.2f, 0.2f, 1.0f);
        }
        else if(priceChanged > 0)
        {
            priceChange.color = new Color(0.2f, 0.6f, 0.2f, 1.0f);
            pricePercentChange.color = new Color(0.2f, 0.6f, 0.2f, 1.0f);
        }
        else
        {
            priceChange.color = new Color(0.6f, 0.2f, 0.2f, 1.0f);
            pricePercentChange.color = new Color(0.6f, 0.2f, 0.2f, 1.0f);
        }
        owned.text = "Owned: " + stockSystem.playerStocks[stock];
    }


    //Ostaa osakkeen
    public void BuyStock()
    {
        if(int.TryParse(buyAmount.text, out int result))
        {
            double totalPrice = priceHistory.Last() * result;
            if(gameSystem.playerMoney >= totalPrice)
            {
                stockSystem.AddStockToPlayer(stock, result, totalPrice);
                owned.text = "Owned: " + stockSystem.playerStocks[stock];
                gameSystem.RemoveMoneyFromPlayer(priceHistory.Last() * result);
            }

        }
    }

    //Myy osakkeen
    public void SellStock()
    {
        if (int.TryParse(sellAmount.text, out int result))
        {
            if (stockSystem.playerStocks[stock] >= result)
            {
                double totalValue = priceHistory.Last() * result;
                stockSystem.RemoveStockFromPlayer(stock, result, totalValue);
                owned.text = "Owned: " + stockSystem.playerStocks[stock];
                gameSystem.AddMoneyToPlayer(totalValue);
            }
        }
    }

    //Sulkee StockInfo ikkunan
    public void CloseStockInfo()
    {
        Destroy(gameObject);
    }
}
