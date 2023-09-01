using Newtonsoft.Json;
using UnityEngine;
using System;
using BackendConnection;
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
        DEV_SERVER,
    }

    public class Init : MonoBehaviour
    {
        [SerializeField] APICalls apiCalls;
        [SerializeField] WebSocketConnection wsConnection;
        [SerializeField] ServerInit serverInit;
        [SerializeField] ClientInit clientInit;
        [SerializeField] TextAsset configFile;

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
            processType = ProcessType.CLIENT;

#if UNITY_EDITOR
            if (ClonesManager.IsClone())
            {
                processType = ProcessType.DEV_CLIENT;
            } else
            {
                processType = ProcessType.DEV_SERVER;
            }
#endif
#if UNITY_SERVER
            processType = ProcessType.SERVER;
#endif
        }

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        void SetConfigData()
        {
            InitData initData;

            switch (processType)
            {
                case ProcessType.CLIENT:
                    string ip = Environment.GetEnvironmentVariable("UNITY_SERVER_IP");
                    initData = new InitData(
                        processType,
                        ip,
                        Config.serverPort,
                        ip,
                        ip,
                        Config.backendPort,
                        "",
                        ""
                        );
                    apiCalls.Init(ip, Config.backendPort);
                    clientInit.Init(initData);
                    break;
                case ProcessType.SERVER:
                    initData = new InitData(
                        processType,
                        Config.ipForServer,
                        Config.serverPort,
                        Config.httpUrl,
                        Config.webSocketUrl,
                        Config.backendPort,
                        Environment.GetEnvironmentVariable("UNITY_SERVER_USERNAME"),
                        Environment.GetEnvironmentVariable("UNITY_SERVER_PASSWORD")
                        );
                    apiCalls.Init(Config.httpUrl, Config.backendPort);
                    wsConnection.Init(Config.webSocketUrl, Config.backendPort);
                    serverInit.Init(initData);
                    break;
                case ProcessType.DEV_CLIENT:
                    initData = new InitData(
                        processType,
                        Config.ipForClient, // CLIENTille salaiset ENVistä
                        Config.serverPort,
                        Config.httpUrl, 
                        Config.webSocketUrl,
                        Config.backendPort,
                        Environment.GetEnvironmentVariable("UNITY_CLIENT_USERNAME"),
                        Environment.GetEnvironmentVariable("UNITY_CLIENT_PASSWORD")
                        );
                    apiCalls.Init(Config.httpUrl, Config.backendPort);
                    clientInit.Init(initData);
                    break;
                case ProcessType.DEV_SERVER:
                    initData = new InitData(
                        processType,
                        Config.ipForServer, 
                        Config.serverPort, 
                        Config.httpUrl,
                        Config.webSocketUrl,
                        Config.backendPort,
                        Environment.GetEnvironmentVariable("UNITY_SERVER_USERNAME"),
                        Environment.GetEnvironmentVariable("UNITY_SERVER_PASSWORD")
                        );
                    apiCalls.Init(Config.httpUrl, Config.backendPort);
                    wsConnection.Init(Config.webSocketUrl, Config.backendPort);
                    serverInit.Init(initData);
                    break;
                default:
                    throw new Exception("The impossible happened: Init failed!");
            }
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }
    }
}
