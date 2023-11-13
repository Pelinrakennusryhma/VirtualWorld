using UnityEngine;
using Authentication;
using UnityEngine.Events;
using BackendConnection;
using FishNet.Object;
using Dev;
using UI;
using FishNet.Connection;

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

        void ModifyItemAmount(string itemId, ModifyItemDataOperation operation, int amount, string itemName = "")
        {
            ModifyItemData data = new ModifyItemData(itemId, itemName, operation, amount);
            ModifyItemServerRpc(LocalConnection, UserSession.Instance.LoggedUserData.id, data);
        }

        [ServerRpc(RequireOwnership = false)]
        void ModifyItemServerRpc(NetworkConnection conn, string userId, ModifyItemData data)
        {
            APICalls_Server.Instance.ModifyInventoryItemAmount(conn, userId, data, ModifyItemTargetRpc);
        }

        [TargetRpc]
        public void ModifyItemTargetRpc(NetworkConnection conn, InventoryItem item)
        {
            if (item.id == "000")
            {
                PlayerEvents.Instance.CallEventMoneyAmountChanged(item);
            }

            else
            {
                PlayerEvents.Instance.CallEventInventoryItemAmountChanged(item);
                Debug.Log("Modify item target rpc is called with item " + item.id + " item amount is " + item.amount);
            }
        }

        #region Antti's additions, inventory stuff
        public void ModifyItem(string itemId, ModifyItemDataOperation operation, int amount, string itemName = "")
        {
            ModifyItemAmount(itemId, operation, amount, itemName);
        }

        public void AddDebt(int amount)
        {
            ModifyItemAmount("10000", ModifyItemDataOperation.ADD, amount, "Debt");
        }

        public void RemoveDebt(int amount)
        {
            ModifyItemAmount("10000", ModifyItemDataOperation.REMOVE, amount, "Debt");
        }

        public void AddToBankBalance(int amount)
        {
            ModifyItemAmount("10001", ModifyItemDataOperation.ADD, amount, "Bank balance");
        }

        public void RemoveFromBankBalance(int amount)
        {
            ModifyItemAmount("10001", ModifyItemDataOperation.REMOVE, amount, "Bank balance");
        }

        public void AddStock(string itemId, int amount, string itemName)
        {
            ModifyItemAmount(itemId, ModifyItemDataOperation.ADD, amount, itemName);
        }

        public void RemoveStock(string itemId, int amount, string itemName)
        {
            ModifyItemAmount(itemId, ModifyItemDataOperation.REMOVE, amount, itemName);
        }

        #endregion
    }
}
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed