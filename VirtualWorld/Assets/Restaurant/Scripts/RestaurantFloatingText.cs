using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RestaurantFloatingText : MonoBehaviour
{
    TextMeshProUGUI text;
    private void Start()
    {
        gameObject.transform.position = Input.mousePosition;
        text = gameObject.GetComponent<TextMeshProUGUI>();
        StartCoroutine(Movement());
    }
    private IEnumerator Movement()
    {
        int i = 0;
        while (true)
        {
            gameObject.transform.position += new Vector3(0, 1, 0);
            byte transparency = Convert.ToByte(text.faceColor.a - 2.49);
            text.faceColor = new Color32(0, 214, 19, transparency);
            if (i == 100)
            {
                Destroy(gameObject);
            }
            i++;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
