using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Hymi
{
public class ContextMenuInventory : MonoBehaviour
{
    [SerializeField] private GameObject buttonUse;
    [SerializeField] private GameObject buttonEquip;
    [SerializeField] private GameObject buttonUnequip;
    [SerializeField] private GameObject buttonSell;
    [SerializeField] private GameObject buttonDiscard;
    public int itemID;
    public InventoryHymisImplementation inventory;
    public ItemDatabase itemDatabase;
    public bool shopping;
    private void Update()
    {
        //Sulkee context menun painettaessa mistä tahansa muualta kuin context menusta tai sen vaihtoehdoista.
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            if (!ClickingSelfOrChild())
            {
                HideMenu();
            }
        }
    }

    //Piilottaa context menusta kaikki vaihtoehdot ja sitten näyttää kaikki itemin tyyppiin liittyvät vaihtoehdot. Näyttää 'Sell' jos pelaaja on kaupassa.
    public void ShowOptions(string type)
    {
        HideAll();
        if (type == "Food" || type == "Drink")
        {
            ShowUse();
            ShowDiscard();
        }
        else if (type == "Key")
        {
            ShowDiscard();
        }
        else if(type == "Ticket")
        {
            ShowDiscard();
        }
        else if (type == "Token")
        {
            ShowDiscard();
        }
        else if (type == "Guidebook")
        {
            ShowUse();
            ShowDiscard();
        }
        else if (type == "Tool")
        {
            ShowDiscard();
        }
        else if (type == "Equipment")
        {
            ShowEquip();
            ShowDiscard();
        }

        if (shopping == true)
        {
            ShowSell();
        }
    }

    //Tuo context menun hiiren luokse
    public void SetPositionToMouse()
    {
        gameObject.transform.position = Input.mousePosition;
    }

    //Piilottaa context menun pois näkyvistä
    public void HideMenu()
    {
        gameObject.transform.position = new Vector3(-1000, 1000, 0);
    }

    //Context menun 'Discard' vaihtoehto
    public void DiscardItem()
    {        
        Debug.Log("Discard pressed");
        inventory.RemoveItem(itemID, 999999999);
        HideMenu();

    }

    //Context menun 'Sell' vaihtoehto
    public void SellItem()
    {
        ItemScript itemScript;
        Hymi.Item item;
        item = inventory.CheckForItem(itemID);
        itemScript = GameObject.Find("Inventory/Scroll/View/Layout/" + itemID).GetComponent<ItemScript>();
        //canvasScript.money += item.value * itemScript.currentItemAmount;
        inventory.RemoveItem(itemID, itemScript.currentItemAmount);
        HideMenu();
    }
    //Context menun 'Equip' vaihtoehto
    public void EquipItem()
    {
        throw new NotImplementedException();
    }
    //Context menun 'Unequip' vaihtoehto
    public void UnequipItem()
    {
        throw new NotImplementedException();
    }

    //Context menun 'Use' vaihtoehto
    public void UseItem()
    {
        Debug.Log("Pressed use item");
        Hymi.Item item;
        item = inventory.CheckForItem(itemID);

        if (item.type == "Food")
        {
            double hungerRecovery = item.stats["Hunger"];
            throw new NotImplementedException();
        }
        else if (item.type == "Drink")
        {
            double thirstRecovery = item.stats["Thirst"];
            throw new NotImplementedException();
        }
    }

    //Tuo näkyville tai piilottaa näkyvistä eri nappeja context menusta
    public void ShowUse()
    {
        buttonUse.SetActive(true);
    }
    public void HideUse()
    {
        buttonUse.SetActive(false);
    }
    public void ShowEquip()
    {
        buttonEquip.SetActive(true);
    }
    public void HideEquip()
    {
        buttonEquip.SetActive(false);
    }
    public void ShowUnequip()
    {
        buttonUnequip.SetActive(true);
    }
    public void HideUnequip()
    {
        buttonUnequip.SetActive(false);
    }
    public void ShowSell()
    {
        buttonSell.SetActive(true);
    }
    public void HideSell()
    {
        buttonSell.SetActive(false);
    }
    public void ShowDiscard()
    {
        buttonDiscard.SetActive(true);
    }
    public void HideDiscard()
    {
        buttonDiscard.SetActive(false);
    }
    //Näyttää kaikki napit
    public void ShowAll()
    {
        ShowUse();
        ShowEquip();
        ShowUnequip();
        ShowSell();
        ShowDiscard();
    }
    //Piilottaa kaikki napit
    public void HideAll()
    {
        HideUse();
        HideEquip();
        HideUnequip();
        HideSell();
        HideDiscard();
    }
    private bool ClickingSelfOrChild()
    {
        RectTransform[] rectTransforms = GetComponentsInChildren<RectTransform>();
        foreach (RectTransform rectTransform in rectTransforms)
        {
            if (EventSystem.current.currentSelectedGameObject == rectTransform.gameObject)
            {
                return true;
            };
        }
        return false;
    }

}
}