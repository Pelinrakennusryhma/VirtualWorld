using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient
{
    public string type;
    public string pour;
    public string cut;
    public string fry;
    public string boil;
    public string whisk;
    public string defrost;
    public bool serve;

    public Ingredient(string type, string pour, string cut, string fry, string boil, string whisk, string defrost, bool serve)
    {
        this.type = type;
        this.pour = pour;
        this.cut = cut;
        this.fry = fry;
        this.boil = boil;
        this.whisk = whisk;
        this.defrost = defrost;
        this.serve = serve;
    }
}
