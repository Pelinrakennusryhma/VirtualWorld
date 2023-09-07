using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Authentication;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using BackendConnection;
using Mirror;

namespace Authentication
{
    public class UserSession : NetworkBehaviour
    {
        public static UserSession Instance { get; private set; }

        public LoggedUserData LoggedUserData { get; private set; }

        [SerializeField] APICalls apiCalls;

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

        public void Init()
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

    }
}

