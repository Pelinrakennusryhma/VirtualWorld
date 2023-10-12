using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;



// This component takes care of displaying a day in
// the calendar. This includes spawning
// and filling the CalendarEntry objects, displaying
// them in a viewport and taking care of destroying
// them whenever needed.

public class DayViewCalendar : MonoBehaviour
{
    // The prefab of the calendar entry object that
    // gets instantiated and filled to an entry in 
    // the calendar.
    // Dropped in the inspector: "CalendarEntry" -prefab.
    [SerializeField] private CalendarEntry CalendarEntryObjectOriginal;

    // A reference to the content object of the viewport
    // where calendar entries are spawned.
    // Set in the inspector.
    [SerializeField] private GameObject ViewportContent;

    // This is the text component, where current date is displayed
    // Reference dropped in the inspector
    [SerializeField] private TextMeshProUGUI DateText;

    // Which DateTime is in the view
    private DateTime currentShownDateTime;

    // Text to inform that there are no diary entries yet.
    // Reference dropped in the inspector
    [SerializeField] private GameObject NoDiaryEntriesYet;

    // A parent object of an input field and associated text
    // Where a player can input the calendar entry
    // Reference dropped in the inspector
    [SerializeField] private GameObject InputFieldHolder;

    [SerializeField] private TMP_InputField InputField;

    // A list of calendar entries that are currently on display.
    private List<CalendarEntry> currentShownCalendarEntries = new List<CalendarEntry>();

    [SerializeField] private CalendarViewChanger CalendarViewChanger;


    public void DeactivateDayView()
    {
        gameObject.SetActive(false);
    }

    // This day is closed, so we react to that
    public void OnCloseIndividualDay()
    {
        SaveDateEntries();
        DeactivateDayView();
    }

    public void RefreshDayView(DateTime date)
    {
        ActivateDayView();
        UpdateDayViewDate(date);

        CalendarController.CalendarItem item;

        // We check CalendarController's singleton instance if we have some diary entries for this date
        bool hasEntry = CalendarController.Instance.CheckForACalendarItem(date, out item);

        // If we had an entry for the date, update DayViewCalendar view's texts...
        if (hasEntry)
        {
            UpdateAllTexts(item);
        }

        // ...otherwise just reset the view
        else
        {
            ResetView();
        }

    }

    // The player pressed the trashcan button and an entry
    // should be removed.
    public void OnDestroyItemPressed(CalendarEntry entry)
    {
        // Remove the entry from the list of currently shown calendar entries
        currentShownCalendarEntries.Remove(entry);

        // Destroy the gameObject, so the Viewport content restructures
        // The remaining entries.
        Destroy(entry.gameObject);

        // Make the remaining entries' numbers reflect their order
        // in the view
        for (int i = 0; i < currentShownCalendarEntries.Count; i++)
        {
            currentShownCalendarEntries[i].Number.text = (i + 1).ToString();

            //Debug.Log("Calendar entries at " + i + " is " + currentShownCalendarEntries[i].Entry.text);
        }

        // In the case we didn't have any more entries left
        // We inform the player with a text that there
        // arenot any calendar entries to display.
        if (currentShownCalendarEntries.Count <= 0)
        {
            NoDiaryEntriesYet.gameObject.SetActive(true);
        }

        SaveDateEntries();
    }

    public void OnInputFieldSubmitted()
    {
        //Debug.Log("Submitted input field with: " + InputField.text);
        //CalendarViewChanger.OnInputFieldSubmitted(InputField.text);

        if (InputField.text != null
            && !InputField.text.Equals(""))
        {
            UpdateEntryText(InputField.text);
        }

        InputField.text = "";

    }

    public void OnInputFieldDeselected()
    {
        //Debug.Log("Deselected input field. Text is " + InputField.text);
        InputField.text = "";
    }

    // Makes sure the objects needed to display an individual day
    // are activated properly.
    private void ActivateDayView()
    {
        gameObject.SetActive(true);
        ViewportContent.SetActive(true);
        DateText.gameObject.SetActive(true);
        InputFieldHolder.gameObject.SetActive(true);
        InputField.text = "";
    }

    // Set the date we are about to display
    private void UpdateDayViewDate(DateTime date)
    {
        // Save the DateTime
        currentShownDateTime = date;

        // Display year, month and day as well as day of week on the text field
        DateText.text = date.Year + " " + CalendarViewChanger.GetMonthString(date.Month) + " " + date.Day + "\n" + date.DayOfWeek.ToString();
    }

