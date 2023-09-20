using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CalendarEntry : MonoBehaviour
{
    public TextMeshProUGUI Number;
    public TextMeshProUGUI Entry;
    public RemoveCalendarItemButton Button;

    private DayViewCalendar dayView;

    public void FillEntry(DayViewCalendar dayViewCalendar,
                          int number,
                          string text)
    {
        dayView = dayViewCalendar;
        Number.text = number.ToString();
        Entry.text = text;
    }

    public void OnTrashcanIconPressed()
    {
        dayView.OnDestroyItemPressed(this);
    }
}
