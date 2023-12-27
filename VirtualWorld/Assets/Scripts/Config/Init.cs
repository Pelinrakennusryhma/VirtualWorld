using Newtonsoft.Json;
using UnityEngine;
using System;
using BackendConnection;
using Authentication;
using FishNet.Transporting.Tugboat;
using FishNet.Transporting.Bayou;
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
        [SerializeField] APICalls_Client apiCalls_Client;
        [SerializeField] APICalls_Server apiCalls_Server;
        [SerializeField] ServerInit serverInit;
        [SerializeField] ClientInit clientInit;
        [SerializeField] UserSession userSession;
        [SerializeField] Tugboat tugboat;
        [SerializeField] Bayou bayou;
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
                processType = ProcessType.DEV_CLIENT; // Cloned editor, client running on editor and connecting to editor server
            }
            else if (runAsClient == true)
            {
                processType = ProcessType.DEV_CLIENT2; // Normal editor with runAsClient on, connects from editor to aws server
            }
            else
            {
                processType = ProcessType.DEV_SERVER; // Normal editor with runAsClient off, server running on editor
            }
#endif
            Debug.Log("processType: " + processType.ToString());
        }

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        void SetConfigData()
        {
            switch (processType)
            {
                case ProcessType.CLIENT:
                    string ip = Config.PROD_IpForClient;
                    string https = Config.PROD_clientBackendUrl;
                    InitBayuo(Config.PROD_URLForClient);
                    apiCalls_Client.Init(https);
                    userSession.Init();
                    clientInit.Init(false);
                    break;
                case ProcessType.SERVER:
                    serverInit.Init();
                    apiCalls_Server.Init(Config.serverBackendUrl);
                    break;
                case ProcessType.DEV_CLIENT:
                    apiCalls_Client.Init(Config.DEV_clientBackendUrl);
                    clientInit.Init(true);
                    break;
                case ProcessType.DEV_CLIENT2:
                    ip = Config.PROD_IpForClient;
                    https = Config.PROD_clientBackendUrl;
                    InitTugboat(Config.PROD_IpForClient);
                    apiCalls_Client.Init(https);
                    userSession.Init();
                    clientInit.Init(false);
                    break;
                case ProcessType.DEV_SERVER:
                    serverInit.Init();
                    apiCalls_Server.Init(Config.serverBackendUrl);
                    break;
                default:
                    throw new Exception("The impossible happened: Init failed!");
            }
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        void InitTugboat(string serverAddress)
        {
            tugboat.SetClientAddress(serverAddress);
        }

        void InitBayuo(string serverAddress)
        {
            bayou.SetClientAddress(serverAddress);
        }
    }
}