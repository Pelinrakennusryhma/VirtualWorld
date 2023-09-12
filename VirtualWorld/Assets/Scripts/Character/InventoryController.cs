using Authentication;
using BackendConnection;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using UnityEngine;

namespace Characters
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] Character character;
        [SerializeField] WebSocketConnection wsConnection;
        //[SerializeField] public Inventory Inventory { get; private set; }

        void Start()
        {
            wsConnection = WebSocketConnection.Instance;

            //character.EventInventoryChanged.AddListener(OnInventoryChanged);
        }

        //void OnInventoryChanged(Inventory inventory)
        //{
        //    Inventory = inventory;
        //    Debug.Log("inventory changed!");
        //}
    }
}
