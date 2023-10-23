using Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quests
{
    public class QuestTriggerEnter : QuestTrigger
    {
        private void OnTriggerEnter(Collider other)
        {
            // When client loads and CharacterManager is not yet initialized, return
            // to avoid errors from any objects already in the trigger area
            if(CharacterManager.Instance == null)
            {
                return;
            }

            if(other.gameObject == CharacterManager.Instance.OwnedCharacter)
            {
                Debug.Log("player entered");
                ProgressQuestStep();
            }
        }
    }
}
