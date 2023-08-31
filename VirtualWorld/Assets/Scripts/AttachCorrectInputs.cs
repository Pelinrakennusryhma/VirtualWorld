using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;


// https://forum.unity.com/threads/mlapi-with-new-input-system-clients-have-the-wrong-actions-in-player-input-component.1232821/

public class AttachCorrectInputs : NetworkBehaviour
{
    // Get "Player Input" component which sould be attached to player prefab
    private PlayerInput playerInput;
    // Assign correct InputActionAsset in the player's prefab inspector
    [SerializeField] private InputActionAsset inputActionAsset;

    private void Start()
    {
        // Get "Player Input" component when Player is initialized
        playerInput = gameObject.GetComponent<PlayerInput>();

        if (!IsOwner)
        {
            playerInput.enabled = false;
        }
    }

    // On spawn
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        playerInput = gameObject.GetComponent<PlayerInput>();

        playerInput.actions = inputActionAsset;

        // Make sure this belongs to us
        if (!IsOwner) { return; }
        // check if we have the wrong inputActionAsset
        if (playerInput.actions != inputActionAsset)
        {
            // if we have the wrong one, we assign the correct one

        }
    }
}