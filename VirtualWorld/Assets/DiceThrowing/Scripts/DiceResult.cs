using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DiceMinigame
{
    public struct DiceResult
    {
        public int result;
        public string type;
        public int typeN;

        public DiceResult(int result, string type, bool sortable = true)
        {
            this.result = result;
            this.type = type;
            if (sortable)
            {
                this.typeN = int.Parse(Regex.Replace(type, "[^0-9]", ""));
            }
            else
            {
                this.typeN = 0;
            }

        }
    }
}
