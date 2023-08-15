using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhasedPins : MonoBehaviour
{
    public Pin[] AllPins;
    public PickUp[] AllPickUps;
    private bool HasBeenInitialized = false;
    public bool AllHaveToppledOver;
    public bool IsActivePhase;
    public PinPhaser PinPhaser;
    public float PhaseStartTime;
    public EndlessPinPhaser EndlessPinPhaser;


    public void FetchAllPins(EndlessPinPhaser pinPhaser)
    {
        EndlessPinPhaser = pinPhaser;
        PinPhaser = null;
        HasBeenInitialized = true;
        AllPins = GetComponentsInChildren<Pin>(true);

        for (int i = 0; i < AllPins.Length; i++)
        {
            AllPins[i].gameObject.SetActive(false);
        }

        AllPickUps = GetComponentsInChildren<PickUp>(true);

        for (int i = 0; i < AllPickUps.Length; i++)
        {
            AllPickUps[i].gameObject.SetActive(false);
        }

        AllHaveToppledOver = false;
    }

    public void FetchAllPins(PinPhaser pinPhaser)
    {
        PinPhaser = pinPhaser;
        EndlessPinPhaser = null;
        HasBeenInitialized = true;
        AllPins = GetComponentsInChildren<Pin>(true);

        for (int i = 0; i < AllPins.Length; i++)
        {
            AllPins[i].gameObject.SetActive(false);
        }

        AllPickUps = GetComponentsInChildren<PickUp>(true);

        for (int i = 0; i < AllPickUps.Length; i++)
        {
            AllPickUps[i].gameObject.SetActive(false);
        }

        AllHaveToppledOver = false;
    }

    public void OnPhaseStart()
    {
        IsActivePhase = true;
        PhaseStartTime = Time.time;

        gameObject.SetActive(true);

        for (int i = 0; i < AllPins.Length; i++)
        {
            AllPins[i].HasBeenSpawned = false;
        }
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
        IsActivePhase = false;
        
        if (PinPhaser != null) 
        {
            PinPhaser.OnPhaseComplete();
        }

        else
        {
            EndlessPinPhaser.OnPhaseComplete();
        }
        //GameFlowManager.Instance.AllPinsToppledOver();
    }

    private void Update()
    {
        if (!HasBeenInitialized
            || !IsActivePhase)
        {
            return;
        }

        for (int i = 0; i < AllPins.Length; i++)
        {
            if (!AllPins[i].HasBeenSpawned)
            {
                if (Time.time >= PhaseStartTime + AllPins[i].SpawnTimeFromPhaseStart )
                {
                    AllPins[i].gameObject.SetActive(true);
                    AllPins[i].Spawn();
                }
            }
        }

        if (GameFlowManager.CurrentGameMode != GameFlowManager.GameMode.IronMan) 
        {

            if (AllPickUps != null && AllPickUps.Length > 0)
            {
                for (int i = 0; i < AllPickUps.Length; i++)
                {
                    bool spawn = false;

                    if ((GameFlowManager.CurrentGameMode == GameFlowManager.GameMode.Easy
                            && AllPickUps[i].SpawnOnMedium)
                        || (GameFlowManager.CurrentGameMode == GameFlowManager.GameMode.Medium
                            && AllPickUps[i].SpawnOnHard))
                    {
                        spawn = true;
                    }

                    if (!AllPickUps[i].HasBeenSpawned
                        && spawn)
                    {
                        if (Time.time >= PhaseStartTime + AllPickUps[i].SpawnTimeFromPhaseStart)
                        {
                            AllPickUps[i].Spawn();
                        }
                    }
                }
            }
        }

        CheckIfAllHaveToppledOver();
    }
}
