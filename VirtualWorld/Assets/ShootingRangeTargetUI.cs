using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShootingRangeTargetUI : MonoBehaviour
{
    public TextMeshProUGUI text;

    private int TargetsDestroyed = -1;
    private int TargetsLeft = -1;
    private int TargetsTotal = -1;

    public void UpdateTargetAmounts(int targetsDestroyed,
                                    int targetsAlive,
                                    int totalAmountOfTargets)
    {
        if (targetsDestroyed != TargetsDestroyed)
        {
            TargetsDestroyed = targetsDestroyed;
            TargetsLeft = targetsAlive;
            TargetsTotal = totalAmountOfTargets;

            text.text = "Targets destroyed: \n" + TargetsDestroyed + "/" + TargetsTotal;
        }
    }
}
