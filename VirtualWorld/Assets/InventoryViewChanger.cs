using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// This component is in charge of showing the inventory.
// It is shown as a render texture on the tablet screen
// when the transitions are coming in or out the tablet view.
// When we are actually on the tablet view, inventory is
// displayed as an overlay canvas.
public class InventoryViewChanger : MonoBehaviour
{

    // A reference to the inventory objects parent
    // This object is moved according to which
    // path the camera took to the closeup tablet view
    public GameObject InventoryObjectsHolder;

    // Reference to the invenotry canvas
    // Render mode of this canvas is changed accordingly.
    public Canvas InventoryCanvas;

    // The canvas scaler component of the InventoryCanvas
    public CanvasScaler InventoryCanvasScaler;

    // A reference to the flying camera that is activated
    // When we start moving towards tablet view
    // and deactivated when we reach the normal gameplay position
    // and the normal third person camera takes over again
    public Camera FlyCamera;

    // The inventory camera that is responsible
    // of rendering inventory to a render texture
    public Camera InventoryCamera;

    // We need to know which shoulder we went over
    // because for some unknown reason the canvas
    // objects are displaced slightly depending
    // on the path that camera took to the tablet view.
    // Couldn't figure out why, so this hackish solution
    // is used for now
    private bool WentOverLeftShoulder;

    // We need to know if we went over left or right shoulder
    // of the character to reach the closeup position
    // See the comment about WentOverLeftShoulder above
    public void SetWentOverLeftShoulder(bool wentOverLeftShoulder)
    {
        WentOverLeftShoulder = wentOverLeftShoulder;
    }

    // Camera has started transitioning
    public void CameraStartedTransitioning()
    {
        // We need to set canvas' render mode to screen space camera
        // that uses the invenotry camera
        InventoryCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        InventoryCanvas.worldCamera = InventoryCamera;

        // Change inventory objects parent position and location
        // to match the tablet screen dimensions nicely
        // Invenotry is visible as a render tecture, on the tablet screen on two cases:
        // 1. We are transitioning out from the closeup view and we left inventory screen
        // as the last open view on the tablet. 2.We are transitioning in to closeup
        // position and inventory was the last viewed screen when the tablet was open
        // last time.
        InventoryObjectsHolder.transform.localPosition = new Vector3(15.59f, 2.24f, 0);
        InventoryObjectsHolder.transform.localScale = new Vector3(1.3f, 1.3f, 1);
        //Debug.LogWarning("Should set inventory to view within a view.");
    }

    // We have reached closeup view of tablet, so we switch from
    // displaying the inventory as render texture to displaying it as
    // a screen space overlay.
    public void CameraReachedTargetPosition()
    {
        // Displace the inventory object holder parent, because
        // It just doesn't stay centered properly depending from the direction
        // the fly camera came in from. Don't know a reason for this
        // so this hackish solution is used for now.
        if (WentOverLeftShoulder)
        {
            InventoryObjectsHolder.transform.localPosition = new Vector3(0.97f, 3.2f, 0);
            InventoryObjectsHolder.transform.localScale = new Vector3(1, 1, 1);
        }

        else
        {
            InventoryObjectsHolder.transform.localPosition = new Vector3(22.26f, 5.23f, 0);
            InventoryObjectsHolder.transform.localScale = new Vector3(1, 1, 1);
        }

        // Set inventory canvas to render as screen space overlay
        InventoryCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // We need to scale the canvas with screen size
        InventoryCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

        // A 16:9 reference resolution
        InventoryCanvasScaler.referenceResolution = new Vector2(1920, 1080);

        // Matching height works best...
        InventoryCanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        // ... so we make the scaler to match the height
        InventoryCanvasScaler.matchWidthOrHeight = 1.0f;

        InventoryCanvasScaler.referencePixelsPerUnit = 100;
    }
}
