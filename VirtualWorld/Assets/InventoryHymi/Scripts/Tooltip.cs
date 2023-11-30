using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Hymi;
using Items;

public class Tooltip : MonoBehaviour
{
    private TextMeshProUGUI tooltipText;
    [SerializeField] RectTransform rectTransform;
    void Start()
    {
        tooltipText = GetComponentInChildren<TextMeshProUGUI>();
        gameObject.SetActive(false);
    }

    //Siirtää tooltipin osoittimen luokse, ja siirtää pivotita riippuen missä osassa ruutua tooltip sijaitsee.
    private void LateUpdate()
    {
        transform.position = Input.mousePosition;
        if(Input.mousePosition.y > Screen.height / 2)
        {
            rectTransform.pivot = new Vector2(rectTransform.pivot.x, 1);
        }
        else
        {
            rectTransform.pivot = new Vector2(rectTransform.pivot.x, 0);
        }
        if (Input.mousePosition.x > Screen.width / 2)
        {
            rectTransform.pivot = new Vector2(1, rectTransform.pivot.y);
        }
        else
        {
            rectTransform.pivot = new Vector2(0, rectTransform.pivot.y);
        }
    }

    public void SetTooltip(InventoryItem invItem) 
    {
        Item item = invItem.item;

        string tooltip = string.Format("<b>{0}</b>\n{1}\n\n<b>{2} C</b>",
            item.DisplayName, item.Description, item.Value);
        tooltipText.text = tooltip;
        gameObject.SetActive(true);
    }

    public void Clear()
    {
        gameObject.SetActive(false);
    }
}
