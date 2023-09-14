using Authentication;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using FishNet.Object;
using UnityEngine;
using FishNet.Object.Synchronizing;

namespace Authentication
{
    public class Nameplate : NetworkBehaviour
    {
        [SerializeField] TMP_Text nameplate;
        [SyncVar] string username;

        private void Awake()
        {
            if (nameplate == null)
            {
                nameplate = GetComponentInChildren<TMP_Text>();
            }
        }

        //private void Start()
        //{
        //    nameplate.text = username;
        //}

        public override void OnStartClient()
        {
            base.OnStartClient();

            if (nameplate != null)
            {
                if (IsOwner && IsClient)
                {
                    SetNameServerRpc(UserSession.Instance.LoggedUserData.username);
                    nameplate.text = UserSession.Instance.LoggedUserData.username;
                } else
                {
                    nameplate.text = username;
                }
            }
        }

        [ServerRpc]
        void SetNameServerRpc(string name)
        {
            username = name;
            SetNameObserversRpc(name);
        }
        [ObserversRpc]
        void SetNameObserversRpc(string name)
        {
            nameplate.text = name;
        }
    }
}
