using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pisteet : MonoBehaviour
{
    // Luodaan Text muuttujat pisteetGameOverMenuun ja aikaaKulunutGameOverMenuun
    public Text pisteetGameOverMenuun;
    public Text aikaaKulunutGameOverMenuun;
    // Start is called before the first frame update
    void Start()
    {
        // Näytetään näytöllä pisteet ja kulunut aika
        pisteetGameOverMenuun.text = "Pisteesi: " + PisteLaskuri.pisteet.ToString();

        int fullSeconds = (int) TimerCountdown.aikaaKulunut;

        aikaaKulunutGameOverMenuun.text = "Aikaa kulunut: " + fullSeconds.ToString();
        // Alustetaan TimerCountdown luokan sekunttiaKulunut muuttujan arvo 0
        TimerCountdown.aikaaKulunut = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
