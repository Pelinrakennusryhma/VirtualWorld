using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class InventoryItem
    {
        public Item item;
        public double amount;

        public InventoryItem(Item item, double amount)
        {
            this.item = item;
            this.amount = amount;
        }
    }
}

