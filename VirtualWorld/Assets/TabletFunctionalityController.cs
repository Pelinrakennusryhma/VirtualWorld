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

    public GameObject NewsFeedButtonOriginal;
    public GameObject NewsFeedButtonParent;

    private void Awake()
    {
        ActivateProperCamera(ViewId.None);
        CurrentView = ViewId.Map;
    }

    public void OnTabletOpened()
    {
        ActivateProperCamera(CurrentView);
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
            ActivateProperCamera(CurrentView);

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
            ActivateProperCamera(CurrentView);

            Debug.Log("Next view is " + newView);
        }
    }

    public void ActivateProperCamera(ViewId viewId)
    {
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
                InitializeNewsFeed();
                Raycaster.ActivateRaycaster();
                break;
            default:
                break;
        }
    }

    public void OnTabletClosed()
    {
        Raycaster.InactivateRaycaster();
        ActivateProperCamera(ViewId.None);
    }

    private void InitializeNewsFeed()
    {
        //TMPro.TextMeshProUGUI u = NewsFeedCamera.gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>(true);
        //u.text = NewsFeedController.Instance.GetLocalNews();

        Button[] childButtons = NewsFeedButtonParent.GetComponentsInChildren<Button>(true);

        for (int i = 0; i < childButtons.Length; i++)
        {
            DestroyImmediate(childButtons[i].gameObject);
        }

        int amountOfNews = 12;

        string[] news = NewsFeedController.Instance.GetLocalNews(amountOfNews);

        for (int i = 0; i < amountOfNews; i++)
        {
            Button button = Instantiate(NewsFeedButtonOriginal, NewsFeedButtonParent.transform).GetComponent<Button>();
            button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = news[i];
        }

        NewsFeedController.Instance.GetGlobalNewsServerRpc();
    }
}
