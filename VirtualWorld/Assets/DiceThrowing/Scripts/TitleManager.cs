using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DiceMinigame
{
    public class TitleManager : MonoBehaviour
    {
        public EventManager eventManager;
        public TextMeshProUGUI scoreTitle;
        private List<DiceAdjust> diceAdjusts;
        private List<BonusAdjust> bonusAdjusts;
        void Awake()
        {
            eventManager.EventAdjustsSpawned.AddListener(AdjustsSpawnedEventHandler);
            eventManager.EventAdjustsChanged.AddListener(AdjustsChangedEventHandler);
            eventManager.EventTitleChanged.AddListener(UpdateTitle);
        }

        void AdjustsSpawnedEventHandler(List<DiceAdjust> _diceAdjusts, List<BonusAdjust> _bonusAdjusts)
        {
            diceAdjusts = _diceAdjusts;
            bonusAdjusts = _bonusAdjusts;

            CreateTitle();
        }

        void AdjustsChangedEventHandler()
        {
            CreateTitle();
        }

        void CreateTitle()
        {
            string titleString = "";

            List<string> dices = new List<string>();
            foreach (DiceAdjust diceAdjust in diceAdjusts)
            {
                int amount = diceAdjust.GetAmount();
                if (amount > 0)
                {
                    string bonusText = "";
                    if (diceAdjust.BonusActive())
                    {
                        amount -= 1;
                        bonusText = "(Bonus)";
                    }
                    else if (diceAdjust.PenaltyActive())
                    {
                        amount -= 1;
                        bonusText = "(Penalty)";
                    }
                    dices.Add($"{amount}{diceAdjust.dicePrefab.name}{bonusText}");
                }
            }

            for (int i = 0; i < dices.Count; i++)
            {
                titleString += dices[i];
                if (dices.Count > 1 && i != dices.Count - 1)
                {
                    titleString += "+";
                }
            }
            foreach (BonusAdjust bonusAdjust in bonusAdjusts)
            {
                int bonusAmount = bonusAdjust.GetAmount();
                if (bonusAmount > 0)
                {
                    titleString += $"+{bonusAmount}";
                }
                else if (bonusAdjust.GetAmount() < 0)
                {
                    titleString += $"{bonusAmount}";
                }
            }

            eventManager.ChangeTitle(titleString);
        }

        void UpdateTitle(string title)
        {

            scoreTitle.text = title;
        }
    }
}
