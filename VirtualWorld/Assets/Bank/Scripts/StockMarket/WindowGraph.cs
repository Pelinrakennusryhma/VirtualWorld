using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WindowGraph : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;
    public List<double> priceHistory;
    //public List<int> priceHistoryInt;

    private void Start()
    {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();

        //Lisää viimeisimmät hinnat. Näytettyjen hintojen määrää voi muuttaa vaihtamalla historyLength. Hintoja pitää myös olla sitten tarpeeksi alussa. Hintojen aloitusmäärää pystyy lisätä StockScriptin InitializePrices funktiosta.
        List<float> valueList = new List<float>();
        for (int historyLength = 90; historyLength > 0; historyLength--)
        {
            valueList.Add((float)priceHistory[priceHistory.Count - historyLength]);
        }
        ShowGraph(valueList);
    }

    //Lisää pisteen tiettyyn kohtaan.
    private GameObject CreateCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }
    private void ShowGraph(List<float> valueList)
    {
        float graphHeight = graphContainer.sizeDelta.y;
        float yMaximum = valueList.Max();
        float yMinimum = valueList.Min();

        yMaximum = yMaximum + ((yMaximum - yMinimum) * 0.3f);
        yMinimum = yMinimum - ((yMaximum - yMinimum) * 0.3f);
        //float xSize = 11.5f;
        //float xSize = 5.8f;
        float xSize = 4f;

        GameObject lastCircleGameObject = null;
        for (int i = 0; i < valueList.Count; i++)
        {
            float xPosition = xSize + i * xSize;
            float yPosition = ((valueList[i] - yMinimum) / (yMaximum - yMinimum)) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            if(lastCircleGameObject != null)
            {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastCircleGameObject = circleGameObject;
        }
    }

    //Lisää pisteitten väleihin viivat.
    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        dir = dir.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;
        rectTransform.localEulerAngles = new Vector3(0, 0, angle);
    }
}
