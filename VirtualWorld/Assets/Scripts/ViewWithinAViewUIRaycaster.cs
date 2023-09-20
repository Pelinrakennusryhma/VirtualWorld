using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


// Got help from here, but it just stopped working:
// https://forum.unity.com/threads/interaction-with-objects-displayed-on-render-texture.517175/
public class ViewWithinAViewUIRaycaster : GraphicRaycaster
{



    public Camera ViewWithinAViewCamera;
    public GraphicRaycaster Raycaster;

    public TMP_InputField lastInteractedInputField;

    public delegate void InputFieldSubmitted(string text);
    public event InputFieldSubmitted OnInputFieldSubmitted;

    public ItemScript LastInteractedItemScript;

    public void SetRaycaster(GraphicRaycaster caster)
    {
        this.enabled = true;
        Raycaster = caster;
    }

    public void DisableRaycaster()
    {
        this.enabled = false;
    }

    // Called by Unity when a Raycaster should raycast because it extends BaseRaycaster.
    public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
    {

        //Debug.Log("Raycasting at view within a view object " + Time.time);

        if (eventCamera == null)
        {
            Debug.LogError("Event camera is null " + Time.time);
        }

        Ray ray = eventCamera.ScreenPointToRay(eventData.position); 

        bool hittingTheScreen = false;

        RaycastHit hit = new RaycastHit();
        RaycastHit[] hits = Physics.RaycastAll(ray);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.transform == transform)
            {
                hittingTheScreen = true;
                hit = hits[i];
                break;
            }

            //Debug.Log("Hitting " + hits[i].collider.gameObject.name);
        }




        if(hittingTheScreen)
        {
            //Debug.Log("Hitting the screen. about to raycast");

            // Figure out where the pointer would be in the second camera based on texture position or RenderTexture.
            Vector3 virtualPos = new Vector3(hit.textureCoord.x, hit.textureCoord.y);
            virtualPos.x *= ViewWithinAViewCamera.targetTexture.width;
            virtualPos.y *= ViewWithinAViewCamera.targetTexture.height;

            eventData.position = virtualPos;


            Raycaster.Raycast(eventData, resultAppendList);

  
            InteractWithScreen(resultAppendList, eventData);
        }
    }

    // A laborous attepmt at making things work, because it just stopped working like it should
    
    private void InteractWithScreen(List<RaycastResult> resultAppendList,
                                    PointerEventData eventData)
    {

        for (int i = 0; i < resultAppendList.Count; i++)
        {
            //Debug.Log("Raycast hit " + resultAppendList[i].gameObject.name.ToString());

            Button button = resultAppendList[i].gameObject.GetComponent<Button>();


            //if (resultAppendList[i].gameObject.name.ToString().Equals("Text (TMP)"))
            //{
            //    TMPro.TextMeshProUGUI ugui = resultAppendList[i].gameObject.GetComponent<TMPro.TextMeshProUGUI>();
            //    Debug.Log("Text on the ugui is " + ugui.text);
            //    //Debug.Break();
            //}

            if (button != null && Input.GetMouseButtonDown(0))
            {
                button.onClick.Invoke();

                lastInteractedInputField = null;
                
                //Debug.Log("Clicked button " + Time.time);
                //Debug.Break();
            }

            Scrollbar scroll = resultAppendList[i].gameObject.GetComponent<Scrollbar>();

            ScrollRect scrollRect = resultAppendList[i].gameObject.GetComponent<ScrollRect>();

            if (scrollRect != null)
            {
                scroll = scrollRect.GetComponentInChildren<Scrollbar>();

                if (scroll != null)
                {
                    float value = scroll.value + Input.mouseScrollDelta.y * 0.5f; // Replace with new input system?
                    value = Mathf.Clamp(value, 0, 1.0f);
                    scroll.value = value;
                    //Debug.Log("Should DRAG");
                }

                else
                {
                    //if (Input.mouseScrollDelta.y >= 0.1f
                    //    || Input.mouseScrollDelta.y <= 0.1f)
                    //{
                    //    scrollRect.OnScroll(eventData);
                    //}
                }
            }

            else if (scroll != null && Input.GetMouseButton(0))
            {
                float value = scroll.value + Input.GetAxis("Mouse Y") * 0.5f; // Replace with new input system?
                value = Mathf.Clamp(value, 0, 1.0f);
                scroll.value = value;
                //Debug.Log("Should DRAG");
            }

            TMPro.TMP_InputField field = resultAppendList[i].gameObject.GetComponent<TMP_InputField>();

            if (field != null)
            {
                //Debug.LogWarning("We are hitting tmpro input field");
                
                field.OnPointerClick(eventData);

                lastInteractedInputField = field;


            }
        }

        if(lastInteractedInputField != null 
            && Input.GetKeyDown(KeyCode.Return))
        {
            lastInteractedInputField.OnSubmit(eventData);

            if (OnInputFieldSubmitted != null)
            {
                OnInputFieldSubmitted(lastInteractedInputField.text);
            }

            lastInteractedInputField.text = "";

            //Debug.Log("We pressed return");
        }
    }
}
