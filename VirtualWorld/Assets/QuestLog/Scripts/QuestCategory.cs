using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestCategory : MonoBehaviour
{
    private QuestLog questLog;
    private string category;
    public void InitializeCategory(string category)
    {
        gameObject.name = category;
        gameObject.transform.GetComponentInChildren<TextMeshProUGUI>().text = category;
        questLog = GameObject.Find("QuestLog").GetComponent<QuestLog>();
        this.category = category;
    }

    public void SelectCategory()
    {
        foreach(Transform transform in gameObject.transform.parent.transform.GetComponentInChildren<Transform>())
        {
            transform.GetComponent<Image>().color = new Color(0.757f, 0.757f, 0.757f);
        }
        gameObject.GetComponent<Image>().color = Color.white;
        questLog.ShowQuestsByCategory(category);
    }
}
