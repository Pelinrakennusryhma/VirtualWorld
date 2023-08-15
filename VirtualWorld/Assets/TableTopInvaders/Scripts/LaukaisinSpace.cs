using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaukaisinSpace : MonoBehaviour
{
    // Tykin laukansin objektit laukaisin ja laukaisin2
    public GameObject laukasin;
    public GameObject laukasin2;

    // Muuttuja tykinLaukaisijanLiike alustettu arvoon 0f. Tämä muuttuja liikuttaa tykkiä
    public float tykinLaukaisijanLiike = 0f;

    // Muuttuja vedettyYhteensä alustetaan arvoon 0f. Tällä pidetään tietoa paljonko tykki on liikkunut.
    public float vedettyYhteensä = 0f;
    
    // Totuusmuuttuja vapautettu alustetaan arvoon false. Tällä muuttujalla pidetään tietoa onko laukaisin nappi vapautetttu.
    public bool vapautettu = false;


    // GameObject muuttuja kuulaPrefab pitää sisällään ammuttavan kuulan.
    public GameObject kuulaPrefab;

    // Transform muuttuja kuulaSpawn ja kuulaSpawn pitävät sisällään paikat mistä kuulat laukaistaan.
    public Transform kuulaSpawn;
    public Transform kuulaSpawn2;
    // Float muuttuja kuulaSpeed alustetaan arvoon 30f. Tämä muuttuja kertoo mikä on kuulan aloitus nopeus.
    public float kuulaSpeed = 15f;

    // Float muuttuja kuulaSpeedMultiple alustetaan arvoon 0f. Tämä muuttuja on todellinen viimeinen nopeus millä kuulat lentää.
    public float kuulaSpeedMultiple = 0f;

    // Float muuttuja lifeTime alustetaan arvoon 3f. Tämä muuttuja kertoo minkä ajan päästä kuula tuhotaan.
    public float lifeTime = 3f;


    void Start() {
    }

    // Luodaan julkinen metodi Fire joka ottaa vastaan float arvon kuulanAmpumisNopeus
    public void Fire(float kuulaAmpumisNopeus)
    {
        // Tehdään kuulan prefabistä GameObjectit kuula ja kuula2
        GameObject kuula = Instantiate(kuulaPrefab);

        GameObject kuula2 = Instantiate(kuulaPrefab);

        // Estää GameObject kuula:n Collider componentin ja Transform kuulaSpawn laukaisijan componentin colliderin osumatta tosiinsa
        Physics.IgnoreCollision(kuula.GetComponent<Collider>(), kuulaSpawn.parent.GetComponent<Collider>());

        // Estää GameObject kuula2:n Collider componentin ja Transform kuulaSpawn2 laukaisijan componentin colliderin osumatta tosiinsa
        Physics.IgnoreCollision(kuula2.GetComponent<Collider>(), kuulaSpawn2.parent.GetComponent<Collider>());
        
        // Asetetaan kuula:n positioniksi (paikka) kuulaSpawm laukaisupaikan positioni (paikka).
        kuula.transform.position = kuulaSpawn.position;

        // Asetetaan kuula2:n positioniksi (paikka) kuulaSpawm2 laukaisupaikan positioni (paikka).
        kuula2.transform.position = kuulaSpawn2.position;

        // Luodaan Vector3 muuttuja rotation (kierto) ja annetaan sille arvoksi kuula GameObjectin rotation (kierto) eulerAngles
        Vector3 rotation = kuula.transform.rotation.eulerAngles;

        // Luodaan Vector3 muuttuja rotation2 (kierto) ja annetaan sille arvoksi kuula2 GameObjectin rotation (kierto) eulerAngles
        Vector3 rotation2 = kuula2.transform.rotation.eulerAngles;

        // Asetetaan kuula rotationiin (kierto) Quaternion.Euler kiertoineen. 
        kuula.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);
        
        // Asetetaan kuula2 rotationiin (kierto) Quaternion.Euler kiertoineen.
        kuula2.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);

        // Haetaan GameObject kuulaille Rigitbody ja asetetaan sille AddForce ja se ForceMode.Impulse:na
        kuula.GetComponent<Rigidbody>().AddForce(kuulaSpawn.forward * kuulaAmpumisNopeus, ForceMode.Impulse);
        
        // Haetaan GameObject kuula2lle Rigitbody ja asetetaan sille AddForce ja se ForceMode.Impulse:na
        kuula2.GetComponent<Rigidbody>().AddForce(kuulaSpawn2.forward * kuulaAmpumisNopeus, ForceMode.Impulse);

        // Kutsutaan metodia joka tuhoaa kuulan tietynajan kuluttua
            // StartCoroutine(DestroykuulaAfterTime(kuula, lifeTime));
            // StartCoroutine(DestroykuulaAfterTime(kuula2, lifeTime));
    }
    
    // Metodi joka tuhoaa kuulan tietynajan kuluttua
    private IEnumerator DestroykuulaAfterTime (GameObject kuula, float delay)
    {
        // Odottaa saadun ajan
        yield return new WaitForSeconds(delay);

        // Tuhoaa kuulan
        Destroy(kuula);
    }
    void Update()
    {

        // Toteutuu niinkauan kun "Jump eli space" näppäin on painettu ja pohjassa
        if (Input.GetButton("Jump"))
        {
            // Asetetaan tykinLaukaisijanLiike arvoksi 0.7f
            tykinLaukaisijanLiike = 0.1f;
        // Toteuttuu niinkauan kun vedettyYhteensä on pienempi kuin 4    
        if(vedettyYhteensä < 3.8f) {
            // Asetetaan vedettyYhteensä arvoksi vedettyYhteensä + tykinLaukaisijanLiike
            vedettyYhteensä = vedettyYhteensä + tykinLaukaisijanLiike;
            // Asetetaan laukaisin ja laukaisin2 position (paikaksi) Vector3 jonka z-akselia vähennetään tykinLaukaisijanLiike nopeudella
            laukasin.transform.position = new Vector3(laukasin.transform.position.x,laukasin.transform.position.y, laukasin.transform.position.z - tykinLaukaisijanLiike);
            laukasin2.transform.position = new Vector3(laukasin2.transform.position.x,laukasin2.transform.position.y, laukasin2.transform.position.z - tykinLaukaisijanLiike);
            // Asetetaa kuulanSpeedMultiple arvoksi kuulanSpeed * vedettyYhteensä. Tällä arvolla ammutaan kuula.
            kuulaSpeedMultiple = kuulaSpeed * vedettyYhteensä;
            // Asetaan vapautettu arvoksi true(tosi).
            vapautettu = true;
        }

        // MuutenJos arvo vapautettu on true(tosi).   
        } else if(vapautettu) {
            // Alustetaan float arvo i = 0
            float i = 0;
            // Toistetaan niin kauan kuin i on pienempi kuin vedettyYhteensä
            while (i < vedettyYhteensä) {
                // Asetetaan laukaisin ja laukaisin2 position (paikaksi) Vector3 jonka z-akselia lisätään tykinLaukaisijanLiike nopeudella
                laukasin.transform.position = new Vector3(laukasin.transform.position.x,laukasin.transform.position.y, laukasin.transform.position.z + tykinLaukaisijanLiike);
                laukasin2.transform.position = new Vector3(laukasin2.transform.position.x,laukasin2.transform.position.y, laukasin2.transform.position.z + tykinLaukaisijanLiike);
                // kasvatetaan i = i + tykinLaukaisijanLiike
                i = i + tykinLaukaisijanLiike;
            }
            
            // Estetään laukaisin ja laukaisin2 menemästä liian sisälle.
            laukasin.transform.position = new Vector3(laukasin.transform.position.x,laukasin.transform.position.y, -20);
            laukasin2.transform.position = new Vector3(laukasin2.transform.position.x,laukasin2.transform.position.y, -20);
            
            // Asetetaan vedettyYhteensä arvoksi 0f, jotta voidaan uudestaan liikuttaa laukasinta
            vedettyYhteensä = 0f;
            // Asetataan vapautettu arvoksi false (epätosi)
            vapautettu = false;
            // Kutsutaan metodia Fire ja annetaan parametriksi kuulaSpeedMultiple
            Fire(kuulaSpeedMultiple);
        }

    }
}
