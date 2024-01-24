using BackendConnection;
using Items;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
namespace Characters
{
    public class Inventory : MonoBehaviour
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

            //CharacterData dummyData = new CharacterData();
            //dummyData.inventory = new InventoryData();
            //dummyData.inventory.items = new List<InventoryItemData>();

            //InventoryItemData dummyData0 = new InventoryItemData();
            //dummyData0.id = DummyItems[0].Id;
            //dummyData0.amount = 10;

            //InventoryItemData dummyData1 = new InventoryItemData();
            //dummyData1.id = DummyItems[1].Id;
            //dummyData1.amount = 10;

            //InventoryItemData dummyData2 = new InventoryItemData();
            //dummyData2.id = DummyItems[2].Id;
            //dummyData2.amount = 10;

            //dummyData.inventory.items.Add(dummyData0);
            //dummyData.inventory.items.Add(dummyData1);
            //dummyData.inventory.items.Add(dummyData2);

            //OnCharacterDataSet(dummyData);
                
        }

        void OnCharacterDataSet(CharacterData data)
        {
            Debug.Log("We listened to the event of character data being set");

            if (data.inventory.items == null || data.inventory.items.Count == 0 )
            {
                Debug.LogError("DAta invenotry items count is zero");
            }
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
            ModifyItemNonNetworked(dataCollection);
            //ModifyItemServerRpc(LocalConnection, UserSession.Instance.LoggedUserData.id, dataCollection);
        }

        public bool CheckIfCanSell(Item item, int amountToSell)
        {
            bool canSell = false;

            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].item.Id == item.Id)
                {
                    if (Items[i].amount >= amountToSell)
                    {
                        canSell = true;
                    }
                }
            }

            return canSell;
        }

        public void SellItem(Item item)
        {
            ModifyItemData costData = new ModifyItemData(CreditItem.Id, ModifyItemDataOperation.ADD, item.Value);
            ModifyItemData purchaseData = new ModifyItemData(item.Id, ModifyItemDataOperation.REMOVE, 1);
            ModifyItemDataCollection dataCollection = new ModifyItemDataCollection(costData, purchaseData);
            ModifyItemNonNetworked(dataCollection);
        }

        void ModifyItemAmount(Item item, ModifyItemDataOperation operation, double amount)
        {
            ModifyItemData data = new ModifyItemData(item.Id, operation, amount);
            ModifyItemDataCollection dataCollection = new ModifyItemDataCollection(data);
            ModifyItemNonNetworked(dataCollection);
            //ModifyItemServerRpc(LocalConnection, UserSession.Instance.LoggedUserData.id, dataCollection);
        }

        private bool CheckIfWeAlreadyHaveItemInInvenotry(string itemId)
        {
            bool weHaveItem = false;

            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].item.Id.Equals(itemId))
                {
                    weHaveItem = true;
                    break;
                }
            }

            return weHaveItem;
        }

        private void ModifyItemNonNetworked(ModifyItemDataCollection dataCollection)
        {
            //Debug.Log("Should modify inventory " + Time.time);

            InventoryItem credits = null;

            for (int i = 0; i < dataCollection.inventoryChanges.Count; i++) 
            {
                if (!CheckIfWeAlreadyHaveItemInInvenotry(dataCollection.inventoryChanges[i].itemId))
                {
                    Items.Add(new InventoryItem(itemDatabase.GetItemById(dataCollection.inventoryChanges[i].itemId), 0));
                }

                for (int j = 0; j < Items.Count; j++)
                {
                    if (Items[j].item.Id == CreditItem.Id)
                    {
                        credits = Items[j];
                    }

                    if (dataCollection.inventoryChanges[i].itemId == Items[j].item.Id)
                    {
                        if (dataCollection.inventoryChanges[i].operation.Equals("ADD"))
                        {
                            Items[j].amount += (int) dataCollection.inventoryChanges[i].amount;
                            //Debug.Log("Should add");
                        }

                        else if (dataCollection.inventoryChanges[i].operation.Equals("REMOVE"))
                        {
                            Items[j].amount -= (int)dataCollection.inventoryChanges[i].amount;

                            if (Items[j].amount <= 0)
                            {
                                Items[j].amount = 0;
                            }
                            //Debug.Log("Should remove");
                        }


                        //Debug.Log("Found an item to modify. Item is " + Items[j].item.DisplayName + " amount to modify is " + dataCollection.inventoryChanges[i].amount + " operation is " + dataCollection.inventoryChanges[i].operation.ToString());
                    }
                    //Debug.Log("Item at " + j + " is " + Items[j].item.DisplayName + " amount is " + Items[j].amount);
                }
            }

            if (credits == null)
            {
                Debug.LogError("Credits is all null");
            }

            List<InventoryItem> toRemoveBecauseOfZeroAmount = new List<InventoryItem>();

            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].amount <= 0)
                {
                    toRemoveBecauseOfZeroAmount.Add(Items[i]);
                }
            }

            for (int i = 0; i < toRemoveBecauseOfZeroAmount.Count; i++)
            {
                Items.Remove(toRemoveBecauseOfZeroAmount[i]);
                Debug.Log("Should have removed an item because of zero amount");
            }

            PlayerEvents.Instance.CallEventMoneyAmountChanged(credits);
            PlayerEvents.Instance.CallEventInventoryChanged(Items);
            Debug.Log("Credit item amount is " + CreditItem.Value);
        }

        //[ServerRpc(RequireOwnership = false)]
        //void ModifyItemServerRpc(NetworkConnection conn, string userId, ModifyItemDataCollection dataCollection)
        //{
        //    APICalls_Server.Instance.ModifyInventoryItemAmount(conn, userId, dataCollection, ModifyItemTargetRpc);
        //}

        //[TargetRpc]
        //public void ModifyItemTargetRpc(NetworkConnection conn, InventoryData inventoryData)
        //{
        //    if (inventoryData.items.Count == 0)
        //    {
        //        Debug.Log("Empty response: " + inventoryData.items + ", likely due to not having enough credits for purchase.");
        //        return;
        //    }

        //    SetInventory(inventoryData);
        //}
    }
}
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

