using StarterAssets;
using UI;
using Unity.Netcode;
using UnityEngine;

namespace Characters
{
    public class PlayerEmitter : NetworkBehaviour
    {
        StarterAssetsInputs inputs;
        bool controlsDisabled = false;

        public InventoryHymisImplementation InventoryHymisImplementation;

        void Start()
        {
            if (IsOwner)
            {
                Character.Instance.inventoryController.SetHymisInventory(InventoryHymisImplementation);

                Character.Instance.SetPlayerGameObject(gameObject);
                UIManager.Instance.SetPlayerCharacter(gameObject);

                inputs = GetComponentInChildren<StarterAssetsInputs>();

                UIManager.Instance.EventMenuToggled.AddListener(TogglePlayerInputs);
            }
        }

        private void Update()
        {
            if (controlsDisabled)
            {
                inputs.ZeroInputs();
            }
        }

        void TogglePlayerInputs(bool menuEnabled)
        {
            controlsDisabled = menuEnabled;
            Debug.Log("Inputs enabled: " + !menuEnabled);
        }

    }
}
