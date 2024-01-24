using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Characters;
using Items;
using Hymi;

namespace InventoryUI
{
    public class InventoryHymisImplementation : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerCash;
        [SerializeField] private TextMeshProUGUI playerBank;
        [SerializeField] private TextMeshProUGUI playerDebt;
        [SerializeField] private TextMeshProUGUI itemCount;
        [SerializeField] private GameObject layout;
        [SerializeField] private GameObject itemRepresentationPrefab;
        [SerializeField] private ContextMenuInventory contextMenu;
        public Tooltip tooltip;

        private void Awake()
        {
            PlayerEvents.Instance.EventInventoryChanged.AddListener(OnInventoryChanged);
            Debug.Log("Hymis inventory began listening to invenotry changed event " + Time.time);
        }

        void OnInventoryChanged(List<InventoryItem> items)
        {
            for (int i = layout.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(layout.transform.GetChild(i).gameObject);
            }

            foreach (InventoryItem invItem in items)
            {
                // Credit item is treated separately, same will go for debt and such once those are implemented
                if(invItem.item.Id == Inventory.Instance.CreditItem.Id)
                {
                    UpdateFinances(invItem);
                } else
                {
                    // Normal items get their graphical representation added to inventory (icon, name, description, context menu functionality)
                    AddItemRepresentation(invItem);
                }
            }
        }

        void UpdateFinances(InventoryItem creditItem)
        {
            int displayAmount = creditItem == null ? 0 : creditItem.amount;
            playerCash.text = $"{displayAmount} C";
        }

        // Add clickable icon type of thing in inventory ui
        void AddItemRepresentation(InventoryItem invItem)
        {
            GameObject itemRepresentationGO = Instantiate(itemRepresentationPrefab, layout.transform);
            ItemScript itemScript = itemRepresentationGO.GetComponent<ItemScript>();
            itemScript.Init(invItem, contextMenu, tooltip);
        }

        public void RemoveItem(InventoryItem invItem)
        {
            Inventory.Instance.RemoveItem(invItem.item, invItem.amount);
            tooltip.Clear();
        }

        public void SortByName()
        {

        }

        public void SortByID()
        {

        }
    }
}