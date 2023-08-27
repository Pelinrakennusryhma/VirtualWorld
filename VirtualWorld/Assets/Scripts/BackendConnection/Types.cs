using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

namespace Authentication
{
    public enum RequestType
    {
        GET,
        POST,
        PUT
    }

    public struct LoginUserData
    {
        public string username;
        public string password;

        public LoginUserData(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }

    public struct LoggedUserData
    {
        public string username;
        public string token;
        public string id;

        public LoggedUserData(string username, string token, string id)
        {
            this.username = username;
            this.token = token;
            this.id = id;
        }
    }

    public struct WebSocketMessageOut
    {
        public string type;
        public string arg;

        public WebSocketMessageOut(string type, string arg)
        {
            this.type = type;
            this.arg = arg;
        }
    }

    // {
    // "inventory" :{
    //   "money":90,
    //   "id":"64e90f204f14013fafc5ef24"
    //  },
    // "user":"64e90f1f4f14013fafc5ef20"}
    public struct InventoryWithUser : INetworkSerializable
    {
        public Inventory inventory;
        public string user;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref inventory);
            serializer.SerializeValue(ref user);
        }
    }

    public struct Inventory : INetworkSerializable
    {
        public int money;

        public Inventory(int money, string id)
        {
            this.money = money;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref money);
        }

    }

    public struct CharacterData : INetworkSerializable
    {
        public Inventory inventory;
        public string user;
        public string id;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref inventory);
            serializer.SerializeValue(ref user);
            serializer.SerializeValue(ref id);
        }

    }

    public struct WebSocketMessageIn
    {
        public string type;
        public string data;
    }
    
}
