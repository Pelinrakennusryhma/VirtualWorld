using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scenes;

public class RaftGameManager : MonoBehaviour
{
    public static RaftGameManager Instance;

    [SerializeField] private ShootingRangeTimer timer;
    [SerializeField] private MoveAlongASplineTest playerRaftController;

    private float goToLobbyTimer;

    private float goToLobbyLenght = 4.0f;

    private bool waitingToGoToLobby;

    [SerializeField] private string LobbySceneName = "LobbyWaterPark";

    private bool alreadyLeavingTheScene;

    private void Awake()
    {
        Instance = this;
        alreadyLeavingTheScene = false;
    }

    private void Start()
    {
        OnLevelStarted();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoToLobby();
        }

        if (waitingToGoToLobby)
        {
            goToLobbyTimer -= Time.deltaTime;

            if (goToLobbyTimer <= 0.0f)
            {
                GoToLobby();
            }
        }
    }

    private void OnLevelStarted()
    {
        timer.OnShow();
        timer.StartTimer();
    }

    public void OnFinishLineReached()
    {
        timer.StopTimer();
    }

    public void OnEnterStopArea()
    {
        playerRaftController.OnStopAreaReached();
        goToLobbyTimer = goToLobbyLenght;
        waitingToGoToLobby = true;
    }

    private void GoToLobby()
    {
        if (!alreadyLeavingTheScene) 
        {
            alreadyLeavingTheScene = true;

            Debug.Log("We should go to lobby");

            if (SceneLoader.Instance != null)
            {
                SceneLoader.Instance.SwitchSubScenes(LobbySceneName);
            }

            else
            {
                // To make it so (for now at least while the initial development is ongoing)
                // that we can launch the scenes in editor, without having to open a networked scene
                UnityEngine.SceneManagement.SceneManager.LoadScene(LobbySceneName);
            }
        }
    }
}
