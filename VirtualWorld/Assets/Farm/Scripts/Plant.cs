using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant
{
    public string species;
    public float lifespan;
    public double value;

    public Plant(string species, float lifespan, double value)
    {
        this.species = species;
        this.lifespan = lifespan;
        this.value = value;
    }
}
