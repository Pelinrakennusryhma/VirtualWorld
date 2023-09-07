using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class TabletFunctionalityController : NetworkBehaviour
{
    public enum ViewId
    {
        None = 0,
        Map = 1,
        Inventory = 2,
        NewsFeed = 3

    }

    public ViewId CurrentView;

    public Camera MapCamera;
    public Camera InventoryCamera;
    public Camera NewsFeedCamera;

    public MeshRenderer ScreenMeshRenderer;


    public Material MapMaterial;
    public Material InventoryMaterial;
    public Material NewsFeedMaterial;

    public ViewWithinAViewRaycaster Raycaster;

    public GameObject NewsFeedButtonOriginalLocal;
    public GameObject NewsFeedButtonOriginalGlobal;
    public GameObject NewsFeedButtonParent;

    public NewsFeedWindowChanger NewsFeedWindowChanger;

    private void Awake()
    {
        ActivateProperView(ViewId.None);
        CurrentView = ViewId.Map;
    }

    public void OnTabletOpened()
    {
        ActivateProperView(CurrentView);
        Raycaster.ActivateRaycaster();
        Debug.Log("Tablet was opened. React to that. " + Time.time);
    }



    public void OnNavigationButtonPressed(TabletNavigationButton.NavigationButtonID buttonID)
    {
        Debug.Log("Tablet functionality knows we pressed navigation button " + buttonID + " at time " + Time.time);

        if (buttonID == TabletNavigationButton.NavigationButtonID.Left)
        {
            StartShowingViewDown();
        }

        else if (buttonID == TabletNavigationButton.NavigationButtonID.Right)
        {
            StartShowingViewUp();
        }
    }

    private void StartShowingViewDown()
    {
        ViewId newView = ViewId.None;

        switch (CurrentView)
        {
            case ViewId.None:
                break;
            case ViewId.Map:
                newView = ViewId.NewsFeed;
                break;
            case ViewId.Inventory:
                newView = ViewId.Map;
                break;
            case ViewId.NewsFeed:
                newView = ViewId.Inventory;
                break;
            default:
                break;
        }

        if (newView == ViewId.None)
        {
            Debug.LogError("Next view is none. Errol, Errol, Errol");
        }

        else
        {
            CurrentView = newView;
            ActivateProperView(CurrentView);

            Debug.Log("Next view is " + newView);
        }
    }

    private void StartShowingViewUp()
    {
        ViewId newView = ViewId.None;

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
                newView = ViewId.Map;
                break;
            default:
                break;
        }

        if (newView == ViewId.None)
        {
            Debug.LogError("Next view is none. Errol, Errol, Errol");
        }

        else
        {
            CurrentView = newView;
            ActivateProperView(CurrentView);

            Debug.Log("Next view is " + newView);
        }
    }

    public void ActivateProperView(ViewId viewId)
    {
        NewsFeedController.Instance.OnNewsUpdated -= OnNewsUpdated;

        Raycaster.InactivateRaycaster();


        MapCamera.gameObject.SetActive(false);
        InventoryCamera.gameObject.SetActive(false);
        NewsFeedCamera.gameObject.SetActive(false);

        switch (viewId)
        {
            case ViewId.None:
                break;

            case ViewId.Map:
                MapCamera.gameObject.SetActive(true);
                ScreenMeshRenderer.material = MapMaterial;

                break;

            case ViewId.Inventory:
                InventoryCamera.gameObject.SetActive(true);
                ScreenMeshRenderer.material = InventoryMaterial;
                break;

            case ViewId.NewsFeed:
                NewsFeedCamera.gameObject.SetActive(true);
                ScreenMeshRenderer.material = NewsFeedMaterial;
                NewsFeedController.Instance.OnNewsUpdated += OnNewsUpdated;

                InitializeNewsFeed();
                Raycaster.ActivateRaycaster();
                break;
            default:
                break;
        }
    }

    public void OnTabletClosed()
    {
        NewsFeedController.Instance.OnNewsUpdated -= OnNewsUpdated;

        Raycaster.InactivateRaycaster();
        ActivateProperView(ViewId.None);
    }


    // WHAT IF NEEDS IS UPDATED AND IDs CHANGED AFTER INITIALIZING AND DURING VIEWING THE NEWSFEED???
    private void InitializeNewsFeed()
    {
        NewsFeedWindowChanger.ShowNewsList();

        Button[] childButtons = NewsFeedButtonParent.GetComponentsInChildren<Button>(true);

        for (int i = 0; i < childButtons.Length; i++)
        {
            NewsFeedButton newsFeedButton = childButtons[i].GetComponent<NewsFeedButton>();

            if (newsFeedButton != null)
            {
                newsFeedButton.OnNewsItemClicked -= OnNewsItemClicked;
            }

            DestroyImmediate(childButtons[i].gameObject);
        }

        List<NewsFeedItem> globalNews = NewsFeedController.Instance.GetGlobalNews();
        List<NewsFeedItem> localNews = NewsFeedController.Instance.GetLocalNews();

        for (int i = 0; i < globalNews.Count; i++)
        {
            InstantiateButton(globalNews, i, NewsFeedButtonOriginalGlobal);
        }

        for (int i = 0; i < localNews.Count; i++)
        {
            InstantiateButton(localNews, i, NewsFeedButtonOriginalLocal);
        }


    }

    private void InstantiateButton(List<NewsFeedItem> array, 
                                   int i,
                                   GameObject buttonOriginal)
    {
        Button button = Instantiate(buttonOriginal, NewsFeedButtonParent.transform).GetComponent<Button>();
        button.gameObject.SetActive(true);
        button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = array[i].Header;
        NewsFeedButton newsFeedButton = button.GetComponent<NewsFeedButton>();
        newsFeedButton.NewsId = array[i].ID;
        newsFeedButton.NewsFeedItem = array[i];
        newsFeedButton.OnNewsItemClicked -= OnNewsItemClicked;
        newsFeedButton.OnNewsItemClicked += OnNewsItemClicked;
        //Debug.LogError("Instantiating news item " + i);
    }

    public void OnNewsItemClicked(int itemID,
                                  NewsFeedItem item)
    {
        //NewsFeedItem newsFeedItem = NewsFeedController.Instance.GetLocalNewsItemByID(itemID);
        //NewsFeedWindowChanger.ShowIndividualNews(newsFeedItem);

        NewsFeedWindowChanger.ShowIndividualNews(item);

         Debug.Log("On news item clicked called");
    }

    public void OnNewsUpdated()
    {
        if (!NewsFeedWindowChanger.IsShowingANewsItem) 
        {
            InitializeNewsFeed();
        }
        Debug.Log("News updated while the view is open " + Time.time);
    }
}
