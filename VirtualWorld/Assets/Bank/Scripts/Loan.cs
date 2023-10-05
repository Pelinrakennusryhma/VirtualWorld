using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Loan : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI debtText;
    [SerializeField] private TMP_InputField loanAmount;
    [SerializeField] private TMP_InputField repayAmount;
    [SerializeField] private TextMeshProUGUI maxLoanText;
    [SerializeField] private TextMeshProUGUI interestText;
    [SerializeField] private TextMeshProUGUI DayCounter;
    public StockMarket stockMarket;
    int day = 1;
    public BankGameSystem gameSystem;
    private double maxLoan = 50000;
    private double interest = 0.1;

    void Start()
    {
        debtText.text = gameSystem.playerDebt.ToString("C", gameSystem.culture);
        maxLoanText.text = maxLoan.ToString("C", gameSystem.culture);
        interestText.text = interest.ToString("P", gameSystem.culture);
    }


    //Lainaa rahaa pankista.
    public void LoanMoney()
    {
        if (double.TryParse(loanAmount.text, out double loanAmountParsed))
        {
            loanAmountParsed = System.Math.Round(loanAmountParsed, 2);
            if ((maxLoan - gameSystem.playerLoanedMoney) >= loanAmountParsed)
            {
                gameSystem.AddMoneyToPlayer(loanAmountParsed);
                gameSystem.AddDebtToPlayer(loanAmountParsed);
            }
            else
            {
                gameSystem.AddMoneyToPlayer(maxLoan - gameSystem.playerLoanedMoney);
                gameSystem.AddDebtToPlayer(maxLoan - gameSystem.playerLoanedMoney);
            }
        }
        debtText.text = gameSystem.playerDebt.ToString("C", gameSystem.culture);
    }

    //Maksaa takaisin velkaa
    public void RepayDebt()
    {
        if (double.TryParse(repayAmount.text, out double repayAmountParsed))
        {
            repayAmountParsed = System.Math.Round(repayAmountParsed, 2);
            if (gameSystem.playerMoney >= repayAmountParsed)
            {
                gameSystem.RemoveMoneyFromPlayer(repayAmountParsed);
                gameSystem.RemoveDebtFromPlayer(repayAmountParsed);
            }
            else if(gameSystem.playerMoney <= gameSystem.playerDebt)
            {
                gameSystem.RemoveDebtFromPlayer(gameSystem.playerMoney);
                gameSystem.RemoveMoneyFromPlayer(gameSystem.playerMoney);
            }
            else
            {
                gameSystem.RemoveMoneyFromPlayer(gameSystem.playerDebt);
                gameSystem.RemoveDebtFromPlayer(gameSystem.playerDebt);
            }
        }
        if(gameSystem.playerDebt == 0)
        {
            gameSystem.playerLoanedMoney = 0;
        }
        debtText.text = gameSystem.playerDebt.ToString("C", gameSystem.culture);
    }

    //Menee seuraavaan päivään.
    public void SkipDay()
    {
        gameSystem.AddInterestToDebt(interest);
        debtText.text = gameSystem.playerDebt.ToString("C", gameSystem.culture);
        day++;
        DayCounter.text = "Day: " + day;
        stockMarket.NextPrices();
        Destroy(GameObject.FindWithTag("StockInfo"));
    }
}
