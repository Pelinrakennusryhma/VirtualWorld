using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// This component is used to display
// calendar entries in a scrollview
// A prefab with this component is 
// instantiated from the DayViewCalendar
// -component as many times as needed
// to display proper amount of 
// calendar entries
public class CalendarEntry : MonoBehaviour
{
    // We have numbers for entries
    // This text -component is used
    // used to display that
    public TextMeshProUGUI Number;

    // The actual text of the calendar entry
    public TextMeshProUGUI Entry;

    // We have a button for the purposes
    // of removing a calendar entry
    public RemoveCalendarItemButton RemoveButton;

    // A reference to the Day view that controls
    // what is shown for a day in calendar
    private DayViewCalendar dayView;


    public void FillEntry(DayViewCalendar dayViewCalendar,
                          int number,
                          string text)
    {
        // Get a reference to the day view, so we
        // can inform it, when trashcan button is pressed
        dayView = dayViewCalendar;
        
        // Set the number of an entry
        Number.text = number.ToString();

        // Set the actual text of the entry
        Entry.text = text;
    }

    // Called as an Unity event set in the inspector
    public void OnTrashcanIconPressed()
    {
        // Inform day view that we should remove the item.
        // Day view does the actual destroying of the object
        // and doing necessary things to display the updated
        // view without this destroyed entry.
        dayView.OnDestroyItemPressed(this);
    }
}
