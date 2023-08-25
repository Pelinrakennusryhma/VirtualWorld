using UnityEngine;
using NativeWebSocket;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using BackendConnection;
using Authentication;

public class WebSocketConnection : NetworkBehaviour
{
    [SerializeField] string webSocketAddress;
    [SerializeField] APICalls apiCalls;
    [SerializeField] UserSession userSession;
    WebSocket websocket;
    LoggedUserData superUserData;

    private void Awake()
    {
        if(apiCalls == null)
        {
            apiCalls = GetComponent<APICalls>();
        }
        if(userSession == null)
        {
            userSession = GetComponent<UserSession>();
        }
        //apiCalls.OnAuthSuccess.AddListener(async (LoggedUserData data) => await Connect(data));
        apiCalls.OnAuthSuccess.AddListener((data) => superUserData = data);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        Connect(superUserData);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    }


    async UniTask Connect(LoggedUserData loggedUserData)
    {

        if (!IsServer)
        {
            Debug.Log("is not server, destroying " + IsServer);
            Destroy(this);
            return;
        }
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

    public async void GetCharacterData(string playerToken)
    {
        if (websocket.State == WebSocketState.Open)
        {
            WebSocketMessageOut msg = new WebSocketMessageOut("GetCharacterData", playerToken);
            await websocket.SendText(JsonUtility.ToJson(msg));
        }
    }

    void ParseMessage(string message)
    {
        WebSocketMessageIn wsMsg = JsonUtility.FromJson<WebSocketMessageIn>(message);

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
        CharacterData charData = JsonUtility.FromJson<CharacterData>(msg);
        //Debug.Log("id: " + charData.id + "user: " + charData.user + "money: " + charData.inventory.money);
        userSession.PublishCharacterDaraClientRpc(charData);
    }
}