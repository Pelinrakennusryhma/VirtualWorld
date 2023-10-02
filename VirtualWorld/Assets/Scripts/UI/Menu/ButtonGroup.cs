using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ButtonGroup : MonoBehaviour, IThemedComponent
    {
        [SerializeField] public ThemedButton ActiveChild { get; private set; }
        [SerializeField] public int fontSize = 72;
        List<ThemedButton> buttons;
        RectTransform rect;
        // stores the default height before child buttons are shown so it can be set back after hiding child buttons
        float originalHeight;
        float originalWidth;

        #region Init

        void Awake()
        {
            rect = GetComponent<RectTransform>();
            originalHeight = rect.rect.height;
            originalWidth = rect.rect.width;
            //rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalHeight);
            rect.sizeDelta = new Vector2(originalWidth, originalHeight);
        }
        void Start()
        {
            GatherButtonGroup();

            // Hide dropdown buttons if we are not the topmost ButtonGroup in UI hierarchy
            if (transform.GetComponent<ThemedButton>())
            {
                HideButtons();          
            }
        }

        void GatherButtonGroup()
        {
            buttons = new List<ThemedButton>();

            // get buttons from children one level deep only
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                ThemedButton button = child.GetComponent<ThemedButton>();
                if (button != null)
                {
                    buttons.Add(button);
                }
            }

            foreach (ThemedButton themedButton in buttons)
            {
                Button button = themedButton.GetComponent<Button>();
                button.onClick.AddListener(() => OnChildClicked(themedButton));
                TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
                buttonText.fontSize = fontSize;
            }
        }

        // Children manage their color theme, we only set font size via ButtonGroup
        public void Init(UIColorTheme dummyTheme, UIManager dummyUiManager)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                ThemedButton button = transform.GetChild(i).GetComponent<ThemedButton>();
                if (button != null)
                {
                    TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>(true);
                    buttonText.fontSize = fontSize;
                }
            }
        }
        #endregion

        // the height of the child buttons is added to our height to avoid children overlapping with Unity's layout group
        public void ShowButtons()
        {
            float totalHeight = rect.rect.height;
            originalHeight = totalHeight;
            foreach (ThemedButton button in buttons)
            {
                button.gameObject.SetActive(true);
                totalHeight += button.transform.GetComponent<RectTransform>().rect.height;
            }

            rect.sizeDelta = new Vector2(originalWidth, totalHeight);
        }

        public void HideButtons()
        {
            foreach (ThemedButton button in buttons)
            {
                button.gameObject.SetActive(false);
            }

            ActiveChild = null;

            rect.sizeDelta = new Vector2(originalWidth, originalHeight);
        }

        public void OnChildClicked(ThemedButton childButton)
        {
            // Reset the style of previously clicked child
            if (ActiveChild)
            {
                if(ActiveChild == childButton)
                {
                    return;
                }

                ActiveChild.Unfreeze();
            }

            ActiveChild = childButton;
            childButton.FreezeAndDecorate();
        }

        #region Resetting

        void OnDisable()
        {
            Reset();
        }

        public void Reset()
        {
            if (ActiveChild)
            {
                ActiveChild.Unfreeze();
                ActiveChild = null;
            }
        }
        #endregion
    }
}
