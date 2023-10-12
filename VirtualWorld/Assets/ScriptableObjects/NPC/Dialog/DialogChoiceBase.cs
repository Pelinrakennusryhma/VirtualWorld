using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialog
{
    public abstract class DialogChoiceBase : ScriptableObject
    {
        [TextArea(3, 30)]
        public string text;
    }
}
