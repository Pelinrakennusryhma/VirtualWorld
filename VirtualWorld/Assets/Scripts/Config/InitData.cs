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
        public string webSocketUrl;

        // for quick development login only
        public string username;
        public string password;

        public InitData(ProcessType processType, string ip, ushort serverPort, string httpsUrl, string webSocketUrl, string username, string password)
        {
            this.processType = processType;
            this.ip = ip;
            this.serverPort = serverPort;
            this.httpsUrl = httpsUrl;
            this.webSocketUrl = webSocketUrl;
            this.username = username;
            this.password = password;
        }
    }
}

