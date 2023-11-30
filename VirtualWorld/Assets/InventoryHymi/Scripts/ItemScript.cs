using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Hymi;
using Items;
public class ItemScript : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private UnityEngine.UI.Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemAmount;
    [SerializeField] private TextMeshProUGUI itemValue;
    private GameObject contextMenu;
    public InventoryItem invItem;
    private ContextMenuInventory contextMenuScript;
    public Tooltip tooltip;

    void Awake()
    {
        //contextMenu = GameObject.Find("ContextMenu");
        //contextMenuScript = contextMenu.GetComponent<ContextMenuInventory>();
    }

    public void Init(InventoryItem invItem, ContextMenuInventory contextMenu, Tooltip tooltip)
    {
        contextMenuScript = contextMenu;
        this.contextMenu = contextMenu.gameObject;
        this.invItem = invItem;
        this.tooltip = tooltip;

        itemImage.sprite = invItem.item.Icon;
        itemName.text = invItem.item.DisplayName;
        if (invItem.amount > 1)
        {
            itemAmount.gameObject.SetActive(true);
            itemAmount.text = invItem.amount.ToString();
        }
    }

    //Avaa context menun painettaessa m2. Painettaessa m1 piilottaa context menun ja tuhoaa info paneelit.
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            contextMenuScript.ShowOptions(invItem);
            contextMenuScript.SetPositionToMouse();
        }
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            contextMenuScript.HideMenu();
        }
    }

    //Näyttää tooltipin
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(this.itemAmount != null)
        {
            tooltip.SetTooltip(invItem);
        }
    }
    //Piilottaa tooltipin
    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.Clear();
    }

}
