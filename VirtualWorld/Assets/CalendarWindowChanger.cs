using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Globalization;
using UnityEngine.EventSystems;

public class CalendarWindowChanger : MonoBehaviour
{
    public Button CloseCalendarItemButton;
    public TextMeshProUGUI TodayIsYearMonth;
    public GameObject MonthObject;

    private bool HasBeenInitialized;


    private GregorianCalendar calendar;

    public CalendarDayButton[] DayButtons;

    private DateTime CurrentShownMonth;

    public GameObject MonthNavigateButtonLeft;
    public GameObject MonthNavigateButtonRight;

    public DayViewCalendar DayViewCalendar;
    public TMP_InputField NoteInputField;
    public GameObject EnterANoteText;

    public ScrollRect ScrollView;

    public ViewWithinAViewUIRaycaster ViewWithinAViewUIRaycaster;

    private bool IsShowingDayView;


    public void Awake()
    {
        if (!HasBeenInitialized)
        {
            Init();
        }
    }


    public void Init()
    {
        calendar = new GregorianCalendar();

        int daysInFebruary = calendar.GetDaysInMonth(2023, 2);

        //Debug.Log("Should init calendar. Days in february are " + daysInFebruary.ToString());
        HasBeenInitialized = true;

        //But can we be sure the days are in the right order after this fetch?
        // DayButtons = MonthObject.GetComponentsInChildren<CalendarDayButton>(true); 

        // Maybe it is safer to populate the array manually?
        for (int i = 0; i < DayButtons.Length; i++)
        {
            DayButtons[i].Init(this);

            //Debug.Log("Week is " + DayButtons[i].Week + " day is " + DayButtons[i].Day);
        }

        ViewWithinAViewUIRaycaster.OnInputFieldSubmitted -= OnInputFieldSubmitted;
        ViewWithinAViewUIRaycaster.OnInputFieldSubmitted += OnInputFieldSubmitted;


    }

    public void OnCalendarCloseButtonPressed()
    {
        //Debug.Log("CalendarWindowChanger knows we pressed a button");

        DayViewCalendar.OnCloseIndividualDay();
        GoBackToPreviouslyShownMonth();
    }

    public void GoBackToPreviouslyShownMonth()
    {
        //Debug.Log("Should go back to showing previously open month");

        IsShowingDayView = false;
        ActivateMonthViewObjects();
        RefreshMonthView(CurrentShownMonth);
    }

    private void ActivateMonthViewObjects()
    {
        TodayIsYearMonth.gameObject.SetActive(true);
        MonthNavigateButtonLeft.gameObject.SetActive(true);
        MonthNavigateButtonRight.gameObject.SetActive(true);
        MonthObject.SetActive(true);
        CloseCalendarItemButton.gameObject.SetActive(false);
        DayViewCalendar.gameObject.SetActive(false);
        NoteInputField.gameObject.SetActive(false);
        EnterANoteText.gameObject.SetActive(false);
        //ScrollView.gameObject.SetActive(false);
    }



    public void ShowCalendar()
    {
        //Debug.Log("Should show calendar");

        if (!HasBeenInitialized)
        {
            Init();
        }

        DateTime dateTime = DateTime.Now;
        //TodayIsTestText.text = "Today is : " + dateTime.ToString() + " \nThere are " + calendar.GetDaysInMonth(dateTime.Year, dateTime.Month) + " days in this month";
        ShowDate(dateTime);

        //Debug.Log("Should show calendar");

        IsShowingDayView = false;
        ActivateMonthViewObjects();

        CurrentShownMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        SetDatesToButtons(CurrentShownMonth);
    }

    public static string GetMonthString(int month)
    {
        string monthString = "";

        switch (month)
        {
            case 1:
                monthString = "JANUARY";
                break;
            case 2:
                monthString = "FEBRUARY";
                break;
            case 3:
                monthString = "MARCH";
                break;
            case 4:
                monthString = "APRIL";
                break;
            case 5:
                monthString = "MAY";
                break;
            case 6:
                monthString = "JUNE";
                break;
            case 7:
                monthString = "JULY";
                break;
            case 8:
                monthString = "AUGUST";
                break;
            case 9:
                monthString = "SEPTEMBER";
                break;
            case 10:
                monthString = "OCTOBER";
                break;
            case 11:
                monthString = "NOVEMBER";
                break;
            case 12:
                monthString = "DECEMBER";
                break;
        }

        return monthString;
    }

    private void ShowDate(DateTime dateTime)
    {
        string month = "";

        month = GetMonthString(dateTime.Month);

        TodayIsYearMonth.text = dateTime.Year.ToString() + " " + month;
    }

