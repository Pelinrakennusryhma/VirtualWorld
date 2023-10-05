using Characters;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class DialogPanel : MonoBehaviour
    {
        [SerializeField] TMP_Text nameText;
        [SerializeField] TMP_Text titleText;
        [SerializeField] TMP_Text description;
        public void Setup(NPC npc)
        {
            nameText.text = npc.Data.fullName;
            titleText.text = npc.Data.title;
        }
    }
}


