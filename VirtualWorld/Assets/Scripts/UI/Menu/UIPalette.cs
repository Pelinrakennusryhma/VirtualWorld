using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace UI
{
    public class UIPalette : MonoBehaviour
    {
        [field: SerializeField] public UIColorTheme Theme { get; private set; }
        [SerializeField] UIManager uiManager;

        [ContextMenu("Refresh themed children")]
        private void Awake()
        {
            RefreshThemedChildren();
        }

        void RefreshThemedChildren()
        {
            Theme.CreatePalette();

            IEnumerable<IThemedComponent> components = FindObjectsOfType<MonoBehaviour>(true).OfType<IThemedComponent>();

            foreach (IThemedComponent component in components)
            {
                component.Init(Theme, uiManager);
            }
        }

    }
}
