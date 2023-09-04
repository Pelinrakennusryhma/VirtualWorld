using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public interface IThemedComponent
{
    public void Init(UIColorTheme theme, UIManager uiManager);
}
