using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using UnityEngine.UI;


// TO BE REFACTORED "HEAVILY"?---------------------------------
// This has functionality quite specific to newsfeed
// and probably that should happen in its own class
// and this component should just track button presses
// and which view should be open?
//
// ALSO, does this even need to be a NetworkBehaviour?
// But there will be a Fishnet implementation anyways, so the refactoring
// has to wait.
//-------------------------------------------------------------

public class TabletFunctionalityController : NetworkBehaviour
{
    // An enum used to identify a view
    public enum ViewId
    {
        None = 0,
        Map = 1,
        Inventory = 2,
        NewsFeed = 3,
        Calendar = 4,
        QuestLog = 5

    }

    // We use this enum to keep track of which view is currently open.
    public ViewId CurrentView;



    // TO BE REFACTORED: -----------------------
    // Move to NewsFeedWindowChanger or elsewhere? Now it is in the wrong place

    // A reference to the prefab used to display local news.
    // Set in the inspector.
    public GameObject NewsFeedButtonOriginalLocal;

    // A reference to the prefab used to display global news.
    // Set in the inspector.
    public GameObject NewsFeedButtonOriginalGlobal;


    // THIS IS TOTALLY IN THE WRONG PLACE!!! Like the rest of the functionality
    // concerning the news feed. Probably the work was started in this class
    // to contain functionality for all of the views
    // but later broken down to smaller and more specific classes. Or I don't know
    // what was I thinking. That all the tablet functionality could be contained
    // in one class? Not going to happen, or it would be a long and confusing mess
    // of a class.

    // The content object of the scrollview 
    // Under PlayerObjects/ViewWithinAViewStuff/NewsFeedCameraPlaceholder/Scroll View

    public GameObject NewsFeedButtonParent;
    //------------------------------------------

    // TO BE REFACTORED? Combine functionality of this object to this class?----------

    // The controller component that activates proper objects
    // and sets materials and view within a view cameras
    // to appropriate one depending on the view to be shown
    public ViewWithinAViewController ViewWithAViewController;
    //-------------------------------------------------------------------------------

    // An instance of a controller class that shows either a list of news or an individual news item
    // Activates proper objects to display either one of the aforementioned.
    // Set in the inspector. Found in PlayerObjects/ViewWithinAViewStuff/NewsFeedCameraPlaceholder
    public NewsFeedWindowChanger NewsFeedWindowChanger;

    // An instance of a controller class that displays things about the calendar, months or days.
    // Set in the inspector. Found in PlayerObjects/ViewWithinAViewStuff/CalendarCameraPlaceholder
    public CalendarWindowChanger CalendarWindowChanger;

    // An instance of a controller class that displays quest log
    // Set in the inspector. Found in PlayerObjects/ViewWithinAViewStuff/QuestLogCameraPlaceholder
    public QuestLogViewChanger QuestLogViewChanger;

    // TO BE REFACTORED: away completely with this-----------------------------------------------
    // probably due to be rewritten input system

    // A static instance of this class to be accessed from anywhere
    // Basically this is only used to track the bool of if the tablet view is open
    // and based on that to prevent unwanted inputs.
    public static TabletFunctionalityController Instance;
    //------------------------------------------------------------------------------------------
    
    // A bool to keep track of the fact of if the tablet view is open currently.
    public bool IsTabletViewOpen;

    private void Awake()
    {
        // We will start by activating a none view. 
        // In the called method ViewWithinAViewController
        // Disables all cameras if any of them was left open
        // during development
        ActivateProperView(ViewId.None);

        // set the current view to map, so the first
        // time we open the tablet we start from the map view.
        CurrentView = ViewId.Map;
    }



     // The player is spawned and this object with it.
    // There will be multiple instances on the players,
    // so we set the static instance to be that of the owner player's
    public override void OnStartClient()
    {
        base.OnStartClient();

        if (IsOwner)
        {
            Instance = this;

            Debug.Log("Set instance to tablet view controller");
        }
    }

    // TO BE REFACTORED? ------------------------------------------------
    // Perhaps this class should listen to the tablet input and not
    // the TabletCameraViewContoller?

    // We are told that the tablet is opened.
    public void OnTabletOpened()
    {
        // Set the bool about tablet view being open to true.
        IsTabletViewOpen = true;

        // Activate the view to be shown.
        // The first time this is called, the current view will be Map.
        // The subsequent times it will be the last one that was open.
        ActivateProperView(CurrentView);
    }
    // ------------------------------------------------------------------


