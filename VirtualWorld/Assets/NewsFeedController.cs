using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NewsFeedController : NetworkBehaviour
{
    public static NewsFeedController Instance;

    // Start is called before the first frame update
    public void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            DestroyImmediate(gameObject);
            Debug.Log("Destroyed extra newsfeed");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string[] GetLocalNews(int amount)
    {
        return GenerateNews(amount);
    }

    [ServerRpc]
    public void GetGlobalNewsServerRpc()
    {
        Debug.LogWarning("Server requested to update newsfeed");
    }

    public string[] GenerateNews(int amount)
    {
        string[] news = new string[amount];

        for (int i = 0; i < amount; i++)
        {
            news[i] = "This is news item number " + i.ToString();
        }

        return news;
    }
}
