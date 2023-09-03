using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UIPalette : MonoBehaviour
    {
        [field: SerializeField] public UIColorTheme Theme { get; private set; }

        //[ContextMenu("Refresh themed children")]

        private void Awake()
        {
            RefreshThemedChildren();
        }

        void RefreshThemedChildren()
        {
            Debug.Log("Refresh children " + gameObject.name);
            Theme.CreatePalette();

            IThemedComponent[] components = GetComponentsInChildren<IThemedComponent>();

            foreach (IThemedComponent component in components)
            {
                component.SetColors(Theme);
            }
        }

    }
}
