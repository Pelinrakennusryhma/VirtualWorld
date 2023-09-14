using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseCalendarButton : MonoBehaviour
{
    public CalendarWindowChanger changer;

    public void CloseCalendarItem()
    {
        changer.OnCalendarCloseButtonPressed();
        //Debug.LogWarning("Should close calendar item");
    }
}
