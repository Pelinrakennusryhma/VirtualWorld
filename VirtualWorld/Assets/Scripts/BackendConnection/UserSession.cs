using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Authentication;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using BackendConnection;
using FishNet.Object;

namespace Authentication
{
    public class UserSession : MonoBehaviour
    {
        public static UserSession Instance { get; private set; }

        public LoggedUserData LoggedUserData { get; private set; }

        [SerializeField] APICalls_Client apiCalls_Client;

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

            if (apiCalls_Client == null)
            {
                apiCalls_Client = GetComponent<APICalls_Client>();
            }
            apiCalls_Client.OnAuthSuccess.AddListener(OnAuthSuccess);
        }

        public void Init()
        {
            CheckForSavedJWT();
        }

        void CheckForSavedJWT()
        {
            string jwt = PlayerPrefs.GetString("jwt", "");

            if (jwt != "")
            {
                apiCalls_Client.AuthWithJWT(jwt);
            }
            else
            {
                apiCalls_Client.OnNoLoggedUser.Invoke();
            }
        }

        void OnAuthSuccess(LoggedUserData data)
        {
            LoggedUserData = data;

        }

    }
}

