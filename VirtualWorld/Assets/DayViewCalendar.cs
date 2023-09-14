using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DayViewCalendar : MonoBehaviour
{
    public TextMeshProUGUI DateText;
    public TextMeshProUGUI EntriesText1;
    public TextMeshProUGUI EntriesText2;
    public TextMeshProUGUI EntriesText3;
    public TextMeshProUGUI EntriesText4;

    private bool Entry1Taken;
    private bool Entry2Taken;
    private bool Entry3Taken;
    private bool Entry4Taken;

    private DateTime currentShownDateTime;

    public RemoveCalendarItemButton RemoveCalendarItemButton1;
    public RemoveCalendarItemButton RemoveCalendarItemButton2;
    public RemoveCalendarItemButton RemoveCalendarItemButton3;
    public RemoveCalendarItemButton RemoveCalendarItemButton4;

    public GameObject Number1Text;
    public GameObject Number2Text;
    public GameObject Number3Text;
    public GameObject Number4Text;

    public GameObject NoDiaryEntriesYet;

    public GameObject InputFieldHolder;

    private void Awake()
    {
        EntriesText1.text = "";
        EntriesText2.text = "";
        EntriesText3.text = "";
        EntriesText4.text = "";
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

        if (item.Entry1 != null
            && !item.Entry1.Equals(""))
        {
            EntriesText1.text = item.Entry1;
            Entry1Taken = true;
        }

        else
        {
            Entry1Taken = false;
        }

        if (item.Entry2 != null
            && !item.Entry2.Equals(""))
        {
            EntriesText2.text = item.Entry2;
            Entry2Taken = true;
        }

        else
        {
            Entry2Taken = false;
        }

        if (item.Entry3 != null
            && !item.Entry3.Equals(""))
        {
            EntriesText3.text = item.Entry3;
            Entry3Taken = true;
        }

        else
        {
            Entry3Taken = false;
        }

        if (item.Entry4 != null
            && !item.Entry4.Equals(""))
        {
            EntriesText4.text = item.Entry4;
            Entry4Taken = true;
        }

        else
        {
            Entry4Taken = false;
        }

        ReorderItems();
        ShowHideButtonsAndWhatever();
    }

    private void ResetView()
    {
        EntriesText1.text = "";
        EntriesText2.text = "";
        EntriesText3.text = "";
        EntriesText4.text = "";

        Entry1Taken = false;
        Entry2Taken = false;
        Entry3Taken = false;
        Entry4Taken = false;

        Number1Text.gameObject.SetActive(false);
        Number2Text.gameObject.SetActive(false);
        Number3Text.gameObject.SetActive(false);
        Number4Text.gameObject.SetActive(false);

        RemoveCalendarItemButton1.HideButton();
        RemoveCalendarItemButton2.HideButton();
        RemoveCalendarItemButton3.HideButton();
        RemoveCalendarItemButton4.HideButton();

        NoDiaryEntriesYet.gameObject.SetActive(true);
    }

    private void ShowHideButtonsAndWhatever()
    {
        bool atLeastOneEntry = false;

        if (Entry1Taken)
        {
            RemoveCalendarItemButton1.ShowButton();
            Number1Text.SetActive(true);
            atLeastOneEntry = true;
        }

        else
        {
            RemoveCalendarItemButton1.HideButton();
            Number1Text.SetActive(false);
        }

        if (Entry2Taken)
        {
            RemoveCalendarItemButton2.ShowButton();
            Number2Text.SetActive(true);
            atLeastOneEntry = true;
        }

        else
        {
            RemoveCalendarItemButton2.HideButton();
            Number2Text.SetActive(false);
        }

        if (Entry3Taken)
        {
            RemoveCalendarItemButton3.ShowButton();
            Number3Text.SetActive(true);
            atLeastOneEntry = true;
        }

        else
        {
            RemoveCalendarItemButton3.HideButton();
            Number3Text.SetActive(false);
        }

        if (Entry4Taken)
        {
            RemoveCalendarItemButton4.ShowButton();
            Number4Text.SetActive(true);
            atLeastOneEntry = true;
        }

        else
        {
            RemoveCalendarItemButton4.HideButton();
            Number4Text.SetActive(false);
        }

        if (atLeastOneEntry)
        {
            NoDiaryEntriesYet.gameObject.SetActive(false);
            //Debug.Log("We had at least one entry");
        }

        else
        {
            NoDiaryEntriesYet.gameObject.SetActive(true);
            //Debug.Log("WE didn't have an entry");
        }

        if (Entry1Taken
            && Entry2Taken
            && Entry3Taken
            && Entry4Taken)
        {
            InputFieldHolder.gameObject.SetActive(false);
        }

        else
        {
            InputFieldHolder.gameObject.SetActive(true);
        }
    }

    public void UpdateEntryText(string entryText)
    {
        if (entryText == null
            || entryText.Equals(""))
        {
            Debug.LogWarning("Tried to submit a null or an empty string");
            return;
        }

        if (!Entry1Taken)
        {
            EntriesText1.text = entryText;
            Entry1Taken = true;
        }

        else if (!Entry2Taken)
        {
            EntriesText2.text = entryText;
            Entry2Taken = true;
        }

        else if (!Entry3Taken)
        {
            EntriesText3.text = entryText;
            Entry3Taken = true;
        }

        else if (!Entry4Taken)
        {
            EntriesText4.text = entryText;
            Entry4Taken = true;
        }

        ShowHideButtonsAndWhatever();
    }

    public void OnCloseIndividualDay()
    {
        //Debug.LogWarning("Here we should probably save the calendar item in question");
        
        if ((EntriesText1.text != null
            && !EntriesText1.text.Equals(""))
            || (EntriesText2.text != null
            && !EntriesText2.text.Equals(""))
            || (EntriesText3.text != null
            && !EntriesText3.text.Equals(""))
            || (EntriesText4.text != null
            && !EntriesText4.text.Equals("")))
        {
            CalendarController.Instance.SaveIndividualDay(currentShownDateTime,
                                                          EntriesText1.text,
                                                          EntriesText2.text,
                                                          EntriesText3.text,
                                                          EntriesText4.text);
        }

        else
        {
            Debug.LogWarning("All empty calendar for this day. Remove the date");
            CalendarController.Instance.RemoveIndividualDay(currentShownDateTime);
        }
    }

    public void OnRemoveCalendarItemButtonPressed(int id)
    {
        Debug.Log("Pressed remove calendar item button " + Time.time + " with id " + id);

        if (id == 1)
        {
            EntriesText1.text = "";
            Entry1Taken = false;
            Debug.Log("entry 1 cleared");
        }

        else if (id == 2)
        {
            EntriesText2.text = "";
            Entry2Taken = false;
            Debug.Log("entry 2 cleared");
        }

        else if (id == 3)
        {
            EntriesText3.text = "";
            Entry3Taken = false;
            Debug.Log("entry 3 cleared");
        }

        else if (id == 4)
        {
            EntriesText4.text = "";
            Entry4Taken = false;
            Debug.Log("entry 4 cleared");
        }

        ReorderItems();
        ShowHideButtonsAndWhatever();
    }

    private void ReorderItems()
    {
        if (EntriesText2.text != null
            && !EntriesText2.text.Equals("")
            && !Entry1Taken)
        {
            MoveToHighestFreePosition(EntriesText2.text);
            Entry2Taken = false;
            EntriesText2.text = "";
        }

        if (EntriesText3.text != null
            && !EntriesText3.text.Equals("")
            && (!Entry2Taken
                || !Entry1Taken))
        {
            MoveToHighestFreePosition(EntriesText3.text);
            Entry3Taken = false;
            EntriesText3.text = "";
        }

        if (EntriesText4.text != null
            && !EntriesText4.text.Equals("")
            && (!Entry3Taken
                || !Entry2Taken
                || !Entry1Taken))
        {
            MoveToHighestFreePosition(EntriesText4.text);
            Entry4Taken = false;
            EntriesText4.text = "";
        }
    }

    private void MoveToHighestFreePosition(string text)
    {
        Debug.LogWarning("Moving to highest free position");

        if (!Entry1Taken)
        {
            EntriesText1.text = text;
            Entry1Taken = true;
        }

        else if (!Entry2Taken)
        {
            EntriesText2.text = text;
            Entry2Taken = true;
        }

        else if (!Entry3Taken)
        {
            EntriesText3.text = text;
            Entry3Taken = true;
        }

        else if (!Entry4Taken)
        {
            EntriesText4.text = text;
            Entry4Taken = true;
        }
    }
}
