using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UIPalette : MonoBehaviour
    {
        [field: SerializeField] public UIColorTheme theme { get; private set; }

        [SerializeField] public Dictionary<PaletteColor, Color> Palette { get; private set; }

    }
}
