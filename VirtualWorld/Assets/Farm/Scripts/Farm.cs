using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : MonoBehaviour
{
    [SerializeField] private GameObject layout;
    PlantDatabase plantDatabase;
    void Start()
    {
        plantDatabase = gameObject.GetComponent<PlantDatabase>();
    }

    public void AddCrop(string species)
    {
        Object prefab = Resources.Load("Prefabs/Crop");
        GameObject newItem = Instantiate(prefab, layout.transform) as GameObject;
        newItem.GetComponent<Crop>().plant = plantDatabase.GetPlant(species);
        newItem.name = species;
        newItem.GetComponent<Crop>().InitializePlant();
    }

    public void AddAnimal(string species)
    {
        Object prefab = Resources.Load("Prefabs/" + species);
        GameObject newItem = Instantiate(prefab, layout.transform) as GameObject;
        newItem.name = species;
        newItem.GetComponent<Animal>().InitializeAnimal();
    }
}
