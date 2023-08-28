using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


// Got help from here:
// https://forum.unity.com/threads/interaction-with-objects-displayed-on-render-texture.517175/
public class ViewWithinAViewUIRaycaster : GraphicRaycaster
{
    public Camera FlyCamera;
    public Camera ViewWithinAViewCamera;
    public GraphicRaycaster Raycaster;

    // Called by Unity when a Raycaster should raycast because it extends BaseRaycaster.
    public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
    {

        //Debug.Log("Raycasting at view within a view object " + Time.time);

        if (eventCamera == null)
        {
            Debug.LogError("Event camera is null " + Time.time);
        }

        if (eventData == null)
        {
            Debug.LogError("Event data is null " + Time.time);
        }

        Ray ray = eventCamera.ScreenPointToRay(eventData.position); // Mouse

        if (FlyCamera == null)
        {
            Debug.LogError("We got a null flycamera");
        }

        //Ray ray = FlyCamera.ScreenPointToRay(eventData.position);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.transform == transform)
            {
                // Figure out where the pointer would be in the second camera based on texture position or RenderTexture.
                Vector3 virtualPos = new Vector3(hit.textureCoord.x, hit.textureCoord.y);
                virtualPos.x *= ViewWithinAViewCamera.targetTexture.width;
                virtualPos.y *= ViewWithinAViewCamera.targetTexture.height;

                eventData.position = virtualPos;

                

                Raycaster.Raycast(eventData, resultAppendList);

                for (int i = 0; i < resultAppendList.Count; i++)
                {
                    Debug.Log("Raycast hit " + resultAppendList[i].gameObject.name.ToString());

                    Button button = resultAppendList[i].gameObject.GetComponent<Button>();

                    if (button != null && Input.GetMouseButtonDown(0))
                    {
                        button.onClick.Invoke();
                        Debug.Log("Clicked button " + Time.time);
                        //Debug.Break();
                    }

                    Scrollbar scroll = resultAppendList[i].gameObject.GetComponent<Scrollbar>();

                    

                    if (scroll != null && Input.GetMouseButton(0))
                    {
                        float value = scroll.value + Input.GetAxis("Mouse Y") * 0.5f; // Replace with new input system
                        value = Mathf.Clamp(value, 0, 1.0f);
                        scroll.value = value;
                        Debug.Log("Should DRAG");
                    }
                }



            }
        }
    }
}
