using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMeteor : MonoBehaviour
{

    public Vector3 RotateVector;
    public float Speed;

    // Start is called before the first frame update
    void Start()
    {
        RotateVector = new Vector3(Random.Range(-1.0f, 1.0f),
                                   Random.Range(-1.0f, 1.0f),
                                   Random.Range(-1.0f, 1.0f));
        Speed = Random.Range(-40.0f, 40.0f);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.Rotate(RotateVector, Speed * Time.deltaTime);
    }
}
