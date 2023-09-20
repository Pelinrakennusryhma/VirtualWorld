using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DayViewCalendar : MonoBehaviour
{
    public CalendarEntry CalendarEntryObjectOriginal;
    public GameObject ViewportContent;
    public Scrollbar ViewportScrollBar;

    public List<CalendarEntry> PreviousCalendarEntries;

    public TextMeshProUGUI DateText;

    private DateTime currentShownDateTime;

    public GameObject NoDiaryEntriesYet;

    public GameObject InputFieldHolder;

    private void Awake()
    {
    }

    public void UpdateDayView(DateTime date)
    {
        currentShownDateTime = date;
        DateText.text = date.Year + " " + CalendarWindowChanger.GetMonthString(date.Month) + " " + date.Day + "\n" + date.DayOfWeek.ToString();
    }

    public void UpdateAllTexts(CalendarController.CalendarItem item)
    {
        ResetView();

        if (item.Year == 1)
        {

            return;
        }

        PopulateView(item);
    }

    private void PopulateView(CalendarController.CalendarItem item)
    {
        for (int i = 0; i < PreviousCalendarEntries.Count; i++)
        {
            //Debug.Log("About to destroy previous calendar entry");
            Destroy(PreviousCalendarEntries[i].gameObject);
        }

        PreviousCalendarEntries.Clear();

        for (int i = 0; i < item.Entries.Count; i++)
        {
            CalendarEntry entry = Instantiate(CalendarEntryObjectOriginal, ViewportContent.transform);
            entry.FillEntry(this, i + 1, item.Entries[i]);
            PreviousCalendarEntries.Add(entry);
        }

        if (item.Entries.Count <= 0)
        {
            NoDiaryEntriesYet.gameObject.SetActive(true);
        }

        else
        {
            NoDiaryEntriesYet.gameObject.SetActive(false);
        }
    }

    private void ResetView()
    {
        NoDiaryEntriesYet.gameObject.SetActive(true);
    }



    public void UpdateEntryText(string entryText)
    {
        if (entryText == null
            || entryText.Equals(""))
        {
            Debug.LogWarning("Tried to submit a null or an empty string");
            return;
        }

        bool hasAnEntry = CalendarController.Instance.CheckForACalendarItem(currentShownDateTime, out CalendarController.CalendarItem item);

        if (!hasAnEntry)
        {
            item.Entries.Clear();
        }

        item.Entries.Add(entryText);
        CalendarController.Instance.SaveIndividualDay(currentShownDateTime, item.Entries);
        PopulateView(item);
    }

    public void OnCloseIndividualDay()
    {
        //Debug.LogWarning("Here we should probably save the calendar item in question");

        if (PreviousCalendarEntries.Count > 0)
        {
            List<string> texts = new List<string>();

            for (int i = 0; i < PreviousCalendarEntries.Count; i++)
            {
                texts.Add(PreviousCalendarEntries[i].Entry.text);
            }

            CalendarController.Instance.SaveIndividualDay(currentShownDateTime,
                                                          texts);
        }

        else
        {
            Debug.LogWarning("All empty calendar for this day. Remove the date");
            CalendarController.Instance.RemoveIndividualDay(currentShownDateTime);
        }

        for (int i = 0; i < PreviousCalendarEntries.Count; i++)
        {
            Destroy(PreviousCalendarEntries[i].gameObject);
        }

        PreviousCalendarEntries.Clear();
    }

    public void OnRemoveCalendarItemButtonPressed(int id)
    {

    }

    public void OnDestroyItemPressed(CalendarEntry entry)
    {
        PreviousCalendarEntries.Remove(entry);
        Destroy(entry.gameObject);

        for (int i = 0; i < PreviousCalendarEntries.Count; i++)
        {
            PreviousCalendarEntries[i].Number.text = (i + 1).ToString();
        }

        if (PreviousCalendarEntries.Count <= 0)
        {
            NoDiaryEntriesYet.gameObject.SetActive(true);
        }
    }
}
