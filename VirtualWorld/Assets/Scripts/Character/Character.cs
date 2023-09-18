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
    public class Character : NetworkBehaviour
    {
        public static Character Instance { get; private set; }
        [field: SerializeField] public GameObject OwnedCharacter { get; private set; }

        [SerializeField] CharacterData characterData;

        [SerializeField] UserSession userSession;

        [SerializeField] public InventoryController inventoryController { get; private set; }
        [SerializeField] public PlayerEmitter PlayerEmitter { get; private set; }

        public UnityEvent<Inventory> EventInventoryChanged;

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

        public override void OnStartClient()
        {
            base.OnStartClient();
            GetCharacterData(UserSession.Instance.LoggedUserData.id);
        }

        private void Start()
        {
            //InstanceFinder.SceneManager.OnLoadEnd += OnSceneLoadEnd;
            userSession = UserSession.Instance;
        }

        //void OnSceneLoadEnd(SceneLoadEndEventArgs args)
        //{
        //    bool mainSceneLoaded = false;
        //    foreach (Scene scene in args.LoadedScenes)
        //    {
        //        if(scene.name == SceneLoader.Instance.MainSceneName)
        //        {
        //            mainSceneLoaded = true;
        //            break;
        //        }
        //    }

        //    if (mainSceneLoaded)
        //    {
        //        GetCharacterData(userSession.LoggedUserData.id);
        //    }
        //}

        public void SetPlayerGameObject(PlayerEmitter playerEmitter, GameObject go)
        {
            PlayerEmitter = playerEmitter;
            OwnedCharacter = go;
        }

        [ServerRpc]
        public void GetCharacterData(string id)
        {

        }

        public void AddMoneyServerRpc(string userId, int moneyChangeAmount)
        {

        }
    }
}