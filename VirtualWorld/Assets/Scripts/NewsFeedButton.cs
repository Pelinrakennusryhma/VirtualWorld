using UnityEngine;
using TMPro;

public class NewsFeedButton : MonoBehaviour
{
    // A news item that is saved on SetupButton
    // So we can pass data about it when a button is
    // clicked and we fire an event about that click
    // to anyone that is interested.
    private NewsFeedItem newsFeedItem;

    // Make a delegate...
    public delegate void ClickedNewsItem(int id, NewsFeedItem item);
    // ...so we can fire an event of news item clicked
    public ClickedNewsItem OnNewsItemClicked;

    // Set in the inspector
    // Should be the text component child of a button.
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;

    // Called when the button should be updated with data
    public void SetupButton(NewsFeedItem item)
    {
        newsFeedItem = item;
        textMeshProUGUI.text = item.Header;
    }

    // Set in the inspector Unity event
    // about the button being clicked
    public void OnClick()
    {
        // Check if we have listeners...
        if (OnNewsItemClicked != null)
        {
            // ...if so, fire the event.
            OnNewsItemClicked(newsFeedItem.ID, newsFeedItem);
        }
    }
}
