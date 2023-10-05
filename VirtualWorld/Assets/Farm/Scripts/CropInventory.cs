using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CropInventory : MonoBehaviour
{
    [SerializeField] private GameObject layout;
    public PlantDatabase plantDatabase;
    public Dictionary<Plant, int> ownedCrops = new Dictionary<Plant, int>();

    void Start()
    {
        foreach (Plant plant in plantDatabase.plants)
        {
            ownedCrops.Add(plant, 0);
        }
        UpdateInventory();
    }

    public void UpdateInventory()
    {
        //Tyhjentää ensin CropInventoryn
        foreach(var go in GameObject.FindGameObjectsWithTag("Cropitem"))
        {
            Destroy(go);
        }

        //Lisää kaikki ownedCropsissa olevat joita pelaajalla on ainakin yksi.
        foreach(var plant in ownedCrops)
        {
            if(plant.Value != 0)
            {
                Object prefab = Resources.Load("Prefabs/CropItem");
                GameObject newItem = Instantiate(prefab, layout.transform) as GameObject;
                newItem.name = plant.Key.species;
                newItem.GetComponentInChildren<TextMeshProUGUI>().text = (plant.Key.species + " x" + plant.Value);
                newItem.GetComponent<SellCrop>().plant = plant.Key;
            }
        }
    }
}
