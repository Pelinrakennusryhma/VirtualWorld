using Characters;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ColorFader : MonoBehaviour
{
    [SerializeField] TMP_Text infoText;
    [SerializeField] float fadeInDuration = 1.5f;
    [SerializeField] float holdColorDuration = 1.5f; 
    [SerializeField] float fadeOutDuration = 1.5f;
    [SerializeField] float pauseDuration = 1f;
    Color originalColor;
    Color fadedColor;
    Queue<string> infoQueue = new Queue<string>();
    Coroutine runningRoutine = null;

    void Start()
    {
        PlayerEvents.Instance.EventInformationReceived.AddListener(OnInformationReceived);
        infoText.text = "";
        originalColor = infoText.color;
        fadedColor = new Color(infoText.color.r, infoText.color.g, infoText.color.b, 0);
        infoText.color = fadedColor;
    }

    private void Update()
    {
        if(infoQueue.Count > 0 && runningRoutine == null)
        {
            runningRoutine = StartCoroutine(ShowInfoText(infoQueue.Dequeue()));
        }
    }

    void OnInformationReceived(string infoMsg)
    {
        infoQueue.Enqueue(infoMsg);
    }

    IEnumerator ShowInfoText(string infoMsg)
    {
        infoText.text = infoMsg;

        float counter = 0f;

        while (counter < fadeInDuration)
        {
            counter += Time.deltaTime;
            infoText.color = Color.Lerp(fadedColor, originalColor, counter / fadeInDuration);
            yield return null;
        }
        yield return new WaitForSeconds(holdColorDuration);

        counter = 0f;

        while (counter < fadeOutDuration)
        {
            counter += Time.deltaTime;
            infoText.color = Color.Lerp(originalColor, fadedColor, counter / fadeInDuration);
            yield return null;
        }

        infoText.text = "";
        yield return new WaitForSeconds(pauseDuration);
        runningRoutine = null;
    }
}
