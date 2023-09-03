using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MenuPanel : MonoBehaviour
    {
        [SerializeField] PaletteColor color;
        UIPalette uiPalette;
        Image image;
        bool inited = false;

        void Start()
        {
            inited = true;
            uiPalette = transform.root.GetComponent<UIPalette>();
            image = GetComponent<Image>();
            SetColor();
        }

        void SetColor()
        {
            //image.color = uiPalette.Palette[color];

        }
        private void Update()
        {
            if (!inited)
            {
                Debug.Log("panel, inited: " + inited);
                Start();
            }
        }

        [ContextMenu("Refresh Component")]
        void RefreshComponent()
        {
            UIPalette uiPalette = transform.root.GetComponent<UIPalette>();
            Color newColor = uiPalette.theme.GetColorFromPalette(color);
            image = GetComponent<Image>();
            image.color = newColor;
        }
    }
}