    // A tablet navigation button tells us that a navigation button was pressed.
    public void OnNavigationButtonPressed(TabletNavigationButton.NavigationButtonID buttonID)
    {
        // If the navigation button id is of the left...
        if (buttonID == TabletNavigationButton.NavigationButtonID.Left)
        {
            // We scroll through the list of views backwards
            StartShowingViewDown();
        }

        // ...else if it of the right...
        else if (buttonID == TabletNavigationButton.NavigationButtonID.Right)
        {
            // We scroll through the list of views forwards
            StartShowingViewUp();
        }
    }

    // Determines the next view and makes a call to activate that
    private void StartShowingViewDown()
    {
        ViewId newView = ViewId.None;


        // Determine the next view to be shown based on the current view.
        switch (CurrentView)
        {
            case ViewId.None:
                break;
            case ViewId.Map:
                newView = ViewId.QuestLog;
                break;
            case ViewId.Inventory:
                newView = ViewId.Map;
                break;
            case ViewId.NewsFeed:
                newView = ViewId.Inventory;
                break;

            case ViewId.Calendar:
                newView = ViewId.NewsFeed;
                break;

            case ViewId.QuestLog:
                newView = ViewId.Calendar;
                break;

            default:
                break;
        }

        if (newView == ViewId.None)
        {
            // We failed to setup a proper view, so we log an error
            Debug.LogError("Next view is none. Errol, Errol, Errol");
        }

        else
        {
            // We successfully determined the next view, so we set the
            // current view to be the new view...
            CurrentView = newView;

            // ...and make a call to activate the view.
            ActivateProperView(CurrentView);
        }
    }

    // Determines the next view and makes a call to activate that
    private void StartShowingViewUp()
    {
        ViewId newView = ViewId.None;

        //Debug.Log("Current view is " + CurrentView.ToString());

        // Determine the next view to be shown based on the current view.
        switch (CurrentView)
        {
            case ViewId.None:
                break;
            case ViewId.Map:
                newView = ViewId.Inventory;
                break;
            case ViewId.Inventory:
                newView = ViewId.NewsFeed;
                break;
            case ViewId.NewsFeed:
                newView = ViewId.Calendar;
                break;
            case ViewId.Calendar:
                newView = ViewId.QuestLog;
                break;

            case ViewId.QuestLog:
                newView = ViewId.Map;
                break;
            default:
                break;
        }

        if (newView == ViewId.None)
        {
            // We failed to setup a proper view, so we log an error
            Debug.LogError("Next view is none. Errol, Errol, Errol");
        }

        else
        {
            // We successfully determined the next view, so we set the
            // current view to be the new view...
            CurrentView = newView;

            // ...and make a call to activate the view.
            ActivateProperView(CurrentView);
        }
    }

    // Does view specific operations and tells
    // the ViewWithinAViewController to change the view
    public void ActivateProperView(ViewId viewId)
    {
        // If the view is not the calendar, close it.
        if (viewId != ViewId.Calendar)
        {
            CalendarWindowChanger.OnViewClosed();
        }

        // If the view is newsfeed...
        if (viewId == ViewId.NewsFeed)
        {        
            //.. Unsubscribe (just in case) and subscribe to the event
            // on NewsFeedController.Instance that tells us when the
            // news are updated.
            NewsFeedController.Instance.OnNewsUpdated -= OnNewsUpdated;
            NewsFeedController.Instance.OnNewsUpdated += OnNewsUpdated;

            // Also make sure the news feed shows proper things on it.
            InitializeNewsFeed();
        }

        else if (viewId == ViewId.Calendar)
        {
            // If the view to be shown is calendar, make the calendar
            // show a month
            CalendarWindowChanger.ShowCurrentMonth();
        }

        else if (viewId == ViewId.QuestLog)
        {
            QuestLogViewChanger.OnViewActivated();
        }

        // Tell the view within a view controller to activate/deactivate
        // needed objects to show the current view
        ViewWithAViewController.OnViewChanged(viewId);
    }

    // Called when the animation transitions have finished
    // and the tablet shouln't be visible and functional anymore in anyway.
    public void OnTabletClosed()
    {

        //Debug.LogWarning("Enabled player inputs again, since the tablet is closed");
        //PlayerInput.enabled = true;

        // TO BE REFACTORED WITH THE REST OF NEWSFEED STUFF--------------------------------
        // Unsubscribe to the event of news being updated
        NewsFeedController.Instance.OnNewsUpdated -= OnNewsUpdated;
        //---------------------------------------------------------------------------------

        // Make sure calendar knows the view is closed and acts according to that.
        if (CurrentView == ViewId.Calendar) 
        {
            CalendarWindowChanger.OnViewClosed();
        }


        // Activate view as none, so any camera will be disabled.
        ActivateProperView(ViewId.None);

        // The tablet is not open anymore, so we can set the bool that tracks it to false.
        IsTabletViewOpen = false;
    }


