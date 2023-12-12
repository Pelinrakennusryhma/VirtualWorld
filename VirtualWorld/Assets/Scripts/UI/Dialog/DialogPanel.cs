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
        [SerializeField] Button completeButton;

        public NPC CurrentNpc { get => _currentNpc; private set => _currentNpc = value; }
        NPC _currentNpc;
        List<GameObject> subDialogButtons = new List<GameObject>();

        /// <summary>
        /// Setup dialog panel to show title, text and associated buttons. <br />
        /// - dialog: the DialogChoiceBase (root or sub) that might or might not have its own sub dialog choices. <br />
        /// - npc: when setupping root, let the script know which npc player is talking to. <br />
        /// - quest: followup quest dialog to open when player completes a quest. <br />
        /// </summary>
        public void Setup(DialogChoiceBase dialog, NPC npc = null, Quest quest = null)
        {
            if (npc != null)
            {
                CurrentNpc = npc;
            }

            if (quest != null)
            {
                Setup(quest);
                return;
            }

            if (dialog is DialogChoiceRoot)
            {
                SetupDialog(CurrentNpc.Data.fullName, CurrentNpc.Data.title, "", CurrentNpc.Data.mainDialog.text);
                SetupSubDialogLinks(CurrentNpc.Data.mainDialog.childDialogChoices, CurrentNpc.Data.mainDialog.quests, dialog as DialogChoiceRoot);
                SetupButtons((DialogChoiceRoot)dialog);
            }

            if (dialog is DialogChoiceSub)
            {
                DialogChoiceSub subDialog = (DialogChoiceSub)dialog;
                SetupDialog("", "", subDialog.title, subDialog.text);
                SetupSubDialogLinks(subDialog.childDialogChoices);
                SetupButtons(subDialog);
            }
        }

        /// <summary>
        /// Setup dialog with quest data. <br />
        /// Set title and text. <br />
        /// Setup buttons (accept, back). <br />
        /// Clear sub dialog choices, quest dialog can't have those. <br />
        /// </summary>
        void Setup(Quest quest)
        {
            SetupDialog("", "", quest.title, quest.text);
            SetupButtons(quest);
            ClearList(subDialogButtons);
        }

        /// <summary>
        /// Setup dialog with quest finisher data. <br />
        /// - activeStep: used to inform which step is completed <br />
        /// - finisherStep: contains the title and text for dialog <br />
        ///  <br />
        /// Set title and text. <br />
        /// Setup buttons (complete, back). <br />
        /// Clear sub dialog choices, quest dialog can't have those. <br />
        /// </summary>
        void Setup(ActiveQuestStep activeStep, QuestFinisherStep finisherStep)
        {
            SetupDialog("", "", finisherStep.dialogTitle, finisherStep.dialogText);
            ClearList(subDialogButtons);
            SetupButtons(activeStep, CurrentNpc.Data.mainDialog);
        }

        /// <summary>
        /// Setup the dialog(title and text).  <br />
        /// - title0: topmost title  <br />
        /// - subTitle0: smaller title below title0  <br />
        /// - title1: bigger title, occupying title0 and subTitle0 spaces  <br />
        /// - mainText: text  <br />
        /// </summary>


        void SetupDialog(string title0, string subTitle0, string title1, string mainText)
        {
            this.title0.text = title0;
            this.subTitle0.text = subTitle0;
            this.title1.text = title1;
            this.mainText.text = mainText;
        }

        void SetupSubDialogLinks(List<DialogChoiceWithTitle> subDialogs, List<Quest> quests = null, DialogChoiceRoot root = null)
        {
            ClearList(subDialogButtons);

            if(subDialogs != null)
            {
                foreach (DialogChoiceWithTitle childDialog in subDialogs)
                {

                    // instantiate different prefab depending on if quest or not.. different font color
                    GameObject buttonObj = Instantiate(dialogChoiceButtonPrefab, dialogContainer);
                    subDialogButtons.Add(buttonObj);
                    TMP_Text text = buttonObj.GetComponentInChildren<TMP_Text>();
                    text.text = childDialog.title;
                    Button button = buttonObj.GetComponent<Button>();
                    button.onClick.AddListener(() => Setup(childDialog));
                }
            }

            // quests that this npc starts
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

            // quests that this npc finishes
            Dictionary<ActiveQuestStep, QuestFinisherStep> activeSteps = QuestManager.Instance.FindQuestStepsWithNPCFinisher(CurrentNpc.Data);
            if(activeSteps.Count > 0)
            {
                foreach (KeyValuePair<ActiveQuestStep, QuestFinisherStep> step in activeSteps)
                {
                    GameObject buttonObj = Instantiate(questChoiceButtonPrefab, dialogContainer);
                    subDialogButtons.Add(buttonObj);
                    TMP_Text text = buttonObj.GetComponentInChildren<TMP_Text>();
                    text.text = step.Value.dialogTitle;
                    Button button = buttonObj.GetComponent<Button>();
                    button.onClick.AddListener(() => Setup(step.Key, step.Value));
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
        }

        void SetupButtons(DialogChoiceSub dialog)
        {
            acceptButton.gameObject.SetActive(false);

            completeButton.gameObject.SetActive(false);

            backButton.onClick.RemoveAllListeners();
            backButton.gameObject.SetActive(true);
            backButton.onClick.AddListener(() => Setup(dialog.parentDialogChoice));
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
        }

        void SetupButtons(ActiveQuestStep activeStep, DialogChoiceRoot prevDialog)
        {
            acceptButton.onClick.RemoveAllListeners();
            acceptButton.gameObject.SetActive(false);

            completeButton.onClick.RemoveAllListeners();
            completeButton.gameObject.SetActive(true);
            completeButton.onClick.AddListener(() => QuestManager.Instance.ProgressStep(activeStep.QuestStep, 1));

            backButton.onClick.RemoveAllListeners();
            backButton.gameObject.SetActive(true);
            backButton.onClick.AddListener(() => Setup(prevDialog));
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


