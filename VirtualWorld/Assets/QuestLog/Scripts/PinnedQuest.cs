using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PinnedQuest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questNameText;
    [SerializeField] private TextMeshProUGUI questObjectiveText;
    [SerializeField] private TextMeshProUGUI questProgressText;

    public void SetQuestName(string questName)
    {
        questNameText.text = questName;
    }
    public void SetQuestObjective(string questObjective)
    {
        questObjectiveText.text = questObjective;
    }
    public void SetQuestProgress(int current, int max)
    {
        if(max != 0)
        {
            questProgressText.text = current + "/" + max;
        }
        else
        {
            questProgressText.text = "";
        }

    }

    public void UnpinQuest()
    {
        if(gameObject.transform.parent.childCount == 1)
        {
            gameObject.transform.parent.gameObject.SetActive(false);
        }
        Destroy(gameObject);
    }
}
