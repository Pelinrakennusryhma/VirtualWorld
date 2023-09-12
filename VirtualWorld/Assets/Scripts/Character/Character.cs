using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Authentication;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using BackendConnection;
using FishNet.Object;

namespace Characters
{
    public class Character : NetworkBehaviour
    {
        public static Character Instance { get; private set; }
        [field: SerializeField] public GameObject OwnedCharacter { get; private set; }

        //[SerializeField] CharacterData characterData;

        [SerializeField] WebSocketConnection wsConnection;
        [SerializeField] UserSession userSession;

        [SerializeField] public InventoryController inventoryController { get; private set; }
        [SerializeField] public PlayerEmitter PlayerEmitter { get; private set; }

        //public UnityEvent<Inventory> EventInventoryChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                //DontDestroyOnLoad(gameObject);
                inventoryController = GetComponent<InventoryController>();
            }
        }

        public void SetPlayerGameObject(PlayerEmitter playerEmitter, GameObject go)
        {
            PlayerEmitter = playerEmitter;
            OwnedCharacter = go;
        }

        //public override void OnNetworkSpawn()
        //{
        //    userSession = UserSession.Instance;
        //    wsConnection = WebSocketConnection.Instance;

        //    wsConnection.EventIncomingCharacterData.AddListener(OnIncomingCharacterDataClientRpc);

        //    if (isClient)
        //    {
        //        GetCharacterDataServer(userSession.LoggedUserData.id);
        //    }
        //}

        //[ClientRpc]
        //void OnIncomingCharacterDataClientRpc(CharacterData charData)
        //{
        //    if(charData.user == userSession.LoggedUserData.id)
        //    {
        //        characterData = charData;
        //        EventInventoryChanged.Invoke(characterData.inventory);
        //        Debug.Log("my data");
        //    } else
        //    {
        //        Debug.Log("NOT my data");
        //    }
        //}

        [ServerRpc]
        public void AddMoneyServer(string userId, int amount)
        {
            wsConnection.AddMoneyToCharacter(userId, amount);
        }

        [ServerRpc]
        void GetCharacterDataServer(string id)
        {
            wsConnection.GetCharacterData(id);
        }

        Vector3 cachedPos = Vector3.zero;
        public void DisablePlayerCharacter()
        {
            cachedPos = OwnedCharacter.transform.position;
            OwnedCharacter.SetActive(false);
        }
    }
}