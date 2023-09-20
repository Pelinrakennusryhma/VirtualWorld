using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ItemScript : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private UnityEngine.UI.Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemAmount;
    [SerializeField] private TextMeshProUGUI itemValue;
    private GameObject contextMenu;
    public int currentItemAmount = 0; 
    public Item item;
    private ContextMenuInventory contextMenuScript;
    public Tooltip tooltip;

    void Awake()
    {
        contextMenu = GameObject.Find("ContextMenu");
        contextMenuScript = contextMenu.GetComponent<ContextMenuInventory>();
    }

    //Päivittää inventoryssa näkyvän määrän
    public void UpdateAmount()
    {
        itemAmount.text = currentItemAmount.ToString();
    }

    //Lisää nykyiseen määrään 'amount'. Päivittää määrän.
    public void AddItem(int amount)
    {
        currentItemAmount += amount;
        UpdateAmount();
    }

    //Poistaa nykyisestä määrästä 'amount'. Päivittää määrän.
    public void RemoveItem(int amount)
    {
        currentItemAmount -= amount;
        UpdateAmount();
    }

    //Avaa context menun painettaessa m2. Painettaessa m1 piilottaa context menun ja tuhoaa info paneelit.
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            contextMenuScript.ShowOptions(item.type);
            contextMenuScript.SetPositionToMouse();
            contextMenuScript.itemID = item.id;
        }
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            contextMenuScript.HideMenu();
        }
    }

    //Aasettaa tiedot Inventory-skriptistä haettujen tietojen mukaan
    public void InitializeItem()
    {
        item = GameObject.Find("Inventory").GetComponent<InventoryHymisImplementation>().itemToAdd;
        itemImage.sprite = Resources.Load<Sprite>("Sprites/" + item.name);
        itemName.text = item.name;
        if (item.stackable)
        {
            itemAmount.gameObject.SetActive(true);
            itemAmount.text = currentItemAmount.ToString();
        }
    }

    //Näyttää tooltipin
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(this.itemAmount != null)
        {
            tooltip.SetTooltip(this.item);
        }
    }
    //Piilottaa tooltipin
    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.gameObject.SetActive(false);
    }

}
