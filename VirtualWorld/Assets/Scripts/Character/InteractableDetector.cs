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
        public UnityEvent<I_Interactable, GameObject> EventInteractableDetected;
        public UnityEvent EventInteractableLost;
        public UnityEvent EventInteractionStarted;
        [SerializeField] StarterAssetsInputs input;
        [SerializeField] InteractionUI ui;
        I_Interactable currentInteractable;
        GameObject currentInteractableGO;

        private void Start()
        {
            NetworkBehaviour networkBehaviour = transform.root.GetComponent<NetworkBehaviour>();
            if (!networkBehaviour.IsOwner)
            {
                Destroy(this);
                return;
            }

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
                Invoke("FindAndInitUI", 1f);
            }
        }

        void Interact()
        {
            input.ClearInteractInput();
            EventInteractionStarted.Invoke();
            currentInteractable.Interact(UserSession.Instance.LoggedUserData.id, new UnityAction(() => EventInteractableLost.Invoke()));
        }

        private void OnTriggerEnter(Collider other)
        {
            I_Interactable interactable = other.GetComponent<I_Interactable>();

            if (interactable != null)
            {    
                currentInteractable = interactable;
                currentInteractableGO = other.gameObject;
                EventInteractableDetected.Invoke(interactable, other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == currentInteractableGO)
            {
                currentInteractable = null;
                currentInteractableGO = null;
                EventInteractableLost.Invoke();
            }
        }
    }
}