    // We are about to update and populate the day view
    // with calendar entries
    private void UpdateAllTexts(CalendarController.CalendarItem item)
    {
        // Do whatever needs to be done
        // to make the view empty...
        ResetView();

        // ... and then populate the view with data found in the item
        PopulateView(item);
    }

    // Populating the day view calendar with data from the
    // CalendarItem struct
    private void PopulateView(CalendarController.CalendarItem item)
    {
        // Item has a list of entries for the day
        // So we iterate through it
        for (int i = 0; i < item.CalendarEntries.Count; i++)
        {
            // Instantiate a calendar entry object from the prefab
            // set in the inspector and make it the child
            // of viewport content object.
            // The viewport handles placing the content to show as a list.
            CalendarEntry entry = Instantiate(CalendarEntryObjectOriginal, ViewportContent.transform);

            // Then we fill the instantiated calendar entry object
            // with a number
            // We also set a reference to this object, so the object can call
            // OnDestroyItemPressed(CalendarEntry entry) when the trashcan icon is pressed.
            entry.FillEntry(this, i + 1, item.CalendarEntries[i]);

            // Save a reference to just added entry by putting it into the list of entries
            currentShownCalendarEntries.Add(entry);
        }

        // No entries, let's set a info text about that active
        if (item.CalendarEntries.Count <= 0)
        {
            NoDiaryEntriesYet.gameObject.SetActive(true);
        }

        // We have entries, so we definitely do not want to
        // inform player that there are not any diary entries,
        // because there are
        else
        {
            NoDiaryEntriesYet.gameObject.SetActive(false);
        }
    }

    private void ResetView()
    {
        // Destroy the previous entry gameObjects that
        // are children of the ViewPortContent object.
        for (int i = 0; i < currentShownCalendarEntries.Count; i++)
        {
            //Debug.Log("About to destroy previous calendar entry");
            Destroy(currentShownCalendarEntries[i].gameObject);
        }

        // Clear the list of calendar entries
        currentShownCalendarEntries.Clear();

        // Now that the view is empty, we inform player
        // with a text that there are no diary entries
        // for this date yet
        NoDiaryEntriesYet.gameObject.SetActive(true);
    }


    // Player just submitted an entry text to the
    // diary entry input field. So we need to
    // update the view accordingly
    private void UpdateEntryText(string entryText)
    {
        // We check just in case that we didn't
        // get a null or empty string
        if (entryText == null
            || entryText.Equals(""))
        {
            //Debug.Log("Tried to submit a null or an empty string");
            return;
        }

        // We check if the CalenrdarController already has an entry for this date
        // We are returned a bool and an out item
        // By the bool we know if the date already has an entry
        // But since we are possibly returned "an empty" default struct with year,
        // month and date set to one and an list with and a list with an empty string
        // we just clear the entries list in that case.
        // Maybe this was a stupid way to implement that one? Maybe not.
        bool hasAnEntry = CalendarController.Instance.CheckForACalendarItem(currentShownDateTime, out CalendarController.CalendarItem item);

        if (!hasAnEntry)
        {
            item.CalendarEntries.Clear();
        }

        // We add the new entry text to the list
        // of the date item list of entry texts
        item.CalendarEntries.Add(entryText);

        // We save the newly updated CalendarItem,
        // so it can be used later
        CalendarController.Instance.SaveIndividualDay(currentShownDateTime, item.CalendarEntries);



        // Make sure to clear the view...
        ResetView();

        // ...before populating it with the newly updated data
        PopulateView(item);
    }



    // Saving the current date entries.
    private void SaveDateEntries()
    {
        // If we had entries currently showing...
        if (currentShownCalendarEntries.Count > 0)
        {
            // ...we want to create a list of the entries...
            List<string> texts = new List<string>();

            // ...add the texts to a list...
            for (int i = 0; i < currentShownCalendarEntries.Count; i++)
            {
                texts.Add(currentShownCalendarEntries[i].Entry.text);
            }

            // ...and save the day with current shown date time and the list
            // of entry texts.
            CalendarController.Instance.SaveIndividualDay(currentShownDateTime,
                                                          texts);
        }

        // If we didn't have entries currently showing, if for an example
        // the player deleted all or didn't fill them in the first place...
        else
        {
            //Debug.LogWarning("All empty calendar for this day. Remove the date");

            // ...we ask CalendarController to remove the current date from the dictionary          
            CalendarController.Instance.RemoveIndividualDay(currentShownDateTime);
        }

    }
}
