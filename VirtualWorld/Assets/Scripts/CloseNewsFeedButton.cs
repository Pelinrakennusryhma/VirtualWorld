using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseNewsFeedButton : MonoBehaviour
{
    public NewsFeedWindowChanger changer;

    public void CloseNewsFeed()
    {
        changer.ShowNewsList();
        Debug.LogWarning("Should close newsfeed");
    }
}
