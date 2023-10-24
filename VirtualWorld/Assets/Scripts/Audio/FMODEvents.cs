using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class FMODEvents : MonoBehaviour
    {
        //[field: Header("Music")]
        //[field:SerializeField] public EventReference Music { get; private set; }

        [field: Header("Character")]
        [field: SerializeField] public EventReference Footsteps { get; private set; }
        [field: SerializeField] public EventReference Land { get; private set; }

        public static FMODEvents Instance { get; private set; }

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
