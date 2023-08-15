using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sekuntia : MonoBehaviour
{
    // Alustetaan Text muuttuja sekuntejaAsetettu
    public Text sekunttejaAsetettu;
    // Alustetaan int muuttuja aikaSekunneissa arvoon 0
    int aikaSekunneissa = 0;

    void Start() {
        // Näytetään näytöllä arvo 60
        sekunttejaAsetettu.text = "60";
    }
    // Luodaan julkinen metodi sekuntia joka ottaa vastaan float arvon sekuntia sekuntia(float sekuntia)
    public void sekuntia(float sekuntia) {
        // Kerrotaan vastaanotettu arvo 60 ja asetetaan se muuttujaan sekuntia
        sekuntia = sekuntia * 60;
        // Näytetään asetettu arvo näytöllä
        sekunttejaAsetettu.text = sekuntia.ToString();
        // Muunnetaan float arvo int arvoksi ja asetetaan se muuttujaan aikaSekunneissa
        aikaSekunneissa = (int) sekuntia;
        // Näytetään muunnettu arvo näytöllä
        sekunttejaAsetettu.text = aikaSekunneissa.ToString();
        // Asetetaan TimerCountdown luokan muuttujaan secondsLeft arvoksi aikaSekunneissa
        TimerCountdown.secondsLeft = aikaSekunneissa;
    }
}
