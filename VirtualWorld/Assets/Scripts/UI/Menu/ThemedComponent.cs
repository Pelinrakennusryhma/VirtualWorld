using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ThemedComponent : MonoBehaviour, IThemedComponent
    {
        [SerializeField] PaletteColor color;
        UIPalette uiPalette;
        Image image;
        UIColorTheme theme;

        void SetImageColor()
        {
            image.color = uiPalette.Theme.GetColorFromPalette(color);
        }

        public void SetColors(UIColorTheme theme)
        {
            this.theme = theme;
            uiPalette = transform.root.GetComponent<UIPalette>();
            image = GetComponent<Image>();

            SetImageColor();
        }
    }
}

