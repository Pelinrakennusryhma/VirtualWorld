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
        [field: SerializeField] public GameObject OwnedCharacter { get; private set; }

        [SerializeField] CharacterData characterData;

        [SerializeField] public InventoryController inventoryController { get; private set; }
        [SerializeField] public PlayerEmitter PlayerEmitter { get; private set; }

        public UnityEvent<CharacterData> EventCharacterDataSet;
        public UnityEvent<InventoryItem> EventMoneyAmountChanged;
        public UnityEvent<NPC> EventDialogOpened;
        public UnityEvent EventDialogClosed;

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

        #region EventCalls
        public void CallEventCharacterDataSet(CharacterData data)
        {
            EventCharacterDataSet.Invoke(data);
        }

        public void CallEventMoneyAmountChanged(InventoryItem inventoryItem)
        {
            EventMoneyAmountChanged.Invoke(inventoryItem);
        }
        public void CallEventDialogOpened(NPC npc)
        {
            EventDialogOpened.Invoke(npc);
        }

        public void CallEventDialogClosed()
        {
            EventDialogClosed.Invoke();
        }

        #endregion

        public void SetOwnedCharacter(GameObject obj)
        {
            OwnedCharacter = obj;
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            PlayerUI playerUI = FindObjectOfType<PlayerUI>();
            playerUI.SetCharacterManager(this);
            GetCharacterDataServerRpc(LocalConnection, UserSession.Instance.LoggedUserData.id);
        }

        [ServerRpc(RequireOwnership = false)]
        public void GetCharacterDataServerRpc(NetworkConnection conn, string id)
        {
            Debug.Log("SERVER RPC CALLED");

            APICalls_Server.Instance.GetCharacterData(conn, id, AcceptCharacterData);
        }

        [TargetRpc]
        void AcceptCharacterData(NetworkConnection conn, CharacterData characterData)
        {
            Debug.Log("TARGET RPC CALLED");
            Utils.DumpToConsole(characterData);
            CallEventCharacterDataSet(characterData);
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
                CallEventMoneyAmountChanged(item);
            }
        }
    }
}
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed