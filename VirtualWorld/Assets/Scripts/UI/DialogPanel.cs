using Characters;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DialogPanel : MonoBehaviour
    {
        [SerializeField] TMP_Text npcNameText;
        [SerializeField] TMP_Text npcTitleText;
        [SerializeField] TMP_Text dialogTitleText;
        [SerializeField] TMP_Text description;
        [SerializeField] GameObject dialogChoiceButtonPrefab;
        [SerializeField] Transform dialogContainer;
        [SerializeField] Button backButton;
        [SerializeField] Button acceptButton;
        NPC currentNpc;
        List<GameObject> subDialogs = new List<GameObject>();

        public void Setup(string name, string title, DialogChoice mainDialog, NPC npc = null)
        {
            if(npc != null)
            {
                currentNpc = npc;
                npcNameText.text = npc.Data.fullName;
                npcTitleText.text = npc.Data.title;
                dialogTitleText.text = "";
            } else
            {
                dialogTitleText.text = name;
                npcNameText.text = "";
                npcTitleText.text = "";
            }


            description.text = mainDialog.text;

            SetupButtons(mainDialog);

            SetupSubDialogs(mainDialog);
        }

        void SetupButtons(DialogChoice mainDialog)
        {
            DialogChoice parent = mainDialog.parentDialogChoice;
            // no parent dialog present, e.g. this is the root/main dialog
            if(parent == null)
            {
                backButton.onClick.RemoveAllListeners();
                backButton.gameObject.SetActive(false);

                acceptButton.onClick.RemoveAllListeners();
                acceptButton.gameObject.SetActive(false);
            } else
            {
                backButton.onClick.RemoveAllListeners();
                backButton.onClick.AddListener(() => Setup(currentNpc.Data.fullName, currentNpc.Data.title, parent));
                backButton.gameObject.SetActive(true);

                // probably check here if this is a quest dialog and add a proper listener
                acceptButton.onClick.RemoveAllListeners();
                acceptButton.gameObject.SetActive(false);
            }
        }

        void SetupSubDialogs(DialogChoice mainDialog)
        {
            ClearSubDialogs();

            foreach (DialogChoice childDialog in mainDialog.childDialogChoices)
            {
                GameObject buttonObj = Instantiate(dialogChoiceButtonPrefab, dialogContainer);
                subDialogs.Add(buttonObj);
                ThemedButton themedButton = buttonObj.GetComponent<ThemedButton>();
                UIPalette uiPalette = UIManager.Instance.GetComponent<UIPalette>();
                themedButton.Init(uiPalette.Theme, UIManager.Instance);
                TMP_Text text = buttonObj.GetComponentInChildren<TMP_Text>();
                text.text = childDialog.title;
                Button button = buttonObj.GetComponent<Button>();
                button.onClick.AddListener(() => Setup(childDialog.title, "", childDialog));
            }
        }

        void ClearSubDialogs()
        {
            for (int i = subDialogs.Count - 1; i >= 0; i--)
            {
                Destroy(subDialogs[i]);
            }

            subDialogs.Clear();
        }
    }
}


