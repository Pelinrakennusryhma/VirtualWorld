using Authentication;
using BackendConnection;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Characters
{
    public class InventoryController : NetworkBehaviour
    {
        [SerializeField] Character character;
        [SerializeField] WebSocketConnection wsConnection;
        [SerializeField] public Inventory Inventory { get; private set; }

        [SerializeField] private InventoryHymisImplementation InventoryHymisImplementation;

        public override void OnNetworkSpawn()
        {
            wsConnection = WebSocketConnection.Instance;

            character.EventInventoryChanged.AddListener(OnInventoryChanged);
        }

        void OnInventoryChanged(Inventory inventory)
        {
            Inventory = inventory;
            Debug.Log("inventory changed!");
        }

        public void SetHymisInventory(InventoryHymisImplementation inventory)
        {
            InventoryHymisImplementation = inventory;

            Debug.Log("Set a reference to inventory implemented by Hymi");
        }
    }
}
