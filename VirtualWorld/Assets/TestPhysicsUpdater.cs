using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPhysicsUpdater : MonoBehaviour
{
    private float timer;

    public static TestPhysicsUpdater Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        else 
        { 
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        gameObject.SetActive(false);

        //Physics.simulationMode = SimulationMode.FixedUpdate;
    }

    private void FixedUpdate()
    {
        //Debug.Log("Fixed deltatime is " + Time.fixedDeltaTime);

        //Physics.Simulate(Time.fixedDeltaTime);
    }

    // https://docs.unity3d.com/ScriptReference/Physics.Simulate.html
    void Update()
    {
        if (Physics.simulationMode != SimulationMode.Script)
            return; // do nothing if the automatic simulation is enabled

        timer += Time.deltaTime;

        // Catch up with the game time.
        // Advance the physics simulation in portions of Time.fixedDeltaTime
        // Note that generally, we don't want to pass variable delta to Simulate as that leads to unstable results.
        while (timer >= Time.fixedDeltaTime)
        {
            timer -= Time.fixedDeltaTime;
            Physics.Simulate(Time.fixedDeltaTime);
        }

        // Here you can access the transforms state right after the simulation, if needed
    }
}
