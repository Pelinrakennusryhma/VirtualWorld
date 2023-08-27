using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    [SerializeField] TMP_Text promptText;
    [SerializeField] float flashTextDuration = 0.5f;
    [SerializeField] Color flashFontColor = Color.white;
    [SerializeField] float flashFontSize = 30f;

    float originalFontSize;
    Color originalFontColor;
    GameObject currentInteractableGO;

    private void Start()
    {
        originalFontSize = promptText.fontSize;
        originalFontColor = promptText.color;
    }

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
        SetPromptText(interactable.DetectionMessage);
        SetCanvasPosition(interactableObj.transform.position);
        promptText.gameObject.SetActive(true);
    }

    void OnInteractableLost()
    {
        ClearPromptText();
        promptText.gameObject.SetActive(false);
    }

    void OnInteractionStarted()
    {
        StartCoroutine(FlashText());
    }

    void SetCanvasPosition(Vector3 pos)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
        promptText.rectTransform.position = screenPos;
    }

    void SetPromptText(string msg)
    {
        promptText.text = $"[E] {msg}";
    }

    void ClearPromptText()
    {
        promptText.text = "";
    }

    IEnumerator FlashText()
    {
        promptText.fontSize = flashFontSize;
        promptText.color = flashFontColor;
        yield return new WaitForSeconds(flashTextDuration);
        promptText.fontSize = originalFontSize;
        promptText.color = originalFontColor;
    }

}
