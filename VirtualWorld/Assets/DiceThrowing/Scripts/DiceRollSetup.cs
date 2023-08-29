using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceMinigame
{
    [System.Serializable]
    public class DiceRollSetup
    {
        public List<DiceRollData> diceRollData;
        public int bonus;
        public string name;
    }

    [System.Serializable]
    public class DiceRollData
    {
        public string name;
        public int amount;
        public bool bonus;
        public bool penalty;
    }
}
