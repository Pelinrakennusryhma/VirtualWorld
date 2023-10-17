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

        public void Interact(string playerId, UnityAction dummy)
        {
            string userId = UserSession.Instance.LoggedUserData.id;
            if(moneyChangeAmount > 0)
            {
                CharacterManager.Instance.AddMoney(moneyChangeAmount);
            } else
            {
                CharacterManager.Instance.RemoveMoney(Mathf.Abs(moneyChangeAmount));
            }

        }
    }
}

