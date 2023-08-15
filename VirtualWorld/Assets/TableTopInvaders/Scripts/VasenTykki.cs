using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VasenTykki : MonoBehaviour
{
    // Tykin laukansin objektit vasenlaukaisin ja laukaisin2
    public GameObject vasenLaukaisin;


    // Muuttuja tykinVasemmanLaukaisijanLiike alustettu arvoon 0f. Tämä muuttuja liikuttaa tykkiä
    public float tykinVasemmanLaukaisijanLiike = 0f;


    // Muuttuja vedettyVasentaTykkiäYhteensä alustetaan arvoon 0f. Tällä pidetään tietoa paljonko tykki on liikkunut.
    public float vedettyVasentaTykkiäYhteensä = 0f;

    
    // Totuusmuuttuja vapautettu alustetaan arvoon false. Tällä muuttujalla pidetään tietoa onko laukaisin nappi vapautetttu.
    public bool vapautettu = false;


    // GameObject muuttuja kuulaPrefab pitää sisällään ammuttavan kuulan.
    public GameObject kuulaPrefab;

    // Transform muuttuja kuulaSpawn ja kuulaSpawn pitävät sisällään paikat mistä kuulat laukaistaan.
    public Transform kuulaSpawnVasen;

    // Float muuttuja kuulaSpeed alustetaan arvoon 30f. Tämä muuttuja kertoo mikä on kuulan aloitus nopeus.
    public float kuulaSpeed = 15f;

    // Float muuttuja kuulaSpeedMultiple alustetaan arvoon 0f. Tämä muuttuja on todellinen viimeinen nopeus millä kuulat lentää.
    public float kuulaSpeedMultiple = 0f;

    // Float muuttuja lifeTime alustetaan arvoon 3f. Tämä muuttuja kertoo minkä ajan päästä kuula tuhotaan.
    public float lifeTime = 3f;

    public static int kuuliaVasemmassaTykissä = 15;


    private bool startedPullSound;

    public CannonAudio CannonAudio;

    public CannonLights CannonLights;

    // Luodaan julkinen metodi Fire joka ottaa vastaan float arvon kuulanAmpumisNopeus
    public void Fire(float kuulaAmpumisNopeus)
    {
        GameFlowManager.Instance.SoundManager.StopSound("Stopped pulling left cannon");
        startedPullSound = false;

        if(kuuliaVasemmassaTykissä > 0) {
        // Tehdään kuulan prefabistä GameObjectit kuula ja kuula2
        GameObject kuula = Instantiate(kuulaPrefab);


        // Estää GameObject kuula:n Collider componentin ja Transform kuulaSpawnVasen laukaisijan componentin colliderin osumatta tosiinsa
        Physics.IgnoreCollision(kuula.GetComponent<Collider>(), kuulaSpawnVasen.parent.GetComponent<Collider>());


        
        // Asetetaan kuula:n positioniksi (paikka) kuulaSpawnVasen laukaisupaikan positioni (paikka).
        kuula.transform.position = kuulaSpawnVasen.position;



        // Luodaan Vector3 muuttuja rotation (kierto) ja annetaan sille arvoksi kuula GameObjectin rotation (kierto) eulerAngles
        Vector3 rotation = kuula.transform.rotation.eulerAngles;


        // Asetetaan kuula rotationiin (kierto) Quaternion.Euler kiertoineen. 
        kuula.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);
        

        // Haetaan GameObject kuulaille Rigitbody ja asetetaan sille AddForce ja se ForceMode.Impulse:na
        kuula.GetComponent<Rigidbody>().AddForce(kuulaSpawnVasen.forward * kuulaAmpumisNopeus, ForceMode.Impulse);
        

        // Kutsutaan metodia joka tuhoaa kuulan tietynajan kuluttua
            // StartCoroutine(DestroykuulaAfterTime(kuula, lifeTime));
            // StartCoroutine(DestroykuulaAfterTime(kuula2, lifeTime));
        kuuliaVasemmassaTykissä--;

            CannonLights.FireCannon();
            CannonAudio.Fire();
            GameFlowManager.Instance.SoundManager.PlaySound("Fire left cannon");
        }

        else
        {
            GameFlowManager.Instance.SoundManager.PlaySound("Fire EMPTY left cannon");
        }
    }
    
