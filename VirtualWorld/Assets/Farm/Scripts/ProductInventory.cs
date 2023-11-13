using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProductInventory : MonoBehaviour
{
    public enum ProductType
    {
        None = 0,
        Milk = 1,
        Egg = 2,
        ChickenMeat = 3,
        Beef = 4

    }

    [SerializeField] private GameObject layout;
    public Dictionary<Item, int> ownedProducts = new Dictionary<Item, int>();
    //private Dictionary<Item, double> productPrices;
    private List<Item> allItems;

    [SerializeField] private List<GameObject> InstantiatedProductItems = new List<GameObject>();

    void Start()
    {
        allItems = new List<Item>();
        allItems.Add(InventoryHymisImplementation.Instance.itemDatabase.GetItem(1110));
        allItems.Add(InventoryHymisImplementation.Instance.itemDatabase.GetItem(1111));
        allItems.Add(InventoryHymisImplementation.Instance.itemDatabase.GetItem(1108));
        allItems.Add(InventoryHymisImplementation.Instance.itemDatabase.GetItem(1109));

        foreach (Item item in allItems)
        {
            int ownedAmount = InventoryHymisImplementation.Instance.GetItemAmount(item.id);
            ownedProducts.Add(item, ownedAmount);
        }
        //initializeProductPrices();

        //foreach (Item product in productPrices.Keys)
        //{
        //    int ownedAmount = InventoryHymisImplementation.Instance.GetItemAmount(product.id);
        //    ownedProducts.Add(product, ownedAmount);
        //}



        UpdateInventory();
    }

    public void UpdateInventory()
    {

        // NOTE: can't just destroy like this, because it will get rid of all the owned crops too
        ////Tyhjentää ensin CropInventoryn
        //foreach (var go in GameObject.FindGameObjectsWithTag("Cropitem"))
        //{
        //    Destroy(go);
        //}

        for (int i = 0; i < InstantiatedProductItems.Count; i++)
        {
            Destroy(InstantiatedProductItems[i]);
        }

        InstantiatedProductItems.Clear();

        //Lisää kaikki ownedCropsissa olevat joita pelaajalla on ainakin yksi.
        foreach (var product in ownedProducts)
        {
            if (product.Value != 0)
            {
                Object prefab = Resources.Load("Prefabs/CropItem");
                GameObject newItem = Instantiate(prefab, layout.transform) as GameObject;
                newItem.name = product.Key.name;
                newItem.GetComponentInChildren<TextMeshProUGUI>().text = product.Key.name + " x" + product.Value;

                SellCrop sellComponent = newItem.GetComponent<SellCrop>();

                //sellComponent.product = product.Key.name;
                //sellComponent.productValue = productPrices[product.Key];
                sellComponent.productValue = product.Key.stats["Value"];
                sellComponent.ProductItem = product.Key;

                InstantiatedProductItems.Add(newItem);

                Debug.Log("Adding a product " + product.Key.ToString() + " new item name is " + newItem.name);
            }
        }
    }
    //private void initializeProductPrices()
    //{

    //    productPrices = new Dictionary<Item, double>
    //    {
    //        { InventoryHymisImplementation.Instance.itemDatabase.GetItem(1110), InventoryHymisImplementation.Instance.itemDatabase.GetItem(1110).stats["Value"]},
    //        { InventoryHymisImplementation.Instance.itemDatabase.GetItem(1111), InventoryHymisImplementation.Instance.itemDatabase.GetItem(1111).stats["Value"]},
    //        { InventoryHymisImplementation.Instance.itemDatabase.GetItem(1108), InventoryHymisImplementation.Instance.itemDatabase.GetItem(1108).stats["Value"]},
    //        { InventoryHymisImplementation.Instance.itemDatabase.GetItem(1109), InventoryHymisImplementation.Instance.itemDatabase.GetItem(1109).stats["Value"]},
    //    };

    //    //productPrices = new Dictionary<string, double>
    //    //{
    //    //    { "Milk", 2.0f },
    //    //    { "Egg", 1.0f },
    //    //    { "Chicken Meat", 8.0f },
    //    //    { "Beef", 10.0f },
    //    //};


    //}

    public void ModifyProductAmount(ProductType product, int amountToModify)
    {
        Item item = null;

        switch (product)
        {
            case ProductType.None:
                break;
            case ProductType.Milk:
                item = InventoryHymisImplementation.Instance.itemDatabase.GetItem(1110);
                break;
            case ProductType.Egg:
                item = InventoryHymisImplementation.Instance.itemDatabase.GetItem(1111);
                break;
            case ProductType.ChickenMeat:
                item = InventoryHymisImplementation.Instance.itemDatabase.GetItem(1108);
                break;
            case ProductType.Beef:
                item = InventoryHymisImplementation.Instance.itemDatabase.GetItem(1109);
                break;
            default:
                break;
        }

        if (item != null)
        {
            ownedProducts[item] += amountToModify;
            InventoryHymisImplementation.Instance.ModifySubsceneItemAmount(item, amountToModify);
        }

        else
        {
            Debug.LogError("Null item. Don't do anything");
        }
    }
}