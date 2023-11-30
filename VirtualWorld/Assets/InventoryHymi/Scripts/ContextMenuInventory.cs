using Characters;
using InventoryUI;
using Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Hymi
{
    public class ContextMenuInventory : MonoBehaviour
    {
        [SerializeField] private GameObject buttonUse;
        [SerializeField] private GameObject buttonEquip;
        [SerializeField] private GameObject buttonUnequip;
        [SerializeField] private GameObject buttonSell;
        [SerializeField] private GameObject buttonDiscard;
        [SerializeField] private InventoryHymisImplementation inv;
        InventoryItem invItem;
        private void Update()
        {
            //Sulkee context menun painettaessa mistä tahansa muualta kuin context menusta tai sen vaihtoehdoista.
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
            {
                if (!ClickingSelfOrChild())
                {
                    HideMenu();
                }
            }
        }

        //Piilottaa context menusta kaikki vaihtoehdot ja sitten näyttää kaikki itemin tyyppiin liittyvät vaihtoehdot. Näyttää 'Sell' jos pelaaja on kaupassa.
        public void ShowOptions(InventoryItem invItem)
        {
            gameObject.SetActive(true);
            HideAll();
            ShowDiscard();
            this.invItem = invItem;
        }

        //Tuo context menun hiiren luokse
        public void SetPositionToMouse()
        {
            gameObject.transform.position = Input.mousePosition;
        }

        //Piilottaa context menun pois näkyvistä
        public void HideMenu()
        {
            //gameObject.transform.position = new Vector3(-1000, 1000, 0);
            gameObject.SetActive(false);
        }

        //Context menun 'Discard' vaihtoehto
        public void DiscardItem()
        {        
            Debug.Log("Discard pressed");
            inv.RemoveItem(invItem);
            HideMenu();

        }

        //Context menun 'Sell' vaihtoehto
        public void SellItem()
        {

        }
        //Context menun 'Equip' vaihtoehto
        public void EquipItem()
        {
            throw new NotImplementedException();
        }
        //Context menun 'Unequip' vaihtoehto
        public void UnequipItem()
        {
            throw new NotImplementedException();
        }

        //Context menun 'Use' vaihtoehto
        public void UseItem()
        {

        }

        //Tuo näkyville tai piilottaa näkyvistä eri nappeja context menusta
        public void ShowUse()
        {
            buttonUse.SetActive(true);
        }
        public void HideUse()
        {
            buttonUse.SetActive(false);
        }
        public void ShowEquip()
        {
            buttonEquip.SetActive(true);
        }
        public void HideEquip()
        {
            buttonEquip.SetActive(false);
        }
        public void ShowUnequip()
        {
            buttonUnequip.SetActive(true);
        }
        public void HideUnequip()
        {
            buttonUnequip.SetActive(false);
        }
        public void ShowSell()
        {
            buttonSell.SetActive(true);
        }
        public void HideSell()
        {
            buttonSell.SetActive(false);
        }
        public void ShowDiscard()
        {
            buttonDiscard.SetActive(true);
        }
        public void HideDiscard()
        {
            buttonDiscard.SetActive(false);
        }
        //Näyttää kaikki napit
        public void ShowAll()
        {
            ShowUse();
            ShowEquip();
            ShowUnequip();
            ShowSell();
            ShowDiscard();
        }
        //Piilottaa kaikki napit
        public void HideAll()
        {
            HideUse();
            HideEquip();
            HideUnequip();
            HideSell();
            HideDiscard();
        }
        private bool ClickingSelfOrChild()
        {
            RectTransform[] rectTransforms = GetComponentsInChildren<RectTransform>();
            foreach (RectTransform rectTransform in rectTransforms)
            {
                if (EventSystem.current.currentSelectedGameObject == rectTransform.gameObject)
                {
                    return true;
                };
            }
            return false;
        }

    }
}