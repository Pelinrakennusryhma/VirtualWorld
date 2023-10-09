using Characters;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Quests;
using System.Linq;

namespace Dialog
{
    public class DialogPanel : MonoBehaviour
    {
        [SerializeField] TMP_Text title0;
        [SerializeField] TMP_Text subTitle0;
        [SerializeField] TMP_Text title1;
        [SerializeField] TMP_Text mainText;
        [SerializeField] GameObject dialogChoiceButtonPrefab;
        [SerializeField] GameObject questChoiceButtonPrefab;
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
            SetupSubDialogsAndQuests(mainDialog);
        }

        // A sub dialog view, something that is clicked open in root dialog
        void SetupSubDialog(DialogChoice dialog)
        {
            title0.text = "";
            subTitle0.text = "";
            title1.text = dialog.title;
            mainText.text = dialog.text;

            if (IsQuest(dialog))
            {
                Quest quest = dialog as Quest;
                mainText.text += "\n\n" + quest.steps[0].objectiveDescLong;
            }

            SetupButtons(dialog);
            SetupSubDialogsAndQuests(dialog);
        }

        void SetupButtons(DialogChoice dialog)
        {
            DialogChoice parent = dialog.parentDialogChoice;
            // no parent dialog present, e.g. this is the root/main dialog
            if(parent == null)
            {
                // Quest view
                if (IsQuest(dialog))
                {
                    acceptButton.onClick.AddListener(() => QuestManager.Instance.AcceptQuest(dialog as Quest));
                    acceptButton.gameObject.SetActive(true);

                    backButton.onClick.RemoveAllListeners();
                    backButton.onClick.AddListener(() => SetupRootDialog(currentNpc));
                    backButton.gameObject.SetActive(true);

                }
                else // root dialog view
                {
                    acceptButton.onClick.RemoveAllListeners();
                    acceptButton.gameObject.SetActive(false);

                    backButton.onClick.RemoveAllListeners();
                    backButton.gameObject.SetActive(false);
                }
            } else // sub dialog view
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
            }
        }

        // clickable text buttons/links to sub dialogs
        void SetupSubDialogsAndQuests(DialogChoice mainDialog)
        {
            ClearSubDialogs();

            List<DialogChoice> dialogsAndQuests = new List<DialogChoice>(mainDialog.childDialogChoices.Concat(mainDialog.quests));

            foreach (DialogChoice childDialog in dialogsAndQuests)
            {
                bool isQuest = IsQuest(childDialog);
                // instantiate, different prefab depending on if quest or not
                GameObject buttonObj = isQuest
                    ? Instantiate(questChoiceButtonPrefab, dialogContainer) 
                    : Instantiate(dialogChoiceButtonPrefab, dialogContainer);
                subDialogs.Add(buttonObj);

                // styling to match the other text
                ThemedButton themedButton = buttonObj.GetComponent<ThemedButton>();
                UIPalette uiPalette = UIManager.Instance.GetComponent<UIPalette>();
                themedButton.Init(uiPalette.Theme, UIManager.Instance);

                // content and functionality - title is used as link text and
                // onClick opens the dialog
                TMP_Text text = buttonObj.GetComponentInChildren<TMP_Text>();
                text.text = isQuest ? $"! {childDialog.title} !" : childDialog.title;
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

        static bool IsQuest(DialogChoice dialogChoice)
        {
            return dialogChoice is Quest;
        }
    }
}


