using Newtonsoft.Json;
using UnityEngine;
using System;
using BackendConnection;
using Authentication;
#if UNITY_EDITOR
using ParrelSync;
#endif

namespace Configuration
{
    public enum ProcessType
    {
        CLIENT,
        SERVER,
        DEV_CLIENT,
        DEV_CLIENT2,
        DEV_SERVER,
    }

    public class Init : MonoBehaviour
    {
        [SerializeField] APICalls apiCalls;
        [SerializeField] WebSocketConnection wsConnection;
        [SerializeField] ServerInit serverInit;
        [SerializeField] ClientInit clientInit;
        [SerializeField] UserSession userSession;
        [SerializeField] TextAsset configFile;

        [Tooltip("Used in development to start a client that connects to the production server.")]
        [SerializeField] bool runAsClient;

        GameConfigData Config;
        ProcessType processType;

        void Start()
        {
            Config = JsonConvert.DeserializeObject<GameConfigData>(configFile.text);
            SetProcessType();
            SetConfigData();
        }

        void SetProcessType()
        {
            processType = ProcessType.CLIENT; // Standalone, WebGL etc. builds
#if UNITY_SERVER
            processType = ProcessType.SERVER; // Dedicated server build
#endif
#if UNITY_EDITOR
            if (ClonesManager.IsClone())
            {
                processType = ProcessType.DEV_CLIENT; // Cloned editor
            }
            else if (runAsClient == true)
            {
                processType = ProcessType.DEV_CLIENT2; // Normal editor with runAsClient on
            }
            else
            {
                processType = ProcessType.DEV_SERVER; // Normal editor with runAsClient off
            }
#endif
            Debug.Log("processType: " + processType.ToString());
            Debug.Log("runAsClient: " + runAsClient);
        }

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        void SetConfigData()
        {
            InitData initData;

            switch (processType)
            {
                case ProcessType.CLIENT:
                    string ip = Config.prodIpForClient;
                    string https = Config.httpsUrl;
                    string wss = Config.wssUrl;
                    initData = new InitData(
                        processType,
                        ip,
                        Config.serverPort,
                        https,
                        wss,
                        "",
                        ""
                        );
                    apiCalls.Init(https);
                    userSession.Init();
                    clientInit.Init(initData);
                    break;
                case ProcessType.SERVER:
                    initData = new InitData(
                        processType,
                        Config.ipForServer,
                        Config.serverPort,
                        Config.httpUrl,
                        Config.wsUrl,
                        Environment.GetEnvironmentVariable("UNITY_SERVER_USERNAME"),
                        Environment.GetEnvironmentVariable("UNITY_SERVER_PASSWORD")
                        );
                    apiCalls.Init(Config.httpUrl);
                    wsConnection.Init(Config.wsUrl);
                    serverInit.Init(initData);
                    break;
                case ProcessType.DEV_CLIENT:
                    initData = new InitData(
                        processType,
                        Config.devIpForClient,
                        Config.serverPort,
                        Config.httpUrl,
                        Config.wsUrl,
                        Environment.GetEnvironmentVariable("UNITY_CLIENT_USERNAME"),
                        Environment.GetEnvironmentVariable("UNITY_CLIENT_PASSWORD")
                        );
                    apiCalls.Init(Config.httpUrl);
                    clientInit.Init(initData);
                    break;
                case ProcessType.DEV_CLIENT2:
                    string ip2 = Environment.GetEnvironmentVariable("UNITY_SERVER_IP");
                    string https2 = Environment.GetEnvironmentVariable("UNITY_HTTPS_URL");
                    string wss2 = Environment.GetEnvironmentVariable("UNITY_WSS_URL");
                    Debug.Log("IP2 IN INIT: " + ip2);
                    initData = new InitData(
                        processType,
                        ip2,
                        Config.serverPort,
                        https2,
                        wss2,
                        "",
                        ""
                        );
                    apiCalls.Init(https2);
                    userSession.Init();
                    clientInit.Init(initData);
                    break;
                case ProcessType.DEV_SERVER:
                    initData = new InitData(
                        processType,
                        Config.ipForServer,
                        Config.serverPort,
                        Config.httpUrl,
                        Config.wsUrl,
                        Environment.GetEnvironmentVariable("UNITY_SERVER_USERNAME"),
                        Environment.GetEnvironmentVariable("UNITY_SERVER_PASSWORD")
                        );
                    apiCalls.Init(Config.httpUrl);
                    wsConnection.Init(Config.wsUrl);
                    serverInit.Init(initData);
                    break;
                default:
                    throw new Exception("The impossible happened: Init failed!");
            }
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }
    }
}