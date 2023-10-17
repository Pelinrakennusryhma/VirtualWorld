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
        [SerializeField] Button proceedButton;
        [SerializeField] Button completeButton;

        public NPC CurrentNpc { get => _currentNpc; private set => _currentNpc = value; }
        NPC _currentNpc;
        List<GameObject> subDialogButtons = new List<GameObject>();

        public void Setup(DialogChoiceBase dialog, NPC npc = null, Quest quest = null)
        {
            if (npc != null)
            {
                CurrentNpc = npc;
            }

            if(quest != null)
            {
                Setup(quest);
                return;
            }

            if (dialog is DialogChoiceRoot)
            {
                SetupDialog(CurrentNpc.Data.fullName, CurrentNpc.Data.title, "", CurrentNpc.Data.mainDialog.text);
                SetupSubDialogLinks(CurrentNpc.Data.mainDialog.childDialogChoices, CurrentNpc.Data.mainDialog.quests);
                SetupButtons((DialogChoiceRoot)dialog);
            }

            if (dialog is DialogChoiceSub)
            {
                DialogChoiceSub subDialog = (DialogChoiceSub)dialog;
                SetupDialog("", "", subDialog.title, subDialog.text);
                SetupSubDialogLinks(subDialog.childDialogChoices);
                SetupButtons(subDialog);
            }

            if(dialog is DialogChoiceWithQuestStepTrigger)
            {
                DialogChoiceWithQuestStepTrigger questTrigger = (DialogChoiceWithQuestStepTrigger)dialog;
                SetupDialog("", "", questTrigger.title, questTrigger.text);
                SetupSubDialogLinks(null);
                SetupButtons(questTrigger);
            }
        }

        void Setup(Quest quest)
        {
            SetupDialog("", "", quest.title, quest.text);
            SetupButtons(quest);
            SetupSubDialogLinks(null);
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

            if(subDialogs != null)
            {
                foreach (DialogChoiceWithTitle childDialog in subDialogs)
                {
                    bool isQuestTrigger = childDialog is DialogChoiceWithQuestStepTrigger;

                    if (isQuestTrigger)
                    {
                        DialogChoiceWithQuestStepTrigger questTrigger = (DialogChoiceWithQuestStepTrigger)childDialog;
                        if (!QuestManager.Instance.IsOnQuestStep(questTrigger.questStep))
                        {
                            continue;
                        }
                    }

                    // instantiate, different prefab depending on if quest or not
                    GameObject buttonObj = isQuestTrigger
                        ? Instantiate(questChoiceButtonPrefab, dialogContainer)
                        : Instantiate(dialogChoiceButtonPrefab, dialogContainer);
                    subDialogButtons.Add(buttonObj);
                    TMP_Text text = buttonObj.GetComponentInChildren<TMP_Text>();
                    text.text = childDialog.title;
                    Button button = buttonObj.GetComponent<Button>();
                    button.onClick.AddListener(() => Setup(childDialog));
                }
            }

            if(quests != null)
            {
                foreach (Quest quest in quests)
                {
                    if (QuestManager.Instance.CanAcceptQuest(quest))
                    {
                        GameObject buttonObj = Instantiate(questChoiceButtonPrefab, dialogContainer);
                        subDialogButtons.Add(buttonObj);
                        TMP_Text text = buttonObj.GetComponentInChildren<TMP_Text>();
                        text.text = "! " + quest.title + " !";
                        Button button = buttonObj.GetComponent<Button>();
                        button.onClick.AddListener(() => Setup(quest));
                    }
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
            acceptButton.gameObject.SetActive(false);
            completeButton.gameObject.SetActive(false);
            backButton.gameObject.SetActive(false);
            proceedButton.gameObject.SetActive(false);
        }

        void SetupButtons(DialogChoiceSub dialog)
        {
            acceptButton.gameObject.SetActive(false);

            completeButton.gameObject.SetActive(false);

            backButton.onClick.RemoveAllListeners();
            backButton.gameObject.SetActive(true);
            backButton.onClick.AddListener(() => Setup(dialog.parentDialogChoice));

            proceedButton.gameObject.SetActive(false);
        }

        void SetupButtons(DialogChoiceWithQuestStepTrigger questTrigger)
        {
            acceptButton.onClick.RemoveAllListeners();
            acceptButton.gameObject.SetActive(false);

            proceedButton.gameObject.SetActive(false);

            completeButton.onClick.RemoveAllListeners();
            completeButton.gameObject.SetActive(true);
            completeButton.onClick.AddListener(() => QuestManager.Instance.ProgressStep(questTrigger.questStep, 1));


            backButton.onClick.RemoveAllListeners();
            backButton.gameObject.SetActive(true);
            backButton.onClick.AddListener(() => Setup(questTrigger.parentDialogChoice));
        }

        void SetupButtons(Quest quest)
        {
            acceptButton.onClick.RemoveAllListeners();
            acceptButton.gameObject.SetActive(true);
            acceptButton.onClick.AddListener(() => QuestManager.Instance.AcceptQuest(quest));

            completeButton.gameObject.SetActive(false);

            backButton.onClick.RemoveAllListeners();
            backButton.gameObject.SetActive(true);
            backButton.onClick.AddListener(() => Setup(CurrentNpc.Data.mainDialog));

            proceedButton.gameObject.SetActive(false);
        }

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


