using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Authentication;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using BackendConnection;
using FishNet.Object;
using FishNet;
using FishNet.Managing.Scened;
using System.Linq;
using Scenes;
using UnityEngine.SceneManagement;
using Dev;
using FishNet.Connection;
using FishNet.Observing;
using FishNet.Component.Observing;

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

        private void Awake()
        {
            Instance = this;
        }

        public override void OnStartNetwork()
        {
            base.OnStartClient();
            if (base.Owner.IsLocalClient)
            {
                GetCharacterDataServerRpc(LocalConnection, UserSession.Instance.LoggedUserData.id);
            }
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
            EventCharacterDataSet.Invoke(characterData);
        }

        public void AddMoneyServerRpc(string userId, int moneyChangeAmount)
        {

        }
    }
}