using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRangeTargetTracker : MonoBehaviour
{

    public TargetPracticeTarget[] targets;

    private void Awake()
    {
        targets = GetComponentsInChildren<TargetPracticeTarget>();
    }

    public bool CheckIfTargetsHaveBeenDestroyed()
    {
        bool allTargetsHaveBeenDestroyed = true;

        for (int i = 0; i < targets.Length; i++)
        {
            if (!targets[i].IsDead)
            {
                allTargetsHaveBeenDestroyed = false;
            }
        }

        return allTargetsHaveBeenDestroyed;
    }
}
