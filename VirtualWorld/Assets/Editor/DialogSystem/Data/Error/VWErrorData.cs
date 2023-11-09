using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialog
{
    public class VWErrorData
    {
        public Color Color { get; private set; }

        public VWErrorData()
        {
            GenerateRandomColor();
        }

        private void GenerateRandomColor()
        {
            Color = new Color32(
                (byte) Random.Range(130,256),
                (byte) Random.Range(50,176),
                (byte) Random.Range(50,176),
                255
                );
        }
    }
}
