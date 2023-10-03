using Cysharp.Threading.Tasks;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using Authentication;
using System;
using Newtonsoft.Json;
using FishNet.Connection;

namespace BackendConnection
{
    public class APICalls_Server : MonoBehaviour
    {
        [SerializeField]
        string baseURL = "https://localhost:3002";
        readonly string characterRoute = "/api/character/";
        readonly string inventoryRoute = "/api/inventory/";
        
        public static APICalls_Server Instance { get; private set; }

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
        }

        public void Init(string httpsUrl)
        {
            baseURL = httpsUrl;
        }

        public async UniTask GetCharacterData(NetworkConnection conn, string userId, Action<NetworkConnection, CharacterData> callback)
        {
            try
            {
                UnityWebRequest req = WebRequestUtils.CreateRequest(baseURL + characterRoute + userId, RequestType.GET);

                string text = await WebRequestUtils.GetTextAsync(req);

                CharacterData characterData = JsonConvert.DeserializeObject<CharacterData>(text);

                callback.Invoke(conn, characterData);
            }
            catch (UnityWebRequestException e)
            {
                throw e;
            }
        }

        public async UniTask ModifyInventoryItemAmount(NetworkConnection conn, string userId, ModifyItemData data, Action<NetworkConnection, InventoryItem> callback)
        {
            try
            {
                UnityWebRequest req = WebRequestUtils.CreateRequest(baseURL + inventoryRoute + userId, RequestType.PUT, data);

                string text = await WebRequestUtils.GetTextAsync(req);

                InventoryItem item = JsonConvert.DeserializeObject<InventoryItem>(text);

                callback.Invoke(conn, item);
            }
            catch (UnityWebRequestException e)
            {
                throw e;
            }
        }
    }
}
