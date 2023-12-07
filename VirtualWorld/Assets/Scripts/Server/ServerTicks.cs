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
        public static ServerTicks Instance { get; private set; }
        [SerializeField] List<ServerTick> serverTicks;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

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
