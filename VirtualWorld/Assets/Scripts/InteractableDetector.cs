using Authentication;
using StarterAssets;
using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class InteractableDetector : NetworkBehaviour
{
    public UnityEvent<I_Interactable, GameObject> EventInteractableDetected;
    public UnityEvent EventInteractableLost;
    public UnityEvent EventInteractionStarted;
    [SerializeField] StarterAssetsInputs input;
    I_Interactable currentInteractable;
    GameObject currentInteractableGO;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            Destroy(this);
            return;
        }

        InteractionUI ui = FindObjectOfType<InteractionUI>();
        if (ui != null)
        {
            ui.InitDetector(this);
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
        if(input.interact && currentInteractable != null)
        {
            Interact();
        }
    }

    void Interact()
    {
        EventInteractionStarted.Invoke();
        currentInteractable.Interact(UserSession.Instance.LoggedUserData.id);
        input.interact = false;
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
