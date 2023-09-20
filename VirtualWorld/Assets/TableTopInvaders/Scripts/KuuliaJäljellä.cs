using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KuuliaJäljellä : MonoBehaviour
{
    // Text muuttujat asemmanTykinKeilojaJäljellä ja oikeanTykinKeilojaJäljellä pitävät sisällään näytön tekstialueen
    public Text vasemmanTykinKeilojaJäljellä;
    public Text oikeanTykinKeilojaJäljellä;

    public InGameUI inGameUI;

    // Start is called before the first frame update
    void Start()
    {
        inGameUI = FindObjectOfType<InGameUI>();


        inGameUI.SetLeftCannonBalls(VasenTykki.kuuliaVasemmassaTykissä);
        inGameUI.SetRightCannonBalls(OikeaTykki.kuuliaOikeassaTykissä);
    }

    // Update is called once per frame
    void Update()
    {
        // Näyttään näytöllä tykkejen kuulamäärän
        vasemmanTykinKeilojaJäljellä.text = "Kuulia Vasemmassa Tykissä: " + VasenTykki.kuuliaVasemmassaTykissä.ToString();
        oikeanTykinKeilojaJäljellä.text = "Kuulia Oikeassa Tykissä: " + OikeaTykki.kuuliaOikeassaTykissä.ToString();

        inGameUI.SetLeftCannonBalls(VasenTykki.kuuliaVasemmassaTykissä);
        inGameUI.SetRightCannonBalls(OikeaTykki.kuuliaOikeassaTykissä);
    }
}
