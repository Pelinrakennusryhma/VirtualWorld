using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KaatuneenKeilanTunnistus : MonoBehaviour
{
    void Start() {
    }
    private void OnCollisionEnter(Collision collision) {
        if(collision.collider.tag == "Lattia") {
            print("hit " + gameObject.name + "!");

            // Selvitetään ehtolauseella pisteiden lisäys. Lisätään pisteet PisteLaskuri luokan pisteet muuttujaan
            if(gameObject.name == "Keila 1") {
                PisteLaskuri.pisteet += 1;
            } else if (gameObject.name == "Keila 2"){
                PisteLaskuri.pisteet += 2;
            } else if (gameObject.name == "Keila 5") {
                PisteLaskuri.pisteet += 5;
            } else if (gameObject.name == "Keila 10") {
                PisteLaskuri.pisteet += 10;
            } else if (gameObject.name == "Keila -2") {
                PisteLaskuri.pisteet -= 2;
            } else if (gameObject.name == "Keila -5") {
                PisteLaskuri.pisteet -= 5;
            }
            Destroy(gameObject);
        }

    }
}
