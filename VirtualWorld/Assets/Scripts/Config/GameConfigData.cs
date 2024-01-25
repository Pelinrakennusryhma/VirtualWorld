using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Configuration
{
    [Serializable]
    public struct GameConfigData
    {
        public string PROD_IpForClient;
        public string PROD_URLForClient;
        public string DEV_clientBackendUrl;
        public string PROD_clientBackendUrl;
        public string serverBackendUrl;
    }
}