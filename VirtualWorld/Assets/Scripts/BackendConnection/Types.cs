using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using FishNet;
using UnityEngine;
using UnityEngine.UIElements;

namespace BackendConnection
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
    public struct Inventory
    {
        public List<InventoryItem> items;
    }

    public struct InventoryItem
    {
        public string id;
        public string name;
        public int amount;
    }

    public struct CharacterData
    {
        public UserData user;
        public Inventory inventory;
    }

    public struct UserData
    {
        public string username;
        public string id;
    }

    public struct WebSocketMessageIn
    {
        public string type;
        public string data;
    }

}
