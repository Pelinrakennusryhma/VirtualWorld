using Authentication;
using BackendConnection;
using Characters;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
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
            //DontDestroyOnLoad(gameObject);
        }
    }

    //public override void OnNetworkSpawn()
    //{
    //    if (!isHost)
    //    {
    //        Destroy(this);
    //    } else
    //    {
    //        StartCoroutine(GetCharData());
    //    }
    //}

    IEnumerator GetCharData()
    {
        do
        {
            CheckIfServerIsReadyServer();
            yield return new WaitForSeconds(0.2f);
        } while (!serverIsReady);

        WebSocketConnection.Instance.GetCharacterData(UserSession.Instance.LoggedUserData.id);
    }
    [Server]
    void CheckIfServerIsReadyServer()
    {
        serverIsReady = true;
    }
}
