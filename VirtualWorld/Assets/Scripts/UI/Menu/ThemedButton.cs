using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{

    public class ThemedButton : MonoBehaviour, IThemedComponent, IPointerEnterHandler, IPointerExitHandler
    {

        [SerializeField] PaletteColor color;
        [SerializeField] PaletteColor textColor;
        [SerializeField] PaletteColor textHoverColor;
        [SerializeField] PaletteColor textClickedColor;
        [SerializeField] PaletteColor textSelectedColor;
        [SerializeField] PaletteColor textDisabledColor;
        //[SerializeField] bool keepClickedColor;
        [SerializeField] float clickFlashDuration = 0.1f;
        Color returnColor;
        Image image;
        TMP_Text text;
        Button button;

        UIColorTheme theme;

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("enter?");
            text.color = theme.GetColorFromPalette(textHoverColor);
        }

        IEnumerator FlashTextColor()
        {
            returnColor = text.color;
            text.color = theme.GetColorFromPalette(textClickedColor);
            yield return new WaitForSeconds(clickFlashDuration);
            text.color = returnColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("exit?");
            text.color = theme.GetColorFromPalette(textColor);
            returnColor = text.color;
        }

        void OnTextClick()
        {
            Debug.Log("click?");
            StartCoroutine(FlashTextColor());
        }

        void SetButtonColors()
        {
            ColorBlock cb = button.colors;
            cb.normalColor = theme.GetColorFromPalette(color);
            cb.disabledColor = theme.GetColorFromPalette(color);
            cb.selectedColor = theme.GetColorFromPalette(color);
            cb.pressedColor = theme.GetColorFromPalette(color);
            cb.highlightedColor = theme.GetColorFromPalette(color);
            button.colors = cb;
        }

        void SetTextColor()
        {
            text.color = theme.GetColorFromPalette(textColor);
        }

        void SetImageColor()
        {
            image.color = theme.GetColorFromPalette(color);
        }

        public void SetColors(UIColorTheme theme)
        {
            this.theme = theme;

            button = GetComponent<Button>();
            image = GetComponent<Image>();
            text = transform.GetChild(0).GetComponent<TMP_Text>();
            button.onClick.AddListener(OnTextClick);

            SetImageColor();
            SetButtonColors();
            SetTextColor();
        }
    }
}

