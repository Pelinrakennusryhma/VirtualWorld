using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceMinigame
{
    public class D100ChildScore : DiceScore
    {
        public D100Score parentDiceScore;
        public void InitParent(D100Score _parentDiceScore)
        {
            parentDiceScore = _parentDiceScore;
            parentDiceScore.childDiceScore = this;
        }
        protected override void Stop()
        {
            stopped = true;
            parentDiceScore.ChildStopped();
        }
    }
}
