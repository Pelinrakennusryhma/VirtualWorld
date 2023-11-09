using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialog
{
    public class VWNodeErrorData
    {
        public VWErrorData ErrorData { get; private set; }
        public List<VWNode> Nodes { get; private set; }

        public VWNodeErrorData()
        {
            ErrorData = new VWErrorData();
            Nodes = new List<VWNode>();
        }
    }
}

