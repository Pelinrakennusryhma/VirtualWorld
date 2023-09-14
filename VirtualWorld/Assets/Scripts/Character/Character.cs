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

namespace Characters
{
    public class Character : MonoBehaviour
    {
        public static Character Instance { get; private set; }
        [field: SerializeField] public GameObject OwnedCharacter { get; private set; }

        [SerializeField] CharacterData characterData;

        [SerializeField] WebSocketConnection wsConnection;
        [SerializeField] UserSession userSession;

        [SerializeField] public InventoryController inventoryController { get; private set; }
        [SerializeField] public PlayerEmitter PlayerEmitter { get; private set; }

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

        private void Start()
        {
            InstanceFinder.SceneManager.OnLoadEnd += OnSceneLoadEnd;
            userSession = UserSession.Instance;
            wsConnection = WebSocketConnection.Instance;
            wsConnection.EventIncomingCharacterData.AddListener(OnIncomingCharacterDataClientRpc);
        }

        void OnSceneLoadEnd(SceneLoadEndEventArgs args)
        {
            bool mainSceneLoaded = false;
            foreach (Scene scene in args.LoadedScenes)
            {
                if(scene.name == SceneLoader.Instance.MainSceneName)
                {
                    mainSceneLoaded = true;
                    break;
                }
            }

            if (mainSceneLoaded)
            {
                GetCharacterData(userSession.LoggedUserData.id);
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

        void OnIncomingCharacterDataClientRpc(CharacterData charData)
        {
            if (charData.user == userSession.LoggedUserData.id)
            {
                characterData = charData;
                EventInventoryChanged.Invoke(characterData.inventory);
                Debug.Log("my data");
            }
            else
            {
                Debug.Log("NOT my data");
            }
        }
        public void AddMoneyServer(string userId, int amount)
        {
            wsConnection.AddMoneyToCharacter(userId, amount);
        }

        public void GetCharacterData(string id)
        {
            wsConnection.GetCharacterData(id);
        }
    }
}