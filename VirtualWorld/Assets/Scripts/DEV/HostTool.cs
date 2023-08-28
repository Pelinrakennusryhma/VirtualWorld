using Authentication;
using BackendConnection;
using Characters;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HostTool : NetworkBehaviour
{
    [Header("Questionable helper script to detect when server \nis ready so Host can init stuff.")]
    [SerializeField] bool serverIsReady = false;
    HostTool Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(GetCharData());

    }

    IEnumerator GetCharData()
    {
        do
        {
            if (!IsHost)
            {
                Destroy(this);
            }
            CheckIfServerIsReadyServerRpc();
            yield return new WaitForSeconds(0.2f);
        } while (!serverIsReady);

        WebSocketConnection.Instance.GetCharacterData(UserSession.Instance.LoggedUserData.id);
    }
    [ServerRpc]
    void CheckIfServerIsReadyServerRpc()
    {
        serverIsReady = true;
    }
}
