using UnityEngine;
using NativeWebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using BackendConnection;
using Authentication;
using Newtonsoft;
using Newtonsoft.Json;
using UnityEngine.TextCore.Text;
using UnityEngine.Events;
using FishNet.Object;

namespace BackendConnection
{
    public class WebSocketConnection : MonoBehaviour
    {
        public static WebSocketConnection Instance { get; private set; }
        [SerializeField] string webSocketAddress;
        [SerializeField] APICalls apiCalls;
        WebSocket websocket;
        LoggedUserData superUserData;

        //public UnityEvent<CharacterData> EventIncomingCharacterData;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            if (apiCalls == null)
            {
                apiCalls = GetComponent<APICalls>();
            }

            apiCalls.OnAuthSuccess.AddListener((data) => superUserData = data);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            apiCalls.OnAuthSuccess.AddListener((data) => Connect(data));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

//        public void Start()
//        {
//#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
//            Connect(superUserData);
//#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
//        }

        public void Init(string wsUrl)
        {
            webSocketAddress = wsUrl;
        }

        async UniTask Connect(LoggedUserData loggedUserData)
        {
            Debug.Log("LoggedUserData: " + loggedUserData);
            //if (!IsServer)
            //{
            //    enabled = false;
            //    return;
            //}

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("user-agent", loggedUserData.token);
            websocket = new WebSocket(webSocketAddress, headers);

            websocket.OnOpen += () =>
            {
                Debug.Log("Connection open!");
            };

            websocket.OnError += (e) =>
            {
                Debug.Log("Error! " + e);
            };

            websocket.OnClose += (e) =>
            {
                Debug.Log("Connection closed!");
            };

            websocket.OnMessage += (bytes) =>
            {
                //Debug.Log("OnMessage!");
                //Debug.Log(bytes);

                //getting the message as a string
                string message = System.Text.Encoding.UTF8.GetString(bytes);
                ParseMessage(message);
            };

            // Keep sending messages at every 0.3s
            //InvokeRepeating("SendWebSocketMessage", 0.0f, 0.3f);

            // waiting for messages
            await websocket.Connect();
        }

        void Update()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            if (websocket != null) websocket.DispatchMessageQueue();
#endif
        }

        async void SendWebSocketMessage()
        {
            if (websocket.State == WebSocketState.Open)
            {
                // Sending bytes
                await websocket.Send(new byte[] { 10, 20, 30 });

                // Sending plain text
                await websocket.SendText("plain text message");
            }
        }

        private async void OnApplicationQuit()
        {
            if (websocket != null)
            {
                await websocket.Close();
            }
        }

        public async void GetCharacterData(string id)
        {
            if (websocket.State == WebSocketState.Open)
            {
                WebSocketMessageOut msg = new WebSocketMessageOut("GetCharacterData", id);
                await websocket.SendText(JsonConvert.SerializeObject(msg));
            }
        }

        public async void AddMoneyToCharacter(string playerToken, int amount)
        {
            if (websocket.State == WebSocketState.Open)
            {
                string[] args = new string[] { playerToken, amount.ToString() };
                WebSocketMessageOut msg = new WebSocketMessageOut("AddMoneyToCharacter", JsonConvert.SerializeObject(args));
                await websocket.SendText(JsonConvert.SerializeObject(msg));
            }
        }

        void ParseMessage(string message)
        {
            WebSocketMessageIn wsMsg = JsonConvert.DeserializeObject<WebSocketMessageIn>(message);

            switch (wsMsg.type)
            {
                case "CharacterData":
                    HandleIncomingCharacterData(wsMsg.data);
                    break;
                default:
                    break;
            }
        }

        void HandleIncomingCharacterData(string msg)
        {
            //CharacterData charData = JsonConvert.DeserializeObject<CharacterData>(msg);
            //EventIncomingCharacterData.Invoke(charData);
        }
    }
}