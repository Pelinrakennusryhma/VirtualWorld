using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant
{
    public enum PlantType
    {
        None = 0,
        Wheat = 1,
        Barley = 2,
        Corn = 3,
        Soybean = 4,
        Potato = 5,
        Carrot = 6,
        Lettuce = 7
    }

    public string species;
    public float lifespan;
    public double value;
    public PlantType type;
    public Item inventoryItem;

    public Plant(PlantType type, float lifespan)
    {
        string speciesString = "";
        double valueAtDatabase = 0;
        int id = -1;

        switch (type)
        {
            case PlantType.None:
                Debug.LogError("Tried to create a plant of PlantType of None");
                break;
            case PlantType.Wheat:
                id = 1101;
                break;
            case PlantType.Barley:
                id = 1102;
                break;
            case PlantType.Corn:
                id = 1103;
                break;
            case PlantType.Soybean:
                id = 1104;
                break;
            case PlantType.Potato:
                id = 1105;
                break;
            case PlantType.Carrot:
                id = 1106;
                break;
            case PlantType.Lettuce:
                id = 1107;
                break;
            default:
                Debug.LogError("Tried to create a plant, but the switch fell to default case");
                break;
        }

        if (id > 0)
        {
            this.inventoryItem = InventoryHymisImplementation.Instance.itemDatabase.GetItem(id);
            
            if (inventoryItem == null)
            {
                Debug.LogError("Inventory item was null at plant creation. This is not good.");
            }

            speciesString = inventoryItem.name;
            valueAtDatabase = inventoryItem.stats["Value"];
        }

        else
        {
            Debug.LogError("Fail with plant id. Id is " + id);
        }

        this.species = speciesString;
        this.lifespan = lifespan;
        this.value = valueAtDatabase;
        this.type = type;


        //this.species = species;
        //this.lifespan = lifespan;
        //this.value = value;
        //this.type = type;
    }
}
