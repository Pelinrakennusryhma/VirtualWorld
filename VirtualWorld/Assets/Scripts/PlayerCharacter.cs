using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{

    public bool IsEnabled;

    public Vector3 HidePosition;

    public void Awake()
    {
        IsEnabled = true;
    }

    public void DisableCharacter()
    {
        IsEnabled = false;
        HidePosition = transform.position;
        transform.parent.gameObject.SetActive(false);
    }

    public void EnableCharacter()
    {
        IsEnabled = true;
        transform.parent.gameObject.SetActive(true);
        transform.position = HidePosition;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!IsEnabled)
        {
            transform.position = HidePosition;
        }
    }
}
