using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlantType = Plant.PlantType;


public class PlantDatabase : MonoBehaviour
{
    public List<Plant> plants = new List<Plant>();

    void Awake()
    {
        BuildDatabase();
    }

    public Plant GetPlant(string species)
    {
        return plants.Find(plant => plant.species == species);
    }

    //species, lifespan, value
    void BuildDatabase()
    {
        plants = new List<Plant> {
            new Plant(PlantType.Wheat, 300.0f),
            new Plant(PlantType.Barley, 300.0f),
            new Plant(PlantType.Corn, 300.0f),
            new Plant(PlantType.Soybean, 300.0f),
            new Plant(PlantType.Potato, 300.0f),
            new Plant(PlantType.Carrot, 300.0f),
            new Plant(PlantType.Lettuce, 300.0f)

        };
    }
}
