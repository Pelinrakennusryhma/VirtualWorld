using Cysharp.Threading.Tasks;
using System.Text;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace BackendConnection
{
    enum RequestType {
        GET, 
        POST, 
        PUT
    }

    public class BackendConnection : MonoBehaviour
    {
        [SerializeField]
        string BaseURL = "https://localhost:3001";
        string authRoute = "api/auth/";
        string loginRoute = "api/login/";
        string registerRoute = "api/users/";

        public UnityEvent<string> OnAuthSuccess;
        public UnityEvent OnNoLoggedUser;
        public UnityEvent<UnityWebRequestException> OnAuthFailed;

        void Awake()
        {
            
            if (!BaseURL.EndsWith("/"))
            {
                BaseURL += "/";
            }
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
                AuthWithJWT(jwt);
            } else
            {
                OnNoLoggedUser.Invoke();
            }
        }

        private UnityWebRequest CreateRequest(string path, RequestType type, object data = null)
        {
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

        async void AuthWithJWT(string jwt)
        {
            try
            {
                UnityWebRequest req = CreateRequest(BaseURL + authRoute, RequestType.POST, null);
                req.SetRequestHeader("Authorization", "Bearer " + jwt);
                string text = await GetTextAsync(req);
                Debug.Log(text);
                LoggedUserData loggedUserData = JsonUtility.FromJson<LoggedUserData>(text);
                OnAuthSuccess.Invoke(loggedUserData.username);
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

        struct LoginUserData
        {
            public string username;
            public string password;
        }

        struct LoggedUserData
        {
            public string username;
            public string token;
        }

        public async void OnBeginLogin(string username, string password, bool rememberMe)
        {
            try
            {
                LoginUserData userData = new LoginUserData();
                userData.username = username;
                userData.password = password;
                UnityWebRequest req = CreateRequest(BaseURL + loginRoute, RequestType.POST, userData);

                string text = await GetTextAsync(req);

                LoggedUserData loggedUserData = JsonUtility.FromJson<LoggedUserData>(text);

                if (rememberMe)
                {
                    PlayerPrefs.SetString("jwt", loggedUserData.token);
                }

                OnAuthSuccess.Invoke(loggedUserData.username);
            }
            catch (UnityWebRequestException e)
            {
                OnAuthFailed.Invoke(e);
                throw;
            }

        }

        public async void OnBeginRegister(string username, string password, bool rememberMe)
        {
            try
            {
                LoginUserData userData = new LoginUserData();
                userData.username = username;
                userData.password = password;
                UnityWebRequest req = CreateRequest(BaseURL + registerRoute, RequestType.POST, userData);

                string text = await GetTextAsync(req);

                LoggedUserData loggedUserData = JsonUtility.FromJson<LoggedUserData>(text);

                if (rememberMe)
                {
                    PlayerPrefs.SetString("jwt", loggedUserData.token);
                }

                OnAuthSuccess.Invoke(loggedUserData.username);
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
        }

    }
}

