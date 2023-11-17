using Authentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;
using UnityEngine.Events;

namespace WorldObjects
{
    public class TransactionBall : MonoBehaviour, I_Interactable
    {
        public int cost = 3;
        public string itemId = "666";
        public string itemName = "A Religious Decoration";
        [field: SerializeReference]
        public string DetectionMessage { get; set; }
        public bool IsActive => true;
        public Vector3 DetectionMessageOffSet { get => Vector3.zero; }

        void Start()
        {
            DetectionMessage = DetectionMessage.Replace("%%cost%%", Mathf.Abs(cost).ToString());
            DetectionMessage = DetectionMessage.Replace("%%item%%", itemName);
        }

        public void Interact(string playerId, UnityAction dummy)
        {
            CharacterManager.Instance.BuyItem(itemId, itemName, cost);

        }
    }
}

