using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using System.Linq;

public class StockScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI symbol;
    [SerializeField] private TextMeshProUGUI companyName;
    [SerializeField] private TextMeshProUGUI price;
    [SerializeField] private TextMeshProUGUI priceChange;
    [SerializeField] private TextMeshProUGUI pricePercentChange;
    public Stock stock;
    private BankGameSystem gameSystem;
    public GameObject canvas;
    private int currentPattern = 0;
    private int patternLength = 7;
    private int patternDay;
    public List<double> priceHistory = new List<double>();
    void Start()
    {
        patternDay = patternLength + 1;
        canvas = GameObject.Find("Canvas");
        gameSystem = canvas.GetComponent<BankGameSystem>();
        symbol.text = stock.symbol;
        companyName.text = stock.companyName;
        priceHistory.Add(stock.basePrice);
        price.text = priceHistory.Last().ToString("C", gameSystem.culture);
        InitializePrices();
    }
    
    //Avaa painettaessa osaketta.
    public void OpenStockInfo()
    {

        //Tuhoaa vanhan
        GameObject[] gos = GameObject.FindGameObjectsWithTag("StockInfo");
        foreach (GameObject go in gos)
        {
            Destroy(go);
        }

        //Lisää uuden
        UnityEngine.Object prefab = Resources.Load("Prefabs/StockInfo");
        GameObject newItem = Instantiate(prefab, canvas.transform) as GameObject;
        newItem.name = stock.symbol + "Info";
        Vector3 mouseLocation = Input.mousePosition;
        newItem.transform.position = mouseLocation;
        StockInfo stockInfo = newItem.GetComponent<StockInfo>();
        stockInfo.stock = stock;
        stockInfo.priceHistory = priceHistory;

        //Lisää lisättyyn StockInfoon WindowGraphin
        UnityEngine.Object prefab2 = Resources.Load("Prefabs/WindowGraph");
        GameObject newItem2 = Instantiate(prefab2, newItem.transform.Find("Graph")) as GameObject;
        newItem2.GetComponent<WindowGraph>().priceHistory = priceHistory;
    }

    //Lisää osakkeelle uuden hinnan, ja päivittää tekstit uuteen hintaan. 
    //Paljonko hinta nousee tai laskee valitaan randomilla kahden numeron väliltä.
    //Esimerkkinä 0.2 muuttaisi osakkeen hintaa +20% osakkeen alkuperäisestä hinnasta.
    public void NewPrice(double max, double min)
    {
        double random = new System.Random().NextDouble();
        priceHistory.Add(priceHistory.Last() + (stock.basePrice * (random * (max - min) + min) / 10));

        price.text = priceHistory.LastOrDefault().ToString("C", gameSystem.culture);
        double priceChanged = (priceHistory.Last() - priceHistory[priceHistory.Count - 2]);
        priceChange.text = priceChanged.ToString("C", gameSystem.culture);
        pricePercentChange.text = (priceChanged / priceHistory.Last()).ToString("P");
    }

    //Alustaa hinnat alkuun.
    private void InitializePrices()
    {
        for (int i = 0; i < 90; i++)
        {
            PatternSelect();
        }
    }

    //Valitsee mitä kuviota pitäisi hintojen seurata.
    //Jos kuvio on päättynyt, valitsee satunnaisesti uuden.
    public void PatternSelect()
    {
        if(patternDay == (patternLength + 1))
        {
            int patternAmount = 7;
            currentPattern = UnityEngine.Random.Range(1, patternAmount + 1);
            patternDay = 1;
        }

        //Testausta varten. Poistamalla tämän hinnat menevät alempana olevien kuvioitten mukaan.
        //currentPattern = 1;

        switch (currentPattern)
        {
            case 1:
                PatternRandom();
                //Debug.Log(stock.symbol + " Random");
                break;
            case 2:
                PatternSmallSpikeUp();
                //Debug.Log(stock.symbol + " Small Spike Up");
                break;
            case 3:
                PatternLargeSpikeUp();
                //Debug.Log(stock.symbol + " Large Spike Up");
                break;
            case 4:
                PatternSmallSpikeDown();
                //Debug.Log(stock.symbol + " Small Spike Down");
                break;
            case 5:
                PatternLargeSpikeDown();
                //Debug.Log(stock.symbol + " Large Spike Down");
                break;
            case 6:
                PatternIncreasing();
                //Debug.Log(stock.symbol + " Increasing");
                break;
            case 7:
                PatternDecreasing();
                //Debug.Log(stock.symbol + " Decreasing");
                break;
        }

    }

    //Kuvio jossa hinta muuttuu satunnaisesti joko ylös tai alas.
    private void PatternRandom()
    {
        StockPattern LargeCapPattern = new StockPattern(
            0.03, -0.03,
            0.03, -0.03,
            0.03, -0.03,
            0.03, -0.03,
            0.03, -0.03,
            0.03, -0.03,
            0.03, -0.03);

        StockPattern MidCapPattern = new StockPattern(
            0.05, -0.05,
            0.05, -0.05,
            0.05, -0.05,
            0.05, -0.05,
            0.05, -0.05,
            0.05, -0.05,
            0.05, -0.05);

        StockPattern SmallCapPattern = new StockPattern(
            0.08, -0.08,
            0.08, -0.08,
            0.08, -0.08,
            0.08, -0.08,
            0.08, -0.08,
            0.08, -0.08,
            0.08, -0.08);


        switch (patternDay)
        {
            case 1:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day1Max, LargeCapPattern.Day1Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day1Max, MidCapPattern.Day1Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day1Max, SmallCapPattern.Day1Min);
                }
                break;

            case 2:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day2Max, LargeCapPattern.Day2Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day2Max, MidCapPattern.Day2Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day2Max, SmallCapPattern.Day2Min);
                }
                break;

            case 3:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day3Max, LargeCapPattern.Day3Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day3Max, MidCapPattern.Day3Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day3Max, SmallCapPattern.Day3Min);
                }
                break;

            case 4:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day4Max, LargeCapPattern.Day4Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day4Max, MidCapPattern.Day4Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day4Max, SmallCapPattern.Day4Min);
                }
                break;

            case 5:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day5Max, LargeCapPattern.Day5Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day5Max, MidCapPattern.Day5Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day5Max, SmallCapPattern.Day5Min);
                }
                break;

            case 6:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day6Max, LargeCapPattern.Day6Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day6Max, MidCapPattern.Day6Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day6Max, SmallCapPattern.Day6Min);
                }
                break;

            case 7:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day7Max, LargeCapPattern.Day7Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day7Max, MidCapPattern.Day7Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day7Max, SmallCapPattern.Day7Min);
                }
                break;
        }

        patternDay++;
    }

    //Kuvio jossa hinta aluksi laskee, mutta keskellä viikkoa tulee pieni piikki ylös ja sitten laskee lopun viikon.
    private void PatternSmallSpikeUp()
    {
        StockPattern LargeCapPattern = new StockPattern(
            -0.01, -0.03,
            -0.01, -0.03,
            0.02, 0.04,
            0.02, 0.04,
            0.02, 0.04,
            -0.01, -0.03,
            -0.01, -0.03);

        StockPattern MidCapPattern = new StockPattern(
            -0.03, -0.05,
            -0.03, -0.05,
            0.04, 0.06,
            0.04, 0.06,
            0.04, 0.06,
            -0.03, -0.05,
            -0.03, -0.05);

        StockPattern SmallCapPattern = new StockPattern(
            -0.06, -0.08,
            -0.06, -0.08,
            0.07, 0.09,
            0.07, 0.09,
            0.07, 0.09,
            -0.06, -0.08,
            -0.06, -0.08);


        switch (patternDay)
        {
            case 1:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day1Max, LargeCapPattern.Day1Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day1Max, MidCapPattern.Day1Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day1Max, SmallCapPattern.Day1Min);
                }
                break;

            case 2:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day2Max, LargeCapPattern.Day2Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day2Max, MidCapPattern.Day2Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day2Max, SmallCapPattern.Day2Min);
                }
                break;

            case 3:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day3Max, LargeCapPattern.Day3Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day3Max, MidCapPattern.Day3Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day3Max, SmallCapPattern.Day3Min);
                }
                break;

            case 4:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day4Max, LargeCapPattern.Day4Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day4Max, MidCapPattern.Day4Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day4Max, SmallCapPattern.Day4Min);
                }
                break;

            case 5:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day5Max, LargeCapPattern.Day5Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day5Max, MidCapPattern.Day5Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day5Max, SmallCapPattern.Day5Min);
                }
                break;

            case 6:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day6Max, LargeCapPattern.Day6Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day6Max, MidCapPattern.Day6Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day6Max, SmallCapPattern.Day6Min);
                }
                break;

            case 7:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day7Max, LargeCapPattern.Day7Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day7Max, MidCapPattern.Day7Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day7Max, SmallCapPattern.Day7Min);
                }
                break;
        }

        patternDay++;
    }

    //Kuvio jossa hinta aluksi laskee, mutta keskellä viikkoa tulee iso piikki ylös ja sitten laskee lopun viikon.
    private void PatternLargeSpikeUp()
    {
        StockPattern LargeCapPattern = new StockPattern(
            -0.02, -0.03,
            -0.02, -0.03,
            -0.02, -0.03,
            0.06, 0.09,
            0.06, 0.09,
            -0.02, -0.03,
            -0.02, -0.03);

        StockPattern MidCapPattern = new StockPattern(
            -0.04, -0.05,
            -0.04, -0.05,
            -0.04, -0.05,
            0.12, 0.15,
            0.12, 0.15,
            -0.04, -0.05,
            -0.04, -0.05);

        StockPattern SmallCapPattern = new StockPattern(
            -0.06, -0.07,
            -0.06, -0.07,
            -0.06, -0.07,
            0.18, 0.21,
            0.18, 0.21,
            -0.06, -0.07,
            -0.06, -0.07);


        switch (patternDay)
        {
            case 1:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day1Max, LargeCapPattern.Day1Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day1Max, MidCapPattern.Day1Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day1Max, SmallCapPattern.Day1Min);
                }
                break;

            case 2:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day2Max, LargeCapPattern.Day2Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day2Max, MidCapPattern.Day2Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day2Max, SmallCapPattern.Day2Min);
                }
                break;

            case 3:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day3Max, LargeCapPattern.Day3Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day3Max, MidCapPattern.Day3Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day3Max, SmallCapPattern.Day3Min);
                }
                break;

            case 4:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day4Max, LargeCapPattern.Day4Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day4Max, MidCapPattern.Day4Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day4Max, SmallCapPattern.Day4Min);
                }
                break;

            case 5:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day5Max, LargeCapPattern.Day5Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day5Max, MidCapPattern.Day5Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day5Max, SmallCapPattern.Day5Min);
                }
                break;

            case 6:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day6Max, LargeCapPattern.Day6Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day6Max, MidCapPattern.Day6Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day6Max, SmallCapPattern.Day6Min);
                }
                break;

            case 7:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day7Max, LargeCapPattern.Day7Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day7Max, MidCapPattern.Day7Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day7Max, SmallCapPattern.Day7Min);
                }
                break;
        }

        patternDay++;
    }

    //Kuvio jossa hinta aluksi nousee, mutta keskellä viikkoa tulee pieni piikki alas ja sitten nousee lopun viikon.
    private void PatternSmallSpikeDown()
    {
        StockPattern LargeCapPattern = new StockPattern(
            0.01, 0.03,
            0.01, 0.03,
            -0.02, -0.04,
            -0.02, -0.04,
            -0.02, -0.04,
            0.01, 0.03,
            0.01, 0.03);

        StockPattern MidCapPattern = new StockPattern(
            0.03, 0.05,
            0.03, 0.05,
            -0.04, -0.06,
            -0.04, -0.06,
            -0.04, -0.06,
            0.03, 0.05,
            0.03, 0.05);

        StockPattern SmallCapPattern = new StockPattern(
            0.06, 0.08,
            0.06, 0.08,
            -0.07, -0.09,
            -0.07, -0.09,
            -0.07, -0.09,
            0.06, 0.08,
            0.06, 0.08);


        switch (patternDay)
        {
            case 1:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day1Max, LargeCapPattern.Day1Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day1Max, MidCapPattern.Day1Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day1Max, SmallCapPattern.Day1Min);
                }
                break;

            case 2:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day2Max, LargeCapPattern.Day2Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day2Max, MidCapPattern.Day2Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day2Max, SmallCapPattern.Day2Min);
                }
                break;

            case 3:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day3Max, LargeCapPattern.Day3Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day3Max, MidCapPattern.Day3Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day3Max, SmallCapPattern.Day3Min);
                }
                break;

            case 4:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day4Max, LargeCapPattern.Day4Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day4Max, MidCapPattern.Day4Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day4Max, SmallCapPattern.Day4Min);
                }
                break;

            case 5:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day5Max, LargeCapPattern.Day5Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day5Max, MidCapPattern.Day5Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day5Max, SmallCapPattern.Day5Min);
                }
                break;

            case 6:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day6Max, LargeCapPattern.Day6Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day6Max, MidCapPattern.Day6Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day6Max, SmallCapPattern.Day6Min);
                }
                break;

            case 7:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day7Max, LargeCapPattern.Day7Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day7Max, MidCapPattern.Day7Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day7Max, SmallCapPattern.Day7Min);
                }
                break;
        }

        patternDay++;
    }

    //Kuvio jossa hinta aluksi nousee, mutta keskellä viikkoa tulee iso piikki alas ja sitten nousee lopun viikon.
    private void PatternLargeSpikeDown()
    {
        StockPattern LargeCapPattern = new StockPattern(
            0.02, 0.03,
            0.02, 0.03,
            0.02, 0.03,
            -0.06, -0.1,
            -0.06, -0.1,
            0.02, 0.03,
            0.02, 0.03);

        StockPattern MidCapPattern = new StockPattern(
            0.04, 0.05,
            0.04, 0.05,
            0.04, 0.05,
            -0.12, -0.15,
            -0.12, -0.15,
            0.04, 0.05,
            0.04, 0.05);

        StockPattern SmallCapPattern = new StockPattern(
            0.06, 0.07,
            0.06, 0.07,
            0.06, 0.07,
            -0.18, -0.21,
            -0.18, -0.21,
            0.06, 0.07,
            0.06, 0.07);


        switch (patternDay)
        {
            case 1:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day1Max, LargeCapPattern.Day1Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day1Max, MidCapPattern.Day1Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day1Max, SmallCapPattern.Day1Min);
                }
                break;

            case 2:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day2Max, LargeCapPattern.Day2Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day2Max, MidCapPattern.Day2Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day2Max, SmallCapPattern.Day2Min);
                }
                break;

            case 3:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day3Max, LargeCapPattern.Day3Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day3Max, MidCapPattern.Day3Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day3Max, SmallCapPattern.Day3Min);
                }
                break;

            case 4:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day4Max, LargeCapPattern.Day4Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day4Max, MidCapPattern.Day4Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day4Max, SmallCapPattern.Day4Min);
                }
                break;

            case 5:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day5Max, LargeCapPattern.Day5Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day5Max, MidCapPattern.Day5Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day5Max, SmallCapPattern.Day5Min);
                }
                break;

            case 6:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day6Max, LargeCapPattern.Day6Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day6Max, MidCapPattern.Day6Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day6Max, SmallCapPattern.Day6Min);
                }
                break;

            case 7:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day7Max, LargeCapPattern.Day7Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day7Max, MidCapPattern.Day7Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day7Max, SmallCapPattern.Day7Min);
                }
                break;
        }

        patternDay++;
    }

    //Kuvio jossa hinta nousee hieman koko viikon.
    private void PatternIncreasing()
    {
        StockPattern LargeCapPattern = new StockPattern(
            0.01, 0.02,
            0.01, 0.02,
            0.01, 0.02,
            0.01, 0.02,
            0.01, 0.02,
            0.01, 0.02,
            0.01, 0.02);

        StockPattern MidCapPattern = new StockPattern(
            0.02, 0.03,
            0.02, 0.03,
            0.02, 0.03,
            0.02, 0.03,
            0.02, 0.03,
            0.02, 0.03,
            0.02, 0.03);

        StockPattern SmallCapPattern = new StockPattern(
            0.03, 0.04,
            0.03, 0.04,
            0.03, 0.04,
            0.03, 0.04,
            0.03, 0.04,
            0.03, 0.04,
            0.03, 0.04);


        switch (patternDay)
        {
            case 1:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day1Max, LargeCapPattern.Day1Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day1Max, MidCapPattern.Day1Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day1Max, SmallCapPattern.Day1Min);
                }
                break;

            case 2:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day2Max, LargeCapPattern.Day2Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day2Max, MidCapPattern.Day2Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day2Max, SmallCapPattern.Day2Min);
                }
                break;

            case 3:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day3Max, LargeCapPattern.Day3Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day3Max, MidCapPattern.Day3Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day3Max, SmallCapPattern.Day3Min);
                }
                break;

            case 4:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day4Max, LargeCapPattern.Day4Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day4Max, MidCapPattern.Day4Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day4Max, SmallCapPattern.Day4Min);
                }
                break;

            case 5:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day5Max, LargeCapPattern.Day5Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day5Max, MidCapPattern.Day5Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day5Max, SmallCapPattern.Day5Min);
                }
                break;

            case 6:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day6Max, LargeCapPattern.Day6Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day6Max, MidCapPattern.Day6Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day6Max, SmallCapPattern.Day6Min);
                }
                break;

            case 7:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day7Max, LargeCapPattern.Day7Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day7Max, MidCapPattern.Day7Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day7Max, SmallCapPattern.Day7Min);
                }
                break;
        }

        patternDay++;
    }

    //Kuvio jossa hinta laskee hieman koko viikon.
    private void PatternDecreasing()
    {
        StockPattern LargeCapPattern = new StockPattern(
            -0.01, -0.02,
            -0.01, -0.02,
            -0.01, -0.02,
            -0.01, -0.02,
            -0.01, -0.02,
            -0.01, -0.02,
            -0.01, -0.02);

        StockPattern MidCapPattern = new StockPattern(
            -0.02, -0.03,
            -0.02, -0.03,
            -0.02, -0.03,
            -0.02, -0.03,
            -0.02, -0.03,
            -0.02, -0.03,
            -0.02, -0.03);

        StockPattern SmallCapPattern = new StockPattern(
            -0.03, -0.04,
            -0.03, -0.04,
            -0.03, -0.04,
            -0.03, -0.04,
            -0.03, -0.04,
            -0.03, -0.04,
            -0.03, -0.04);


        switch (patternDay)
        {
            case 1:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day1Max, LargeCapPattern.Day1Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day1Max, MidCapPattern.Day1Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day1Max, SmallCapPattern.Day1Min);
                }
                break;

            case 2:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day2Max, LargeCapPattern.Day2Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day2Max, MidCapPattern.Day2Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day2Max, SmallCapPattern.Day2Min);
                }
                break;

            case 3:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day3Max, LargeCapPattern.Day3Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day3Max, MidCapPattern.Day3Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day3Max, SmallCapPattern.Day3Min);
                }
                break;

            case 4:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day4Max, LargeCapPattern.Day4Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day4Max, MidCapPattern.Day4Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day4Max, SmallCapPattern.Day4Min);
                }
                break;

            case 5:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day5Max, LargeCapPattern.Day5Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day5Max, MidCapPattern.Day5Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day5Max, SmallCapPattern.Day5Min);
                }
                break;

            case 6:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day6Max, LargeCapPattern.Day6Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day6Max, MidCapPattern.Day6Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day6Max, SmallCapPattern.Day6Min);
                }
                break;

            case 7:
                if (stock.marketCap == "Large")
                {
                    NewPrice(LargeCapPattern.Day7Max, LargeCapPattern.Day7Min);
                }
                else if (stock.marketCap == "Mid")
                {
                    NewPrice(MidCapPattern.Day7Max, MidCapPattern.Day7Min);
                }
                else if (stock.marketCap == "Small")
                {
                    NewPrice(SmallCapPattern.Day7Max, SmallCapPattern.Day7Min);
                }
                break;
        }

        patternDay++;
    }
}