    // TO BE REFACTORED:-------------------------------------------
    // SHOULD PROBABLY BE MOVED TO NEWSFEED WINDOW CHANGER

    // Make the news feed view show the news buttons
    private void InitializeNewsFeed()
    {
        //Debug.Log("Initialize news feed");

        // Tell the news feed window changer to activate/deactivate proper objects
        // to show the news feed
        NewsFeedWindowChanger.ShowNewsList();

        // TO BE REFACTORED? WHAT IS HAPPENING HERE?--------------------------------------------
        // why fetch the buttons first. There must be a reason for that.
        
        // Fetch children button components from the Scroll View parent
        Button[] childButtons = NewsFeedButtonParent.GetComponentsInChildren<Button>(true);

        // Iterate through the found children buttons
        for (int i = 0; i < childButtons.Length; i++)
        {
            NewsFeedButton newsFeedButton = childButtons[i].GetComponent<NewsFeedButton>();

            // If the button has a NewsFeedButton component...
            if (newsFeedButton != null)
            {
                // ...unsubscribe from the event that the news item is clicked
                // because the button is aboout to be destroyed
                newsFeedButton.OnNewsItemClicked -= OnNewsItemClicked;
            }

            // Destroy the child button
            DestroyImmediate(childButtons[i].gameObject);
        }
        //------------------------------------------------------------------------------------

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
    //-------------------------------------------------------------


    // TO BE REFACTORED:-------------------------------------------
    // SHOULD PROBABLY BE MOVED TO NEWSFEED WINDOW CHANGER
    // Also: the instantiation into the list seems a bit confusing
    // All in all, very weirdly implemented.
    // Seems like Refactor-->Ectract method was used

    // This method instantiates a clickable newsfeed button as a child of
    // the Scroll View content that is the NewsFeedButtonParent -object.
    private void InstantiateButton(List<NewsFeedItem> newsFeedItemList, 
                                   int i,
                                   GameObject buttonOriginal)
    {
        // Instantiates a new button according to what was passed into the method into the scroll view parent,
        // then it fetches the button component of the object
        Button button = Instantiate(buttonOriginal, NewsFeedButtonParent.transform).GetComponent<Button>();

        // Make sure the button is actually active.
        button.gameObject.SetActive(true);

        // Set the text component of the button to display the header of the news found in the list at i
        button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = newsFeedItemList[i].Header;

        // Get the NewsFeedButton component so we can update that.
        NewsFeedButton newsFeedButton = button.GetComponent<NewsFeedButton>();

        // Set the news id of the button to that found from the list at i
        newsFeedButton.NewsId = newsFeedItemList[i].ID;

        // Set the news feed item of the button to be that of the one found from the list at i
        newsFeedButton.NewsFeedItem = newsFeedItemList[i];

        // Just in case unsubscribe and then subscribe to the event of news item clicked.
        newsFeedButton.OnNewsItemClicked -= OnNewsItemClicked;
        newsFeedButton.OnNewsItemClicked += OnNewsItemClicked;
    }
    //-------------------------------------------------------------

    // TO BE REFACTORED:-------------------------------------------
    // SHOULD PROBABLY BE MOVED TO NEWSFEED WINDOW CHANGER

    // Called when a NewsFeedButton fires an event that informs us that
    // the news item was clicked and we get data about the item id and
    // the actual NewsFeedItem that informs us about the header, content,
    // priority and id of the item in question
    public void OnNewsItemClicked(int itemID,
                                  NewsFeedItem item)
    {
        // Tell the NewsFeedWindowChanger to display the item.
        NewsFeedWindowChanger.ShowIndividualNews(item);
    }
    //-------------------------------------------------------------

    // TO BE REFACTORED:-------------------------------------------
    // SHOULD PROBABLY BE MOVED TO NEWSFEED WINDOW CHANGER

    // Called when NewsFeedController fires an event about the news being
    // updated
    public void OnNewsUpdated()
    {
        // If NewsFeedWindowChanger is not showing an individual news item...
        if (!NewsFeedWindowChanger.IsShowingANewsItem) 
        {
            // ... we update the view
            InitializeNewsFeed();
        }
        //Debug.Log("News updated while the view is open " + Time.time);
    }
    //-------------------------------------------------------------
}
