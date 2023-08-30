using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceMinigame
{
    public class D100Score : DiceScore
    {
        public GameObject childDicePrefab;
        public D100ChildScore childDiceScore;
        public bool doubleZeroAs100;
        private bool childStopped;

        public void InitChild(GameObject childDice)
        {
            D100ChildScore d100ChildScore = childDice.GetComponent<D100ChildScore>();
            d100ChildScore.InitParent(this);
        }

        public override void Throw(float throwingForce, Vector3 throwDirection)
        {
            base.Throw(throwingForce, throwDirection);
            childDiceScore.Throw(throwingForce, throwDirection);
        }

        protected override void Stop()
        {
            stopped = true;
            if (childStopped)
            {
                eventManager.DiceStopped(this);
            }
        }

        public void ChildStopped()
        {
            childStopped = true;
            if (stopped)
            {
                eventManager.DiceStopped(this);
            }
        }

        public override int GetResult()
        {
            GameObject highestSide = sides.transform.GetChild(0).gameObject;
            foreach (Transform child in sides.transform)
            {
                if (child.position.y > highestSide.transform.position.y)
                {
                    highestSide = child.gameObject;
                }
            }
            int score = int.Parse(highestSide.name) * 10;

            score += childDiceScore.GetResult();
            if (doubleZeroAs100 && score == 0)
            {
                return 100;
            }
            return score;
        }

        private void OnDestroy()
        {
            Destroy(childDiceScore.gameObject);
        }
    }
}