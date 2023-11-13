using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using BackendConnection;
using Characters;

public class InventoryHymisImplementation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerCash;
    [SerializeField] private TextMeshProUGUI playerBank;
    [SerializeField] private TextMeshProUGUI playerDebt;
    [SerializeField] private TextMeshProUGUI itemCount;
    [SerializeField] private GameObject layout;
    public GameSystem gameSystem;
    public Tooltip tooltip;
    public List<Item> playerItems = new List<Item>(); //Pelaajan itemit. Ei laske kuinka monta stackattavaa itemiä on. Määrä löytyy ItemScriptistä.
    public ItemDatabase itemDatabase;
    public Item itemToAdd;
    private int maxItemCount = 6000; //Kuinka monta itemiä pelaajalla voi olla kerralla.

    public static InventoryHymisImplementation Instance;

    public GameObject InventoryParent;

    public GameObject ViewWithiAViewObjectParent;

    public CharacterData CharacterData;

    public Dictionary<Item, int> ItemAmounts = new Dictionary<Item, int>();



    public void Init()
    {
        Instance = this;

        PlayerEvents.Instance.EventCharacterDataSet.AddListener(OnCharacterDataSet);
        PlayerEvents.Instance.EventMoneyAmountChanged.AddListener(OnMoneyAmountChanged);
        PlayerEvents.Instance.EventInventoryAmountChanged.AddListener(OnInventoryItemAmountChanged);
        //UpdateInventory();
    }

    #region ToBeRemoved

    //public void ModifyCropAmount(Plant.PlantType typeOfCrop, int amount)
    //{
    //    int id = -1;

    //    switch (typeOfCrop)
    //    {
    //        case Plant.PlantType.None:
    //            Debug.LogWarning("Tried to add a crop of type none");
    //            break;
    //        case Plant.PlantType.Wheat:
    //            id = 1101;
    //            //AddItem(1101, amount);
    //            break;
    //        case Plant.PlantType.Barley:
    //            id = 1102;
    //            //AddItem(1102, amount);
    //            break;
    //        case Plant.PlantType.Corn:
    //            id = 1103;
    //            //AddItem(1103, amount);
    //            break;
    //        case Plant.PlantType.Soybean:
    //            id = 1104;
    //            //AddItem(1104, amount);
    //            break;
    //        case Plant.PlantType.Potato:
    //            id = 1105;
    //            //AddItem(1105, amount);
    //            break;
    //        case Plant.PlantType.Carrot:
    //            id = 1106;
    //            //AddItem(1106, amount);
    //            break;
    //        case Plant.PlantType.Lettuce:
    //            id = 1107;
    //            //AddItem(1107, amount);
    //            break;
    //        default:
    //            Debug.LogWarning("Tried to add a crop, but the switch fell into the default case");
    //            break;
    //    }

    //    if (id > 0) 
    //    {
    //        Item item = itemDatabase.GetItem(id);

    //        if (amount > 0)
    //        {
    //            CharacterManager.Instance.ModifyItem(id.ToString(), ModifyItemDataOperation.ADD, amount, item.name);

    //            Debug.Log("Adding to crops. Item " + item.name);
    //        }

    //        else if (amount < 0)
    //        {
    //            int amountToRemove = 0;
    //            int currentItemAmount = GetItemAmount(id);

    //            amountToRemove = Mathf.Clamp(Mathf.Abs(amount), 0, currentItemAmount);

    //            CharacterManager.Instance.ModifyItem(id.ToString(), ModifyItemDataOperation.REMOVE, Mathf.Abs(amountToRemove), item.name);
    //            Debug.Log("Removing from crops. Item " + item.name + " amount to remove is " + amountToRemove + " amount " + amount + " currentitem amount " + currentItemAmount);
    //        }
    //    }
    //}

    //public void ModifyFarmProductAmount(Item item, int amount)
    //{
    //    //int id = 0;

    //    //switch (typeOfProduct)
    //    //{
    //    //    case ProductInventory.ProductType.None:
    //    //        Debug.LogWarning("Tried to add a product of type none");
    //    //        break;
    //    //    case ProductInventory.ProductType.Milk:
    //    //        //AddItem(1110, amount);
    //    //        id = 1110;
    //    //        break;
    //    //    case ProductInventory.ProductType.Egg:
    //    //        //AddItem(1111, amount);
    //    //        id = 1111;
    //    //        break;
    //    //    case ProductInventory.ProductType.ChickenMeat:
    //    //        //AddItem(1108, amount);
    //    //        id = 1108;
    //    //        break;
    //    //    case ProductInventory.ProductType.Beef:
    //    //        //AddItem(1109, amount);
    //    //        id = 1109;
    //    //        break;
    //    //    default:
    //    //        Debug.LogWarning("Tried to add a product, but the switch fell into the default case");
    //    //        break;
    //    //}

    //    if (item != null && item.id > 0)
    //    {
    //        if (amount > 0) 
    //        {
    //            CharacterManager.Instance.ModifyItem(item.id.ToString(), ModifyItemDataOperation.ADD, amount, item.name);
    //        }

    //        else if (amount < 0)
    //        {
    //            int amountToRemove = 0;
    //            int currentItemAmount = GetItemAmount(item.id);

    //            amountToRemove = Mathf.Clamp(Mathf.Abs(amount), 0, currentItemAmount);

    //            CharacterManager.Instance.ModifyItem(item.id.ToString(), ModifyItemDataOperation.REMOVE, Mathf.Abs(amountToRemove), item.name);
    //            Debug.Log("Removing from products. Item " + item.name + " amount to remove is " + amountToRemove + " amount " + amount + " currentitem amount " + currentItemAmount);


    //        }
    //    }
    //}

    //public int GetInventoryAmountOfCrop(Plant.PlantType type)
    //{
    //    int id = -1;
    //    int amount = 0;

    //    switch (type)
    //    {
    //        case Plant.PlantType.None:
    //            Debug.LogWarning("Tried to get a crop of type none");
    //            break;
    //        case Plant.PlantType.Wheat:
    //            id = 1101;
    //            //AddItem(1101, amount);
    //            break;
    //        case Plant.PlantType.Barley:
    //            id = 1102;
    //            //AddItem(1102, amount);
    //            break;
    //        case Plant.PlantType.Corn:
    //            id = 1103;
    //            //AddItem(1103, amount);
    //            break;
    //        case Plant.PlantType.Soybean:
    //            id = 1104;
    //            //AddItem(1104, amount);
    //            break;
    //        case Plant.PlantType.Potato:
    //            id = 1105;
    //            //AddItem(1105, amount);
    //            break;
    //        case Plant.PlantType.Carrot:
    //            id = 1106;
    //            //AddItem(1106, amount);
    //            break;
    //        case Plant.PlantType.Lettuce:
    //            id = 1107;
    //            //AddItem(1107, amount);
    //            break;
    //        default:
    //            Debug.LogWarning("Tried to get a crop, but the switch fell into the default case");
    //            break;
    //    }

    //    Item item = itemDatabase.GetItem(id);

    //    if (ItemAmounts.ContainsKey(item))
    //    {
    //        amount = ItemAmounts[item];
    //    }

    //    Debug.Log("Getting amount of crop for plant " + type.ToString());

    //    return amount;
    //}

    //public int GetInventoryAmountOfFarmProduct(ProductInventory.ProductType typeOfProduct)
    //{
    //    int id = 0;
    //    int amount = 0;

    //    switch (typeOfProduct)
    //    {
    //        case ProductInventory.ProductType.None:
    //            Debug.LogWarning("Tried to add a product of type none");
    //            break;
    //        case ProductInventory.ProductType.Milk:
    //            //AddItem(1110, amount);
    //            id = 1110;
    //            break;
    //        case ProductInventory.ProductType.Egg:
    //            //AddItem(1111, amount);
    //            id = 1111;
    //            break;
    //        case ProductInventory.ProductType.ChickenMeat:
    //            //AddItem(1108, amount);
    //            id = 1108;
    //            break;
    //        case ProductInventory.ProductType.Beef:
    //            //AddItem(1109, amount);
    //            id = 1109;
    //            break;
    //        default:
    //            Debug.LogWarning("Tried to add a product, but the switch fell into the default case");
    //            break;
    //    }

    //    Item item = itemDatabase.GetItem(id);

    //    if (ItemAmounts.ContainsKey(item))
    //    {
    //        amount = ItemAmounts[item];
    //    }

    //    return amount;
    //}

    #endregion


    public void AddMoneyFromSubscene(double amount)
    {
        //Debug.LogWarning("Adding money as an int. Cents will be missed. Amount to add is " + amount);
        CharacterManager.Instance.AddMoney((int) amount);
    }

    public void RemoveMoneyFromSubscene(double amount)
    {
        //Debug.LogWarning("Removing money as an int. Cents will be missed. Amount to remove is " + amount);
        CharacterManager.Instance.RemoveMoney((int) amount);
    }

    public void AddDebtFromSubscene(double amount)
    {
        //Debug.LogWarning("Adding debt as an int. Cents will be missed. Amount to add is " + amount);
        CharacterManager.Instance.AddDebt((int) amount);
    }

    public void RemoveDebtFromSubscene(double amount)
    {
        //Debug.LogWarning("Removing debt as an int. Cents will be missed. Amount to remove is " + amount);
        CharacterManager.Instance.RemoveDebt((int) amount);
    }

    public void AddBankBalanceFromSubscene(double amount)
    {
        //Debug.LogWarning("Adding bank balance as an int. Cents will be missed. Amount to add is " + amount);
        CharacterManager.Instance.AddToBankBalance((int) amount);
    }

    public void RemoveBankBalanceFromSubscene(double amount)
    {
        Debug.LogWarning("Removing bank balance as an int. Cents will be missed. Amount to remove is " + amount);
        CharacterManager.Instance.RemoveFromBankBalance((int) amount);
    }

    public void ModifyStockAmountFromSubscene(Stock stock,
                                              int amount)
    {
        Item itemToModify = stock.inventoryItem;

        if (itemToModify == null)
        {
            Debug.LogError("Trying to modify a null item. Errors are bound to happen.");
        }

        if (amount > 0)
        {
            CharacterManager.Instance.AddStock(itemToModify.id.ToString(), amount, itemToModify.name);
        }

        else if (amount < 0)
        {
            int currentItemAmount = GetItemAmount(itemToModify.id);
            int amountToRemove = Mathf.Clamp(Mathf.Abs(amount), 0, currentItemAmount);

            CharacterManager.Instance.RemoveStock(itemToModify.id.ToString(), Mathf.Abs(amountToRemove), itemToModify.name);
        }
    }

    public void ModifySubsceneItemAmount(Item itemToModify,
                                         int amount)
    {
        if (itemToModify == null)
        {
            Debug.LogError("Passed a null item to modify. Errors are bound to happen.");
        }

        if (amount > 0)
        {
            CharacterManager.Instance.ModifyItem(itemToModify.id.ToString(), ModifyItemDataOperation.ADD, amount, itemToModify.name);
            Debug.Log("Adding an item. Item " + itemToModify.name + " amount " + amount);
        }

        else if (amount < 0)
        {
            int currentItemAmount = GetItemAmount(itemToModify.id);
            int amountToRemove = Mathf.Clamp(Mathf.Abs(amount), 0, currentItemAmount);

            CharacterManager.Instance.ModifyItem(itemToModify.id.ToString(), ModifyItemDataOperation.REMOVE, Mathf.Abs(amountToRemove), itemToModify.name);
            Debug.Log("Removing an item. Item " + itemToModify.name + " amount to remove is " + amountToRemove + " amount " + amount + " currentitem amount " + currentItemAmount);
        }
    }



    public void OnCharacterDataSet(CharacterData data)
    {
        Debug.LogWarning("Inventory knows character data is being set");
        CharacterData = data;
        PopulateInventoryWithCharacterData(data);
    }


    public void GetMoneyThings(out double money,
                               out double debt,
                               out double bank,
                               out double loanedMoney)
    {
        money = gameSystem.playerMoney;
        debt = gameSystem.playerDebt;
        loanedMoney = gameSystem.playerLoanedMoney;
        bank = gameSystem.bankMoney;
    }

    private void PopulateInventoryWithCharacterData(CharacterData data)
    {
        //Debug.Log("Data length is " + data.inventory.items.Count);

        for (int i = 0; i < data.inventory.items.Count; i++)
        {
            //Debug.Log("Inventory item at i " + i + " is " + data.inventory.items[i].id);

            string id = data.inventory.items[i].id;

            if (id.Equals("000"))
            {
                // Add money
                gameSystem.playerMoney = data.inventory.items[i].amount;
                //Debug.Log("Added money to inventory system. Amount is " + gameSystem.playerMoney);
            }

            //else if (id.Equals("003"))
            //{
            //    Debug.LogWarning("This id " + id + " was used for testing purposes and should not be used. Rather it should be removed from database");
            //}

            else if (id.Equals("10000"))
            {
                // Add debt 
                gameSystem.playerDebt = data.inventory.items[i].amount;
                //Debug.Log("Added debt to inventory system. Amount is " + gameSystem.playerDebt);
            }

            else if (id.Equals("10001"))
            {
                // Add bank balance
                gameSystem.bankMoney = data.inventory.items[i].amount;
                //Debug.Log("Added bank balance to inventory system. Amount is " + gameSystem.playerLoanedMoney);
            }

            else
            {

                bool couldParse = int.TryParse(data.inventory.items[i].id, out int idParsed);

                if (couldParse) 
                {
                    //Debug.Log("Found an item with id. Could parse the string to int. Parsed int is " + idParsed);

                    if (data.inventory.items[i].amount > 0)
                    {
                        SetItemAmount(idParsed, data.inventory.items[i].amount);
                    }
                }

                else
                {
                    Debug.Log("Found an item with id. Could NOT parse the string to int " + id);
                }
            }
        }
    }

    public void OnMoneyAmountChanged(InventoryItem item)
    {
        gameSystem.playerMoney = item.amount;
    }

    public void OnDebtAmountChanged(double amount)
    {
        gameSystem.playerDebt = amount;
    }

    public void OnBankBalanceChanged(double amount)
    {
        gameSystem.bankMoney = amount;
        Debug.LogWarning("Bank balance changed with amount " + amount + " amount at gameystem bank is " + gameSystem.bankMoney);
    }

    public void OnInventoryItemAmountChanged(InventoryItem item)
    {
        bool couldParse = int.TryParse(item.id, out int parsedId);

        if (couldParse) 
        {
            if (parsedId == 10001)
            {
                OnBankBalanceChanged(item.amount);
            }

            else if (parsedId == 10000)
            {
                OnDebtAmountChanged(item.amount);
            }

            else
            {
                SetItemAmount(parsedId, item.amount);
            }
        }

        else
        {
            Debug.LogError("Could not parse id and therefore item can not be added. Item id was " + item.id);
        }

        Debug.Log("On inventory amount changed. Item id is " + item.id + " amount is " + item.amount);

    }

    public int GetItemAmount(int itemId)
    {
        Item item = itemDatabase.GetItem(itemId);

        int amount = 0;

        if (ItemAmounts.ContainsKey(item)) 
        {
            amount = ItemAmounts[item];
        }

        //Debug.LogWarning("Getting item amount for " + itemId + " amount is " + amount);

        return amount;
    }

    public void SetItemAmount(int itemId, int amount)
    {
        int oldAmount = 0;
        int difference = 0;

        Item item = itemDatabase.GetItem(itemId);

        if (item == null)
        {
            Debug.LogError("Null item, what is wrong with this?");
        }

        if (ItemAmounts.ContainsKey(item))
        {
            oldAmount = ItemAmounts[item];
        }

        difference = amount - oldAmount;

        if (difference > 0)
        {
            AddItem(itemId, difference);
        }

        else if (difference < 0)
        {
            RemoveItem(itemId, Mathf.Abs(difference));
        }

        else
        {
            //Debug.Log("The new amount and old amounts match. Amount is " + amount);
        }

        //Debug.Log("Setting item amount of item id " + itemId + " amount is " + amount + " old amount was " + oldAmount + " difference is " + difference);
    }

    private void OnEnable()
    {
        UpdateInventory();
    }
    //Tarkistaa onko pelaajalla itemiä
    public Item CheckForItem(int id)
    {
        return playerItems.Find(item => item.id == id);
    }

    public void OnInventoryScreenOpened()
    {

        Debug.LogWarning("Opened inventory screen");
    }

    public void OnInventoryScreenClosed()
    {
        Debug.LogWarning("Closed invenotry screen");
    }
    
    //Antaa pelaajalle itemin. Jos pelaajalla ei ole ennestään sitä, lisää uuden rivin inventoryyn. Jos pelaajalla on jo inventoryssa se ja tavara on stackattava, lisää määrään lisää.
    public void AddItem(int id, int amount)
    {
        //Debug.LogWarning("About to add item " + id + " of amount " + amount);



        // Because of unfortunate desing of the code,
        // the objects have to actually be active for
        // the find and getcomponents to work
        bool wasActiveSelf = gameObject.activeSelf;
        bool wasActiveParent = InventoryParent.gameObject.activeSelf;
        bool wasActiveViewWithinAViewParent = ViewWithiAViewObjectParent.activeSelf;


        gameObject.SetActive(true);
        InventoryParent.gameObject.SetActive(true);
        ViewWithiAViewObjectParent.SetActive(true);

        Item item = CheckForItem(id);
        itemToAdd = itemDatabase.GetItem(id);

        if(itemToAdd != null)
        {
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

                    ItemAmounts.Add(itemToAdd, amount);

                }
                else if (item.stackable) //Jos item on stackattava
                {
                    Debug.Log("About to add a non-null stackable item " + item.name);

                    //GameObject.Find("Inventory/Scroll/View/Layout/" + id).GetComponent<ItemScript>().AddItem(amount);

                    ItemScript itemToUpdate = GameObject.Find("Inventory/Scroll/View/Layout/" + id).GetComponent<ItemScript>();

                    int oldAmount = itemToUpdate.currentItemAmount;

                    itemToUpdate.AddItem(amount);

                    int newAmount = itemToUpdate.currentItemAmount;

                    ItemAmounts[itemToAdd] = newAmount;

                    Debug.Log("Adding item " + itemToUpdate.item.id + " old amount is " + oldAmount + " new amount is " + newAmount);
                }
            }
            UpdateItemCount();

            Debug.Log("Updating item amount of item " + id.ToString() + " with amount " + amount);
        }

        else
        {
            Debug.LogError("Item database returned null item. Aborting adding an item");
        }

        gameObject.SetActive(wasActiveSelf);
        InventoryParent.gameObject.SetActive(wasActiveParent);
        ViewWithiAViewObjectParent.SetActive(wasActiveViewWithinAViewParent);
    }

    //Poistaa pelaajalta itemin. Jos pelaajalla on jo ennestään sitä enemmän kuin poistettava määrä, poistaa määrästä. Jos pelaajalla on saman verran tai vähemmän kuin poistettava määrä, poistaa rivin inventorysta.
    public void RemoveItem(int id, int amount)
    {

        // Because of unfortunate desing of the code,
        // the objects have to actually be active for
        // the find and getcomponents to work
        bool wasActiveSelf = gameObject.activeSelf;
        bool wasActiveParent = InventoryParent.gameObject.activeSelf;
        bool wasActiveViewWithinAViewParent = ViewWithiAViewObjectParent.activeSelf;

        gameObject.SetActive(true);
        InventoryParent.gameObject.SetActive(true);
        ViewWithiAViewObjectParent.SetActive(true);

        //Debug.LogWarning("Removing item");
        Item item = CheckForItem(id);
        if(item != null)
        {
            ItemScript itemScript = GameObject.Find("Inventory/Scroll/View/Layout/" + id.ToString()).GetComponent<ItemScript>();

            if (itemScript.currentItemAmount <= amount)
            {
                playerItems.Remove(item);
                itemScript.currentItemAmount = 0;
                Destroy(GameObject.Find("Inventory/Scroll/View/Layout/" + id.ToString()));

                ItemAmounts.Remove(item);
            }
            else
            {
                itemScript.RemoveItem(amount);
                ItemAmounts[item] -= amount;
            }
        }
        UpdateItemCount();

        gameObject.SetActive(wasActiveSelf);
        InventoryParent.gameObject.SetActive(wasActiveParent);
        ViewWithiAViewObjectParent.SetActive(wasActiveViewWithinAViewParent);
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
        Debug.LogWarning("Adding all items");

        foreach(Item item in itemDatabase.items)
        {
            AddItem(item.id, 1);
        }
    }


}
