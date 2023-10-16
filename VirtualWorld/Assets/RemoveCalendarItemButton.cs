using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveCalendarItemButton : MonoBehaviour
{
    public int ID;
    public DayViewCalendar DayViewCalendar;

    public void OnButtonPressed()
    {
        Debug.LogWarning("On calendar trashcan icon pressed. id is " + ID + " " + Time.time);

    }

    public void ShowButton()
    {
        gameObject.SetActive(true);
    }

    public void HideButton()
    {
        gameObject.SetActive(false);
    }
}