    public void ShowIndividualCalendarItem(DateTime dateTime)
    {
        //Debug.Log("Show individual calendar item");

        NoteInputField.text = "";
        IsShowingDayView = true;
        ActivateDayViewObjects();

        //Debug.Log("At this point we probably should fetch entries for this day's log?");

        DayViewCalendar.UpdateDayView(dateTime);

        CalendarController.CalendarItem item; 
        bool hasEntry = CalendarController.Instance.CheckForACalendarItem(dateTime, out item);

        // The above method returns empty items with empty fields for strings. With year set to 1, month 1 and day 1
        DayViewCalendar.UpdateAllTexts(item);
    }

    private void ActivateDayViewObjects()
    {
        CloseCalendarItemButton.gameObject.SetActive(true);
        MonthObject.gameObject.SetActive(false);
        TodayIsYearMonth.gameObject.SetActive(false);
        MonthNavigateButtonLeft.gameObject.SetActive(false);
        MonthNavigateButtonRight.gameObject.SetActive(false);
        DayViewCalendar.gameObject.SetActive(true);
        NoteInputField.gameObject.SetActive(true);
        EnterANoteText.gameObject.SetActive(true);
        //ScrollView.gameObject.SetActive(true);
    }

    public void OnCalendarButtonPressed(DateTime date)
    {
        //Debug.Log("Window changer knows a calendar button was pressed " + Time.time + " date time is " + date.ToString());
        ShowIndividualCalendarItem(date);
    }

    public void OnMonthChangeButtonPressed(bool isLeftButtonPressed)
    {
        //Debug.Log("We pressed a month change button. Should go left " + isLeftButtonPressed.ToString());
        ChangeMonth(isLeftButtonPressed);
    }

    public void ChangeMonth(bool goLeft)
    {
        DateTime newMonth;

        if (goLeft)
        {
            // Don't allow to go backwards?
            if (CurrentShownMonth.Month == DateTime.Now.Month
                && CurrentShownMonth.Year == DateTime.Now.Year) 
            {
                Debug.Log("Trying to go past the current month. Don't allow that. We have only the now and the future.");
            }

            else
            {
               newMonth = calendar.AddMonths(CurrentShownMonth, -1);
                RefreshMonthView(newMonth);
            }
        }

        else
        {
            newMonth = calendar.AddMonths(CurrentShownMonth, 1);
            RefreshMonthView(newMonth);
        }

        Debug.Log("The next month to be shown is " + CurrentShownMonth.ToString());


    }

    private void RefreshMonthView(DateTime monthToShow)
    {
        CurrentShownMonth = monthToShow;

        ShowDate(CurrentShownMonth);
        SetDatesToButtons(CurrentShownMonth);
    }

    private void SetDatesToButtons(DateTime monthToShow)
    {
        if (monthToShow.Month == DateTime.Now.Month
            && monthToShow.Year == DateTime.Now.Year)
        {
            MonthNavigateButtonLeft.GetComponent<Image>().color = Color.gray;
        }

        else
        {
            MonthNavigateButtonLeft.GetComponent<Image>().color = Color.white;
        }

        DateTime monthStart = new DateTime(monthToShow.Year, monthToShow.Month, 1);

        DayOfWeek startDayOfWeek = monthStart.DayOfWeek;

        int startIndex = 0;

        switch (startDayOfWeek)
        {
            case DayOfWeek.Monday:
                startIndex = 0;
                break;

            case DayOfWeek.Tuesday:
                startIndex = 1;
                break;

            case DayOfWeek.Wednesday:
                startIndex = 2;
                break;

            case DayOfWeek.Thursday:
                startIndex = 3;
                break;

            case DayOfWeek.Friday:
                startIndex = 4;
                break;

            case DayOfWeek.Saturday:
                startIndex = 5;
                break;

            case DayOfWeek.Sunday:
                startIndex = 6;
                break;



            default:
                break;
        }

        int dateAdd = 0;


        // Assign dates to buttons.

        for (int i = startIndex; i >= 0; i--)
        {
            DateTime newDate = calendar.AddDays(monthStart, dateAdd);
            DayButtons[i].SetDateTime(newDate, true, false);
            dateAdd--;
        }

        dateAdd = 0;

        for (int i = startIndex; i < DayButtons.Length; i++)
        {
            DateTime newDate = calendar.AddDays(monthStart, dateAdd);
            CalendarController.CalendarItem calendarItem;
            bool hasEntry =  CalendarController.Instance.CheckForACalendarItem(newDate, out calendarItem);

            

            bool grayOut = true;

            if (newDate.Month == monthToShow.Month
                && newDate.Year == monthToShow.Year)
            {
                grayOut = false; 
            }

            else
            {
                hasEntry = false;
            }


            DayButtons[i].SetDateTime(newDate, grayOut, hasEntry);
            dateAdd++;
        }       
    }

    public void OnInputFieldSubmitted(string text)
    {
        if (IsShowingDayView)
        {
            DayViewCalendar.UpdateEntryText(text);
        }
    }

    public void OnViewClosed()
    {
        if (IsShowingDayView)
        {
            DayViewCalendar.OnCloseIndividualDay();
        }
    }
}
