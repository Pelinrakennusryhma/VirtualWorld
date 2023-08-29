using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

namespace DiceMinigame
{
    public class DiceScore : MonoBehaviour
    {
        [System.NonSerialized]
        public bool thrown = false;
        [SerializeField] float torque = 0f;
        protected GameObject sides;
        protected Rigidbody rb;
        protected bool stopped = false;
        protected EventManager eventManager;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            sides = transform.GetChild(0).gameObject;
        }

        void Update()
        {
            if (thrown && !stopped && rb.IsSleeping())
            {
                Stop();
            }
        }

        public virtual void Init(EventManager _eventManager)
        {
            eventManager = _eventManager;
        }

        public virtual void Throw(float throwingForce, Vector3 throwDirection)
        {
            thrown = true;

            rb.constraints = RigidbodyConstraints.None;

            rb.useGravity = true;
            rb.AddForce(throwDirection * throwingForce);
            //Vector3 throwTorque = new(
            //    Random.Range(-torque, torque),
            //    Random.Range(-torque, torque),
            //    Random.Range(-torque, torque));
            //rb.AddTorque(throwTorque, ForceMode.Impulse);
        }

        protected virtual void Stop()
        {
            stopped = true;
            eventManager.DiceStopped(this);
        }

        public virtual int GetResult()
        {
            GameObject highestSide = sides.transform.GetChild(0).gameObject;
            foreach (Transform child in sides.transform)
            {
                if (child.position.y > highestSide.transform.position.y)
                {
                    highestSide = child.gameObject;
                }
            }
            int score = int.Parse(highestSide.name);
            return score;
        }
    }
}
