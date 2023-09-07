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

        void Start()
        {
            DetectionMessage = DetectionMessage.Replace("%%amount%%", Mathf.Abs(moneyChangeAmount).ToString());
        }

        public void Interact(string playerId, UnityAction dummy)
        {
            string userId = UserSession.Instance.LoggedUserData.id;
            Character.Instance.AddMoneyServer(userId, moneyChangeAmount);
        }
    }
}

