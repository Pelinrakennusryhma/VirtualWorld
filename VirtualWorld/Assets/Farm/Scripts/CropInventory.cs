using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CropInventory : MonoBehaviour
{
    [SerializeField] private GameObject layout;
    public PlantDatabase plantDatabase;
    public Dictionary<Plant, int> ownedCrops = new Dictionary<Plant, int>();

    [SerializeField] private List<GameObject> InstantiatedCropItems = new List<GameObject>();


    void Start()
    {
        Debug.LogWarning("Crop inventory start called " + Time.time);

        foreach (Plant plant in plantDatabase.plants)
        {
            int ownedAmount = InventoryHymisImplementation.Instance.GetItemAmount(plant.inventoryItem.id);
            ownedCrops.Add(plant, ownedAmount);
            Debug.LogWarning("Adding a plant from database. The amount should be " + ownedAmount + ". Plant is " + plant.type.ToString());
        }
        UpdateInventory();
    }

    public void UpdateInventory()
    {
        // NOTE: can't remove like this, because it will get rid of all the owned products too
        ////Tyhjentää ensin CropInventoryn
        //foreach(var go in GameObject.FindGameObjectsWithTag("Cropitem"))
        //{
        //    Destroy(go);
        //}

        for (int i = 0; i < InstantiatedCropItems.Count; i++)
        {
            Destroy(InstantiatedCropItems[i]);
        }

        InstantiatedCropItems.Clear();

        //Lisää kaikki ownedCropsissa olevat joita pelaajalla on ainakin yksi.
        foreach (var plant in ownedCrops)
        {
            //Debug.Log("Looping through ");

            if(plant.Value != 0)
            {
                Object prefab = Resources.Load("Prefabs/CropItem");
                GameObject newItem = Instantiate(prefab, layout.transform) as GameObject;
                newItem.name = plant.Key.species;
                newItem.GetComponentInChildren<TextMeshProUGUI>().text = (plant.Key.species + " x" + plant.Value);
                newItem.GetComponent<SellCrop>().plant = plant.Key;

                InstantiatedCropItems.Add(newItem);

                //Debug.Log("Adding a crop " + plant.Key.ToString() + " plant type is " + plant.Key.type.ToString());


            }
        }
    }
}
