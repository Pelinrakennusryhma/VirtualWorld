using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FishNet.Object;

public class CalendarController : NetworkBehaviour
{
    [System.Serializable]
    public struct CalendarItem
    {
        public int Day;
        public int Month;
        public int Year;
        public string Entry1;
        public string Entry2;
        public string Entry3;
        public string Entry4;

        public List<string> Entries;

        public CalendarItem(int year,
                            int month,
                            int day,
                            List<string> entries)
        {
            Year = year;
            Month = month;
            Day = day;
            Entries = entries;

            Entry1 = "";
            Entry2 = "";
            Entry3 = "";
            Entry4 = "";
        }

        public CalendarItem(int year,
                            int month,
                            int day,
                            string entry1,
                            string entry2,
                            string entry3,
                            string entry4)
        {
            Day = day;
            Month = month;
            Year = year;
            Entry1 = entry1;
            Entry2 = entry2;
            Entry3 = entry3;
            Entry4 = entry4;

            if (Entry1 == null)
            {
                Entry1 = "";
            }

            if (Entry2 == null)
            {
                Entry2 = "";
            }

            if (Entry3 == null)
            {
                Entry3 = "";
            }

            if (Entry4 == null)
            {
                Entry4 = "";
            }

            Entries = new List<string>();
            Entries.Add(Entry1);
            Entries.Add(Entry2);
            Entries.Add(Entry3);
            Entries.Add(Entry4);
        }
    }


    public static CalendarController Instance;
    public List<CalendarItem> CalendarItems;

    public Dictionary<DateTime, CalendarItem> CalendarDictionary;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        CalendarItems = new List<CalendarItem>();
        //CalendarItems.Add(new CalendarItem(2023, 9, 12, new string[]));
        //CalendarItems.Add(new CalendarItem(2023, 9, 12, new List<string> { "Ohhlala", "whattodo", "plaah", "platku" }));
        //CalendarItems.Add(new CalendarItem(2023, 9, 16, new List<string> { "yada yada", "huohjavoih", "platku6000" }));
        //CalendarItems.Add(new CalendarItem(2023, 9, 17, new List<string> { "a calendar entry", "just a really long message that makes no sense and holds no real information." }));
        //CalendarItems.Add(new CalendarItem(2023, 9, 18, new List<string> { "voila", "ällötyttävät tykyttävät tikit tikittävät tykyttävät tytöllä", "saippuanmyyjä", "ei ollut palindromi tuo edellinen, mutta sama informaatiosisältö kuin vastaavassa klassikkopalindromissa" }));
        //CalendarItems.Add(new CalendarItem(2023, 9, 20, new List<string> { "huihai kauhistus", "nötkönnötkönnöö", "kaikkionpilalla", "herranjestas" }));
        //CalendarItems.Add(new CalendarItem(2023, 9, 30, new List<string> { "mutta toisaalta", "kuka se oli? En tiedä, mutta joku sen täytyi olla!", "miten rakentaa data?", "ja noutaa sitä ja siirrellä edestakaisin?" }));

        CalendarDictionary = new Dictionary<DateTime, CalendarItem>();

        for (int i = 0; i < CalendarItems.Count; i++)
        {
            int year = CalendarItems[i].Year;
            int month = CalendarItems[i].Month;
            int day = CalendarItems[i].Day;

            CalendarDictionary.Add(new DateTime(year, month, day), CalendarItems[i]);
        }

        int iterator = 0;
        foreach (var kvp in CalendarDictionary)
        {
            //Debug.Log("Calendar item " + iterator + " date time is " + kvp.Key + " value at third entry is " + kvp.Value.Entry3);
            iterator++;
        }
    }

    //public CalendarItem CheckForACalendarItem(DateTime dateToCheck)
    //{
    //    if (CalendarDictionary.ContainsKey(dateToCheck))
    //    {
    //        //Debug.Log("We found a matching key. ooh lala!!");
    //        return CalendarDictionary[dateToCheck];
    //    }

    //    return null;
    //}

    public bool CheckForACalendarItem(DateTime dateToCheck,
                                      out CalendarItem item)
    {
        if (CalendarDictionary.ContainsKey(dateToCheck))
        {
            //Debug.Log("We found a matching key. ooh lala!!");
            item = CalendarDictionary[dateToCheck];
            return true;
        }

        item = new CalendarItem(1, 1, 1, new List<string> { ""});
        return false;
    }

    public void SaveIndividualDay(DateTime dateTime,
                                  List<string> entries)
    {
        CalendarItem newCalendarItem = new CalendarItem(dateTime.Year,
                                                        dateTime.Month,
                                                        dateTime.Day,
                                                        entries);

        if (CalendarDictionary.ContainsKey(dateTime))
        {
            CalendarDictionary[dateTime] = newCalendarItem;
        }

        else
        {
            CalendarDictionary.Add(dateTime, newCalendarItem);
        }
    }

    public void SaveIndividualDay(DateTime dateTime,
                                  string entry1,
                                  string entry2,
                                  string entry3,
                                  string entry4)
    {
        //Debug.LogWarning("Saving individual day");

        CalendarItem newCalendarItem = new CalendarItem(dateTime.Year,
                                                        dateTime.Month,
                                                        dateTime.Day,
                                                        entry1,
                                                        entry2,
                                                        entry3,
                                                        entry4);

        if (CalendarDictionary.ContainsKey(dateTime))
        {
            CalendarDictionary[dateTime] = newCalendarItem;
        }

        else
        {
            CalendarDictionary.Add(dateTime, newCalendarItem);
        }
    }

    public void RemoveIndividualDay(DateTime dateTime)
    {
        if (CalendarDictionary.ContainsKey(dateTime))
        {
            CalendarDictionary.Remove(dateTime);
        }
    }

    // For saving purposes. This awaits for the save system to be implemented. In the cloud or otherwise
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
