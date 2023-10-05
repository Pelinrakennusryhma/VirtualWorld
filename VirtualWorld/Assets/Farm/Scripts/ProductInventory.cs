using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProductInventory : MonoBehaviour
{
    [SerializeField] private GameObject layout;
    public Dictionary<string, int> ownedProducts = new Dictionary<string, int>();
    private Dictionary<string, double> productPrices;

    void Start()
    {
        initializeProductPrices();
        foreach (string products in productPrices.Keys)
        {
            ownedProducts.Add(products, 0);
        }
        UpdateInventory();
    }

    public void UpdateInventory()
    {
        //Tyhjentää ensin CropInventoryn
        foreach (var go in GameObject.FindGameObjectsWithTag("Cropitem"))
        {
            Destroy(go);
        }

        //Lisää kaikki ownedCropsissa olevat joita pelaajalla on ainakin yksi.
        foreach (var product in ownedProducts)
        {
            if (product.Value != 0)
            {
                Object prefab = Resources.Load("Prefabs/CropItem");
                GameObject newItem = Instantiate(prefab, layout.transform) as GameObject;
                newItem.name = product.Key;
                newItem.GetComponentInChildren<TextMeshProUGUI>().text = product.Key + " x" + product.Value;
                newItem.GetComponent<SellCrop>().product = product.Key;
                newItem.GetComponent<SellCrop>().productValue = productPrices[product.Key];
            }
        }
    }
    private void initializeProductPrices()
    {
        productPrices = new Dictionary<string, double>
        {
            { "Milk", 2.0f },
            { "Egg", 1.0f },
            { "Chicken Meat", 8.0f },
            { "Beef", 10.0f },
        };
    }
}
