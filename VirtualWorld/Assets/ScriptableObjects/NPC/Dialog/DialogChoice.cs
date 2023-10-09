using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialog
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Dialog/DialogChoice", order = 2)]
    public class DialogChoice : ScriptableObject
    {
        [Tooltip("Under which dialog this one is.")]
        public DialogChoice parentDialogChoice;
        public string title;
        [TextArea(3, 30)]
        public string text;
        [Tooltip("Clickable sub dialog choices that are under this one.")]
        public List<DialogChoice> childDialogChoices;
        [Tooltip("Clickable quests that are under this one.")]
        public List<Quests.Quest> quests;
    }
}
