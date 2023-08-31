using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class QuickMovementTester : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        transform.position += new Vector3(1.5f * horizontal * Time.deltaTime, 
                                          1.5f * vertical * Time.deltaTime, 
                                          0);

        Debug.Log("Horizontal is " + horizontal + " vertical is " + vertical);
    }
}
