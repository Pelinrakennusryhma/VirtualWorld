using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

namespace TableTopInvaders
{
    public class FMODEventsTTI : MonoBehaviour
    {
        [field: SerializeField] public EventReference Countdown123 { get; private set; }
        [field: SerializeField] public EventReference CountdownGo { get; private set; }
        [field: SerializeField] public EventReference UIPress { get; private set; }
        [field: SerializeField] public EventReference HitWall1 { get; private set; }
        [field: SerializeField] public EventReference HitWall2 { get; private set; }
        [field: SerializeField] public EventReference HitWall3 { get; private set; }
        [field: SerializeField] public EventReference Fire1 { get; private set; }
        [field: SerializeField] public EventReference SpawnPickup { get; private set; }
        [field: SerializeField] public EventReference Explosion { get; private set; }
        [field: SerializeField] public EventReference BallHit1 { get; private set; }
        [field: SerializeField] public EventReference BallHit2 { get; private set; }
        [field: SerializeField] public EventReference BallHit3 { get; private set; }
        [field: SerializeField] public EventReference SpawnPin { get; private set; }
        [field: SerializeField] public EventReference FadeOut { get; private set; }
        [field: SerializeField] public EventReference ToppleOver { get; private set; }

        public static FMODEventsTTI Instance { get; private set; }

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("More than one FMODEvents around!");
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }
    }
}
