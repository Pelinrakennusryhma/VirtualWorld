using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseCalendarButton : MonoBehaviour
{
    // A reference to the controller object that
    // changes the calendar view.
    // Set in the inspector.
    public CalendarViewChanger changer;

    // Set in the inspector unity eventthat listens
    // to a button press
    public void CloseCalendarItem()
    {
        // Inform the calendar window changer controller
        // that we should close the view in question.
        // That is, the displayed individual day.
        changer.OnIndividualCalendarDayCloseButtonPressed();
    }
}
