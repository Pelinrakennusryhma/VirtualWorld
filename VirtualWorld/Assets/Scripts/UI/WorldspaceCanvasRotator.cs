using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class WorldspaceCanvasRotator : MonoBehaviour
    {
        void LateUpdate()
        {
            if (Camera.main != null)
            {
                transform.rotation = Camera.main.transform.rotation;
            }
        }
    }
}

