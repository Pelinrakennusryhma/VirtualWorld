using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HymiQuests
{
    public class PlayerQuest
    {
        //Player Quest seuraa meneillään olevaa questia
        //step = Kuinka monennes askel questissa meneillään
        //progress = Kuinka paljon nykyisessä askeleessa on edistystä
        public Quest quest { get; set; }
        public int step { get; set; } = 0;
        public int progress { get; set; } = 0;
    }
}
