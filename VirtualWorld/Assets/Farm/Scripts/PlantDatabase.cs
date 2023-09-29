using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            new Plant("Wheat", 300.0f, 15.0),
            new Plant("Barley", 300.0f, 15.0),
            new Plant("Corn", 300.0f, 15.0),
            new Plant("Soybean", 300.0f, 15.0),
            new Plant("Potato", 300.0f, 15.0),
            new Plant("Carrot", 300.0f, 15.0),
            new Plant("Lettuce", 300.0f, 15.0),

        };
    }
}
