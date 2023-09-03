using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{

    public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] PaletteColor textColor;
        [SerializeField] Color textHoverColor;
        [SerializeField] Color textClickedColor;
        UIPalette uiPalette;
        Image image;
        TMP_Text text;
        Button button;
        bool inited = false;

        void Start()
        {
            inited = true;
            uiPalette = transform.root.GetComponent<UIPalette>();
            image = GetComponent<Image>();
            text = GetComponentInChildren<TMP_Text>();
            button = GetComponent<Button>();
            SetColor();

            button.onClick.AddListener(OnTextClick);
        }

        void SetColor()
        {
            //text.color = uiPalette.Palette[textColor];

        }
        private void Update()
        {
            if (!inited)
            {
                Debug.Log("panel, inited: " + inited);
                Start();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            text.color = textHoverColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            text.color = uiPalette.theme.GetColorFromPalette(textColor);
        }

        void OnTextClick()
        {
            text.color = textClickedColor;
        }

    }
}

