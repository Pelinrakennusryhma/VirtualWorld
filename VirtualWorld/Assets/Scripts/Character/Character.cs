using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Authentication;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using BackendConnection;
using Unity.Netcode;

namespace Characters { 
    public class Character : NetworkBehaviour
    {
        public static Character Instance { get; private set; }
        [field: SerializeField] public GameObject OwnedCharacter { get; private set; }

        [SerializeField] CharacterData characterData;

        [SerializeField] WebSocketConnection wsConnection;
        [SerializeField] UserSession userSession;

        [SerializeField] public InventoryController inventoryController { get; private set; }

        public UnityEvent<Inventory> EventInventoryChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                inventoryController = GetComponent<InventoryController>();
            }
        }

        public void SetPlayerGameObject(GameObject go)
        {
            OwnedCharacter = go;
        }

        public override void OnNetworkSpawn()
        {
            userSession = UserSession.Instance;
            wsConnection = WebSocketConnection.Instance;

            wsConnection.EventIncomingCharacterData.AddListener(OnIncomingCharacterDataClientRpc);

            if (IsClient)
            {
                GetCharacterDataServerRpc(userSession.LoggedUserData.id);
            }
        }

        [ClientRpc]
        void OnIncomingCharacterDataClientRpc(CharacterData charData)
        {
            if(charData.user == userSession.LoggedUserData.id)
            {
                characterData = charData;
                EventInventoryChanged.Invoke(characterData.inventory);
                Debug.Log("my data");
            } else
            {
                Debug.Log("NOT my data");
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void AddMoneyServerRpc(string userId, int amount)
        {
            wsConnection.AddMoneyToCharacter(userId, amount);
        }

        [ServerRpc(RequireOwnership = false)]
        void GetCharacterDataServerRpc(string id)
        {
            wsConnection.GetCharacterData(id);
        }
    }
}