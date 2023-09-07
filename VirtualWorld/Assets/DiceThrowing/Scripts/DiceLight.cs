using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceMinigame
{
    public class DiceLight : MonoBehaviour
    {
        public float maxRange = 17f;
        public float brightenSpeed = 1f;
        private Light highlight;
        private Transform dice;
        private float elapsedTime = 0f;

        void Awake()
        {
            highlight = GetComponent<Light>();
        }

        void Update()
        {
            if (dice && highlight)
            {
                transform.position = dice.position + Vector3.up * 2;
                transform.LookAt(dice.position);

                if (elapsedTime < brightenSpeed)
                {
                    float lerpFactor = Mathf.SmoothStep(0f, 1f, elapsedTime / brightenSpeed);
                    elapsedTime += Time.deltaTime;

                    highlight.range = Mathf.Lerp(0, maxRange, lerpFactor);
                }
            }
        }

        public void Setup(Transform _dice)
        {
            dice = _dice;
            highlight.range = 0;
            transform.SetParent(dice);
        }
    }
}
