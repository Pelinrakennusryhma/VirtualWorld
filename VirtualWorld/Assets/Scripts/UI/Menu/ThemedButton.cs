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
        [SerializeField] PaletteColor buttonHoverColor;
        [SerializeField] PaletteColor buttonClickedColor;
        [SerializeField] PaletteColor buttonSelectedColor;
        [SerializeField] PaletteColor buttonDisabledColor;

        [SerializeField] PaletteColor textColor;
        [SerializeField] PaletteColor textHoverColor;
        [SerializeField] PaletteColor textClickedColor;
        [SerializeField] PaletteColor textSelectedColor;
        [SerializeField] PaletteColor textDisabledColor;
        [SerializeField] float clickFlashDuration = 0.1f;

        ButtonGroup bg;
        bool frozen = false; // set true when the button opens a ButtonGroup; bunch of child buttons
        Color returnColor;
        Image image;
        TMP_Text text;
        Button button;
        string originalText = "";

        UIColorTheme theme;
        UIManager uiManager;

        #region Init
        public void Init(UIColorTheme theme, UIManager uiManager)
        {
            this.theme = theme;
            this.uiManager = uiManager;

            button = GetComponent<Button>();
            image = GetComponent<Image>();
            bg = GetComponent<ButtonGroup>();
            text = transform.GetChild(0).GetComponent<TMP_Text>();
            button.onClick.AddListener(OnTextClick);

            SetImageColor();
            SetButtonColors();
            SetTextColor();
        }

        void SetButtonColors()
        {
            ColorBlock cb = button.colors;
            cb.normalColor = theme.GetColorFromPalette(color);
            cb.disabledColor = theme.GetColorFromPalette(buttonDisabledColor);
            cb.selectedColor = theme.GetColorFromPalette(buttonSelectedColor);
            cb.pressedColor = theme.GetColorFromPalette(buttonClickedColor);
            cb.highlightedColor = theme.GetColorFromPalette(buttonHoverColor);
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
        #endregion

        #region Pointer And Clicking
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (frozen)
            {
                return;
            }
            text.color = theme.GetColorFromPalette(textHoverColor);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (frozen)
            {
                return;
            }
            text.color = theme.GetColorFromPalette(textColor);
            returnColor = text.color;
        }

        void OnTextClick()
        {
            if (frozen)
            {
                return;
            }

            if(textHoverColor != textClickedColor)
            {
                StartCoroutine(FlashTextColor());
            }
        }

        IEnumerator FlashTextColor()
        {
            returnColor = text.color;
            text.color = theme.GetColorFromPalette(textClickedColor);
            yield return new WaitForSeconds(clickFlashDuration);
            text.color = returnColor;
        }
        #endregion

        #region Methods for parent ButtonGroup

        public void FreezeAndDecorate()
        {
            frozen = true;
            originalText = text.text;
            text.text = $"<u>{text.text}</u>";

            if(bg != null)
            {
                bg.ShowButtons();
            }
        }

        public void Unfreeze()
        {
            frozen = false;
            text.text = originalText;
            SetTextColor();

            if (bg != null)
            {
                if(bg.ActiveChild != null)
                {
                    bg.ActiveChild.Unfreeze();
                }
                bg.HideButtons();
            }
        }

        #endregion
    }
}

