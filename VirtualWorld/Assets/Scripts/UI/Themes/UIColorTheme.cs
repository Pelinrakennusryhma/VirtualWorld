using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

namespace UI
{
    public enum PaletteColor
    {
        COLOR_DARK,
        COLOR_NORMAL,
        COLOR_LIGHT,
        COLOR_LIGHTER,
        COLOR_LIGHTEST,
        COLOR_TRANSPARENT_FULL,
        COLOR_TRANSPARENT_MEDIUM,
        COLOR_TRANSPARENT_SLIGHT,
        COLOR_WHITE,
        COLOR_BLACK,
        COLOR_QUEST_MAIN
    }

    [CreateAssetMenu(fileName = "UIColorTheme", menuName = "ScriptableObjects/UIColorTheme", order = 1)]
    public class UIColorTheme : ScriptableObject
    {
        [field: SerializeField] public Color ColorDark { get; private set; }
        [field: SerializeField] public Color ColorNormal { get; private set; }
        [field: SerializeField] public Color ColorLight { get; private set; }
        [field: SerializeField] public Color ColorLighter { get; private set; }
        [field: SerializeField] public Color ColorLightest { get; private set; }
        [field: SerializeField] public Color ColorTransparentFull { get; private set; }
        [field: SerializeField] public Color ColorTransparentMedium { get; private set; }
        [field: SerializeField] public Color ColorTransparentSlight { get; private set; }
        [field: SerializeField] public Color ColorWhite { get; private set; }
        [field: SerializeField] public Color ColorBlack { get; private set; }
        [field: SerializeField] public Color ColorQuestMain { get; private set; }
        [SerializeField] Dictionary<PaletteColor, Color> Palette { get; set; }

        public void CreatePalette()
        {
            Palette = new Dictionary<PaletteColor, Color>();
            Palette.Add(PaletteColor.COLOR_DARK, ColorDark);
            Palette.Add(PaletteColor.COLOR_NORMAL, ColorNormal);
            Palette.Add(PaletteColor.COLOR_LIGHT, ColorLight);
            Palette.Add(PaletteColor.COLOR_LIGHTER, ColorLighter);
            Palette.Add(PaletteColor.COLOR_LIGHTEST, ColorLightest);
            Palette.Add(PaletteColor.COLOR_TRANSPARENT_FULL, ColorTransparentFull);
            Palette.Add(PaletteColor.COLOR_TRANSPARENT_MEDIUM, ColorTransparentMedium);
            Palette.Add(PaletteColor.COLOR_TRANSPARENT_SLIGHT, ColorTransparentSlight);
            Palette.Add(PaletteColor.COLOR_WHITE, ColorWhite);
            Palette.Add(PaletteColor.COLOR_BLACK, ColorBlack);
            Palette.Add(PaletteColor.COLOR_QUEST_MAIN, ColorQuestMain);
        }

        public Color GetColorFromPalette(PaletteColor paletteColor)
        {
            return Palette[paletteColor];
        }
    }
}