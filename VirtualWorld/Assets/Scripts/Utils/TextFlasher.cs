using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class TextFlasher : MonoBehaviour
    {
        [SerializeField] TMP_Text text;
        [SerializeField] float flashTextDuration = 0.09f;
        [SerializeField] Color flashFontColor = Color.white;
        [SerializeField] float flashFontSize = 48f;
        [SerializeField] public bool Flashing { get; private set; }
        [SerializeField] public UnityAction OnFlashCompletion { get; set; }

        float originalFontSize;
        Color originalFontColor;

        private void Start()
        {
            if(text == null)
            {
                text = GetComponent<TMP_Text>();
            }

            originalFontSize = text.fontSize;
            originalFontColor = text.color;
        }

        public void FlashText()
        {
            // in hopes of fixing an error about failing to start coroutine because object is not active
            if (enabled && gameObject.activeSelf)
            {
                StartCoroutine(IEFlashText());
            }
        }

        IEnumerator IEFlashText()
        {
            Flashing = true;
            text.fontSize = flashFontSize;
            text.color = flashFontColor;

            yield return new WaitForSeconds(flashTextDuration);

            text.fontSize = originalFontSize;
            text.color = originalFontColor;
            Flashing = false;

            yield return new WaitForEndOfFrame();

            if(OnFlashCompletion != null)
            {
                OnFlashCompletion();
                OnFlashCompletion = null;
            }
        }

        public void Reset()
        {
            text.fontSize = originalFontSize;
            text.color = originalFontColor;
            Flashing = false;
        }
    }
}

