using Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialog
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Dialog/DialogChoiceRoot", order = 0)]
    public class DialogChoiceRoot : DialogChoiceBase
    {
        [Tooltip("Clickable sub dialog choices that are under this one.")]
        public List<DialogChoiceWithTitle> childDialogChoices;
        [Tooltip("Clickable quests that are under this one.")]
        public List<Quest> quests;
    }
}
