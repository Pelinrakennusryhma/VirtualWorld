using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Authentication;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using BackendConnection;
using Unity.Netcode;

namespace Authentication
{
    public class UserSession : NetworkBehaviour
    {
        public static UserSession Instance { get; private set; }

        public LoggedUserData LoggedUserData { get; private set; }
        [SerializeField] CharacterData characterData;
        public UnityEvent CharacterDataInited;

        [SerializeField] APICalls apiCalls;
        [SerializeField] WebSocketConnection wsConnection;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            if (apiCalls == null)
            {
                apiCalls = GetComponent<APICalls>();
            }
            apiCalls.OnAuthSuccess.AddListener(OnAuthSuccess);
            CharacterDataInited.AddListener(OnCharacterDataInited);
        }

        private void Start()
        {
            CheckForSavedJWT();
        }

        void CheckForSavedJWT()
        {
            string jwt = PlayerPrefs.GetString("jwt", "");

            if (jwt != "")
            {
                apiCalls.AuthWithJWT(jwt);
            }
            else
            {
                apiCalls.OnNoLoggedUser.Invoke();
            }
        }

        void OnAuthSuccess(LoggedUserData data)
        {
            LoggedUserData = data;
            
        }

        void OnCharacterDataInited()
        {
            Debug.Log("money: " + characterData.inventory.money);
            AddMoneyServerRpc(LoggedUserData.id, 5);
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsClient)
            {
                GetCharacterDataServerRpc(LoggedUserData.id);
            } 
            else if(IsServer) 
            {
                wsConnection = apiCalls.GetComponent<WebSocketConnection>();
            }

        }

        [ServerRpc(RequireOwnership = false)]
        void AddMoneyServerRpc(string userId, int amount)
        {
            wsConnection.AddMoneyToCharacter(userId, amount);
        }

        [ServerRpc(RequireOwnership = false)]
        void GetCharacterDataServerRpc(string id)
        {
            wsConnection.GetCharacterData(id);
        }

        [ClientRpc]
        public void SetCharacterDataClientRpc(CharacterData charData)
        {
            if (IsServer)
            {
                return;
            }

            if(charData.user == LoggedUserData.id)
            {
                Debug.Log("MY CHAR DATA");
                characterData = charData;
                CharacterDataInited.Invoke();
            } else
            {
                Debug.Log("NOT MY CHAR DATA");
            }
        }

        [ClientRpc]
        public void SetInventoryClientRpc(Inventory inventory, string user)
        {
            if (IsServer)
            {
                return;
            }
            Debug.Log(user + " --- " + LoggedUserData.id);
            if (user == LoggedUserData.id)
            {
                Debug.Log("MY INVENTORY, money: " + inventory.money);

            }
            else
            {
                Debug.Log("NOT MY INVENTORY");
            }
        }
    }
}

