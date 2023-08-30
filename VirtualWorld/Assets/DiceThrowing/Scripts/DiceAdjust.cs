using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

namespace DiceMinigame
{
    public class DiceAdjust : Adjust
    {
        public Toggle bonusToggle;
        public Toggle penaltyToggle;
        public override void Init(GameObject prefab, int amount, EventManager _eventManager)
        {
            bonusToggle = transform.GetChild(4).GetComponent<Toggle>();
            bonusToggle.onValueChanged.AddListener(value => OnBonusToggled(value));
            penaltyToggle = transform.GetChild(5).GetComponent<Toggle>();
            penaltyToggle.onValueChanged.AddListener(value => OnPenaltyToggled(value));
            base.Init(prefab, amount, _eventManager);
            dicePrefab = prefab;
            nameField.text = prefab.name;
            amountField.characterValidation = TMP_InputField.CharacterValidation.Digit;
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

            SetAmount(int.Parse(amountField.text), true);

            //if(diceSetup.TotalDiceSetup() < 1)
            //{
            //    amountField.text = "1";
            //}
        }

        public override void SetAmount(int value, bool invokeChange = true)
        {
            if (value < 0)
            {
                value = 0;
            }

            if (value > 1)
            {
                bonusToggle.interactable = true;
                penaltyToggle.interactable = true;
            }
            else
            {
                bonusToggle.interactable = false;
                bonusToggle.isOn = false;
                penaltyToggle.interactable = false;
                penaltyToggle.isOn = false;
            }

            amountField.text = value.ToString();
            if (invokeChange)
            {
                OnAmountChanged();
            }
        }

        public bool BonusActive()
        {
            if (!bonusToggle.interactable)
            {
                return false;
            }
            return bonusToggle.isOn;
        }

        public bool PenaltyActive()
        {
            if (!penaltyToggle.interactable)
            {
                return false;
            }
            return penaltyToggle.isOn;
        }

        public void EnableBonus()
        {
            bonusToggle.isOn = true;
            OnBonusToggled(true);
        }

        public void EnablePenalty()
        {
            penaltyToggle.isOn = true;
            OnPenaltyToggled(true);
        }

        private void OnBonusToggled(bool selected)
        {
            if (selected)
            {
                penaltyToggle.isOn = false;
            }
            OnAmountChanged();
        }

        private void OnPenaltyToggled(bool selected)
        {
            if (selected)
            {
                bonusToggle.isOn = false;
            }
            OnAmountChanged();
        }
    }
}
