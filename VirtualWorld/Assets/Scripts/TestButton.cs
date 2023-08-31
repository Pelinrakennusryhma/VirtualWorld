using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButton : MonoBehaviour
{
   public void OnClick()
    {
        Debug.LogWarning("we clicked test button " + Time.time);
        //Debug.Break();
    }
}
