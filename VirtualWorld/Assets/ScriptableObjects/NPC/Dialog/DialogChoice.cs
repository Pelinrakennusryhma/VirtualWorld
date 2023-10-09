using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Dialog/DialogChoice", order = 2)]
public class DialogChoice : ScriptableObject
{
    public DialogChoice parentDialogChoice;
    public string title;
    [TextArea(3, 30)]
    public string text;
    public List<DialogChoice> childDialogChoices;

}
