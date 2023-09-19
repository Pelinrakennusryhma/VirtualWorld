using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    private TextMeshProUGUI tooltipText;
    public GameSystem gameSystem;
    [SerializeField] RectTransform rectTransform;
    void Start()
    {
        tooltipText = GetComponentInChildren<TextMeshProUGUI>();
        gameObject.SetActive(false);
    }

    //Siirtää tooltipin osoittimen luokse, ja siirtää pivotita riippuen missä osassa ruutua tooltip sijaitsee.
    private void LateUpdate()
    {
        transform.position = Input.mousePosition;
        if(Input.mousePosition.y > Screen.height / 2)
        {
            rectTransform.pivot = new Vector2(rectTransform.pivot.x, 1);
        }
        else
        {
            rectTransform.pivot = new Vector2(rectTransform.pivot.x, 0);
        }
        if (Input.mousePosition.x > Screen.width / 2)
        {
            rectTransform.pivot = new Vector2(1, rectTransform.pivot.y);
        }
        else
        {
            rectTransform.pivot = new Vector2(0, rectTransform.pivot.y);
        }
    }

    //Asettaa tooltippiin tekstit
    public void SetTooltip(Item item) 
    {
        string statText = "";
        if(item.stats.Count > 0)
        {
            foreach(var stat in item.stats)
            {
                if(stat.Key == "Value")
                {
                    statText += stat.Key.ToString() + ": " + stat.Value.ToString("C", gameSystem.culture) + "\n";
                }
                else
                {
                    statText += stat.Key.ToString() + ": " + stat.Value.ToString() + "\n";
                }
            }
        }
        string tooltip = string.Format("<b>{0}</b>\n{1}\n\n<b>{2}</b>",
            item.name, item.description, statText);
        tooltipText.text = tooltip;
        gameObject.SetActive(true);
    }
}
