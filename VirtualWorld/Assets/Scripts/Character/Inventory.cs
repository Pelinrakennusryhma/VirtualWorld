using Authentication;
using BackendConnection;
using Dev;
using FishNet.Connection;
using FishNet.Object;
using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
namespace Characters
{
    public class Inventory : NetworkBehaviour
    {
        [SerializeField] ItemDatabase itemDatabase;
        public static Inventory Instance { get; private set; }
        [SerializeField] public List<InventoryItem> Items { get; private set; }

        [field: SerializeField] public Item CreditItem { get; private set; }
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }

            PlayerEvents.Instance.EventCharacterDataSet.AddListener(OnCharacterDataSet);
            itemDatabase.Init();
        }

        void OnCharacterDataSet(CharacterData data)
        {
            SetInventory(data.inventory);
        }

        void SetInventory(InventoryData inventoryData)
        {
            Items = new List<InventoryItem>();

            // used to keep track if backend sends credits item
            InventoryItem credits = null;

            foreach (InventoryItemData itemData in inventoryData.items)
            {

                Item item = itemDatabase.GetItemById(itemData.id);

                if (item != null)
                {

                    InventoryItem invItem = new InventoryItem(item, itemData.amount);
                    Items.Add(invItem);

                    if (item.Id == CreditItem.Id)
                    {
                        credits = invItem;                 
                    }
                }
            }

            // in case all credits have been spent and backend doesn't return the item at all,
            // create InventoryItem with amount of 0 so we can call the event for UI and such
            if(credits == null)
            {
                credits = new InventoryItem(CreditItem, 0);
            }

            PlayerEvents.Instance.CallEventMoneyAmountChanged(credits);
            PlayerEvents.Instance.CallEventInventoryChanged(Items);
        }

        public void AddMoney(double amount)
        {
            ModifyItemAmount(CreditItem, ModifyItemDataOperation.ADD, amount);
        }

        public void RemoveMoney(double amount)
        {
            ModifyItemAmount(CreditItem, ModifyItemDataOperation.REMOVE, amount);
        }

        public void AddItem(Item item, int amount = 1)
        {
            ModifyItemAmount(item, ModifyItemDataOperation.ADD, amount);
        }

        public void RemoveItem(Item item, int amount = 1)
        {
            ModifyItemAmount(item, ModifyItemDataOperation.REMOVE, amount);
        }

        public void BuyItem(Item item)
        {
            ModifyItemData costData = new ModifyItemData(CreditItem.Id, ModifyItemDataOperation.REMOVE, item.Value);
            ModifyItemData purchaseData = new ModifyItemData(item.Id, ModifyItemDataOperation.ADD, 1);
            ModifyItemDataCollection dataCollection = new ModifyItemDataCollection(costData, purchaseData);
            ModifyItemServerRpc(LocalConnection, UserSession.Instance.LoggedUserData.id, dataCollection);
        }

        void ModifyItemAmount(Item item, ModifyItemDataOperation operation, double amount)
        {
            ModifyItemData data = new ModifyItemData(item.Id, operation, amount);
            ModifyItemDataCollection dataCollection = new ModifyItemDataCollection(data);
            ModifyItemServerRpc(LocalConnection, UserSession.Instance.LoggedUserData.id, dataCollection);
        }

        [ServerRpc(RequireOwnership = false)]
        void ModifyItemServerRpc(NetworkConnection conn, string userId, ModifyItemDataCollection dataCollection)
        {
            APICalls_Server.Instance.ModifyInventoryItemAmount(conn, userId, dataCollection, ModifyItemTargetRpc);
        }

        [TargetRpc]
        public void ModifyItemTargetRpc(NetworkConnection conn, InventoryData inventoryData)
        {
            if (inventoryData.items.Count == 0)
            {
                Debug.Log("Empty response: " + inventoryData.items + ", likely due to not having enough credits for purchase.");
                return;
            }

            SetInventory(inventoryData);
        }
    }
}
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

