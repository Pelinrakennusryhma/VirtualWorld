using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Globalization;
using UnityEngine.EventSystems;


// This component is in charge of displaying
// the calendar on the tablet screen
public class CalendarWindowChanger : MonoBehaviour
{
    // A reference to the button that closes
    // and individual calenrar item, that is
    // the day shown
    // TO BE REFACTORED? Moved to DayViewCalendar?------------------
    public Button CloseCalendarItemButton;
    //--------------------------------------------------------------

    // The text field that displays which
    // day today is
    public TextMeshProUGUI TodayIsYearMonth;

    // The parent object that holds weeks (that hold days)
    // shown on the calendar as well as texts that tell
    // which day of the week is which
    public GameObject MonthObject;

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

    // An array of buttons used to display days and their number
    // in a month. These buttons hold info about their DateTime
    // as well as display the proper number and an exclamation mark
    // if the date in question happens to have a diary entry on them
    public CalendarDayButton[] DayButtons;

    // Used to keep track of which date (month and year primarily,
    // the days are tracked in the Day Buttons)
    // is currently displayed
    private DateTime CurrentShownDate;

    // The game object that has a button to navigate to the month
    // that is before the currently shown month
    public GameObject MonthNavigateButtonLeft;

    // The game object that has a button to navigate to the month
    // that comes after the currently shown month
    public GameObject MonthNavigateButtonRight;

    // A reference to the object that displays a day in the calendar
    // and controls whatever is displayed there.
    public DayViewCalendar DayViewCalendar;


    // TO BE REFACTORED? Moved under DayViewCalendar?----------------------------------
    
    // A reference to the input field into which player can enter
    // notes about the day. Activated and deactivated, also cleared.
    public TMP_InputField NoteInputField;

    // A text that prompts the player to enter text into the input field.
    // Activated and deactivated, depending if a month or individual day is shown.
    public GameObject EnterANoteText;
    //--------------------------------------------------------------------------------

    // A reference to the view within a view UI raycaster
    // that is on the tablet screen. We submit to the event
    // that informs us when the input field has been submitted
    // and we tell the day view to update the diary entry that
    // just got entered
    public ViewWithinAViewUIRaycaster ViewWithinAViewUIRaycaster;

    // Used to keep track of if we are currently showing
    // an individual day.
    private bool IsShowingDayView;


    public void Awake()
    {
        // If this object has not been initialized --> initialize it
        if (!HasBeenInitialized)
        {
            Init();
        }
    }

    // Initializing the calendar window changer making it ready to display
    // whatever is needed to be displayed
    public void Init()
    {
        // Create a new Gregorian calendar and save the reference to it
        // so we can used C# calendar functionality.
        calendar = new GregorianCalendar();

        // Set HasBeenInitialized to true, so we do not do this again.
        HasBeenInitialized = true;

        // But can we be sure the days are in the right order after this fetch?
        // If I remember correctly, the order of components returned is not guaranteed.
        // DayButtons = MonthObject.GetComponentsInChildren<CalendarDayButton>(true); 
        
        // So, maybe it is safer to populate the array manually?
        for (int i = 0; i < DayButtons.Length; i++)
        {
            // Initialized the day buttons, so they have a reference 
            // to this object and they fetch their image component too.
            DayButtons[i].Init(this);
        }

        // Subscribe to the event of submitting to the input field.
        // That is, when the user has entered text and pressed enter
        ViewWithinAViewUIRaycaster.OnInputFieldSubmitted -= OnInputFieldSubmitted;
        ViewWithinAViewUIRaycaster.OnInputFieldSubmitted += OnInputFieldSubmitted;


    }

    // We are informed that the close button of the individual day view has been
    // pressed and we should act accordingly.
    public void OnCalendarCloseButtonPressed()
    {
        // We tell the individual day view controller to close
        // the individual day and act according to that, that is 
        // to save the info that is on display, if any.
        DayViewCalendar.OnCloseIndividualDay();

        // We go back to showing the month that was previously shown.
        GoBackToPreviouslyShownMonth();
    }

    // We go back to displaying the month that was previously shown.
    public void GoBackToPreviouslyShownMonth()
    {
        // We are not showing an individual day view
        IsShowingDayView = false;

        // Activate whatever objects need to be activated to show a month.
        ActivateMonthViewObjects();

        // Make sure to refresh the month with proper data.
        // Probably this wouldn't even need to be called, because the
        // data about a month should still be the same,
        // but just in case of future restructuring of code
        // we will leave this here just to be safe.
        RefreshMonthView(CurrentShownDate);
    }

    // We set active the objects we need to display a month
    // and inactive the objects we use to display an individual day.
    // TO BE REFACTORED? -------------------------------------------
    // Maybe this functionality could be grouped under a (currently inexistent)
    // month view object and the day view controller
    private void ActivateMonthViewObjects()
    {
        // Set active the text object that displays data about current date
        TodayIsYearMonth.gameObject.SetActive(true);

        // Set active the buttons used to navigate months
        MonthNavigateButtonLeft.gameObject.SetActive(true);
        MonthNavigateButtonRight.gameObject.SetActive(true);

        // Make sure the whole month object is set active
        // So we see the children days and texts about weekdays.
        MonthObject.SetActive(true);

        // Hide the button that closes an individual day view.
        CloseCalendarItemButton.gameObject.SetActive(false);

        // The day view shouldn't be shown
        DayViewCalendar.gameObject.SetActive(false);

        // Also hide the input field...
        NoteInputField.gameObject.SetActive(false);        
        
        // and the prompt that tells player to enter some text
        EnterANoteText.gameObject.SetActive(false);
    }
    //-------------------------------------------------------------

