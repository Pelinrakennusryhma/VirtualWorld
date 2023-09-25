using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Configuration
{
    public struct InitData
    {
        public ProcessType processType;
        public string ip;
        public ushort serverPort;
        public string httpsUrl;

        public InitData(ProcessType processType, string ip, ushort serverPort, string httpsUrl)
        {
            this.processType = processType;
            this.ip = ip;
            this.serverPort = serverPort;
            this.httpsUrl = httpsUrl;
        }
    }
}

