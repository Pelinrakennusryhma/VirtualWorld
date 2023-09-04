using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ThemedComponent : MonoBehaviour, IThemedComponent
    {
        [SerializeField] PaletteColor color;
        Image image;
        UIColorTheme theme;

        void SetImageColor()
        {
            image.color = theme.GetColorFromPalette(color);
        }

        public void Init(UIColorTheme theme, UIManager uiManager)
        {
            this.theme = theme;
            image = GetComponent<Image>();

            SetImageColor();
        }
    }
}

