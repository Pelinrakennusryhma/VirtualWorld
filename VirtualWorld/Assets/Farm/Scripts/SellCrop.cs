using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Characters;

public class SellCrop : MonoBehaviour
{
    private FarmGameSystem gameSystem;
    private CropInventory cropInventory;
    private ProductInventory productInventory;
    public Plant plant;
    //public string product;    
    public Item ProductItem;
    public double productValue;



    private Transform cropInventoryTransform;
    private Transform productInventoryTransform;

    public void Sell()
    {
        if(plant != null)
        {
            CropSell();
        }
        else if(ProductItem != null)
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

        double amountOfMoneyToGet = plant.value * cropInventory.ownedCrops[plant];
        InventoryHymisImplementation.Instance.AddMoneyFromSubscene(amountOfMoneyToGet);

        Object prefab = Resources.Load("Prefabs/FarmFloatingText");
        GameObject newItem = Instantiate(prefab, cropInventoryTransform) as GameObject;
        newItem.GetComponentInChildren<TextMeshProUGUI>().text = (plant.value * cropInventory.ownedCrops[plant]).ToString("C", gameSystem.culture);

        //Debug.LogWarning("About to sell crop " + plant.species + " amount of " + cropInventory.ownedCrops[plant]);
        
        cropInventory.ownedCrops[plant] = 0;


        //InventoryHymisImplementation.Instance.ModifyCropAmount(plant.type, -999999999);
        InventoryHymisImplementation.Instance.ModifySubsceneItemAmount(plant.inventoryItem, -999999999);

        cropInventory.UpdateInventory();

        Destroy(gameObject);
    }

    private void ProductSell()
    {
        productInventoryTransform = GameObject.Find("ProductInventory").transform;
        gameSystem = GameObject.Find("Canvas").GetComponent<FarmGameSystem>();
        productInventory = GameObject.Find("ProductInventory").GetComponent<ProductInventory>();


        gameSystem.AddMoneyToPlayer(productValue * productInventory.ownedProducts[ProductItem]);

        double amountOfMoneyToGet = productValue * productInventory.ownedProducts[ProductItem];
        InventoryHymisImplementation.Instance.AddMoneyFromSubscene(amountOfMoneyToGet);

        Object prefab = Resources.Load("Prefabs/FarmFloatingText");
        GameObject newItem = Instantiate(prefab, productInventoryTransform) as GameObject;
        newItem.GetComponentInChildren<TextMeshProUGUI>().text = (productValue * productInventory.ownedProducts[ProductItem]).ToString("C", gameSystem.culture);
        productInventory.ownedProducts[ProductItem] = 0;

        //InventoryHymisImplementation.Instance.ModifyFarmProductAmount(ProductItem, -999999999);
        InventoryHymisImplementation.Instance.ModifySubsceneItemAmount(ProductItem, -999999999);


        productInventory.UpdateInventory();


        Destroy(gameObject);
    }
}
