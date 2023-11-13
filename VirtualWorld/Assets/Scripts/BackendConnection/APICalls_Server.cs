using Cysharp.Threading.Tasks;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using Authentication;
using System;
using Newtonsoft.Json;
using FishNet.Connection;
using Dev;

namespace BackendConnection
{
    public class APICalls_Server : MonoBehaviour
    {
        [SerializeField]
        string baseURL = "https://localhost:3002";
        readonly string characterRoute = "/api/character";
        readonly string inventoryRoute = "/api/inventory";
        readonly string questRoute = "/quests";
        readonly string activeQuestRoute = "/active-quests";
        readonly string completedQuestRoute = "/completed-quests";
        readonly string resetQuestsRoute = "/reset-all";
        readonly string focusedQuestRoute = "/focused-quest";

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
                UnityWebRequest req = WebRequestUtils.CreateRequest(baseURL + characterRoute + "/" +userId, RequestType.GET);

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
                UnityWebRequest req = WebRequestUtils.CreateRequest(baseURL + inventoryRoute + "/" + userId, RequestType.PUT, data);

                string text = await WebRequestUtils.GetTextAsync(req);

                InventoryItem item = JsonConvert.DeserializeObject<InventoryItem>(text);

                callback.Invoke(conn, item);
            }
            catch (UnityWebRequestException e)
            {               
                Debug.LogError(e.Message);
                throw e;


            }
        }

        public async UniTask AddActiveQuest(NetworkConnection conn, string userId, ActiveQuestData data,Action<NetworkConnection, ActiveQuestData> callback)
        {
            try
            {
                UnityWebRequest req = WebRequestUtils.CreateRequest(baseURL + characterRoute + "/" + userId + questRoute + activeQuestRoute, RequestType.POST, data);
                string text = await WebRequestUtils.GetTextAsync(req);

                ActiveQuestData questData = JsonConvert.DeserializeObject<ActiveQuestData>(text);

                callback.Invoke(conn, questData);
            }
            catch (UnityWebRequestException e)
            {
                throw e;
            }
        }

        public async UniTask RemoveActiveQuest(NetworkConnection conn, string userId, ActiveQuestData data)
        {
            try
            {
                UnityWebRequest req = WebRequestUtils.CreateRequest(baseURL + characterRoute + "/" + userId + questRoute + activeQuestRoute, RequestType.DELETE, data);

                // response might not be needed for anything?
                string text = await WebRequestUtils.GetTextAsync(req);
            }
            catch (UnityWebRequestException e)
            {
                throw e;
            }
        }

        public async UniTask AddCompletedQuest(NetworkConnection conn, string userId, CompletedQuestData data, Action<NetworkConnection, CompletedQuestData> callback = null)
        {
            try
            {
                UnityWebRequest req = WebRequestUtils.CreateRequest(baseURL + characterRoute + "/" + userId + questRoute + completedQuestRoute, RequestType.POST, data);
                string text = await WebRequestUtils.GetTextAsync(req);

                if (callback != null)
                {

                    CompletedQuestData questData = JsonConvert.DeserializeObject<CompletedQuestData>(text);
                    callback.Invoke(conn, questData);
                }

            }
            catch (UnityWebRequestException e)
            {
                throw e;
            }
        }

        public async UniTask AddFocusedQuest(NetworkConnection conn, string userId, FocusedQuestData data, Action<NetworkConnection, FocusedQuestData> callback = null)
        {
            try
            {
                UnityWebRequest req = WebRequestUtils.CreateRequest(baseURL + characterRoute + "/" + userId + questRoute + focusedQuestRoute, RequestType.POST, data);
                string text = await WebRequestUtils.GetTextAsync(req);

                if (callback != null)
                {

                    FocusedQuestData questData = JsonConvert.DeserializeObject<FocusedQuestData>(text);
                    callback.Invoke(conn, questData);
                }

            }
            catch (UnityWebRequestException e)
            {
                throw e;
            }
        }

        public async UniTask ClearQuestData(NetworkConnection conn, string userId)
        {
            try
            {
                UnityWebRequest req = WebRequestUtils.CreateRequest(baseURL + characterRoute + "/" + userId + questRoute + resetQuestsRoute, RequestType.DELETE);
                string text = await WebRequestUtils.GetTextAsync(req);

            }
            catch (UnityWebRequestException e)
            {
                throw e;
            }
        }
    }
}

