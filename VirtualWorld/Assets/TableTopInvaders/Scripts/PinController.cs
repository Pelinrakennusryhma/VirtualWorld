using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinController : MonoBehaviour
{
    public Pin[] AllPins;
    private bool HasBeenInitialized = false;
    public bool AllHaveToppledOver;



    public void FetchAllPins()
    {
        HasBeenInitialized = true;
        AllPins = GetComponentsInChildren<Pin>();
        AllHaveToppledOver = false;
    }

    public void CheckIfAllHaveToppledOver()
    {
        bool onlyMinusPinsInScene = true;
        bool atLeastOnePinNotToppledOver = false;
        bool atLeastOnePointPinNotToppledOver = false;

        // John Carmack's advice applied: do always, filter out later. This keeps perf consistent, even if it really doesn't matter here.
        for (int i = 0; i < AllPins.Length; i++)
        {
            if (!AllPins[i].HasToppledOver)
            {
                atLeastOnePinNotToppledOver = true;
            }

            if (AllPins[i].PinType != RespawnToPreviousPositions.PinType.Minus2
                && AllPins[i].PinType != RespawnToPreviousPositions.PinType.Minus5
                && AllPins[i].PinType != RespawnToPreviousPositions.PinType.None)
            {
                if (!AllPins[i].HasToppledOver)
                {
                    atLeastOnePointPinNotToppledOver = true;
                }

                onlyMinusPinsInScene = false;
            }
        }

        if ((!atLeastOnePinNotToppledOver 
            && onlyMinusPinsInScene) 
            || (!onlyMinusPinsInScene
                && !atLeastOnePointPinNotToppledOver))
        {
            AllPinsHaveToppledOver();
        }
    }

    public void AllPinsHaveToppledOver()
    {
        AllHaveToppledOver = true;
        GameFlowManager.Instance.AllPinsToppledOver();
    }

    private void Update()
    {
        if (!HasBeenInitialized)
        {
            return;
        }

        CheckIfAllHaveToppledOver();
    }
}
