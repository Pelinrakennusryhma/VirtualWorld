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
    enum DialogType
    {
        ROOT,
        SUB,
        QUEST,
    }
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
        List<GameObject> subDialogButtons = new List<GameObject>();

        public void Setup(DialogChoiceBase dialog, NPC npc = null)
        {
            if (npc != null)
            {
                currentNpc = npc;
            }

            if (dialog is DialogChoiceRoot)
            {
                SetupDialog(currentNpc.Data.fullName, currentNpc.Data.title, "", currentNpc.Data.mainDialog.text);
                SetupSubDialogLinks(currentNpc.Data.mainDialog.childDialogChoices, currentNpc.Data.mainDialog.quests);
                SetupButtons((DialogChoiceRoot)dialog);
            }

            if (dialog is DialogChoiceSub)
            {
                DialogChoiceSub subDialog = (DialogChoiceSub)dialog;
                SetupDialog("", "", subDialog.title, subDialog.text);
                SetupSubDialogLinks(subDialog.childDialogChoices);
                SetupButtons(subDialog);
            }

            if (dialog is Quest)
            {
                Quest questDialog = (Quest)dialog;
                SetupDialog("", "", questDialog.title, questDialog.text);
                SetupButtons(questDialog);
            }
        }

        void SetupDialog(string title0, string subTitle0, string title1, string mainText)
        {
            this.title0.text = title0;
            this.subTitle0.text = subTitle0;
            this.title1.text = title1;
            this.mainText.text = mainText;
        }

        void SetupSubDialogLinks(List<DialogChoiceWithTitle> subDialogs, List<Quest> quests = null)
        {
            ClearList(subDialogButtons);


            //List<DialogChoiceBase> dialogsAndQuests = new List<DialogChoiceBase>(subDialogs.Concat(quests));

            foreach (DialogChoiceWithTitle childDialog in subDialogs)
            {
                // instantiate, different prefab depending on if quest or not
                GameObject buttonObj = Instantiate(dialogChoiceButtonPrefab, dialogContainer);
                subDialogButtons.Add(buttonObj);
                TMP_Text text = buttonObj.GetComponentInChildren<TMP_Text>();
                text.text = childDialog.title;
                Button button = buttonObj.GetComponent<Button>();
                button.onClick.AddListener(() => Setup(childDialog));
            }

            if(quests != null)
            {
                foreach (Quest quest in quests)
                {
                    // instantiate, different prefab depending on if quest or not
                    GameObject buttonObj = Instantiate(questChoiceButtonPrefab, dialogContainer);
                    subDialogButtons.Add(buttonObj);
                    TMP_Text text = buttonObj.GetComponentInChildren<TMP_Text>();
                    text.text = "! " + quest.title + " !";
                    Button button = buttonObj.GetComponent<Button>();
                    button.onClick.AddListener(() => Setup(quest));
                }
            }

            UIPalette uiPalette = UIManager.Instance.GetComponent<UIPalette>();

            foreach (GameObject buttonObj in subDialogButtons)
            {
                // styling to match the other text
                ThemedButton themedButton = buttonObj.GetComponent<ThemedButton>();
                themedButton.Init(uiPalette.Theme, UIManager.Instance);
            }
        }

        void SetupButtons(DialogChoiceRoot dialog)
        {
            acceptButton.onClick.RemoveAllListeners();
            acceptButton.gameObject.SetActive(false);

            backButton.onClick.RemoveAllListeners();
            backButton.gameObject.SetActive(false);
        }

        void SetupButtons(DialogChoiceSub dialog)
        {
            acceptButton.onClick.RemoveAllListeners();
            acceptButton.gameObject.SetActive(false);

            backButton.onClick.RemoveAllListeners();
            backButton.gameObject.SetActive(true);
            backButton.onClick.AddListener(() => Setup(dialog.parentDialogChoice));
        }

        void SetupButtons(Quest dialog)
        {
            acceptButton.onClick.RemoveAllListeners();
            acceptButton.gameObject.SetActive(true);
            acceptButton.onClick.AddListener(() => QuestManager.Instance.AcceptQuest(dialog as Quest));

            backButton.onClick.RemoveAllListeners();
            backButton.gameObject.SetActive(true);
            backButton.onClick.AddListener(() => Setup(currentNpc.Data.mainDialog));
        }

        //void SetupSubDialogsAndQuests(DialogChoiceBase mainDialog)
        //{
        //    ClearSubDialogs();

        //    List<DialogChoiceBase> dialogsAndQuests = new List<DialogChoiceBase>(mainDialog.childDialogChoices.Concat(mainDialog.quests));

        //    foreach (DialogChoiceBase childDialog in dialogsAndQuests)
        //    {
        //        bool isQuest = IsQuest(childDialog);
        //        // instantiate, different prefab depending on if quest or not
        //        GameObject buttonObj = isQuest
        //            ? Instantiate(questChoiceButtonPrefab, dialogContainer)
        //            : Instantiate(dialogChoiceButtonPrefab, dialogContainer);
        //        subDialogs.Add(buttonObj);

        //        // styling to match the other text
        //        ThemedButton themedButton = buttonObj.GetComponent<ThemedButton>();
        //        UIPalette uiPalette = UIManager.Instance.GetComponent<UIPalette>();
        //        themedButton.Init(uiPalette.Theme, UIManager.Instance);

        //        // content and functionality - title is used as link text and
        //        // onClick opens the dialog
        //        TMP_Text text = buttonObj.GetComponentInChildren<TMP_Text>();
        //        text.text = isQuest ? $"! {childDialog.title} !" : childDialog.title;
        //        Button button = buttonObj.GetComponent<Button>();
        //        button.onClick.AddListener(() => SetupSubDialog(childDialog));
        //    }
        //}

        // Root/start view of a npc dialog
        //public void SetupRootDialog(NPC npc)
        //{
        //    currentNpc = npc;
        //    title0.text = npc.Data.fullName;
        //    subTitle0.text = npc.Data.title;
        //    title1.text = "";

        //    DialogChoiceBase mainDialog = npc.Data.mainDialog;
        //    mainText.text = mainDialog.text;

        //    //SetupButtons(mainDialog);
        //    //SetupSubDialogsAndQuests(mainDialog);
        //}

        // A sub dialog view, something that is clicked open in root dialog
        //void SetupSubDialog(DialogChoiceBase dialog)
        //{
        //    title0.text = "";
        //    subTitle0.text = "";
        //    //title1.text = dialog.title;
        //    mainText.text = dialog.text;

        //    if (IsQuest(dialog))
        //    {
        //        Quest quest = dialog as Quest;
        //        mainText.text += "\n\n" + quest.steps[0].objectiveDescLong;
        //    }

        //    //SetupButtons(dialog);
        //    //SetupSubDialogsAndQuests(dialog);
        //}

        //void SetupButtons(DialogChoiceBase dialog)
        //{
        //    // sub dialog
        //    if(IsSub(dialog))
        //    {
        //        // Quest view
        //        if (IsQuest(dialog))
        //        {
        //            acceptButton.onClick.AddListener(() => QuestManager.Instance.AcceptQuest(dialog as Quest));
        //            acceptButton.gameObject.SetActive(true);

        //            backButton.onClick.RemoveAllListeners();
        //            backButton.onClick.AddListener(() => SetupRootDialog(currentNpc));
        //            backButton.gameObject.SetActive(true);

        //        }
        //        else // root dialog view
        //        {
        //            acceptButton.onClick.RemoveAllListeners();
        //            acceptButton.gameObject.SetActive(false);

        //            backButton.onClick.RemoveAllListeners();
        //            backButton.gameObject.SetActive(false);
        //        }
        //    } else // sub dialog view
        //    {
        //        backButton.onClick.RemoveAllListeners();
        //        // parent is root
        //        DialogChoiceBase parent = dialog.parent
        //        if(parent.parentDialogChoice == null)
        //        {
        //            backButton.onClick.AddListener(() => SetupRootDialog(currentNpc));
        //        } else
        //        {
        //            backButton.onClick.AddListener(() => SetupSubDialog(parent));
        //        }

        //        backButton.gameObject.SetActive(true);
        //    }
        //}

        // clickable text buttons/links to sub dialogs


        void ClearList(List<GameObject> list)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                Destroy(list[i]);
            }

            list.Clear();
        }
    }
}