/*     // Metodi joka tuhoaa kuulan tietynajan kuluttua
    private IEnumerator DestroykuulaAfterTime (GameObject kuula, float delay)
    {
        // Odottaa saadun ajan
        yield return new WaitForSeconds(delay);

        // Tuhoaa kuulan
        Destroy(kuula);
    } */

    void Update()
    {
        if (GameFlowManager.PlacingPins
            || GameFlowManager.WaitingForStartTimer)
        {
            return;
        }

        // Toteutuu niinkauan kun "Jump eli space" näppäin on painettu ja pohjassa
        if (Input.GetButton("Fire3"))
        {
            if (!startedPullSound)
            {
                startedPullSound = true;
                GameFlowManager.Instance.SoundManager.PlaySound("Started pulling left cannon");
            }

            // Asetetaan tykinVasemmanLaukaisijanLiike arvoksi 0.7f
            tykinVasemmanLaukaisijanLiike = 0.1f * Time.deltaTime * 140.0f;


            // Toteuttuu niinkauan kun vedettyVasentaTykkiäYhteensä on pienempi kuin 4    
            if (vedettyVasentaTykkiäYhteensä < 3.8f) {
            // Asetetaan vedettyVasentaTykkiäYhteensä arvoksi vedettyVasentaTykkiäYhteensä + tykinLaukaisijanLiike
            vedettyVasentaTykkiäYhteensä = vedettyVasentaTykkiäYhteensä + tykinVasemmanLaukaisijanLiike;
            // Asetetaan vasenLaukaisin position (paikaksi) Vector3 jonka z-akselia vähennetään tykinVasemmanLaukaisijanLiike nopeudella
            vasenLaukaisin.transform.position = new Vector3(vasenLaukaisin.transform.position.x,vasenLaukaisin.transform.position.y, vasenLaukaisin.transform.position.z - tykinVasemmanLaukaisijanLiike);
            // laukasin2.transform.position = new Vector3(laukasin2.transform.position.x,laukasin2.transform.position.y, laukasin2.transform.position.z - tykinLaukaisijanLiike);
            // Asetetaa kuulanSpeedMultiple arvoksi kuulanSpeed * vedettyVasentaTykkiäYhteensä. Tällä arvolla ammutaan kuula.
            kuulaSpeedMultiple = kuulaSpeed * vedettyVasentaTykkiäYhteensä;
            // Asetaan vapautettu arvoksi true(tosi).
            vapautettu = true;
        }

        // MuutenJos arvo vapautettu on true(tosi).   
        } else if(vapautettu) {
            // Alustetaan float arvo i = 0
            float i = 0;
            // Toistetaan niin kauan kuin i on pienempi kuin vedettyVasentaTykkiäYhteensä
            while (i < vedettyVasentaTykkiäYhteensä) {
                // Asetetaan vasenLaukaisin position (paikaksi) Vector3 jonka z-akselia lisätään tykinVasemmanLaukaisijanLiike nopeudella
                vasenLaukaisin.transform.position = new Vector3(vasenLaukaisin.transform.position.x,vasenLaukaisin.transform.position.y, vasenLaukaisin.transform.position.z + tykinVasemmanLaukaisijanLiike);
               // laukasin2.transform.position = new Vector3(laukasin2.transform.position.x,laukasin2.transform.position.y, laukasin2.transform.position.z + tykinLaukaisijanLiike);
                // kasvatetaan i = i + tykinVasemmanLaukaisijanLiike
                i = i + tykinVasemmanLaukaisijanLiike;
            }
            
            // Estetään vasenLaukaisin menemästä liian sisälle.
            vasenLaukaisin.transform.position = new Vector3(vasenLaukaisin.transform.position.x,vasenLaukaisin.transform.position.y, -20);
           // laukasin2.transform.position = new Vector3(laukasin2.transform.position.x,laukasin2.transform.position.y, -20);
            
            // Asetetaan vedettyVasentaTykkiäYhteensä arvoksi 0f, jotta voidaan uudestaan liikuttaa laukasinta
            vedettyVasentaTykkiäYhteensä = 0f;
            // Asetataan vapautettu arvoksi false (epätosi)
            vapautettu = false;
            // Kutsutaan metodia Fire ja annetaan parametriksi kuulaSpeedMultiple
            Fire(kuulaSpeedMultiple);
        }
    }
}
