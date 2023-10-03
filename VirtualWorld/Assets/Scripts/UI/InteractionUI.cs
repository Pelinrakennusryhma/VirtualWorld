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


        private void Update()
        {
            if (currentInteractableGO != null)
            {
                SetCanvasPosition(currentInteractableGO.transform.position);
            }
        }
        public void InitDetector(InteractableDetector interactableDetector)
        {
            interactableDetector.EventInteractableDetected.AddListener(OnInteractableDetected);
            interactableDetector.EventInteractableLost.AddListener(OnInteractableLost);
            interactableDetector.EventInteractionStarted.AddListener(OnInteractionStarted);
        }

        void OnInteractableDetected(I_Interactable interactable, GameObject interactableObj)
        {
            currentInteractableGO = interactableObj;
            SetPromptText(interactable.DetectionMessage);
            SetCanvasPosition(interactableObj.transform.position);
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
