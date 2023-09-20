using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ViewWithinAViewRaycaster : MonoBehaviour
{
    public Camera FlyCamera;

    public Camera NewsFeedCamera;

    public GameObject TabletScreenGameObject;
    public Collider TabletScreenCollider;

    public RaycastHit[] RaycastHits = new RaycastHit[32];

    //public LayerMask ScreenWithinAScreenLayerMask;


    private void Awake()
    {
        InactivateRaycaster();
    }

    public void ActivateRaycaster()
    {
        gameObject.SetActive(true);
    }

    public void InactivateRaycaster()
    {
        gameObject.SetActive(false);
    }

    // Got some help from here:
    // https://discussions.unity.com/t/is-there-a-way-to-click-on-a-render-texture-to-select-soemthing-within-the-view/145082/2
    // Got some more help from here:
    // https://forum.unity.com/threads/interaction-with-objects-displayed-on-render-texture.517175/
    public void Update()
    {
        Ray ray = FlyCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        ClearRaycastArray();

        Physics.RaycastNonAlloc(ray, RaycastHits);

        Vector2 textureCoordinateHit = Vector2.zero;
        bool hitsScreen = false;

        for (int i = 0; i < RaycastHits.Length; i++)
        {
            if (RaycastHits[i].collider == TabletScreenCollider)
            {
                //Debug.Log("We are hitting the tablet screen with a raycast " + Time.time);
                textureCoordinateHit = RaycastHits[i].textureCoord;
                hitsScreen = true;

                //Debug.Log("Texture coordinate hit is x " + textureCoordinateHit.x + " y " + textureCoordinateHit.y + " at time " + Time.time);

                break;
            }
        }

        if (hitsScreen) 
        {
            ClearRaycastArray();

            Ray withinAViewRay = NewsFeedCamera.ScreenPointToRay(new Vector2(textureCoordinateHit.x * NewsFeedCamera.pixelWidth,
                                                                        textureCoordinateHit.y * NewsFeedCamera.pixelHeight));

            // For some reason the layermask approach does not work. No hits are deteced on UI layer.

            //Physics.RaycastNonAlloc(withinAViewRay, RaycastHits, ScreenWithinAScreenLayerMask);
            Physics.RaycastNonAlloc(withinAViewRay, RaycastHits);

            // 3D object hits

            for (int i = 0; i < RaycastHits.Length; i++)
            {
                if (RaycastHits[i].collider != null) 
                {
                    //Debug.Log("Hits " + RaycastHits[i].collider.gameObject.name);

                    TestClickDetector clickDetector = RaycastHits[i].collider.gameObject.GetComponentInChildren<TestClickDetector>(true);

                    if (clickDetector != null)
                    {
                        clickDetector.OnHoverAbove();
                    }
                }
            }

            // And UI elements

            //unityeven

            //ClearRaycastArray();
            //GraphicRaycaster.Raycast();

        }
    }

    private void ClearRaycastArray()
    {
        // Got to clear the array, so we don't get old hits.
        for (int i = 0; i < RaycastHits.Length; i++)
        {
            RaycastHits[i] = new RaycastHit();
        }
    }
}
