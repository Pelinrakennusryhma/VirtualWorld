using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NewsFeedWindowChanger : MonoBehaviour
{

    public GameObject ScrollView;
    public TextMeshProUGUI NewsFeedHeader;

    public TextMeshProUGUI NewsFeedNewsHeader;
    public TextMeshProUGUI NewsFeedNewsContent;

    public Button CloseButton;

    public bool IsShowingANewsItem;

    public void ShowNewsList()
    {

        IsShowingANewsItem = false;
        ScrollView.SetActive(true);
        NewsFeedHeader.gameObject.SetActive(true);

        NewsFeedNewsHeader.gameObject.SetActive(false);
        NewsFeedNewsContent.gameObject.SetActive(false);

        CloseButton.gameObject.SetActive(false);
    }

    public void ShowIndividualNews(NewsFeedItem news)
    {
        IsShowingANewsItem = true;

        ScrollView.SetActive(false);
        NewsFeedHeader.gameObject.SetActive(false);

        NewsFeedNewsHeader.gameObject.SetActive(true);
        NewsFeedNewsContent.gameObject.SetActive(true);

        CloseButton.gameObject.SetActive(true);

        NewsFeedNewsHeader.text = news.Header;
        NewsFeedNewsContent.text = news.Content;
    }
}
