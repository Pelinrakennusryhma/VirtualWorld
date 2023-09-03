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
        float originalHeight;
        // Start is called before the first frame update
        void Start()
        {
            rect = GetComponent<RectTransform>();
            GatherButtonGroup();

            // Hide dropdown buttons if we are a sub group, a button in another group
            if (transform.GetComponent<ThemedButton>())
            {
                HideButtons();          
            } else
            {
                ShowButtons();
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
                if(button != null)
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

        public void HideButtons()
        {
            foreach (ThemedButton button in buttons)
            {
                button.gameObject.SetActive(false);
            }

            ActiveChild = null;

            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalHeight);
        }

        public void ShowButtons()
        {
            float totalHeight = rect.rect.height;
            originalHeight = totalHeight;
            foreach (ThemedButton button in buttons)
            {
                button.gameObject.SetActive(true);
                totalHeight += button.transform.GetComponent<RectTransform>().rect.height;
            }

            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, totalHeight);
        }


        public void OnChildClicked(ThemedButton childButton)
        {
            if (ActiveChild)
            {
                if(ActiveChild == childButton)
                {
                    return;
                }

                ActiveChild.Unfreeze();
            }

            Debug.Log("child clicked: " + childButton.name);
            ActiveChild = childButton;
            childButton.FreezeAndDecorate();
        }

        public void ResetGroup()
        {
            ActiveChild.Unfreeze();
        }

        public void SetColors(UIColorTheme theme)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                ThemedButton button = transform.GetChild(i).GetComponent<ThemedButton>();
                if(button != null)
                {
                    TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>(true);
                    buttonText.fontSize = fontSize;
                }
            }
        }
    }
}
