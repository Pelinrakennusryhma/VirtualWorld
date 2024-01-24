using Authentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;
using UnityEngine.Events;
using Items;
using System;

namespace WorldObjects
{
    public class SellBallTest : MonoBehaviour, I_Interactable
    {
        [SerializeField] Item item;
        [field: SerializeReference]
        public string DetectionMessage { get; set; }
        public bool IsActive => true;
        public Vector3 DetectionMessageOffSet { get => Vector3.zero; }

        void Start()
        {
            DetectionMessage = DetectionMessage.Replace("%%cost%%", Math.Abs(item.Value).ToString());
            DetectionMessage = DetectionMessage.Replace("%%item%%", item.DisplayName);
        }

        public void Interact(UnityAction _)
        {
            if (Inventory.Instance.CheckIfCanSell(item, 1)) 
            {
                Inventory.Instance.SellItem(item);
            }
        }
    }
}
