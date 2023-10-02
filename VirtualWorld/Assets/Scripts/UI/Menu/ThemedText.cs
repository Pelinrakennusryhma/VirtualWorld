using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class ThemedText : MonoBehaviour, IThemedComponent
{
    [SerializeField] PaletteColor color;
    TMP_Text text;
    UIColorTheme theme;

    void SetTextColor()
    {
        text.color = theme.GetColorFromPalette(color);
        Debug.Log(color);
    }

    public void Init(UIColorTheme theme, UIManager uiManager)
    {
        this.theme = theme;

        text = GetComponent<TMP_Text>();

        SetTextColor();
    }
}
