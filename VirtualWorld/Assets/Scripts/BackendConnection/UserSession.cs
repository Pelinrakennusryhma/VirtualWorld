using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Authentication;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using APICalls;

namespace Authentication
{
    public class UserSession : MonoBehaviour
    {
        public static UserSession Instance { get; private set; }

        public LoggedUserData LoggedUserData { get; private set; }

        [SerializeField] BackendConnection backendConnection;

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

            if (backendConnection == null)
            {
                backendConnection = GetComponent<BackendConnection>();
            }
            backendConnection.OnAuthSuccess.AddListener(OnAuthSuccess);
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
                backendConnection.AuthWithJWT(jwt);
            }
            else
            {
                backendConnection.OnNoLoggedUser.Invoke();
            }
        }

        void OnAuthSuccess(LoggedUserData data)
        {
            LoggedUserData = data;
        }
    }
}

