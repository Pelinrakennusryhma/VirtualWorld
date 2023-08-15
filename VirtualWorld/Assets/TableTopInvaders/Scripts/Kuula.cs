using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kuula : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision) {
        if(collision.collider.tag == "Seinä") {
            print("hit " + gameObject.name + "!");
            Destroy(gameObject);
        }

    }
}
