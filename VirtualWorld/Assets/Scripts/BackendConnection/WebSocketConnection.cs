using UnityEngine;
using NativeWebSocket;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using APICalls;
using Authentication;

public class WebSocketConnection : NetworkBehaviour
{
    [SerializeField] string webSocketAddress;
    [SerializeField] BackendConnection backendConnection;
    WebSocket websocket;

    private void Awake()
    {
        if(backendConnection == null)
        {
            backendConnection = GetComponent<BackendConnection>();
        }
        backendConnection.OnAuthSuccess.AddListener(async (LoggedUserData data) => await Connect(data));
    }

    async UniTask Connect(LoggedUserData loggedUserData)
    {
        //if (!IsServer || !IsHost) Destroy(this);
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
            Debug.Log("OnMessage!");
            Debug.Log(bytes);

            // getting the message as a string
            // var message = System.Text.Encoding.UTF8.GetString(bytes);
            // Debug.Log("OnMessage! " + message);
        };

        // Keep sending messages at every 0.3s

        //InvokeRepeating("SendWebSocketMessage", 0.0f, 0.3f);

        // waiting for messages
        await websocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        if(websocket != null) websocket.DispatchMessageQueue();
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
        if(websocket != null)
        {
            await websocket.Close();
        }
    }
}