    // Called when the tablet view is changed to the calendar view.
    public void ShowCurrentMonth()
    {
        // Make sure we are initialized and ready to go.
        if (!HasBeenInitialized)
        {
            Init();
        }

        // We always show the current month and year
        // Whenever calendar view gets activated.
        DateTime dateTime = DateTime.Now;

        // Display the current date
        ShowDate(dateTime);

        // We are not showing the day view, because
        // we just opened the calendar month
        IsShowingDayView = false;

        // Make sure to activate the proper objects to display a month.
        ActivateMonthViewObjects();






        // TO BE REFACTORED, OR NOT? ----------------------------------------------
        // Save the current shown date as the first of this month??????????????????
        // But why??? Why it is not saved as current date time?
        // But at least the time gets reset to 12:00 AM, but does anything depend on that
        // And if so, THAT IS POOR DESIGN, IMPLMENTATION, STRUCTURING and 
        // all in all POOR CODE!
        // Because, maybe the CalendarController checks dictionary for a date and that
        // working requires the clock to be set to 12:00 ???

        CurrentShownDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        //------------------------------------------------------------------------





        // Setup the buttons with day numbers of the current month and year
        SetDatesToButtons(CurrentShownDate);
    }


    // Displays a date on the month calendar view on tablet screen.
    private void ShowDate(DateTime dateTime)
    {
        // Get the month to be shown as a string
        string month = GetMonthString(dateTime.Month);

        // Set the year and month to the text object that displays them
        // on the calendar view on the tablet.
        TodayIsYearMonth.text = dateTime.Year.ToString() + " " + month;
    }

    // An individual day of the calendar shouls be displayed.
    public void ShowIndividualCalendarDay(DateTime dateTime)
    {
        //Debug.Log("Show individual calendar item");

        // Clear the input field of any notes left on there
        NoteInputField.text = "";

        // We are showing the day view now
        IsShowingDayView = true;

        // Activate/deactivate the proper objects to display
        // a day view correctly.
        ActivateDayViewObjects();

        // We ask the day view object to update the date time
        // that is, to display day, month and year
        DayViewCalendar.UpdateDayViewDate(dateTime);

        CalendarController.CalendarItem item; 

        // We check CalendarController's singleton instance if we have some diary entries for this date
        bool hasEntry = CalendarController.Instance.CheckForACalendarItem(dateTime, out item);

        // If we had an entry for the date, update DayViewCalendar view's texts...
        if (hasEntry) 
        {
            DayViewCalendar.UpdateAllTexts(item);
        }

        // ...otherwise just reset the view
        else
        {
            DayViewCalendar.ResetView();
        }
    }

    // We are showing a day view, so we should activate/deactivate proper objects
    // for that purpose.
    private void ActivateDayViewObjects()
    {
        // Disable the month objects holder that has weeks and date buttons as children
        MonthObject.gameObject.SetActive(false);

        // Disable the object that shows year and month
        TodayIsYearMonth.gameObject.SetActive(false);

        // Disable the month navigation buttons
        MonthNavigateButtonLeft.gameObject.SetActive(false);
        MonthNavigateButtonRight.gameObject.SetActive(false); 
        
        // Activate the button that is used to close the individual day view.
        CloseCalendarItemButton.gameObject.SetActive(true);

        // Activate the day view calendar object that has it's children
        // the objects that hold info about the current date
        DayViewCalendar.gameObject.SetActive(true);

        // Activate the input field for entering diary entries...
        NoteInputField.gameObject.SetActive(true);

        //... and activate the prompt that tells the player to enter text
        EnterANoteText.gameObject.SetActive(true);
    }

    // A button informs us that it was pressed. We get info about the date
    // that is in question
    public void OnCalendarButtonPressed(DateTime date)
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

    // We change the displayed month.
    public void ChangeMonth(bool goLeft)
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

    // Make the month view show a month
    private void RefreshMonthView(DateTime monthToShow)
    {
        // Save the current month to show
        CurrentShownDate = monthToShow;

        // Make the date object show the current month and year
        ShowDate(CurrentShownDate);

        // Update the day buttons with month's day numbers
        SetDatesToButtons(CurrentShownDate);
    }

    // TODO: make a neat comment
    private void SetDatesToButtons(DateTime monthToShow)
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
        // But we have to start rhw week from Monday, because that is
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



        // TODO: make the loop not loop through the start date?---------------------------------
        // Afraid of an off-by-one -error...
        //------------------------------------------------------------        
        
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


    // TO BE REFACTORED??? -------------------------------------------------
    // Currently of course there's only the diary entry input field
    // that gets activated, but since we subscribe to the event on Init
    // and never unsubscribe, we practically get input from any input field
    // that uses the ViewWithinAViewUIRaycaster, that might come in the future.
    // OR PERHAPS NOT, since we check if we happen to be showing the day view.

    // This method is called, when the ViewWithinAViewUIRaycaster fires an
    // event that the input field has been submitted, that is it has text
    // and enter is pressed. The input field in question is of course
    // the one with the entry prompt: NoteInputField
    public void OnInputFieldSubmitted(string text)
    {
        if (IsShowingDayView)
        {
            DayViewCalendar.UpdateEntryText(text);
        }
    }
    //---------------------------------------------------------------------

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
}
