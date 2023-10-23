using UnityEngine;
using System;
using System.Globalization;

// This component is in charge of displaying
// the calendar on the tablet screen.
// Controls Month View and Day View -objects.
// Listens to button presses
// and acts according to them.
public class CalendarViewChanger : MonoBehaviour
{

    // Just a bool to keep track if we have initialized this object
    // so we don't initialize twice, but initialize on Awake or
    // at the latest before the calendar is displayed
    private bool HasBeenInitialized;

    // We'll use the Gregorian calendar and create a new one of that
    // kind, so we can use functions like AddDays and AddMonths
    // to display months and days as needed. Also, of course the
    // Gregorian calendar knows the days in a month and so on, which is 
    // of course absolutely needed to display a month properly.
    private GregorianCalendar calendar;

    // Used to keep track of which date (month and year primarily,
    // the days are tracked in the Day Buttons)
    // is currently displayed
    private DateTime CurrentShownDate;

    // A reference to the object that displays a day in the calendar
    // and controls whatever is displayed there.
    private DayViewCalendar DayViewCalendar;

    // A reference to the object that displays a month in the calendar
    // and controls whatever is displayed there.
    private MonthView MonthViewCalendar;

    // Used to keep track of if we are currently showing
    // an individual day.
    private bool IsShowingDayView;



    // A public static method to be accessed from anywhere,
    // that returns the month as a string according to the number given.
    public static string GetMonthString(int month)
    {
        // Set the string empty, so if an invalid month
        // is given, the empty string gets returned.
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

            default:
                // Log an error about invalid month number
                Debug.LogError("Tried to convert a month number to a string, but an erroneus month number " + month + " was given. Returned an empty string.");
                break;
        }

        return monthString;
    }

    private void Awake()
    {
        // If this object has not been initialized --> initialize it
        if (!HasBeenInitialized)
        {
            Init();
        }
    }

    // Initializing the calendar window changer making it ready to display
    // whatever is needed to be displayed
    private void Init()
    {
        // Fetch the day view from children
        DayViewCalendar = GetComponentInChildren<DayViewCalendar>(true);

        // Fetch the month view from children
        MonthViewCalendar = GetComponentInChildren<MonthView>(true);
        MonthViewCalendar.Init(this);

        // Create a new Gregorian calendar and save the reference to it
        // so we can use C# calendar functionality.
        calendar = new GregorianCalendar();

        // Set HasBeenInitialized to true, so we do not do this again.
        HasBeenInitialized = true;
    }

    // Called when the tablet view is changed to the calendar view.
    public void OnCalendarOpened()
    {
        // Make sure we are initialized and ready to go.
        if (!HasBeenInitialized)
        {
            Init();
        }

        // We are not showing the day view, because
        // we just opened the calendar month
        IsShowingDayView = false;

        CurrentShownDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        // Deactivate day view
        DayViewCalendar.DeactivateDayView();

        // Refresh the month view
        MonthViewCalendar.RefreshMonthView(CurrentShownDate, calendar);
    }

    // A button informs us that it was pressed. We get info about the date
    // that is in question
    public void OnCalendarDayButtonPressed(DateTime date)
    {
        // Just start showing the individual day in question
        ShowIndividualCalendarDay(date);
    }

    // An unity event set in the inspector
    // A bool tells us whether it was the left button pressed, and if false
    // then it was the right
    public void OnMonthChangeButtonPressed(bool isLeftButtonPressed)
    {

        // We change the displayed month. According to if we want to go left (backwards)
        // or right of course and we move into the future.
        ChangeMonth(isLeftButtonPressed);
    }

    // We are informed that the close button of the individual day view has been
    // pressed and we should act accordingly.
    public void OnIndividualCalendarDayCloseButtonPressed()
    {
        // We tell the individual day view controller to close
        // the individual day and act according to that, that is 
        // to save the info that is on display, if any
        // and then deactivate the view
        DayViewCalendar.OnCloseIndividualDay();

        // We are not showing an individual day view
        IsShowingDayView = false;

        // Make sure to refresh the month with proper data.
        // Probably this wouldn't even need to be called, because the
        // data about a month should still be the same,
        // but just in case of future restructuring of code
        // we will leave this here just to be safe.
        RefreshMonthView(CurrentShownDate);
    }

    // Called when the tablet view is changed from the calendar view to something else
    // and when the tablet is closed
    public void OnViewClosed()
    {
        // If we happen to have view of a day open...
        if (IsShowingDayView)
        {
            // ... we inform the day view controller that
            // it should close the individual day, so it 
            // possibly saves any diary data that was there on display.
            DayViewCalendar.OnCloseIndividualDay();
        }
    }

    // We change the displayed month.
    private void ChangeMonth(bool goLeft)
    {
        DateTime newMonth;

        // We go backwards in months from what was displayed previously
        if (goLeft)
        {
            if (CurrentShownDate.Month == DateTime.Now.Month
                && CurrentShownDate.Year == DateTime.Now.Year) 
            {
                // We don't allow to navigate past the current month.

                //Debug.Log("Trying to go past the current month. Don't allow that. We have only the now and the future.");
            }

            else
            {
                // We use the Gregorian calendar to get the previous month to the displayed one...
                newMonth = calendar.AddMonths(CurrentShownDate, -1);

                // ...and refresh the month view with the new month.
                RefreshMonthView(newMonth);
            }
        }

        // We go forwards in months from what was displayed previously
        else
        {
            // We use the Gregorian calendar to get the next month to the displayed one.
            newMonth = calendar.AddMonths(CurrentShownDate, 1);

            // ...and refresh the month view with the new month.
            RefreshMonthView(newMonth);
        }
    }

    // An individual day of the calendar should be displayed.
    private void ShowIndividualCalendarDay(DateTime dateTime)
    {
        // We are showing the day view now
        IsShowingDayView = true;

        // Deactivate the month view
        MonthViewCalendar.DeactivateMonthView();

        // Tell the day view to refresh
        DayViewCalendar.RefreshDayView(dateTime);
    }

    // Make the month view show a month
    private void RefreshMonthView(DateTime monthToShow)
    {
        // Save the current month to show
        CurrentShownDate = monthToShow;

        DayViewCalendar.DeactivateDayView();
        MonthViewCalendar.RefreshMonthView(monthToShow, calendar);
    }
}
