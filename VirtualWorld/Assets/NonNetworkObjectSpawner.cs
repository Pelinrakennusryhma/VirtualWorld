using UnityEngine;
using UnityEngine.SceneManagement;

public class NonNetworkObjectSpawner : MonoBehaviour
{
    public static NonNetworkObjectSpawner Instance;
    [SerializeField] GameObject[] objectsToSpawn;

    [SerializeField] GameObject playerPrefab;

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
            //SpawnObjects();
        }
    }

    private void Start()
    {
        SpawnObjects();
    }

    private void SpawnObjects()
    {
        Debug.Log("About to spawn objects");

        foreach (GameObject obj in objectsToSpawn)
        {
            GameObject go = Instantiate(obj);
            SceneManager.MoveGameObjectToScene(go, SceneManager.GetSceneByName("Playground"));
        }

        GameObject player = Instantiate(playerPrefab);
        SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByName("Playground"));


    }
}
