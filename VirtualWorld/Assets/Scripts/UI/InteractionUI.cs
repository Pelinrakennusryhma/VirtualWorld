using TMPro;
using UnityEngine;
using Characters;
using WorldObjects;

namespace UI
{
    public class InteractionUI : MonoBehaviour
    {
        [SerializeField] string interactionButton = "E";
        [SerializeField] TMP_Text promptText;
        [SerializeField] TextFlasher promptTextFlasher;
        GameObject currentInteractableGO;
        I_Interactable currentInteractable;

        void Awake()
        {
            // interaction prompt text object needs to be enabled in awake
            // or else tablet breaks it..
            // the object is also disabled later in the InitDetector method just for clarity
            promptText.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            promptTextFlasher.Reset();
        }

        private void Update()
        {
            if (currentInteractableGO != null)
            {
                SetCanvasPosition(currentInteractableGO.transform.position + currentInteractable.DetectionMessageOffSet);
            }
        }
        public void InitDetector(InteractableDetector interactableDetector)
        {
            PlayerEvents.Instance.EventInteractableDetected.AddListener(OnInteractableDetected);
            PlayerEvents.Instance.EventInteractableLost.AddListener(OnInteractableLost);
            PlayerEvents.Instance.EventInteractionStarted.AddListener(OnInteractionStarted);

            promptText.gameObject.SetActive(false);
        }

        void OnInteractableDetected(I_Interactable interactable, GameObject interactableObj)
        {
            currentInteractableGO = interactableObj;
            currentInteractable = interactable;
            SetPromptText(interactable.DetectionMessage);
            SetCanvasPosition(interactableObj.transform.position + interactable.DetectionMessageOffSet);
            promptText.gameObject.SetActive(true);
        }

        void OnInteractableLost()
        {
            currentInteractableGO = null;
            ClearPromptText();
            promptText.gameObject.SetActive(false);
        }

        void OnInteractionStarted()
        {
            promptTextFlasher.FlashText();
        }

        void SetCanvasPosition(Vector3 pos)
        {
            if(Camera.main != null)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
                promptText.rectTransform.position = screenPos;
            }
        }

        void SetPromptText(string msg)
        {
            promptText.text = $"[{interactionButton}] {msg}";
        }

        void ClearPromptText()
        {
            promptText.text = "";
            promptTextFlasher.Reset();
        }
    }
}
