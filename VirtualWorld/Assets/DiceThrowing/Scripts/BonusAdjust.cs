using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

namespace DiceMinigame
{
    public class BonusAdjust : Adjust
    {
        public string displayName = "Bonus";

        public override void Init(GameObject prefab, int amount, EventManager _eventManager)
        {
            base.Init(prefab, amount, _eventManager);
            nameField.text = displayName;
            amountField.characterValidation = TMP_InputField.CharacterValidation.Integer;
        }

        protected override void TrimValue(string value)
        {
            if (value == "")
            {
                amountField.text = "0";
            }

            if (value.Length > 1 && value.StartsWith("0"))
            {
                amountField.text = value.Substring(1);
            }

            if (value == "-0")
            {
                amountField.text = "0";
            }
        }

        public override void SetAmount(int value, bool invokeChange = true)
        {
            amountField.text = value.ToString();
            if (invokeChange)
            {
                OnAmountChanged();
            }
        }
    }
}
