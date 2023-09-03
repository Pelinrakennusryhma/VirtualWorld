using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public enum PaletteColor
    {
        COLOR_DARK,
        COLOR_NORMAL,
        COLOR_LIGHT,
        COLOR_LIGHTEST,
        COLOR_TRANSPARENT_0,
        COLOR_TRANSPARENT_1,
    }

    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/UIColorTheme", order = 1)]
    public class UIColorTheme : ScriptableObject
    {
        [field: SerializeField] public Color ColorDark { get; private set; }
        [field: SerializeField] public Color ColorNormal { get; private set; }
        [field: SerializeField] public Color ColorLight { get; private set; }
        [field: SerializeField] public Color ColorLightest { get; private set; }
        [field: SerializeField] public Color ColorTransparent0 { get; private set; }
        [field: SerializeField] public Color ColorTransparent1 { get; private set; }

        [SerializeField] public Dictionary<PaletteColor, Color> Palette { get; private set; }

        void CreatePalette()
        {
            Palette = new Dictionary<PaletteColor, Color>();
            Palette.Add(PaletteColor.COLOR_DARK, ColorDark);
            Palette.Add(PaletteColor.COLOR_NORMAL, ColorNormal);
            Palette.Add(PaletteColor.COLOR_LIGHT, ColorLight);
            Palette.Add(PaletteColor.COLOR_LIGHTEST, ColorLightest);
            Palette.Add(PaletteColor.COLOR_TRANSPARENT_0, ColorTransparent0);
            Palette.Add(PaletteColor.COLOR_TRANSPARENT_1, ColorTransparent1);
            Debug.Log("created palette?!");
        }

        [Tooltip("For editor use")]
        public Color GetColorFromPalette(PaletteColor paletteColor)
        {
            CreatePalette();
            return Palette[paletteColor];
        }
    }
}