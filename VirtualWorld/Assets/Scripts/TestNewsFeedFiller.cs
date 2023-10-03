using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNewsFeedFiller : NetworkBehaviour
{
    public float Timer;
    private int runningGlobalID;
    private int runningLocalID;

    public void Awake()
    {
        runningLocalID = 10;
        runningLocalID = 10;
        Timer = 1.0f + Random.Range(5.0f, 12.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;

        if (Timer <= 0)
        {
            GenerateAGlobalNewsItemServerRpc();
            GenerateALocalNewsItem();
            Timer = 10.0f + Random.Range(-2.0f, 2.0f);
        }
    }

    private void GenerateALocalNewsItem()
    {
        NewsFeedItem item = new NewsFeedItem();

        item.ID = runningLocalID;

        runningLocalID++;

        if (runningLocalID > 20)
        {
            runningLocalID = 10;
        }

        item.Header = "Local news item " + Random.Range(0, 10020);
        item.Content = "This " + Random.Range(0, 10020).ToString()
                       + " is " + Random.Range(0, 10020).ToString()
                       + " test " + Random.Range(0, 10020).ToString()
                       + "content " + Random.Range(0, 10020).ToString();

        item.Priority = Random.Range(0, 100);

        NewsFeedController.Instance.AddNewsItemToLocalNews(item);
        //NewsFeedController.Instance.AddNewsItemToGlobalNewsServerRpc(item.Priority,
        //                                                             item.ID,
        //                                                             item.Header,
        //                                                             item.Content);
    }

    // We probably want to generate the news on the server. At least global news
    // Local news shouldn't matter that much, because if someone wants to mess with
    // their own news feed, perhaps they should be able to? Because in the end, who cares if the news can't be seen by others?
    [ServerRpc(RequireOwnership = false)]
    public void GenerateAGlobalNewsItemServerRpc()
    {
        // Note: this is called server side, so any parameters
        // that should define the news should be addded to the method call

        //Debug.Log("A server rpc is called");

        // Here some logic for generating the item in question...
        string header = "Global news item " + Random.Range(0, 9999).ToString();
        string content = "Yada ";

        int rando = Random.Range(0, 23);

        for (int i = 0; i < rando; i++)
        {
            content += "yada ";
        }

        content += "and yada.";

        //int id = -1; // Less than zero id's will be assigned at NewsFeedController
        int id = runningLocalID;
        runningGlobalID++;

        if (runningGlobalID > 20)
        {
            runningGlobalID = 10;
        }

        int priority = Random.Range(-5, 100); // The smaller the number, the higher it shows on the News feed

        NewsFeedController.Instance.AddNewsItemToGlobalNewsServerRpc(priority,
                                                                     id,
                                                                     header,
                                                                     content); // The server will broadcast the news item to clients
    }


}
