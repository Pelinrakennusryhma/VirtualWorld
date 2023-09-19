using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class InventoryHymisImplementation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerCash;
    [SerializeField] private TextMeshProUGUI playerBank;
    [SerializeField] private TextMeshProUGUI playerDebt;
    [SerializeField] private TextMeshProUGUI itemCount;
    [SerializeField] private GameObject layout;
    [SerializeField] private GameSystem gameSystem;
    public Tooltip tooltip;
    public List<Item> playerItems = new List<Item>(); //Pelaajan itemit. Ei laske kuinka monta stackattavaa itemiä on. Määrä löytyy ItemScriptistä.
    public ItemDatabase itemDatabase;
    public Item itemToAdd;
    private int maxItemCount = 60; //Kuinka monta itemiä pelaajalla voi olla kerralla.

    private void OnEnable()
    {
        UpdateInventory();
    }
    //Tarkistaa onko pelaajalla itemiä
    public Item CheckForItem(int id)
    {
        return playerItems.Find(item => item.id == id);
    }
    
    //Antaa pelaajalle itemin. Jos pelaajalla ei ole ennestään sitä, lisää uuden rivin inventoryyn. Jos pelaajalla on jo inventoryssa se ja tavara on stackattava, lisää määrään lisää.
    public void AddItem(int id, int amount)
    {
        Item item = CheckForItem(id);
        itemToAdd = itemDatabase.GetItem(id);

        if (playerItems.Count < maxItemCount) //Jos pelaajalla on tilaa inventoryssa
        {
            if (item == null || !item.stackable) //Jos itemiä ei ole ennestään, tai item ei ole stackattava
            {
                playerItems.Add(itemToAdd);
                Object prefab = Resources.Load("Prefabs/item");
                GameObject newItem = Instantiate(prefab, layout.transform) as GameObject;
                newItem.name = itemToAdd.id.ToString();
                ItemScript itemScript = newItem.GetComponent<ItemScript>();
                itemScript.InitializeItem();
                itemScript.AddItem(amount);
                itemScript.tooltip = tooltip;
            }
            else if (item.stackable) //Jos item on stackattava
            {
                GameObject.Find("Inventory/Scroll/View/Layout/" + id).GetComponent<ItemScript>().AddItem(amount);
            }
        }
        UpdateItemCount();
    }

    //Poistaa pelaajalta itemin. Jos pelaajalla on jo ennestään sitä enemmän kuin poistettava määrä, poistaa määrästä. Jos pelaajalla on saman verran tai vähemmän kuin poistettava määrä, poistaa rivin inventorysta.
    public void RemoveItem(int id, int amount)
    {
        Debug.LogWarning("Removing item");
        Item item = CheckForItem(id);
        if(item != null)
        {
            ItemScript itemScript = GameObject.Find("Inventory/Scroll/View/Layout/" + id.ToString()).GetComponent<ItemScript>();
            if (itemScript.currentItemAmount <= amount)
            {
                playerItems.Remove(item);
                itemScript.currentItemAmount = 0;
                Destroy(GameObject.Find("Inventory/Scroll/View/Layout/" + id.ToString()));
            }
            else
            {
                itemScript.RemoveItem(amount);
            }
        }
        UpdateItemCount();
    }
    public void UpdateInventory()
    {
        playerCash.text = gameSystem.playerMoney.ToString("C", gameSystem.culture);
        playerBank.text = gameSystem.bankMoney.ToString("C", gameSystem.culture);
        playerDebt.text = gameSystem.playerDebt.ToString("C", gameSystem.culture);
        UpdateItemCount();
    }
    public void SortByName()
    {
        playerItems.Sort((a, b) => a.name.CompareTo(b.name));
        foreach (Item item in playerItems)
        {
            foreach(ItemScript itemScript in GetComponentsInChildren<ItemScript>())
            {
                if(item == itemScript.item)
                {
                    itemScript.gameObject.transform.SetAsLastSibling();
                }
            }
        }
    }

    public void SortByID()
    {
        List<Transform> children = new List<Transform>();
        children = layout.GetComponentsInChildren<Transform>().Cast<Transform>().ToList();
        children.Sort((Transform a, Transform b) => a.name.CompareTo(b.name));
        foreach(Transform child in children)
        {
            child.SetAsLastSibling();
        }
    }

    private void UpdateItemCount()
    {
        itemCount.text = playerItems.Count() + "/" + maxItemCount;
    }






    //Testausta varten. Poistettava myöhemmin.
    public void TestButtonAdd(int id)
    {
        AddItem(id, 1);
    }
    public void TestButtonRemove(int id)
    {
        RemoveItem(id, 1);
    }
    public void TestAddEverything()
    {
        foreach(Item item in itemDatabase.items)
        {
            AddItem(item.id, 1);
        }
    }
}
