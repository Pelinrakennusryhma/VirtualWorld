using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsFeedButton : MonoBehaviour
{
    public int NewsId;

    public NewsFeedItem NewsFeedItem;

    public delegate void ClickedNewsItem(int id, NewsFeedItem item);
    public ClickedNewsItem OnNewsItemClicked;

    public void OnClick()
    {
        if (OnNewsItemClicked != null)
        {
            OnNewsItemClicked(NewsId, NewsFeedItem);
        }
    }
}
