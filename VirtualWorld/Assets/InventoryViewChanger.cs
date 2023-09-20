using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryViewChanger : MonoBehaviour
{

    public bool WentOverLeftShoulder;

    public GameObject InventoryObjectsHolder;

    public Canvas InventoryCanvas;
    public CanvasScaler InventoryCanvasScaler;
    public Camera FlyCamera;
    public Camera InventoryCamera;

    public void SetWentOverLeftShoulder(bool wentOverLeftShoulder)
    {
        WentOverLeftShoulder = wentOverLeftShoulder;
    }

    public void CameraStartedTransitioning()
    {
        InventoryCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        InventoryCanvas.worldCamera = InventoryCamera;

        InventoryObjectsHolder.transform.localPosition = new Vector3(15.59f, 2.24f, 0);
        InventoryObjectsHolder.transform.localScale = new Vector3(1.3f, 1.3f, 1);
        //Debug.LogWarning("Should set inventory to view within a view.");
    }

    public void CameraReachedTargetPosition()
    {
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

        InventoryCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        InventoryCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        InventoryCanvasScaler.referenceResolution = new Vector2(1920, 1080);
        InventoryCanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        InventoryCanvasScaler.matchWidthOrHeight = 1.0f;
        InventoryCanvasScaler.referencePixelsPerUnit = 100;
        //Debug.LogWarning("Should set inventory to overlay canvas");
    }
}
