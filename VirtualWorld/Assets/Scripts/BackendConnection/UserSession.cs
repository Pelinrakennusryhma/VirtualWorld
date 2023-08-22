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
        [SerializeField] LoggedUserData loggedUserData;

        [SerializeField] BackendConnection backendConnection;

        private void Awake()
        {
            if(backendConnection == null)
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

        }
    }
}

