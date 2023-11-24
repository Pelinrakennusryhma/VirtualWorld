using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hymi
{
    public class Item
    {
        public int id;
        public string name;
        public string type;
        public bool stackable;
        public string description;
        public Sprite icon;
        public Dictionary<string, double> stats = new Dictionary<string, double>();

        public Item(int id, string name, string type, bool stackable, string description, Dictionary<string, double> stats)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.stackable = stackable;
            this.description = description;
            this.icon = Resources.Load<Sprite>("Sprites/" + name);
            this.stats = stats;
        }
    }
}

