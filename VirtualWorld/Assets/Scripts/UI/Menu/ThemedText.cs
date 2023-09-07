using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class ThemedText : MonoBehaviour, IThemedComponent
{
    [SerializeField] PaletteColor color;
    TMP_Text text;
    UIColorTheme theme;

    void SetImageColor()
    {
        text.color = theme.GetColorFromPalette(color);
    }

    public void Init(UIColorTheme theme, UIManager uiManager)
    {
        this.theme = theme;

        text = GetComponent<TMP_Text>();

        SetImageColor();
    }
}
