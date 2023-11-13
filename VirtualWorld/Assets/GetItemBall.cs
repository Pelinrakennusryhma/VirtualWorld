using Authentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;
using UnityEngine.Events;

namespace WorldObjects
{
    public class GetItemBall : MonoBehaviour, I_Interactable
    {
        public string ItemId;
        public int itemChangeAmount = 1;
        [field: SerializeReference]
        public string DetectionMessage { get; set; }
        public bool IsActive => true;
        public Vector3 DetectionMessageOffSet { get => Vector3.zero; }

        void Start()
        {
            DetectionMessage = DetectionMessage.Replace("%%amount%%", Mathf.Abs(itemChangeAmount).ToString());
            DetectionMessage = DetectionMessage.Replace("%%id%%", ItemId);
        }

        public void Interact(string playerId, UnityAction dummy)
        {
            if (itemChangeAmount > 0)
            {
                //CharacterManager.Instance.AddMoney(itemChangeAmount);
                CharacterManager.Instance.ModifyItem(ItemId, BackendConnection.ModifyItemDataOperation.ADD, itemChangeAmount, "test item 1");

                InventoryHymisImplementation.Instance.AddItem(1, itemChangeAmount);
            }
            else
            {
                //CharacterManager.Instance.RemoveMoney(Mathf.Abs(itemChangeAmount));
                CharacterManager.Instance.ModifyItem(ItemId, BackendConnection.ModifyItemDataOperation.REMOVE, Mathf.Abs(itemChangeAmount), "test item 1") ;

                InventoryHymisImplementation.Instance.RemoveItem(1, itemChangeAmount);
            }

        }
    }
}