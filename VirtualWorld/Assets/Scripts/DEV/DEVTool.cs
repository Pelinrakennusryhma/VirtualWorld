#if UNITY_EDITOR
using ParrelSync;
using UnityEngine;
using BackendConnection;

namespace Dev
{
    public class DEVTool : MonoBehaviour
    {
        [SerializeField] APICalls apiCalls;

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

            apiCalls.LogOut();
            await apiCalls.OnBeginLogin(userName, password, false);

            Debug.Log("FINISHED DEV LOGIN " + Time.time);
        }

    }
}
#endif
