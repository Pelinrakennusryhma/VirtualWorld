using Authentication;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class InventoryController : NetworkBehaviour
{
    [SerializeField] Character character;
    [SerializeField] WebSocketConnection wsConnection;
    [SerializeField] UserSession userSession;
    [SerializeField] public Inventory Inventory { get; private set; }

    public override void OnNetworkSpawn()
    {
        wsConnection = WebSocketConnection.Instance;
        userSession = UserSession.Instance;

        wsConnection.EventIncomingCharacterData.AddListener(OnIncomingCharacterDataClientRpc);
    }

    [ClientRpc]
    void OnIncomingCharacterDataClientRpc(CharacterData charData)
    {
        if (charData.user == userSession.LoggedUserData.id)
        {
            Inventory = charData.inventory;
            Debug.Log("my inventory, money: " + Inventory.money);
        }
        else
        {
            Debug.Log("NOT my inventory");
        }
    }
}
