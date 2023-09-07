using Authentication;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Mirror;
using UnityEngine;

namespace Authentication
{
    public class Nameplate : NetworkBehaviour
    {
        [SerializeField] TMP_Text namePlate;
        [SyncVar] string username;

        private void Awake()
        {
            if (namePlate == null)
            {
                namePlate = GetComponentInChildren<TMP_Text>();
            }
        }

        void Start()
        {
            if (namePlate != null && UserSession.Instance.LoggedUserData.username != null)
            {
                if (isLocalPlayer)
                {
                    username = UserSession.Instance.LoggedUserData.username;
                }
                else
                {
                    namePlate.text = username;
                }
            }
        }
    }
}
