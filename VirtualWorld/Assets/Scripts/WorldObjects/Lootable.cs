using Characters;
using FishNet.Object;
using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WorldObjects;

namespace WorldObjects
{
    public class Lootable : MonoBehaviour, I_Interactable
    {
        [SerializeField] Item item;
        [field: SerializeReference]
        public string DetectionMessage { get; set; }
        public bool IsActive => true;
        public Vector3 DetectionMessageOffSet { get => Vector3.zero; }

        Respawnable respawnable;

        void Start()
        {
            respawnable = transform.parent.GetComponent<Respawnable>();
            DetectionMessage = DetectionMessage.Replace("%%item%%", item.DisplayName);
        }

        public void Interact(UnityAction callback)
        {
            Inventory.Instance.AddItem(item, 1);

            respawnable.Despawn(this, gameObject);
        }
    }
}
