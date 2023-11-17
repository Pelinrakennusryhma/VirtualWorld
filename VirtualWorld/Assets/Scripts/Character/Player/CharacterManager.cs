using UnityEngine;
using Authentication;
using UnityEngine.Events;
using BackendConnection;
using FishNet.Object;
using Dev;
using UI;
using FishNet.Connection;
using System.Collections.Generic;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
namespace Characters
{
    public class CharacterManager : NetworkBehaviour
    {
        public static CharacterManager Instance { get; private set; }
        public GAME_STATE gameState = GAME_STATE.FREE;
        [field: SerializeField] public GameObject OwnedCharacter { get; private set; }

        [SerializeField] CharacterData characterData;
        [SerializeField] public PlayerEmitter PlayerEmitter { get; private set; }

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
        }

        public void SetGameState(GAME_STATE newState)
        {
            CharacterManager.Instance.gameState = newState;
            PlayerEvents.Instance.CallEventGameStateChanged(newState);
        }

        public void SetOwnedCharacter(GameObject obj)
        {
            OwnedCharacter = obj;
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            GetCharacterDataServerRpc(LocalConnection, UserSession.Instance.LoggedUserData.id);
        }

        [ServerRpc(RequireOwnership = false)]
        public void GetCharacterDataServerRpc(NetworkConnection conn, string id)
        {
            APICalls_Server.Instance.GetCharacterData(conn, id, AcceptCharacterData);
        }

        [TargetRpc]
        void AcceptCharacterData(NetworkConnection conn, CharacterData characterData)
        {
            Utils.DumpToConsole(characterData);
            PlayerEvents.Instance.CallEventCharacterDataSet(characterData);
        }

        public void AddMoney(int amount)
        {
            // 000 is temp id for money, the "money" itemName should be grabbed from a Unity item database, it's just there for being visible in mongodb
            ModifyItemAmount("000", ModifyItemDataOperation.ADD, amount, "money");
        }

        public void RemoveMoney(int amount)
        {
            // 000 is temp id for money, the "money" itemName should be grabbed from a Unity item database, it's just there for being visible in mongodb
            ModifyItemAmount("000", ModifyItemDataOperation.REMOVE, amount, "money");
        }

        public void BuyItem(string itemId, string itemName, int cost)
        {
            ModifyItemData costData = new ModifyItemData("000", "money", ModifyItemDataOperation.REMOVE, cost);
            ModifyItemData purchaseData = new ModifyItemData("666", itemName, ModifyItemDataOperation.ADD, 1);
            ModifyItemDataCollection dataCollection = new ModifyItemDataCollection(costData, purchaseData);
            ModifyItemServerRpc(LocalConnection, UserSession.Instance.LoggedUserData.id, dataCollection);
        }

        void ModifyItemAmount(string itemId, ModifyItemDataOperation operation, int amount, string itemName = "")
        {
            ModifyItemData data = new ModifyItemData(itemId, itemName, operation, amount);
            ModifyItemDataCollection dataCollection = new ModifyItemDataCollection(data);
            ModifyItemServerRpc(LocalConnection, UserSession.Instance.LoggedUserData.id, dataCollection);
        }

        [ServerRpc(RequireOwnership = false)]
        void ModifyItemServerRpc(NetworkConnection conn, string userId, ModifyItemDataCollection dataCollection)
        {
            APICalls_Server.Instance.ModifyInventoryItemAmount(conn, userId, dataCollection, ModifyItemTargetRpc);
        }

        [TargetRpc]
        public void ModifyItemTargetRpc(NetworkConnection conn, Inventory inventory)
        {
            if(inventory.items.Count == 0)
            {
                Debug.Log("Empty response: " + inventory.items + ", likely due to not having enough credits for purchase.");
                return;
            }

            foreach (InventoryItem item in inventory.items)
            {
                if (item.id == "000")
                {
                    PlayerEvents.Instance.CallEventMoneyAmountChanged(item);
                }
            }
        }
    }
}
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed