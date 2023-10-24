using Audio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GravityShip
{
    public class FMODEventsGravityShip : MonoBehaviour
    {
        [field: Header("Character")]
        [field: SerializeField] public EventReference UIClick { get; private set; }
        [field: SerializeField] public EventReference Explosion { get; private set; }
        [field: SerializeField] public EventReference Boost { get; private set; }
        [field: SerializeField] public EventReference Pulsar { get; private set; }

        public static FMODEventsGravityShip Instance { get; private set; }

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("More than one FMODEvents around!");
            }
            else
            {
                Instance = this;
            }
        }
    }
}
