using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seinä : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Keila")) {
            Debug.Log("Toimii121314124");
        }
    }
}
