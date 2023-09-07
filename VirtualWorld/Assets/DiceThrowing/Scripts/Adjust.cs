using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

namespace DiceMinigame
{
    abstract public class Adjust : MonoBehaviour
    {
        public TextMeshProUGUI nameField;
        public TMP_InputField amountField;
        public GameObject dicePrefab;
        public Button minusButton;
        public Button plusButton;
        private EventManager eventManager;
        public virtual void Init(GameObject prefab, int amount, EventManager _eventManager)
        {
            nameField = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            amountField = transform.GetChild(1).GetComponent<TMP_InputField>();

            eventManager = _eventManager;
            SetAmount(amount, false);

            minusButton = transform.GetChild(2).GetComponent<Button>();
            plusButton = transform.GetChild(3).GetComponent<Button>();
            minusButton.onClick.AddListener(Decrease);
            plusButton.onClick.AddListener(Increase);

            amountField.onValueChanged.AddListener(delegate (string value) { TrimValue(value); });
        }

        public int GetAmount()
        {
            return int.Parse(amountField.text);
        }
        protected void Increase()
        {
            int newAmount = GetAmount() + 1;
            SetAmount(newAmount);
        }

        protected void Decrease()
        {
            int newAmount = GetAmount() - 1;
            SetAmount(newAmount);
        }

        protected void OnAmountChanged()
        {
            eventManager.AdjustsChanged();
        }

        public abstract void SetAmount(int amount, bool invokeChange = true);

        protected abstract void TrimValue(string value);
    }
}