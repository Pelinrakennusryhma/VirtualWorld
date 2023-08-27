using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class TextFlasher : MonoBehaviour
    {
        [SerializeField] TMP_Text text;
        [SerializeField] float flashTextDuration = 0.09f;
        [SerializeField] Color flashFontColor = Color.white;
        [SerializeField] float flashFontSize = 48f;

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
            StartCoroutine(IEFlashText());
        }

        IEnumerator IEFlashText()
        {
            text.fontSize = flashFontSize;
            text.color = flashFontColor;
            yield return new WaitForSeconds(flashTextDuration);
            text.fontSize = originalFontSize;
            text.color = originalFontColor;
        }
    }
}

