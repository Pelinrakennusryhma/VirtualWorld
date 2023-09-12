using Authentication;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using FishNet.Object;
using UnityEngine;

namespace Authentication
{
    public class Nameplate : NetworkBehaviour
    {
        [SerializeField] TMP_Text namePlate;
        string username;

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
                //if (IsOwner)
                //{
                //    username = UserSession.Instance.LoggedUserData.username;
                //}
                //else
                {
                    namePlate.text = username;
                }
            }
        }

        
    }
}
