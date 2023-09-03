using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public interface IThemedUIComponent
    {
        [ContextMenu("Refresh Component")]
        void RefreshComponent();
    }
}

