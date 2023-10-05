using System.Collections.Generic;
using UnityEngine;
using System;
using FishNet.Object;


// TO BE REFACTORED:--------------------------------------
// What is the need of the list
// other than possibly saving it. And what the hell
// is happening here?
// --------------------------------------------------------


// This controller is used to keep track of calendar things.
public class CalendarController : NetworkBehaviour
{

    // A struct for saving data about a day
    // with a string list of calendar entries
    // For the day in question

    // TO BE REFACTORED ----------------------------------------
    // Previously this was implemented with only
    // four string entries
    // This is system.serializable just fine
    // But if we ever start to pass 
    // these items over the network
    // We have to make this properly serialized
    // But how? How FishNet does things?
    // And are we even going to pass these through the network?
    //
    // ALSO: how about timezones?
    //
    //----------------------------------------------------------


    [System.Serializable]
    public struct CalendarItem
    {
        public int Day;
        public int Month;
        public int Year;

        public List<string> CalendarEntries;

        // Just a constructor
        // where we enter the year, month, day
        // and a string list of calendar entries
        // for the date
        public CalendarItem(int year,
                            int month,
                            int day,
                            List<string> calendarEntries)
        {
            Year = year;
            Month = month;
            Day = day;
            CalendarEntries = calendarEntries;
        }
    }

    // Make this a static singleton, so we can access
    // Instance from anywhere from the codebase
    // without specifially finding objects
    // or gettin components from god knows where
    // There should only be one of these ever.
    public static CalendarController Instance;

    // TO BE REFACTORED: Get rid of this? -------------------------------- 
    // A list of calendar items. Probably should get rid of this.
    private List<CalendarItem> CalendarItems = new List<CalendarItem>();
    //-------------------------------------------------------------------

    // Dictionary to keep track of date times and entries that belong to them
    private Dictionary<DateTime, CalendarItem> CalendarDictionary = new Dictionary<DateTime, CalendarItem>();

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        else
        {
            // We set the instance to this one, so we can access the static field
            Instance = this;
        }

        CalendarItems = new List<CalendarItem>();
      

        // Just a bunch of items that were there for testing purposes
        // Maybe we'll leave it there commented out, if we need them to resurface 

        // TO BE REFACTORED:------------------------------------------------------------------------------------------
        // These seem to have been here for testing purposes, so it should be fine to
        // get rid of it all.
        // Including the list. 


        //CalendarItems.Add(new CalendarItem(2023, 9, 12, new string[]));
        //CalendarItems.Add(new CalendarItem(2023, 9, 12, new List<string> { "Ohhlala", "whattodo", "plaah", "platku" }));
        //CalendarItems.Add(new CalendarItem(2023, 9, 16, new List<string> { "yada yada", "huohjavoih", "platku6000" }));
        //CalendarItems.Add(new CalendarItem(2023, 9, 17, new List<string> { "a calendar entry", "just a really long message that makes no sense and holds no real information." }));
        //CalendarItems.Add(new CalendarItem(2023, 9, 18, new List<string> { "voila", "ällötyttävät tykyttävät tikit tikittävät tykyttävät tytöllä", "saippuanmyyjä", "ei ollut palindromi tuo edellinen, mutta sama informaatiosisältö kuin vastaavassa klassikkopalindromissa" }));
        //CalendarItems.Add(new CalendarItem(2023, 9, 20, new List<string> { "huihai kauhistus", "nötkönnötkönnöö", "kaikkionpilalla", "herranjestas" }));
        //CalendarItems.Add(new CalendarItem(2023, 9, 30, new List<string> { "mutta toisaalta", "kuka se oli? En tiedä, mutta joku sen täytyi olla!", "miten rakentaa data?", "ja noutaa sitä ja siirrellä edestakaisin?" }));

        //for (int i = 0; i < CalendarItems.Count; i++)
        //{
        //    int year = CalendarItems[i].Year;
        //    int month = CalendarItems[i].Month;
        //    int day = CalendarItems[i].Day;

