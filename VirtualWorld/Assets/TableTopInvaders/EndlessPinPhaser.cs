using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessPinPhaser : MonoBehaviour
{
    public static EndlessPinPhaser Instance;
    public PhasedPins[] AllPhases;

    public int CurrentPhaseIndex;

    public float StartTimer;
    public float StartTime;
    public bool WaitingForStart;

    private List<Ball> AllActiveBallsInScene;

    public int CurrentPhase;

    public bool WaitingToEnd;

    public float PhaseIntervallLength;
    public bool DoingPhaseIntervall;

    private float EndTimer;

    public void Initialize()
    {
        //Debug.Log("Initializing endless pin phaser.");

        Instance = this;
        AllActiveBallsInScene = new List<Ball>();

        for (int i = 0; i < AllPhases.Length; i++)
        {
            AllPhases[i].FetchAllPins(this);
        }

        CurrentPhase = Random.Range(0, AllPhases.Length);
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

    public void OnLevelStart()
    {
        WaitingForStart = true;
        WaitingToEnd = false;
        StartTimer = 4;
        StartTime = Time.time;
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

        DoingPhaseIntervall = true;
        PhaseIntervallLength = 1.0f;
        //Debug.Log("End of phase waiting for new one " + Time.time);
        

        //Debug.Log("On phase complete");
    }


    public void Update()
    {
        if (WaitingForStart
            && Time.time > StartTime + 4.0f)
        {
            WaitingForStart = false;
            StartNextPhase();
        }

        if (WaitingToEnd)
        {
            EndTimer -= Time.deltaTime;

            // add time to timer countdown, so the clock stays the same
            //TimerCountdown.AddTime(Time.deltaTime);


            if (EndTimer <= 0.0f)
            {
                //Debug.Log("End timer finished");
                WaitingToEnd = false;
                ReadyToCloseScene();
            }
        }

        if (DoingPhaseIntervall)
        {
            PhaseIntervallLength -= Time.deltaTime;

            if (PhaseIntervallLength <= 0)
            {
                DoingPhaseIntervall = false;
                StartNextPhase();
            }
        }
    }

    public void StartNextPhase()
    {
        while (true)
        {
            int rand = Random.Range(0, AllPhases.Length);

            if (rand != CurrentPhase)
            {
                CurrentPhase = rand;
                AllPhases[rand].OnPhaseStart();
                break;
            }
        }

    }

    public void OnPhasesFinished()
    {
        // Maybe add a second or two here, to wait for last pin to fall over and roll?
        //Debug.Log("Last phase finished ");
        EndTimer = 3.0f;
        WaitingToEnd = true;
        TimerCountdown.peliVoiAlkaa = false;

    }

    public void OnTimerRanOut()
    {
        OnPhasesFinished();
    }

    public void ReadyToCloseScene()
    {
        if (PisteLaskuri.pisteet >= GameFlowManager.Instance.TargetScore) 
        {
            GameFlowManager.Instance.OnReadyToCloseTimeAttackSceneWin();
        }

        else
        {
            GameFlowManager.Instance.OnReadyToCloseTimeAttackSceneLose();
        }
    }
}
