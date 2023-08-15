using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeilapaikkojenAsettelu : MonoBehaviour
{
    // Luodaan julkinen GameObject lista nimeltään keilaAlusta
    public GameObject[] keilaAlusta;
    // Start is called before the first frame update
    void Start()
    {
        // Asetetaan Vector 3 koordinaatit keilaAlusta[0]
        keilaAlusta[0].transform.position = new Vector3(22,3.5f,26);
        // Asetetaan keila paikalleen
        Instantiate(keilaAlusta[0], keilaAlusta[0].transform.position, keilaAlusta[0].transform.rotation);

        keilaAlusta[1].transform.position = new Vector3(30,3.5f,26);
        // Asetetaan keila paikalleen
        Instantiate(keilaAlusta[1], keilaAlusta[1].transform.position, keilaAlusta[1].transform.rotation);

        keilaAlusta[2].transform.position = new Vector3(22,3.5f,35);
        // Asetetaan keila paikalleen
        Instantiate(keilaAlusta[2], keilaAlusta[2].transform.position, keilaAlusta[2].transform.rotation);

        keilaAlusta[3].transform.position = new Vector3(35,4.4f,35);
        // Asetetaan keila paikalleen
        Instantiate(keilaAlusta[3], keilaAlusta[3].transform.position, keilaAlusta[3].transform.rotation);

        keilaAlusta[4].transform.position = new Vector3(20,4,15);
        // Asetetaan keila paikalleen
        Instantiate(keilaAlusta[4], keilaAlusta[4].transform.position, keilaAlusta[4].transform.rotation);

        keilaAlusta[5].transform.position = new Vector3(40,4.4f,15);
        // Asetetaan keila paikalleen
        Instantiate(keilaAlusta[5], keilaAlusta[5].transform.position, keilaAlusta[5].transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
