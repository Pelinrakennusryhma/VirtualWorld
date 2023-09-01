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
        public string httpUrl;
        public string webSocketUrl;
        public ushort backendPort;

        // for quick development login only
        public string username;
        public string password;

        public InitData(ProcessType processType, string ip, ushort serverPort, string httpUrl, string webSocketUrl, ushort backendPort, string username, string password)
        {
            this.processType = processType;
            this.ip = ip;
            this.serverPort = serverPort;
            this.httpUrl = httpUrl;
            this.webSocketUrl = webSocketUrl;
            this.backendPort = backendPort;
            this.username = username;
            this.password = password;
        }
    }
}

