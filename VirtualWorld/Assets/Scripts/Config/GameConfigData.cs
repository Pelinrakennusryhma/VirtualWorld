using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Configuration
{
    [Serializable]
    public struct GameConfigData
    {
        public string devIpForClient;
        public string prodIpForClient;
        public string ipForServer;
        public ushort serverPort;
        public string httpUrl;
        public string wsUrl;
        public string httpsUrl;
        public string wssUrl;
    }
}