        //    CalendarDictionary.Add(new DateTime(year, month, day), CalendarItems[i]);
        //}


        // END OF REFACTOR/DELETION
        // -----------------------------------------------------------

    }

    // This function is used to check
    // If the calendar item dictionary has
    // entries for the date
    // We return true or false, depending 
    // if we had entries for the date
    // As an out parameter the item is also returned.
    // But since it is a struct as an out parameter, we need to assing something to
    // it, even if we don't find anything in the dictionary
    // Maybe this is poor design, but be careful
    // with the data that has been returned. To act according to
    // bool if an entry is found or not, is recommended.
    public bool CheckForACalendarItem(DateTime dateToCheck,
                                      out CalendarItem item)
    {
        // TO BE REFACTORED?----------------------------------
        // We check the dateToCheck, but does this work only
        // because CalendarWindowChanger has been implemented 
        // poorly, with a new DateTime created, with a day set to one
        // and because it is a new one the clock is set to 12:00 AM?
        // Also, milliseconds and ticks might not match?

        // The refactored implmentation. Good enough?
        dateToCheck = GetDateTimeForDictionary(dateToCheck);

        if (CalendarDictionary.ContainsKey(dateToCheck))
        {
            //Debug.Log("We found a matching key. ooh lala!!");

            // We found an entry for this date.
            // Out goes the item
            // and true (as in: found) returned
            item = CalendarDictionary[dateToCheck];
            return true;
        }

        //----------------------------------------------------




        // In the case the dictionary didn't contain the date
        // We create a default calendar item with unusable data
        // and return false
        item = new CalendarItem(1, 1, 1, new List<string> { ""});
        return false;
    }

    // We save the individual date with up to date entries
    public void SaveIndividualDay(DateTime dateTime,
                                  List<string> entries)
    {
        // The refactored implmentation. Good enough?
        dateTime = GetDateTimeForDictionary(dateTime);

        // Create the CalendarItem data struct
        CalendarItem newCalendarItem = new CalendarItem(dateTime.Year,
                                                        dateTime.Month,
                                                        dateTime.Day,
                                                        entries);

        // Check if the dictionary already has something for the date...
        if (CalendarDictionary.ContainsKey(dateTime))
        {
            // ...if so, but the item in the dictionary
            // in place of the previous out of date entry
            CalendarDictionary[dateTime] = newCalendarItem;
        }

        else
        {
            // Otherwise we just add a new entry to the dictionary.
            CalendarDictionary.Add(dateTime, newCalendarItem);
        }
    }

    // We make a version of the dateTime, that we can use as a key in 
    // dictionary. Basically this just creates a new DateTime with default
    // values for other than year, month and day.
    // Basically we use this just to be sure we don't have
    // any hours, seconds and milliseconds to mess things up, when we are
    // savinga date or fetching one from the dictionary.
    private DateTime GetDateTimeForDictionary(DateTime dateTime)
    {
        return new DateTime(dateTime.Year, 
                            dateTime.Month, 
                            dateTime.Day);
    }

    // In the case we don't have any entries for the date,
    // we just remove the day from the dictionary
    public void RemoveIndividualDay(DateTime dateTime)
    {
        if (CalendarDictionary.ContainsKey(dateTime))
        {
            CalendarDictionary.Remove(dateTime);
        }
    }

    // TODO: Saving
    // For saving purposes. This awaits for the save system to be implemented.
    // In the cloud or otherwise.
    public CalendarItem[] MakeAnArrayOutOfDictionary(Dictionary<DateTime, CalendarItem> dict)
    {
        CalendarItem[] calendarItems = new CalendarItem[dict.Count];

        int i = 0;

        foreach (var kvp in dict)
        {
            calendarItems[i] = dict[kvp.Key];
            i++;
        }

        return calendarItems;
    }
}
