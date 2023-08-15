using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinPhaser : MonoBehaviour
{
    public static PinPhaser Instance;
    public PhasedPins[] AllPhases;

    public PhasedPins CurrentPhase;
    public int PhaseNumber;

    private float EndTimer;
    private bool WaitingToEnd;

    private float PhaseIntervallLength;
    private bool DoingPhaseIntervall;

    private List<Ball> AllActiveBallsInScene;

    public void Initialize()
    {
        Instance = this;
        AllActiveBallsInScene = new List<Ball>();
        for (int i = 0; i < AllPhases.Length; i++)
        {
            AllPhases[i].FetchAllPins(this);
        }

        //Debug.Log("Initialized pin phaser");
    }

    public void OnLevelStart()
    {
        PhaseNumber = 0;
        WaitingToEnd = false;
        AllPhases[PhaseNumber].OnPhaseStart();
        //Debug.Log("Level start called on PinPhaser");
    }

    public void AddBall(Ball ball)
    {
        AllActiveBallsInScene.Add(ball);
    }

    public void RemoveBall(Ball ball)
    {
        if (AllActiveBallsInScene.Contains(ball))
        {
            AllActiveBallsInScene.Remove(ball);
        }
    }

    public void OnPhaseComplete()
    {
        for (int i = 0; i < AllActiveBallsInScene.Count; i++)
        {
            if (AllActiveBallsInScene[i] != null) 
            {
                AllActiveBallsInScene[i].StartRemoval(0.98f);
            }
        }

        PhaseNumber++;
        
        if (PhaseNumber >= AllPhases.Length)
        {
            OnPhasesFinished();
        }

        else
        {
            DoingPhaseIntervall = true;
            PhaseIntervallLength = 1.0f;
            //Debug.Log("End of phase waiting for new one " + Time.time);
        }

        //Debug.Log("On phase complete");
    }

    public void OnPhasesFinished()
    {
        // Maybe add a second or two here, to wait for last pin to fall over and roll?
        //Debug.Log("Last phase finished ");
        EndTimer = 3.0f;
        WaitingToEnd = true;
        TimerCountdown.peliVoiAlkaa = false;

    }

    private void Update()
    {
        if (WaitingToEnd)
        {
            EndTimer -= Time.deltaTime;
            
            // add time to timer countdown, so the clock stays the same
            //TimerCountdown.AddTime(Time.deltaTime);


            if (EndTimer <= 0.0f)
            {
                //Debug.Log("End timer finished");
                WaitingToEnd = false;
                GameFlowManager.Instance.AllPinsToppledOver();
            }
        }

        if (DoingPhaseIntervall)
        {
            PhaseIntervallLength -= Time.deltaTime;

            if (PhaseIntervallLength <= 0)
            {
                DoingPhaseIntervall = false;
                AllPhases[PhaseNumber].OnPhaseStart();
            }
        }
    }
}
