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
        PUT,
        DELETE
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
        public QuestsData quests;
    }

    public struct QuestsData
    {
        public List<ActiveQuestData> activeQuests;
        public List<CompletedQuestData> completedQuests;
        public FocusedQuestData focusedQuest;
    }

    public struct CompletedQuestData
    {
        public string id;
        public bool deleteFromActives;
        public bool resetFocused;

        public CompletedQuestData(string id, bool deleteFromActives = true, bool resetFocused = true)
        {
            this.id = id;
            this.deleteFromActives = deleteFromActives;
            this.resetFocused = resetFocused;
        }
    }

    public struct FocusedQuestData
    {
        public string id;
        public FocusedQuestData(string id)
        {
            this.id = id;
        }
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

    public enum ModifyItemDataOperation
    {
        ADD,
        REMOVE
    }

    public struct ModifyItemData
    {
        public string itemId;
        public string itemName;
        public string operation;
        public int amount;
        
        public ModifyItemData(string itemId, string itemName, ModifyItemDataOperation operation, int amount)
        {
            this.itemId = itemId;
            this.itemName = itemName;
            this.operation = operation.ToString();
            this.amount = amount;
        }
    }

    public struct ActiveQuestData
    {
        public string id;
        public int step;
        public int stepProgress;

        public ActiveQuestData(string id, int step, int stepProgress)
        {
            this.id = id;
            this.step = step;
            this.stepProgress = stepProgress;
        }
    }
}
