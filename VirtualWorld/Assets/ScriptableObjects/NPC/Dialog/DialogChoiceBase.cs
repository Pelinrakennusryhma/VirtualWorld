using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialog
{
    public abstract class DialogChoiceBase : ScriptableObject
    {
        //[Tooltip("Under which dialog this one is.")]
        //public DialogChoice parentDialogChoice;
        [TextArea(3, 30)]
        public string text;
        //[Tooltip("Clickable sub dialog choices that are under this one.")]
        //public List<DialogChoice> childDialogChoices;
        //[Tooltip("Clickable quests that are under this one.")]
        //public List<Quests.Quest> quests;
    }
}
