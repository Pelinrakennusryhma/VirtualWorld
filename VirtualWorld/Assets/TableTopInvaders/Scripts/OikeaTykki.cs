using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OikeaTykki : MonoBehaviour
{
    public NewMiniGameInputs Inputs;

    // Tykin laukansin objektit oikealaukasin
    public GameObject oikealaukasin;


    // Muuttuja tykinVasemmanLaukaisijanLiike alustettu arvoon 0f. Tämä muuttuja liikuttaa tykkiä
    public float tykinOikeanLaukaisijanLiike = 0f;

    // Muuttuja vedettyOikeaaTykkiäYhteensä alustetaan arvoon 0f. Tällä pidetään tietoa paljonko tykki on liikkunut.
    public float vedettyOikeaaTykkiäYhteensä = 0f;
    
    // Totuusmuuttuja vapautettu alustetaan arvoon false. Tällä muuttujalla pidetään tietoa onko laukaisin nappi vapautetttu.
    public bool vapautettu = false;


    // GameObject muuttuja kuulaPrefab pitää sisällään ammuttavan kuulan.
    public GameObject kuulaPrefab;

    // Transform muuttuja kuulaSpawn ja kuulaSpawn pitävät sisällään paikat mistä kuulat laukaistaan.
    public Transform kuulaSpawn;

    // Float muuttuja kuulaSpeed alustetaan arvoon 30f. Tämä muuttuja kertoo mikä on kuulan aloitus nopeus.
    public float kuulaSpeed = 15f;

    // Float muuttuja kuulaSpeedMultiple alustetaan arvoon 0f. Tämä muuttuja on todellinen viimeinen nopeus millä kuulat lentää.
    public float kuulaSpeedMultiple = 0f;

    // Float muuttuja lifeTime alustetaan arvoon 3f. Tämä muuttuja kertoo minkä ajan päästä kuula tuhotaan.
    public float lifeTime = 3f;

    public static int kuuliaOikeassaTykissä = 15;

    private bool startedPullSound = false;

    public CannonAudio CannonAudio;
    public CannonLights CannonLights;
    void Start() {
    }

    // Luodaan julkinen metodi Fire joka ottaa vastaan float arvon kuulanAmpumisNopeus
    public void Fire(float kuulaAmpumisNopeus)
    {
        GameFlowManager.Instance.SoundManager.StopSound("Stopped pulling right cannon");
        startedPullSound = false;

        if (kuuliaOikeassaTykissä > 0) {
        // Tehdään kuulan prefabistä GameObjectit kuula ja kuula2
        GameObject kuula = Instantiate(kuulaPrefab);


        // Estää GameObject kuula:n Collider componentin ja Transform kuulaSpawn laukaisijan componentin colliderin osumatta tosiinsa
        Physics.IgnoreCollision(kuula.GetComponent<Collider>(), kuulaSpawn.parent.GetComponent<Collider>());


        
        // Asetetaan kuula:n positioniksi (paikka) kuulaSpawm laukaisupaikan positioni (paikka).
        kuula.transform.position = kuulaSpawn.position;



        // Luodaan Vector3 muuttuja rotation (kierto) ja annetaan sille arvoksi kuula GameObjectin rotation (kierto) eulerAngles
        Vector3 rotation = kuula.transform.rotation.eulerAngles;


        // Asetetaan kuula rotationiin (kierto) Quaternion.Euler kiertoineen. 
        kuula.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);
        

        // Haetaan GameObject kuulaille Rigitbody ja asetetaan sille AddForce ja se ForceMode.Impulse:na
        kuula.GetComponent<Rigidbody>().AddForce(kuulaSpawn.forward * kuulaAmpumisNopeus, ForceMode.Impulse);
        

        // Kutsutaan metodia joka tuhoaa kuulan tietynajan kuluttua
            // StartCoroutine(DestroykuulaAfterTime(kuula, lifeTime));
            // StartCoroutine(DestroykuulaAfterTime(kuula2, lifeTime));
        kuuliaOikeassaTykissä--;

            CannonLights.FireCannon();
            CannonAudio.Fire();
            GameFlowManager.Instance.SoundManager.PlaySound("Fire right cannon");
        }

        else
        {
            GameFlowManager.Instance.SoundManager.PlaySound("Fire EMPTY right cannon");
        }
    }


    

    void Update()
    {

        if (GameFlowManager.PlacingPins
            || GameFlowManager.WaitingForStartTimer)
        {
            return;
        }

        // Toteutuu niinkauan kun "Jump eli space" näppäin on painettu ja pohjassa
        if (Inputs.fire2)
        {
            if (!startedPullSound)
            {
                startedPullSound = true;
                GameFlowManager.Instance.SoundManager.PlaySound("Started pulling right cannon");
            }

            // Asetetaan tykinOikeanLaukaisijanLiike arvoksi 0.7f
            tykinOikeanLaukaisijanLiike = 0.1f * Time.deltaTime * 140.0f;
            // Toteuttuu niinkauan kun vedettyOikeaaTykkiäYhteensä on pienempi kuin 4    
            if (vedettyOikeaaTykkiäYhteensä < 3.8f) {
            // Asetetaan vedettyOikeaaTykkiäYhteensä arvoksi vedettyOikeaaTykkiäYhteensä + tykinLaukaisijanLiike
            vedettyOikeaaTykkiäYhteensä = vedettyOikeaaTykkiäYhteensä + tykinOikeanLaukaisijanLiike ;
            // Asetetaan oikealaukasin position (paikaksi) Vector3 jonka z-akselia vähennetään tykinOikeanLaukaisijanLiike nopeudella
            oikealaukasin.transform.position = new Vector3(oikealaukasin.transform.position.x,oikealaukasin.transform.position.y, oikealaukasin.transform.position.z - tykinOikeanLaukaisijanLiike);
            // laukasin2.transform.position = new Vector3(laukasin2.transform.position.x,laukasin2.transform.position.y, laukasin2.transform.position.z - tykinLaukaisijanLiike);
            // Asetetaa kuulanSpeedMultiple arvoksi kuulanSpeed * vedettyOikeaaTykkiäYhteensä. Tällä arvolla ammutaan kuula.
            kuulaSpeedMultiple = kuulaSpeed * vedettyOikeaaTykkiäYhteensä;
            // Asetaan vapautettu arvoksi true(tosi).
            vapautettu = true;
        }

        // MuutenJos arvo vapautettu on true(tosi).   
        } else if(vapautettu) {
            // Alustetaan float arvo i = 0
            float i = 0;
            // Toistetaan niin kauan kuin i on pienempi kuin vedettyOikeaaTykkiäYhteensä
            while (i < vedettyOikeaaTykkiäYhteensä) {
                // Asetetaan oikealaukasin position (paikaksi) Vector3 jonka z-akselia lisätään tykinOikeanLaukaisijanLiike nopeudella
                oikealaukasin.transform.position = new Vector3(oikealaukasin.transform.position.x,oikealaukasin.transform.position.y, oikealaukasin.transform.position.z + tykinOikeanLaukaisijanLiike);
               // laukasin2.transform.position = new Vector3(laukasin2.transform.position.x,laukasin2.transform.position.y, laukasin2.transform.position.z + tykinLaukaisijanLiike);
                // kasvatetaan i = i + tykinOikeanLaukaisijanLiike
                i = i + tykinOikeanLaukaisijanLiike;
            }
            
            // Estetään oikealaukasin menemästä liian sisälle.
            oikealaukasin.transform.position = new Vector3(oikealaukasin.transform.position.x,oikealaukasin.transform.position.y, -20);
           // laukasin2.transform.position = new Vector3(laukasin2.transform.position.x,laukasin2.transform.position.y, -20);
            
            // Asetetaan vedettyOikeaaTykkiäYhteensä arvoksi 0f, jotta voidaan uudestaan liikuttaa laukasinta
            vedettyOikeaaTykkiäYhteensä = 0f;
            // Asetataan vapautettu arvoksi false (epätosi)
            vapautettu = false;
            // Kutsutaan metodia Fire ja annetaan parametriksi kuulaSpeedMultiple
            Fire(kuulaSpeedMultiple);
        }

    }
}
