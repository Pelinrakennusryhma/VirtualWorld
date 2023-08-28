#if UNITY_EDITOR
using ParrelSync;
using UnityEngine;
using BackendConnection;

namespace Dev
{
    public class DEVTool : MonoBehaviour
    {
        [Header("Helper dev script to automatically login a user, \nsuperUser for server(normal unity project)\nand a normal for client(cloned project).")]
        [SerializeField] APICalls apiCalls;

        async void Start()
        {
            float startTime = Time.time;
            Debug.Log("STARTED DEV LOGIN");

            string username;
            string password;

            if (ClonesManager.IsClone())
            {
                username = System.Environment.GetEnvironmentVariable("UNITY_CLIENT_USERNAME");
                password = System.Environment.GetEnvironmentVariable("UNITY_CLIENT_PASSWORD");

            }
            else
            {
                username = System.Environment.GetEnvironmentVariable("UNITY_SERVER_USERNAME");
                password = System.Environment.GetEnvironmentVariable("UNITY_SERVER_PASSWORD");
            }

            if(username == "" || password == "")
            {
                Debug.Log("CANCELLED DEV LOGIN: No username and/or password");
            }

            apiCalls.LogOut();
            await apiCalls.OnBeginLogin(username, password, false);

            Debug.Log("FINISHED DEV LOGIN: completed in " + (Time.time - startTime) * 1000 + " milliseconds.");
        }

    }
}
#endif
