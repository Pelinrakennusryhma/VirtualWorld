using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using StarterAssets;
using UI;
using Characters;

public class NonNetworkPlayerEmitter : MonoBehaviour
{
    [SerializeField] StarterAssetsInputs inputs;
    [SerializeField] PlayerInput playerInput;

    [SerializeField] ThirdPersonController controller;

    [SerializeField] CinemachineVirtualCamera _cinemachineVirtualCamera;
    [SerializeField] List<GameObject> ownedObjects;

    private void Awake()
    {
        foreach (GameObject gameObject in ownedObjects)
        {
            gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        _cinemachineVirtualCamera.Priority = 100;

        UIManager.Instance.SetPlayerCharacter(gameObject);
        //CharacterManager.Instance?.SetOwnedCharacter(gameObject);
        CharacterManagerNonNetworked.Instance?.SetOwnedCharacter(gameObject);

        foreach (GameObject gameObject in ownedObjects)
        {
            gameObject.SetActive(true);
        }

        controller.shouldAnimate = true;

        inputs.enabled = true;
        playerInput.enabled = true;
    }


}
