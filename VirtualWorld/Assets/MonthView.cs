using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using System.Globalization;

public class MonthView : MonoBehaviour
{
    // The text field that displays which
    // month and year today is
    [SerializeField] private TextMeshProUGUI TodayIsYearMonth;

    // An array of buttons used to display days and their number
    // in a month. These buttons hold info about their DateTime
    // as well as display the proper number and an exclamation mark
    // if the date in question happens to have a diary entry on them

    // DayButtons are dragged in the inspector to ensure correct ordering
    // If I remmeber correctly, getcomponent -functions are not reliable
    // in this way, so the order might change.
    [SerializeField] private CalendarDayButton[] dayButtons;

    public CalendarDayButton[] DayButtons { get => dayButtons; private set => dayButtons = value; }

    // The game object that has a button to navigate to the month
    // that is before the currently shown month
    [SerializeField] private GameObject MonthNavigateButtonLeft;

    // The game object that has a button to navigate to the month
    // that comes after the currently shown month
    [SerializeField] private GameObject MonthNavigateButtonRight;

    private CalendarViewChanger CalendarViewChanger;

    public void Init(CalendarViewChanger calendarViewChanger)
    {
        CalendarViewChanger = calendarViewChanger;

        for (int i = 0; i < DayButtons.Length; i++)
        {
            DayButtons[i].Init(CalendarViewChanger);
        }
    }

    public void ActivateMonthView()
    {
        gameObject.SetActive(true);        
        MonthNavigateButtonLeft.SetActive(true);
        MonthNavigateButtonRight.SetActive(true);

    }

    public void RefreshMonthView(DateTime monthToShow,
                                 GregorianCalendar calendar)
    {

        ActivateMonthView();
        ShowDate(monthToShow);
        SetMonthChangeButtonColors(monthToShow);
        SetDatesToButtons(monthToShow, calendar);
    }

    public void DeactivateMonthView()
    {
        gameObject.SetActive(false);
    }

    private void ShowDate(DateTime dateTime)
    {
        // Get the month to be shown as a string
        string month = CalendarViewChanger.GetMonthString(dateTime.Month);

        // Set the year and month to the text object that displays them
        // on the calendar view on the tablet.
        TodayIsYearMonth.text = dateTime.Year.ToString() + " " + month;
    }

    private void SetMonthChangeButtonColors(DateTime monthToShow)
    {
        // If we are currently displaying the current month...
        if (monthToShow.Month == DateTime.Now.Month
            && monthToShow.Year == DateTime.Now.Year)
        {
            // We set the left month navigation button's background image to gray
            // to indicate that it can not be interacted with.
            MonthNavigateButtonLeft.GetComponent<Image>().color = Color.gray;
        }

        // If we happen to be displaying any other month than the current one...
        else
        {
            // We set the left month navigation button's background image to white
            // to indicate that it can be interacted with the same way as the right one can be.
            MonthNavigateButtonLeft.GetComponent<Image>().color = Color.white;
        }
    }

    private void SetDatesToButtons(DateTime monthToShow,
                                  GregorianCalendar calendar)
    {

        // We get the first day of the month...
        DateTime monthStart = new DateTime(monthToShow.Year, monthToShow.Month, 1);

        // ... so we can get the week day that the month starts on.
        DayOfWeek monthStartDayOfWeek = monthStart.DayOfWeek;



        // The index of the first day button we start from looping
        // the DayButtons array. The first day of the month
        // will fall on one of the days of the first week (the top one)
        // of the calendar, but we need to determine which one.
        int startIndex = 0;

        // Determine the day of week the month starts on.
        // Note: the numbering is different from the DayOfWeek -enum.
        // The numbering of that enum starts from 0 as Sunday.
        // But we have to start the week from Monday, because that is
        // the first day on the calendar's month object and the DayButtons
        // -array of days
        switch (monthStartDayOfWeek)
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
        }

        // To keep track of how many days we substract or add
        // to the day of the month start. We get the date to display
        // on the button from Gregorian calendar with adding or substracting
        // days from the current shown month's start.
        int dateAdd = 0;

        // Loop backwards from the startIndex, that we set earlier according to the day of week
        for (int i = startIndex; i >= 0; i--)
        {
            // Use the Gregorian calendar's AddDays -function to get a date
            // according to how many days apart it is from the first day of month
            // We pass negative values to the function, so we go backwards in time
            DateTime newDate = calendar.AddDays(monthStart, dateAdd);

            // Make the CalendarDayButton in the DayButtons -array
            // display the date we just got.
            // At this point we are populating the rest of the first week
            // on display in the calendar, with dates of the previous month.
            // So we don't want to make the buttons interactable and display
            // that they have a diary entry on them.
            DayButtons[i].DisplayADateTime(newDate, false, false);

            // Substract one from the dateAdd, so the next time we loop
            // this one, we get the previous date to display
            dateAdd--;
        }

        // Reset the value of dateAdd...
        dateAdd = 0;

        // ... because we start iterating again
        // over the DayButtons -array, but this time forwards.
        for (int i = startIndex; i < DayButtons.Length; i++)
        {
            // Use the Gregorian calendar's AddDays -function, to get
            // a date in the future by adding days to the month start date.
            DateTime newDate = calendar.AddDays(monthStart, dateAdd);


            // We check the calendar controller if we happen to have
            // have diary entries on the day in question
            bool hasEntry = CalendarController.Instance.CheckForACalendarItem(newDate,
                                                                              out CalendarController.CalendarItem calendarItem);


            // By default we don't want to make the CalendarDayButton interactable
            // because it doesn't belong to the current month.
            bool makeInteractable = false;

            // However, if the new date's month and year are of the currently displayed month...
            if (newDate.Month == monthToShow.Month
                && newDate.Year == monthToShow.Year)
            {
                // ...we do want to make the CalendarDayButton interactable and not grayscaled.
                makeInteractable = true;
            }

            else
            {
                // We don't care about diary entries that don't belong to currently displayed month
                // because only the current month's day buttons can be interacted with,
                // so we just set the hasEntry to false, so we can inform the button
                // that it doesn't need to display an exclamation mark to indicate an entry.
                hasEntry = false;
            }

            // We tell the CalendardayButton currently looped through to update
            // it's date and how it is displayed
            DayButtons[i].DisplayADateTime(newDate, makeInteractable, hasEntry);

            // Add one to dateAdd, so the next time we loop through the array
            // we get a date one more day in the future comapred to the start date
            dateAdd++;
        }
    }

}
