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

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsClient)
            {
                GetCharacterDataServerRpc(LoggedUserData.token);
            } 
            else if(IsServer) 
            {
                wsConnection = apiCalls.GetComponent<WebSocketConnection>();
            }

        }

        [ServerRpc(RequireOwnership = false)]
        void GetCharacterDataServerRpc(string token)
        {
            wsConnection.GetCharacterData(token);
        }

        [ClientRpc]
        public void PublishCharacterDaraClientRpc(CharacterData charData)
        {
            if (IsServer)
            {
                return;
            }

            if(charData.user == LoggedUserData.id)
            {
                Debug.Log("MY CHAR DATA");
            } else
            {
                Debug.Log("NOT MY CHAR DATA");
            }
        }
    }
}

