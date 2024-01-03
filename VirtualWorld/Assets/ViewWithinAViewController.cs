using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Characters;


// TO BE REFACTORED:
// Mix and match all messed up functionality overlapping with
// TabletFunctionalityController??????????????????




// A component that is in control of things that should happen
// when a view within a view is activated or changed
// That is which canvases and things should be active
// when the tablet is open
public class ViewWithinAViewController : MonoBehaviour
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
    private ViewId CurrentView;

    // The camera that shows an overhead view of the world
    [SerializeField] private Camera MapCamera;

    // This raycaster enables us to make a render texture interactive
    // Tablet screen meshrenderer has this component.
    [SerializeField] private ViewWithinAViewUIRaycaster ViewWithinAViewUIRaycaster;

    // A reference to the tablet screen mesh renderer
    [SerializeField] private MeshRenderer TabletScreenMeshRenderer;

    [SerializeField] private MapBlips MapBlips;
    [SerializeField] private Camera RenderToTextureCamera;
    [SerializeField] private RenderTexture OutputRenderTexture;
    [SerializeField] private Material RenderToTextureMaterial;
    [SerializeField] private Canvas InventoryCanvas;
    [SerializeField] private Canvas NewsFeedCanvas;
    [SerializeField] private Canvas CalendarCanvas;
    [SerializeField] private Canvas QuestLogCanvas;

    public bool IsTabletViewOpen;


    // An instance of a controller class that shows either a list of news or an individual news item
    // Activates proper objects to display either one of the aforementioned.
    private NewsFeedViewChanger NewsFeedViewChanger;

    // An instance of a controller class that displays things about the calendar, months or days.
    private CalendarViewChanger CalendarViewChanger;


    // An instance of a controller class that displays quest log
    private QuestLogViewChanger QuestLogViewChanger;


    // An instance of a controller class that displays inventory
    private InventoryViewChanger InventoryViewChanger;

    // A reference to the raycaster on the inventory canvas
    // We would need this to enable the interactivity of render texture with ViewWithAViewUIRaycaster
    // but actually we display the inventory as an overlay canvas, so maybe this reference could be
    // just removed
    private GraphicRaycaster InventoryRaycaster;

    // A reference to the raycaster on the newsfeed canvas
    // We need this to enable the interactivity of render texture with ViewWithAViewUIRaycaster
    private GraphicRaycaster NewsFeedRaycaster;

    // A reference to the raycaster on the calendar canvas
    // We need this to enable the interactivity of render texture with ViewWithAViewUIRaycaster
    private GraphicRaycaster CalendarRaycaster;

    // A reference to the raycaster on the quest log canvas
    // We need this to enable the interactivity of render texture with ViewWithAViewUIRaycaster
    private GraphicRaycaster QuestLogRaycaster;




    private void Awake()
    {
        //if (!HasBeenInitted) 
        //{
        //    Init();
        //}
    }

    public void Init()
    {
        MapCamera.targetTexture = OutputRenderTexture;

        bool wasActive = gameObject.activeSelf;

        gameObject.SetActive(true);

        InventoryRaycaster = InventoryCanvas.GetComponentInChildren<GraphicRaycaster>(true);
        NewsFeedRaycaster = NewsFeedCanvas.GetComponentInChildren<GraphicRaycaster>(true);
        CalendarRaycaster = CalendarCanvas.GetComponentInChildren<GraphicRaycaster>(true);
        QuestLogRaycaster = QuestLogCanvas.GetComponentInChildren<GraphicRaycaster>(true);

        NewsFeedViewChanger = NewsFeedCanvas.GetComponentInChildren<NewsFeedViewChanger>(true);
        CalendarViewChanger = CalendarCanvas.GetComponentInChildren<CalendarViewChanger>(true);
        QuestLogViewChanger = QuestLogCanvas.GetComponentInChildren<QuestLogViewChanger>(true);
        InventoryViewChanger = InventoryCanvas.GetComponentInChildren<InventoryViewChanger>(true);

        gameObject.SetActive(wasActive);

        InventoryRaycaster.enabled = false;
        NewsFeedRaycaster.enabled = false;
        CalendarRaycaster.enabled = false;
        QuestLogRaycaster.enabled = false;

        InventoryCanvas.gameObject.SetActive(false);
        NewsFeedCanvas.gameObject.SetActive(false);
        CalendarCanvas.gameObject.SetActive(false);
        QuestLogCanvas.gameObject.SetActive(false);

        ViewWithinAViewUIRaycaster.SetViewWithinAViewCamera(RenderToTextureCamera);

        // We will start by activating a none view. 
        // In the called method ViewWithinAViewController
        // Disables all canvases if any of them was left open
        // during development
        ActivateProperView(ViewId.None);

        // set the current view to map, so the first
        // time we open the tablet we start from the map view.
        CurrentView = ViewId.Map;
    }


    // Called when tablet navigation buttons are pressed
    // and the view should be changed
    public void OnViewChanged(ViewId viewId)
    {
        //Debug.Log("View is changed. ViewId is " + viewId.ToString());

        CurrentView = viewId;

        // Disable map camera
        MapCamera.gameObject.SetActive(false);

        // Disable all canvases, we will activate the correct one later
        InventoryCanvas.gameObject.SetActive(false);
        NewsFeedCanvas.gameObject.SetActive(false);
        CalendarCanvas.gameObject.SetActive(false);
        QuestLogCanvas.gameObject.SetActive(false);


        RenderToTextureCamera.gameObject.SetActive(true);
        RenderToTextureCamera.enabled = true;
        TabletScreenMeshRenderer.material = RenderToTextureMaterial;

        if (InventoryRaycaster == null)
        {
            Debug.LogError("Inventory raycaster is null");
        }

        InventoryRaycaster.enabled = false;

        switch (viewId)
        {
            case ViewId.None:
                break;

            case ViewId.Map:
                MapCamera.gameObject.SetActive(true);
                ViewWithinAViewUIRaycaster.DisableRaycaster();
                break;

            case ViewId.Inventory:
                InventoryCanvas.gameObject.SetActive(true);
                InventoryCanvas.worldCamera = RenderToTextureCamera;
                ViewWithinAViewUIRaycaster.DisableRaycaster();
                InventoryRaycaster.enabled = true;
                //ViewWithinAViewUIRaycaster.SetRaycaster(InventoryRaycaster);
                break;

            case ViewId.NewsFeed:
                NewsFeedCanvas.gameObject.SetActive(true);
                NewsFeedCanvas.worldCamera = RenderToTextureCamera;
                ViewWithinAViewUIRaycaster.SetRaycaster(NewsFeedRaycaster);

                break;

            case ViewId.Calendar:
                CalendarCanvas.gameObject.SetActive(true);
                CalendarCanvas.worldCamera = RenderToTextureCamera;
                ViewWithinAViewUIRaycaster.SetRaycaster(CalendarRaycaster);

                break;

            case ViewId.QuestLog:
                QuestLogCanvas.gameObject.SetActive(true);
                QuestLogCanvas.worldCamera = RenderToTextureCamera;
                ViewWithinAViewUIRaycaster.SetRaycaster(QuestLogRaycaster);

                break;

            default:
                Debug.LogError("Switch case fell through to default. This should never happen. Probably the switch has missing enums.");
                break;
        }

        // If the view is not the calendar, close it.
        if (viewId != ViewId.Calendar)
        {
            CalendarViewChanger.OnViewClosed();
        }

        // If the view is newsfeed...
        if (viewId == ViewId.NewsFeed)
        {
            // Also make sure the news feed shows proper things on it.
            NewsFeedViewChanger.InitializeNewsFeed();
        }

        else if (viewId == ViewId.Calendar)
        {
            // If the view to be shown is calendar, make the calendar
            // show a month
            CalendarViewChanger.OnCalendarOpened();
        }

        else if (viewId == ViewId.QuestLog)
        {
            QuestLogViewChanger.OnViewActivated();
        }
    }

    #region Refactored TabletFunctionalityController away

    // Sets the map camera position and rotation
    public void SetupMapCamera(Vector3 originPos)
    {
        // Map camera should be from above the player facing down, so we make it so.
        MapCamera.transform.position = originPos + Vector3.up * 600;
        MapCamera.transform.rotation = Quaternion.Euler(90, 0, 0);

        //Debug.Log("Set map camera position and rotation");
    }

    public void SetupMapBlips(bool activateGreen,
                              bool activateYellow)
    {
        MapBlips.SetMapBlip(activateGreen, activateYellow);
        //Debug.Log("Setupping map blips. Green is active " + activateGreen + " yellow is active " + activateYellow);
    }

    public void OnCameraStartedTransitioning()
    {
        if (InventoryViewChanger == null)
        {
            Debug.LogError("Inventory view changer is null");
        }

        InventoryViewChanger.CameraStartedTransitioning();
    }

    public void OnCameraReachedTransitionPos()
    {
        InventoryViewChanger.CameraReachedTargetPosition();
        CharacterManager.Instance.SetGameState(GAME_STATE.TABLET);
    }

    // Does view specific operations and tells
    // the ViewWithinAViewController to change the view
    public void ActivateProperView(ViewId viewId)
    {
        // Activate/deactivate
        // needed objects to show the current view
        OnViewChanged(viewId);
    }

    // We are told that the tablet is opened.
    public void OnTabletOpened()
    {
        // Set the bool about tablet view being open to true.
        IsTabletViewOpen = true;

        if (CurrentView == ViewId.None)
        {
            CurrentView = ViewId.Map;
        }

        // Activate the view to be shown.
        // The first time this is called, the current view will be Map.
        // The subsequent times it will be the last one that was open.
        ActivateProperView(CurrentView);
    }
    // ------------------------------------------------------------------


    // Called when the animation transitions have finished
    // and the tablet shouln't be visible and functional anymore in anyway.
    public void OnTabletClosed()
    {
        // Make sure calendar knows the view is closed and acts according to that.
        if (CurrentView == ViewId.Calendar)
        {
            CalendarViewChanger.OnViewClosed();
        }

        // Unsubscribe from the event that happens in the method, whether or not we
        // have newsfeed as the active view
        NewsFeedViewChanger.OnCloseNewsFeed();


        // Activate view as none, so any camera will be disabled.
        ActivateProperView(ViewId.None);

        // The tablet is not open anymore, so we can set the bool that tracks it to false.
        IsTabletViewOpen = false;
    }

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

        //Debug.Log("Current view is " + CurrentView.ToString() + " new view is " + newView.ToString());


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

        //Debug.Log("Current view is " + CurrentView.ToString() + " new view is " + newView.ToString());


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


    #endregion

}
