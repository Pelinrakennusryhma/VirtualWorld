using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SellCrop : MonoBehaviour
{
    private FarmGameSystem gameSystem;
    private CropInventory cropInventory;
    private ProductInventory productInventory;
    public Plant plant;
    public string product;
    public double productValue;
    private Transform cropInventoryTransform;
    private Transform productInventoryTransform;

    public void Sell()
    {
        if(plant != null)
        {
            CropSell();
        }
        else if(product != null)
        {
            ProductSell();
        }
    }
    private void CropSell()
    {
        cropInventoryTransform = GameObject.Find("CropInventory").transform;
        gameSystem = GameObject.Find("Canvas").GetComponent<FarmGameSystem>();
        cropInventory = GameObject.Find("CropInventory").GetComponent<CropInventory>();
        gameSystem.AddMoneyToPlayer(plant.value * cropInventory.ownedCrops[plant]);
        Object prefab = Resources.Load("Prefabs/FarmFloatingText");
        GameObject newItem = Instantiate(prefab, cropInventoryTransform) as GameObject;
        newItem.GetComponentInChildren<TextMeshProUGUI>().text = (plant.value * cropInventory.ownedCrops[plant]).ToString("C", gameSystem.culture);
        cropInventory.ownedCrops[plant] = 0;
        cropInventory.UpdateInventory();


        Destroy(gameObject);
    }

    private void ProductSell()
    {
        productInventoryTransform = GameObject.Find("ProductInventory").transform;
        gameSystem = GameObject.Find("Canvas").GetComponent<FarmGameSystem>();
        productInventory = GameObject.Find("ProductInventory").GetComponent<ProductInventory>();
        gameSystem.AddMoneyToPlayer(productValue * productInventory.ownedProducts[product]);
        Object prefab = Resources.Load("Prefabs/FarmFloatingText");
        GameObject newItem = Instantiate(prefab, productInventoryTransform) as GameObject;
        newItem.GetComponentInChildren<TextMeshProUGUI>().text = (productValue * productInventory.ownedProducts[product]).ToString("C", gameSystem.culture);
        productInventory.ownedProducts[product] = 0;
        productInventory.UpdateInventory();


        Destroy(gameObject);
    }
}
