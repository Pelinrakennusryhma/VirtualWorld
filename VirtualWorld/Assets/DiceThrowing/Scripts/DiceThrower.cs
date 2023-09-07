using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DiceMinigame
{
    public class DiceThrower : MonoBehaviour
    {
        public Camera cam;
        public float throwingForce;
        private EventManager eventManager;
        private DiceManager diceManager;

        void Awake()
        {
            eventManager = GetComponent<EventManager>();
            diceManager = GetComponent<DiceManager>();
            eventManager.EventThrowPressed.AddListener(DiceThrownEventHandler);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        void DiceThrownEventHandler()
        {
            StartThrow();
        }

        void StartThrow()
        {
            foreach (GameObject dice in diceManager.activeDice)
            {
                DiceScore diceScore = dice.GetComponent<DiceScore>();
                diceScore.Throw(throwingForce, cam.transform.forward);
            }
        }
    }
}
