using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ViewId = TabletFunctionalityController.ViewId;



// TO BE REFACTORED:
// Mix and match all messed up functionality overlapping with
// TabletFunctionalityController??????????????????








// A component that is in control of things that should happen
// when a view within a view is activated or changed
// That is which cameras and things should be active
// when the tablet is open
public class ViewWithinAViewController : MonoBehaviour
{
    // The camera that shows an overhead view of the world
    public Camera MapCamera;
    
    // This camera is used for both actual rendering to render
    // texturing and as an overlay on the tablet
    // Invenotry canvas is the child of this camera 
    public Camera InventoryCamera; 

    // This camera is used to display the newsfeed
    // The actual newsfeed canvas is a child of this camera
    public Camera NewsFeedCamera;

    // This camera is used to display the calendar
    // Calendar canvas and objects are children of this camera
    public Camera CalendarCamera;

    // A reference to the raycaster on the inventory canvas
    // We need this to enable the interactivity render teture with ViewWithAViewUIRaycaster
    public GraphicRaycaster InventoryRaycaster;

    // A reference to the raycaster on the newsfeed canvas
    // We need this to enable the interactivity render teture with ViewWithAViewUIRaycaster
    public GraphicRaycaster NewsFeedRaycaster;

    // A reference to the raycaster on the calendar canvas
    // We need this to enable the interactivity render teture with ViewWithAViewUIRaycaster
    public GraphicRaycaster CalendarRaycaster;

    // This raycaster enables us to make a render texture interactive
    // Tablet screen meshrenderer has this component.
    public ViewWithinAViewUIRaycaster ViewWithinAViewUIRaycaster;

    // A reference to the tablet screen mesh renderer
    public MeshRenderer ScreenMeshRenderer;

    // A dedicated material for the map. Has render texture.
    public Material MapMaterial;

    // A dedicated material for the inventory. Has render texture.
    public Material InventoryMaterial;

    // A dedicated material for the news feed. Has render texture.
    public Material NewsFeedMaterial;

    // A dedicated material for the calendar. Has render texture.
    public Material CalendarMaterial;




    // Called when tablet navigation buttons are pressed
    // and the view should be changed
    public void OnViewChanged(ViewId viewId)
    {
        // Disable all cameras
        // We will set active the correct one later
        MapCamera.gameObject.SetActive(false);
        InventoryCamera.gameObject.SetActive(false);
        NewsFeedCamera.gameObject.SetActive(false);
        CalendarCamera.gameObject.SetActive(false);

        // Determine what should be done, based on the viewId
        switch (viewId)
        {
            case ViewId.None:
                // The next view is none, no need to do anything
                break;

            case ViewId.Map:

                // We need map camera, so we will set it active.
                MapCamera.gameObject.SetActive(true);

                // Screen meshrenderer material should be set to map material
                // because it has the render texture the map camera renders on
                ScreenMeshRenderer.material = MapMaterial;

                break;

            case ViewId.Inventory:

                // We need inventory camera, so we set it active
                InventoryCamera.gameObject.SetActive(true);

                // We won't use the view within a view raycaster now, because
                // it doesn't work totally like it should (see notes on that script)
                // and instead we render the invenotry as overlay canvas
                // so it's completely interactive
                ViewWithinAViewUIRaycaster.DisableRaycaster();

                // But we still have to render to texture, before
                // we switch to overlay canvas, so let's set the screen
                // material to be the appropriate one.
                ScreenMeshRenderer.material = InventoryMaterial;

                break;

            case ViewId.NewsFeed:

                // We need NewsFeedCamera, so we set it active
                NewsFeedCamera.gameObject.SetActive(true);

                // View within a view ui raycaster needs a reference
                // of raycaster on news feed canvas
                ViewWithinAViewUIRaycaster.SetRaycaster(NewsFeedRaycaster);

                // Screen meshrenderer material should be set to news feed material
                // because it has the render texture the news feed camera renders on
                ScreenMeshRenderer.material = NewsFeedMaterial;

                break;

            case ViewId.Calendar:

                // We need calendar camera, so we set it active
                CalendarCamera.gameObject.SetActive(true);

                // Again the view within a view ui raycaster needs a reference
                // on the calendar canvas. So we set it.
                ViewWithinAViewUIRaycaster.SetRaycaster(CalendarRaycaster);

                // Screen meshrenderer material should be set to calendar material
                // because it has the render texture the calendar camera renders on
                ScreenMeshRenderer.material = CalendarMaterial;


                break;

            default:
                Debug.LogError("Switch case fell through to default. This should never happen. Probably the switch has missing enums.");
                break;
        }
    }
}
