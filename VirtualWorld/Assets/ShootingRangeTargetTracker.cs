using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRangeTargetTracker : MonoBehaviour
{

    [SerializeField] private TargetPracticeTarget[] targets;
    public TargetPracticeTarget[] Targets { get => targets; 
                                            private set => targets = value; }

    [SerializeField] private int aliveTargets;
    public int AliveTargets { get => aliveTargets; 
                              private set => aliveTargets = value; }

    [SerializeField] private int destroyedTargets;
    public int DestroyedTargets { get => destroyedTargets; 
                                  private set => destroyedTargets = value; }

    [SerializeField] private int totalTargets;
    public int TotalTargets { get => totalTargets; 
                              private set => totalTargets = value; }



    private void Awake()
    {
        Targets = GetComponentsInChildren<TargetPracticeTarget>();
        TotalTargets = Targets.Length;
    }

    public bool CheckIfTargetsHaveBeenDestroyed(out int destroyedTargets,
                                                out int aliveTargets,
                                                out int totalTagets)
    {
        bool allTargetsHaveBeenDestroyed = true;
        AliveTargets = 0;
        DestroyedTargets = 0;

        for (int i = 0; i < Targets.Length; i++)
        {
            if (!Targets[i].IsDead)
            {
                AliveTargets++;
                allTargetsHaveBeenDestroyed = false;
            }

            else
            {
                DestroyedTargets++;
            }
        }

        destroyedTargets = DestroyedTargets;
        aliveTargets = AliveTargets;
        totalTagets = TotalTargets;

        return allTargetsHaveBeenDestroyed;
    }
}
