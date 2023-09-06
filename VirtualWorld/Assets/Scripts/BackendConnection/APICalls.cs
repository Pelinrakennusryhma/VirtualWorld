using Cysharp.Threading.Tasks;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using Authentication;
using System;

namespace BackendConnection
{
    public class APICalls : MonoBehaviour
    {
        [SerializeField]
        string baseURL = "https://localhost:3001";
        readonly string authRoute = "/api/auth/";
        readonly string loginRoute = "/api/login/";
        readonly string registerRoute = "/api/users/";
        
        public static APICalls Instance { get; private set; }

        public UnityEvent<LoggedUserData> OnAuthSuccess;
        public UnityEvent OnNoLoggedUser;
        public UnityEvent<UnityWebRequestException> OnAuthFailed;
        public UnityEvent OnLogout;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public void Init(string httpsUrl)
        {
            baseURL = httpsUrl;
        }

        private UnityWebRequest CreateRequest(string path, RequestType type, object data = null)
        {
            Debug.Log("path: " + path);
            UnityWebRequest request = new UnityWebRequest(path, type.ToString());

            if(data != null)
            {
                string json = JsonUtility.ToJson(data);
                byte[] jsonToSend = new UTF8Encoding().GetBytes(json);
                request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            }

            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            return request;
        }

        public async void AuthWithJWT(string jwt)
        {
            try
            {
                UnityWebRequest req = CreateRequest(baseURL + authRoute, RequestType.POST, null);
                req.SetRequestHeader("Authorization", "Bearer " + jwt);
                string text = await GetTextAsync(req);
                Debug.Log(text);
                LoggedUserData loggedUserData = JsonUtility.FromJson<LoggedUserData>(text);
                OnAuthSuccess.Invoke(loggedUserData);
            }
            catch (UnityWebRequestException e)
            {
                OnAuthFailed.Invoke(e);
            }

        }

        // get async webrequest
        async UniTask<string> GetTextAsync(UnityWebRequest req)
        {
            var op = await req.SendWebRequest();
            return op.downloadHandler.text;
        }

        public async UniTask OnBeginLogin(string username, string password, bool rememberMe)
        {
            try
            {
                LoginUserData userData = new LoginUserData(username, password);

                UnityWebRequest req = CreateRequest(baseURL + loginRoute, RequestType.POST, userData);

                string text = await GetTextAsync(req);

                LoggedUserData loggedUserData = JsonUtility.FromJson<LoggedUserData>(text);

                if (rememberMe)
                {
                    PlayerPrefs.SetString("jwt", loggedUserData.token);
                }

                OnAuthSuccess.Invoke(loggedUserData);
            }
            catch (UnityWebRequestException e)
            {
                OnAuthFailed.Invoke(e);
                throw;
            }

        }

        public async UniTask OnBeginRegister(string username, string password, bool rememberMe)
        {
            try
            {
                LoginUserData userData = new LoginUserData();
                userData.username = username;
                userData.password = password;
                UnityWebRequest req = CreateRequest(baseURL + registerRoute, RequestType.POST, userData);

                string text = await GetTextAsync(req);

                LoggedUserData loggedUserData = JsonUtility.FromJson<LoggedUserData>(text);

                if (rememberMe)
                {
                    PlayerPrefs.SetString("jwt", loggedUserData.token);
                }

                OnAuthSuccess.Invoke(loggedUserData);
            }
            catch (UnityWebRequestException e)
            {
                OnAuthFailed.Invoke(e);
                throw;
            }

        }

        public void LogOut()
        {
            PlayerPrefs.SetString("jwt", "");
            OnLogout.Invoke();
        }
    }
}

