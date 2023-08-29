using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceMinigame
{
    public class SmoothRotation : MonoBehaviour
    {
        float maxSpeed = 0.3f;
        float maxAngle = 120f;
        float minSpeed = 0.05f;
        float minAngle = 15f;

        private float speed = 0f;
        private float startX = 0f;
        private float startY = 0f;
        private float startZ = 0f;
        private float RotAngleX = 0;
        private float RotAngleY = 0;
        private float RotAngleZ = 0;
        private DiceScore diceScore;

        private void Start()
        {
            diceScore = GetComponent<DiceScore>();
            speed = Random.Range(minSpeed, maxSpeed);
            startX = Random.Range(-180, 180);
            startY = Random.Range(-180, 180);
            startZ = Random.Range(-180, 180);
            RotAngleX = startX + Random.Range(minAngle, maxAngle);
            RotAngleY = startY + Random.Range(minAngle, maxAngle);
            RotAngleZ = startZ + Random.Range(minAngle, maxAngle);
        }

        void Update()
        {
            if (diceScore && diceScore.thrown)
            {
                Destroy(this);
            }

            float rX = Mathf.SmoothStep(startX, RotAngleX, Mathf.PingPong(Time.time * speed, 1));
            float rY = Mathf.SmoothStep(startY, RotAngleY, Mathf.PingPong(Time.time * speed, 1));
            float rZ = Mathf.SmoothStep(startZ, RotAngleZ, Mathf.PingPong(Time.time * speed, 1));
            transform.rotation = Quaternion.Euler(rX, rY, rZ);
        }
    }
}
