using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Configuration
{
    [Serializable]
    public struct GameConfigData
    {
        public string ipForClient;
        public string ipForServer;
        public ushort serverPort;
        public string httpUrl;
        public string webSocketUrl;
        public ushort backendPort;
    }
}


