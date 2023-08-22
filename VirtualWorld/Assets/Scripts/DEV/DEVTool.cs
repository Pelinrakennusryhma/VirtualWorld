#if UNITY_EDITOR
using ParrelSync;
using UnityEngine;
using APICalls;


namespace Dev
{
    public class DEVTool : MonoBehaviour
    {
        [SerializeField] BackendConnection backendConnection;

        async void Start()
        {
            Debug.Log("STARTED DEV LOGIN " + Time.time);

            string userName;
            string password;

            if (ClonesManager.IsClone())
            {
                userName = System.Environment.GetEnvironmentVariable("UNITY_CLIENT_USERNAME");
                password = System.Environment.GetEnvironmentVariable("UNITY_CLIENT_PASSWORD");

            }
            else
            {
                userName = System.Environment.GetEnvironmentVariable("UNITY_SERVER_USERNAME");
                password = System.Environment.GetEnvironmentVariable("UNITY_SERVER_PASSWORD");
            }

            backendConnection.LogOut();
            await backendConnection.OnBeginLogin(userName, password, false);

            Debug.Log("FINISHED DEV LOGIN " + Time.time);
        }

    }
}
#endif
