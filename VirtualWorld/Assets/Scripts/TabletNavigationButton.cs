using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TabletNavigationButton : MonoBehaviour, IPointerClickHandler
{
    public enum NavigationButtonID
    {
        None = 0,
        Left = 1,
        Right = 2
    }

    public NavigationButtonID ButtonID;

    private Vector3 ParentOriginalScale;

    public ViewWithinAViewController ViewWithinAViewController;

    private void Awake()
    {
        ParentOriginalScale = transform.parent.localScale;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Debug.Log("We clicked a table navigation button " + ButtonID.ToString() + Time.time);

        transform.parent.localScale = new Vector3(ParentOriginalScale.x * 0.5f, ParentOriginalScale.y * 0.5f, ParentOriginalScale.z);

        ViewWithinAViewController.OnNavigationButtonPressed(ButtonID);
    }

    public void Update()
    {
        transform.parent.localScale = Vector3.Lerp(transform.parent.localScale, ParentOriginalScale, Time.deltaTime * 5.0f);
    }
}
