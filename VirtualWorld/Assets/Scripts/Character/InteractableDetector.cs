using Authentication;
using StarterAssets;
using UI;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using WorldObjects;

namespace Characters
{
    public class InteractableDetector : NetworkBehaviour
    {
        public UnityEvent<I_Interactable, GameObject> EventInteractableDetected;
        public UnityEvent EventInteractableLost;
        public UnityEvent EventInteractionStarted;
        [SerializeField] StarterAssetsInputs input;
        [SerializeField] InteractionUI ui;
        I_Interactable currentInteractable;
        GameObject currentInteractableGO;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
            {
                Destroy(this);
                return;
            }

            if (IsHost)
            {
                Invoke("FindAndInitUI", 1f);
            } else
            {
                FindAndInitUI();
            }

        }

        private void Start()
        {
            if(Camera.main == null)
            {
                Destroy(this);
                return;
            }
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
            } else
            {
                Invoke("FindAndInitUI", 1f);
            }
        }

        void Interact()
        {
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
            if(other.gameObject == currentInteractableGO)
            {
                currentInteractable = null;
                currentInteractableGO = null;
                EventInteractableLost.Invoke();
            }
        }
    }
}
