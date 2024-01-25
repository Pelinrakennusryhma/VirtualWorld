using Characters;
using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Server {
    public class ServerTicks : NetworkBehaviour
    {
        [SerializeField] List<ServerTick> serverTicks;
        bool isEnabled = false;

        public override void OnStartServer()
        {
            base.OnStartServer();

            foreach (ServerTick serverTick in serverTicks)
            {
                serverTick.Init();
            }

            StartCoroutine(DelayedEnable());
        }

        IEnumerator DelayedEnable()
        {
            yield return new WaitForSeconds(2f);
            isEnabled = true;
        }

        // Disable on clients. Only listening to event invokes needs to work on clients.
        public override void OnStartClient()
        {
            base.OnStartClient();
            enabled = false;
        }

        private void Update()
        {
            if (!isEnabled) return;
            foreach (ServerTick serverTick in serverTicks)
            {
                serverTick.CheckTick(DateTime.Now);
            }
        }
    }
}
