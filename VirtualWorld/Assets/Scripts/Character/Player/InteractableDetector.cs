using Authentication;
using StarterAssets;
using UI;
using FishNet.Object;
using UnityEngine;
using UnityEngine.Events;
using WorldObjects;

namespace Characters
{
    public class InteractableDetector : MonoBehaviour
    {
        [SerializeField] StarterAssetsInputs input;
        [SerializeField] InteractionUI ui;
        I_Interactable currentInteractable;
        GameObject currentInteractableGO;

        private void Start()
        {
            FindAndInitUI();
        }

        private void Update()
        {
            if (input.interact && currentInteractable != null)
            {
                Interact();
            }
        }

        void FindAndInitUI()
        {
            ui = FindObjectOfType<InteractionUI>();
            if (ui != null)
            {
                ui.InitDetector(this);
            }
            else
            {
                // I can't remember why this is delayed.. for host purposes I'd guess?
                Invoke("FindAndInitUI", 1f);
            }
        }

        void Interact()
        {
            input.ClearInteractInput();
            PlayerEvents.Instance.CallEventInteractionStarted();
            currentInteractable.Interact(UserSession.Instance.LoggedUserData.id, new UnityAction(() => PlayerEvents.Instance.CallEventInteractableLost()));
        }

        private void OnTriggerStay(Collider other)
        {
            I_Interactable interactable = other.GetComponent<I_Interactable>();

            if (interactable != null)
            {
                if (interactable.IsActive)
                {
                    currentInteractable = interactable;
                    currentInteractableGO = other.gameObject;
                    PlayerEvents.Instance.CallEventInteractableDetected(interactable, other.gameObject);
                } else
                {
                    // trigger exit to disable the scanner when interactable becomes inactive
                    // e.g. quest object when active quest step changes
                    OnTriggerExit(other);
                }

            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == currentInteractableGO)
            {
                currentInteractable = null;
                currentInteractableGO = null;
                PlayerEvents.Instance.CallEventInteractableLost();
            }
        }
    }
}
