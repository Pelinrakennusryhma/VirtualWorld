using Authentication;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using FishNet.Object;
using UnityEngine;
using FishNet.Object.Synchronizing;
using Characters;
using BackendConnection;

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

            CharacterManager.Instance?.EventCharacterDataSet.AddListener(OnCharacterDataSet);
        }

        void OnCharacterDataSet(CharacterData data)
        {
            if (IsOwner)
            {
                SetNameServerRpc(data.user.username);
            }
        }

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();

            if (!base.Owner.IsLocalClient)
            {
                nameplate.text = username;
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
