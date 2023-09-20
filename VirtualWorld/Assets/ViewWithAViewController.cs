using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ViewId = TabletFunctionalityController.ViewId;

public class ViewWithAViewController : MonoBehaviour
{
    public Camera FlyCamera;
    public Camera MapCamera;
    public Camera InventoryCamera;
    public Camera NewsFeedCamera;
    public Camera CalendarCamera;

    public GraphicRaycaster InventoryRaycaster;
    public GraphicRaycaster NewsFeedRaycaster;
    public GraphicRaycaster CalendarRaycaster;

    public ViewWithinAViewUIRaycaster ViewWithinAViewUIRaycaster;
    public MeshRenderer ScreenMeshRenderer;

    public Material MapMaterial;
    public Material InventoryMaterial;
    public Material NewsFeedMaterial;
    public Material CalendarMaterial;

    public void OnViewChanged(ViewId viewId)
    {
        MapCamera.gameObject.SetActive(false);
        InventoryCamera.gameObject.SetActive(false);
        NewsFeedCamera.gameObject.SetActive(false);
        CalendarCamera.gameObject.SetActive(false);

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

                //ViewWithinAViewUIRaycaster.SetRaycaster(InventoryRaycaster);
                ViewWithinAViewUIRaycaster.DisableRaycaster();
                ScreenMeshRenderer.material = InventoryMaterial;
                break;

            case ViewId.NewsFeed:
                NewsFeedCamera.gameObject.SetActive(true);
                ViewWithinAViewUIRaycaster.SetRaycaster(NewsFeedRaycaster);
                ScreenMeshRenderer.material = NewsFeedMaterial;
                break;

            case ViewId.Calendar:
                CalendarCamera.gameObject.SetActive(true);
                ViewWithinAViewUIRaycaster.SetRaycaster(CalendarRaycaster);
                ScreenMeshRenderer.material = CalendarMaterial;

                //Debug.Log("Material set to calendar material");
                break;

            default:
                break;
        }
    }
}
