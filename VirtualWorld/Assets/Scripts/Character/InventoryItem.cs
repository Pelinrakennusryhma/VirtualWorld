using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class InventoryItem
    {
        public Item item;
        public int amount;

        public InventoryItem(Item item, int amount)
        {
            this.item = item;
            this.amount = amount;
        }
    }
}

