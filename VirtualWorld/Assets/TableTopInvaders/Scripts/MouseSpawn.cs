using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSpawn : MonoBehaviour
{
    public GameObject keila;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
/*             Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos); */
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(keila.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            Instantiate(keila, worldPosition, Quaternion.identity);
        }
    }
}
