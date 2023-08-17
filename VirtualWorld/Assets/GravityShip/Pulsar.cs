using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulsar : MonoBehaviour
{
    public GameObject Laser1;
    public GameObject Laser2;

    public float IntervallTimer;
    public float LaserStartTime;

    [SerializeField] private float OnTime = 0.75f;
    [SerializeField] private float OffTime = 1.25f;

    public float StartInterVallTimer = 0;

    public bool IsOn;

    public GameObject Star;


    public void Awake()
    {
        Laser1.gameObject.SetActive(false);
        Laser2.gameObject.SetActive(false);
        IsOn = false;
        IntervallTimer = StartInterVallTimer;
    }

    private void FixedUpdate()
    {
        Star.gameObject.transform.Rotate(Vector3.forward, 66.6f * Time.deltaTime, Space.World);
    }

    public void Update()
    {
        // Scaling looks pretty bad.

        //float fromZeroToOne = (Mathf.Sin(Time.time * Speed) + 1.0f) / 2.0f;
        //fromZeroToOne = Mathf.PingPong(Time.time, 1.0f);
        //Vector3 scale = new Vector3(fromZeroToOne, 1, 1);
        //Laser1.transform.localScale = scale;
        //Laser2.transform.localScale = scale;    

        IntervallTimer -= Time.deltaTime;

        if (IntervallTimer <= 0)
        {
            if (IsOn)
            {
                Laser1.gameObject.SetActive(false);
                Laser2.gameObject.SetActive(false);
                IntervallTimer = OffTime;
                IsOn = false;
            }

            else
            {
                Laser1.gameObject.SetActive(true);
                Laser2.gameObject.SetActive(true);
                IntervallTimer = OnTime;
                IsOn = true;
                GameManagerGravityShip.Instance.SoundManager.PlayOnPulsar();
            }
        }
    }
}
