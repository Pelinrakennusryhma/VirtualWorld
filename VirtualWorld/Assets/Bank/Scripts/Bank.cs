using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class Bank : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI bankMoneyText;
    [SerializeField] private TMP_InputField depositAmount;
    [SerializeField] private TMP_InputField withdrawAmount;
    public BankGameSystem gameSystem;
    void Start()
    {

        bankMoneyText.text = gameSystem.bankMoney.ToString("C", gameSystem.culture);
    }

    //Tallettaa rahaa pankkiin.
    public void DepositMoney()
    {
        if (double.TryParse(depositAmount.text, out double depositAmountParsed)){
            depositAmountParsed = Math.Round(depositAmountParsed, 2);
            if (gameSystem.playerMoney >= depositAmountParsed)
            {
                gameSystem.AddMoneyToBank(depositAmountParsed);
                gameSystem.RemoveMoneyFromPlayer(depositAmountParsed);
            }
            else
            {
                gameSystem.AddMoneyToBank(gameSystem.playerMoney);
                gameSystem.RemoveMoneyFromPlayer(gameSystem.playerMoney);
            }
        }
        bankMoneyText.text = gameSystem.bankMoney.ToString("C", gameSystem.culture);
    }

    //Nostaa rahaa pankista
    public void WithdrawMoney()
    {
        if (double.TryParse(withdrawAmount.text, out double withdrawAmountParsed))
        {
            withdrawAmountParsed = Math.Round(withdrawAmountParsed, 2);
            if (gameSystem.bankMoney >= withdrawAmountParsed)
            {
                gameSystem.AddMoneyToPlayer(withdrawAmountParsed);
                gameSystem.RemoveMoneyFromBank(withdrawAmountParsed);
            }
            else
            {
                gameSystem.AddMoneyToPlayer(gameSystem.bankMoney);
                gameSystem.RemoveMoneyFromBank(gameSystem.bankMoney);
            }
        }
        bankMoneyText.text = gameSystem.bankMoney.ToString("C", gameSystem.culture);
    }

}
