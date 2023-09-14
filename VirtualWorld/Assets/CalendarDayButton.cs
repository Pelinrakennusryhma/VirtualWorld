using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
using TMPro;
using UnityEngine.UI;

public class CalendarDayButton : MonoBehaviour
{
    public int Week; // From 1 to 6
    public int Day; // 1 is monday, 2 is tuesday, 7 is sunday

    private CalendarWindowChanger changer;

    private DateTime dateTime;

    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private TextMeshProUGUI exclamationMarkUGUI;

    private Image image;

    public bool IsInteractable;

    public void Init(CalendarWindowChanger calendarWindowChanger)
    {
        changer = calendarWindowChanger;
        //textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
        image = GetComponent<Image>();
    }

    public void SetDateTime(DateTime date,
                            bool grayScale,
                            bool setExclamationMarkActive)
    {
        dateTime = date;
        textMeshProUGUI.text = date.Day.ToString();



        if (!grayScale)
        {
            image.color = Color.white;
            IsInteractable = true;
        }

        else
        {
            image.color = Color.gray;
            IsInteractable = false;
        }        
        
        if (dateTime.Day == DateTime.Now.Day
            && dateTime.Month == DateTime.Now.Month
            && dateTime.Year == DateTime.Now.Year)
        {
            image.color = new Color(0, 0.75f, 0, 1);
        }

        exclamationMarkUGUI.gameObject.SetActive(setExclamationMarkActive);
    }

    public void OnClick()
    {
        //Debug.Log("We clicked calendar day button " + Time.time);
        
        if (IsInteractable) 
        {
            changer.OnCalendarButtonPressed(dateTime);
        }

        else
        {
            //Debug.Log("Tried to press a grayed out button");
        }
    }
}
