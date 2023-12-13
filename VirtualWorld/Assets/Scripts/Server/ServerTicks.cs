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

        public override void OnStartServer()
        {
            base.OnStartServer();
        }

        // Disable on clients. Only listening to event invokes needs to work on clients.
        public override void OnStartClient()
        {
            base.OnStartClient();
            enabled = false;
        }

        private void Update()
        {
            foreach (ServerTick serverTick in serverTicks)
            {
                serverTick.CheckTick(DateTime.Now);
            }
        }
    }
}
