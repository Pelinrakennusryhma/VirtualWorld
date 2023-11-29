using Authentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;
using UnityEngine.Events;

namespace WorldObjects
{
    public class MoneyBall : MonoBehaviour, I_Interactable
    {
        public int moneyChangeAmount = 5;
        [field: SerializeReference]
        public string DetectionMessage { get; set; }
        public bool IsActive => true;
        public Vector3 DetectionMessageOffSet { get => Vector3.zero; }

        void Start()
        {
            DetectionMessage = DetectionMessage.Replace("%%amount%%", Mathf.Abs(moneyChangeAmount).ToString());
        }

        public void Interact(UnityAction _)
        {
            if(moneyChangeAmount > 0)
            {
                Inventory.Instance.AddMoney(moneyChangeAmount);
            } else
            {
                Inventory.Instance.RemoveMoney(Mathf.Abs(moneyChangeAmount));
            }

        }
    }
}

