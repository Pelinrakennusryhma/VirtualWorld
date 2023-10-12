using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseNewsFeedIndividualNewsViewButton : MonoBehaviour
{
    public NewsFeedViewChanger changer;

    public void CloseNewsFeed()
    {
        changer.OnCloseButtonPressed();
    }
}
