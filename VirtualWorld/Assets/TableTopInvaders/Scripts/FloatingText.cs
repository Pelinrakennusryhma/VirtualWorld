using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    // muuttuja DestoroyTime ajalle, koska pisteteksti häipyy näytöltä
    public float DestroyTime = 3f;
    public Vector3 Offset = new Vector3(0, 2, 0);
    public Vector3 RandomizeIntensity = new Vector3(0.5f,0,0);
    
    // Start is called before the first frame update
    void Start()
    {
        // Tuhoo objektin (gameObject) ajan päästä (DestroyTime)
        Destroy(gameObject, DestroyTime);
        transform.localPosition += Offset;
        transform.localPosition += new Vector3(Random.Range(-RandomizeIntensity.x, RandomizeIntensity.x),
        Random.Range(-RandomizeIntensity.y, RandomizeIntensity.y),
        Random.Range(-RandomizeIntensity.z, RandomizeIntensity.z));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
