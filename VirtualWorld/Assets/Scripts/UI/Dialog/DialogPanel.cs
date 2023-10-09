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
        [SerializeField] TMP_Text title0;
        [SerializeField] TMP_Text subTitle0;
        [SerializeField] TMP_Text title1;
        [SerializeField] TMP_Text mainText;
        [SerializeField] GameObject dialogChoiceButtonPrefab;
        [SerializeField] Transform dialogContainer;
        [SerializeField] Button backButton;
        [SerializeField] Button acceptButton;
        NPC currentNpc;
        List<GameObject> subDialogs = new List<GameObject>();

        // Root/start view of a npc dialog
        public void SetupRootDialog(NPC npc)
        {
            currentNpc = npc;
            title0.text = npc.Data.fullName;
            subTitle0.text = npc.Data.title;
            title1.text = "";

            DialogChoice mainDialog = npc.Data.mainDialog;
            mainText.text = mainDialog.text;

            SetupButtons(mainDialog);
            SetupSubDialogs(mainDialog);
        }

        // A sub dialog view, something that is clicked open in root dialog
        void SetupSubDialog(DialogChoice dialog)
        {
            title0.text = "";
            subTitle0.text = "";
            title1.text = dialog.title;
            mainText.text = dialog.text;

            SetupButtons(dialog);
            SetupSubDialogs(dialog);
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
                // parent is root
                if(parent.parentDialogChoice == null)
                {
                    backButton.onClick.AddListener(() => SetupRootDialog(currentNpc));
                } else
                {
                    backButton.onClick.AddListener(() => SetupSubDialog(parent));
                }

                backButton.gameObject.SetActive(true);

                // probably check here if this is a quest dialog and add a proper listener
                acceptButton.onClick.RemoveAllListeners();
                acceptButton.gameObject.SetActive(false);
            }
        }

        // clickable text buttons/links to sub dialogs
        void SetupSubDialogs(DialogChoice mainDialog)
        {
            ClearSubDialogs();

            foreach (DialogChoice childDialog in mainDialog.childDialogChoices)
            {
                // instantiate
                GameObject buttonObj = Instantiate(dialogChoiceButtonPrefab, dialogContainer);
                subDialogs.Add(buttonObj);

                // styling to match the other text
                ThemedButton themedButton = buttonObj.GetComponent<ThemedButton>();
                UIPalette uiPalette = UIManager.Instance.GetComponent<UIPalette>();
                themedButton.Init(uiPalette.Theme, UIManager.Instance);

                // content and functionality - title is used as link text and
                // onClick opens the dialog
                TMP_Text text = buttonObj.GetComponentInChildren<TMP_Text>();
                text.text = childDialog.title;
                Button button = buttonObj.GetComponent<Button>();
                button.onClick.AddListener(() => SetupSubDialog(childDialog));
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


