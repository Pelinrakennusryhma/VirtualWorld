using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialog
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Dialog/DialogChoiceSub", order = 1)]
    public class DialogChoiceSub : DialogChoiceWithTitle
    {
        [Tooltip("Under which dialog this one is.")]
        public DialogChoiceBase parentDialogChoice;
        [Tooltip("Clickable sub dialog choices that are under this one.")]
        public List<DialogChoiceWithTitle> childDialogChoices;
    }
}
