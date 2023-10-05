using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
using TMPro;
using UnityEngine.UI;

public class CalendarDayButton : MonoBehaviour
{
    // TO BE REMOVED, BECAUSE NOT USED?
    public int Week; // From 1 to 6. Set in the inspector.

    // TO BE REMOVED, BECAUSE NOT USED?
    public int Day; // 1 is monday, 2 is tuesday, 7 is sunday. Set in the inspector.

    // A reference to the window changer controller 
    private CalendarWindowChanger changer;

    // Which date this button is supposed to represent?
    private DateTime dateTime;

    // The number text of the date in the displayed month.
    // Set in the inspector.
    [SerializeField] private TextMeshProUGUI dayNumberUGUI;

    // An exclamation mark text object to show if we have
    // a calendar entry on this date.
    // Set in the inspector.
    [SerializeField] private TextMeshProUGUI exclamationMarkUGUI;

    // The image component of the button. Used to change the background
    // color of the button.
    private Image image;

    // A bool used to keep track of the fact that the button is interactable, or not.
    private bool isInteractable;

    // Initialize the button to make it ready to be used.
    public void Init(CalendarWindowChanger calendarWindowChanger)
    {
        // Set a reference to the calendar window changer controller
        // so we can inform it when the date button has been clicked.
        changer = calendarWindowChanger;

        // Get a reference to the image component, so we can modify it.
        image = GetComponent<Image>();
    }

    // Called when we are about to show the day in question
    public void DisplayADateTime(DateTime date,
                                 bool makeInteractable,
                                 bool setExclamationMarkActive)
    {
        // Save the date.
        dateTime = date;

        // Set the day number text to show the day number
        dayNumberUGUI.text = date.Day.ToString();

        // TO BE REFACTORED? A weird if-clause ------------------------

        // If we don't want to make the button grayscaled
        // and want to make it interactable
        if (makeInteractable)
        {
            // Set the bacground image color to white...
            image.color = Color.white;

            // ...and we want to make it interactable
            // because the button is of the current displayed month
            isInteractable = true;
        }

        else
        {
            // Set the background image to gray...
            image.color = Color.gray;

            // ...and make the button not interactable, since
            // it doesn't belong to the month that is displayed.
            // Extra days are added to the start and/or end of
            // the month to fill the displayed weeks.

            isInteractable = false;
        }        
        
        // In the case the date happens to be today...
        if (dateTime.Day == DateTime.Now.Day
            && dateTime.Month == DateTime.Now.Month
            && dateTime.Year == DateTime.Now.Year)
        {
            // ... we set the background image to green.
            image.color = new Color(0, 0.75f, 0, 1);
        }
        //-------------------------------------------------------------------

        // We set the exclamation mark object active,
        // if the day happens to have a diary entry on it
        exclamationMarkUGUI.gameObject.SetActive(setExclamationMarkActive);
    }

    // Called as an unity event, set in the inspector
    // When the button is clicked.
    public void OnClick()
    {
        // If the button is not grayscaled and it is set to be interactive
        if (isInteractable) 
        {
            // Inform the calendar window changer controller object that
            // a day button has been clicked
            changer.OnCalendarButtonPressed(dateTime);
        }
    }
}
