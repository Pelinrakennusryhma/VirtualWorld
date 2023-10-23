using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NewsFeedViewChanger : MonoBehaviour
{
    // A reference to the prefab used to display local news.
    // Set in the inspector.
    //[Tooltip("NewsFeedHeaderButtonLocalNews is the correct prefab")]
    [SerializeField ]private GameObject NewsFeedButtonOriginalLocal;

    // A reference to the prefab used to display global news.
    // Set in the inspector.
    //[Tooltip("NewsFeedHeaderButtonGlobalNews is the correct prefab")]
    [SerializeField] private GameObject NewsFeedButtonOriginalGlobal;

    // The content object of the scrollview
    // Set in the inspector.
    //[Tooltip("Under ViewWithinAViewObjects/NewsFeedCameraPlaceholder/Canvas/Scroll View/Viewport/Content")]
    [SerializeField] private GameObject NewsFeedButtonParent;

    // A reference to the scroll view object that is activated/deactivated
    // depending on if we are showing the list of news or an individual news item
    // Set in the inspector.
    //[Tooltip("Under ViewWithinAViewObjects/NewsFeedCameraPlaceholder/Canvas/Scroll View")]
    [SerializeField] private GameObject ScrollView;

    // The header of the whole newsfeed. It probably reads something like: NEWS
    // Set in the inspector.
    //[Tooltip("Under ViewWithinAViewObjects/NewsFeedCameraPlaceholder/Canvas/Header")]
    [SerializeField] private TextMeshProUGUI NewsFeedHeader;

    // The header of an individual news item. The text is set when an individual
    // news item becomes displayed.
    // Set in the inspector.
    //[Tooltip("Under ViewWithinAViewObjects/NewsFeedCameraPlaceholder/Canvas/NewsHeader")]
    [SerializeField] private TextMeshProUGUI NewsFeedNewsHeader;

    // The content of an individual news item. The text is set when an individual
    // news item becomes displayed.
    // Set in the inspector.
    //[Tooltip("Under ViewWithinAViewObjects/NewsFeedCameraPlaceholder/Canvas/NewsContent")]
    [SerializeField] private TextMeshProUGUI NewsFeedNewsContent;

    // Reference to the button used to close an individual news item view 
    [SerializeField] private Button CloseButton;

    // Just a bool to keep track of the fact if we are showing an individual news item or not.
    private bool isShowingANewsItem;

    // Activates/deactivates proper objects to show a list of news
    public void ShowNewsList()
    {
        // We are not showing an individual news item,
        // so set the bool to false.
        isShowingANewsItem = false;

        // We need to activate the scroll view that
        // holds the news item buttons as it's content
        ScrollView.SetActive(true);

        // Display "NEWS" text
        NewsFeedHeader.gameObject.SetActive(true);

        // Set the indidual news item header inactive...
        NewsFeedNewsHeader.gameObject.SetActive(false);

        // as well as the content part of the individual news item
        NewsFeedNewsContent.gameObject.SetActive(false);

        // We don't need a close button right now, only
        // when we are showing an individual news item
        CloseButton.gameObject.SetActive(false);

        //Debug.LogError("Showing news list");
    }


    // Activates/deactivates proper objects to show an individual news item
    public void ShowIndividualNews(NewsFeedItem news)
    {
        // We are showing an individual news item,
        // so set the bool to true
        isShowingANewsItem = true;

        // We don't want to show the list of news item headers
        ScrollView.SetActive(false);

        // No need for the text "NEWS"
        NewsFeedHeader.gameObject.SetActive(false);

        // Set the header of the individual news item active...
        NewsFeedNewsHeader.gameObject.SetActive(true);

        // ...as well as the content.
        NewsFeedNewsContent.gameObject.SetActive(true);

        // Set the text to news item header from news item
        NewsFeedNewsHeader.text = news.Header;

        // Set the text to news item content from news item
        NewsFeedNewsContent.text = news.Content;        
        
        // We want to be able to close the view,
        // so we set the close button active
        CloseButton.gameObject.SetActive(true);

    }

    // Called when the tablet is closed
    public void OnCloseNewsFeed()
    {
        // Unsubscribe from the event of news being updated
        NewsFeedController.Instance.OnNewsUpdated -= OnNewsUpdated;
    }

    // Make the news feed view show the news buttons
    public void InitializeNewsFeed()
    {

        //Debug.LogError("Initializing news feed");
        //.. Unsubscribe (just in case) and subscribe to the event
        // on NewsFeedController.Instance that tells us when the
        // news are updated.
        NewsFeedController.Instance.OnNewsUpdated -= OnNewsUpdated;
        NewsFeedController.Instance.OnNewsUpdated += OnNewsUpdated;


        // Activate/deactivate proper objects
        // to show the news feed
        ShowNewsList();


        // We clear the view of any previous news feed buttons...

        // ...first fetch the newsfeed buttons from the scroll view content parent
        NewsFeedButton[] newsFeedButtons = NewsFeedButtonParent.GetComponentsInChildren<NewsFeedButton>();

        // Loop through the buttons...
        for (int i = 0; i < newsFeedButtons.Length; i++)
        {
            // ...unsibscribe from the event of news item clicked...
            newsFeedButtons[i].OnNewsItemClicked -= OnNewsItemClicked;
            // ...and destroy the button with the gameObject.
            DestroyImmediate(newsFeedButtons[i].gameObject);
        }


        // Get a list of global news from NewsFeedController.Instance
        List<NewsFeedItem> globalNews = NewsFeedController.Instance.GetGlobalNews();

        // Get a list of local news from NewsFeedController.Instance
        List<NewsFeedItem> localNews = NewsFeedController.Instance.GetLocalNews();

        // Instantiate all global news as buttons to be clicked
        for (int i = 0; i < globalNews.Count; i++)
        {
            InstantiateButton(globalNews, i, NewsFeedButtonOriginalGlobal);
        }

        // Instantiate all local news as buttons to be clicked
        for (int i = 0; i < localNews.Count; i++)
        {
            InstantiateButton(localNews, i, NewsFeedButtonOriginalLocal);
        }
    }

    // This method instantiates a clickable newsfeed button as a child of
    // the Scroll View content that is the NewsFeedButtonParent -object.
    private void InstantiateButton(List<NewsFeedItem> newsFeedItemList,
                                   int i,
                                   GameObject buttonOriginal)
    {
        // Instantiates a new news feed button according to what was passed into the method into the scroll view parent,
        // then it fetches the news feed button component of the object
        NewsFeedButton newsFeedButton = Instantiate(buttonOriginal, NewsFeedButtonParent.transform).GetComponent<NewsFeedButton>();

        // Make sure the button is actually active.
        newsFeedButton.gameObject.SetActive(true);

        // Setup the button with data from the news feed item list
        newsFeedButton.SetupButton(newsFeedItemList[i]);

        // Just in case unsubscribe and then subscribe to the event of news item clicked.
        newsFeedButton.OnNewsItemClicked -= OnNewsItemClicked;
        newsFeedButton.OnNewsItemClicked += OnNewsItemClicked;
    }

    // Called when a NewsFeedButton fires an event that informs us that
    // the news item was clicked and we get data about the item id and
    // the actual NewsFeedItem that informs us about the header, content,
    // priority and id of the item in question
    private void OnNewsItemClicked(int itemID,
                                   NewsFeedItem item)
    {
        // Tell the NewsFeedWindowChanger to display the item.
        ShowIndividualNews(item);
    }

    // Called when NewsFeedController fires an event about the news being
    // updated
    private void OnNewsUpdated()
    {
        // If we are not showing an individual news item...
        if (!isShowingANewsItem)
        {
            // ... we update the view
            InitializeNewsFeed();
        }
    }

    public void OnCloseButtonPressed()
    {
        InitializeNewsFeed();
        //Debug.Log("Close button pressed");
    }
}
