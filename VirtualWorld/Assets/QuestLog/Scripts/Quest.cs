using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public int id;
    public string name;
    public string category;
    public Dictionary<string, int> steps;

    public Quest(int id, string name, string category, Dictionary<string, int> steps)
    {
        this.id = id;
        this.name = name;
        this.category = category;
        this.steps = steps;
    }
}
