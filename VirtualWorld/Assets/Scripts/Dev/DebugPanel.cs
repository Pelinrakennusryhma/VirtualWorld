using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quests;

namespace Dev
{
    public class DebugPanel : MonoBehaviour
    {
        public void ResetQuests()
        {
            QuestManager.Instance.ClearQuests();
        }
    }
}
