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

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();

            if (nameplate != null)
            {
                if (base.Owner.IsLocalClient)
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

            // for server only.. probably not necessary?
            nameplate.text = name;
        }
        [ObserversRpc]
        void SetNameObserversRpc(string name)
        {
            nameplate.text = name;
        }
    }
}
