using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{

    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/NPC/NPCData", order = 1)]
    public class NPCData : ScriptableObject
    {
        public string fullName;
        public string title;
        public DialogChoice mainDialog;

    }
}
