using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
