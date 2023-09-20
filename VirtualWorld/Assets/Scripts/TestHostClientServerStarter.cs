using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class TestHostClientServerStarter : MonoBehaviour
{
    //private string TestSceneName = "Test1";
    private string TestSceneName = "Playground";

    public TextMeshProUGUI LocalIPText;
    public TextMeshProUGUI GlobalIPText;

    public TMP_InputField NetworkManagerIP;
    public TMP_InputField ServerHostPortInputField;

    public TMP_InputField ClientIPInputField;
    public TMP_InputField ClientPortInputField;

    public void Awake()
    {
        string ipAddress = "no ip address found";
        ipAddress = GetLocalIPAddress();
        LocalIPText.text = ipAddress;
        GlobalIPText.text = FetchIPAddressFromInternet();
        ServerHostPortInputField.text = "7777";
    }


    // https://discussions.unity.com/t/get-the-device-ip-address-from-unity/235351/4
    public string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                //hintText.text = ip.ToString();
                return ip.ToString();
            }
        }
        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }

    // There apparently is no way to check the ip internally
    // What if the site goes down?
    // But we are probably going to use matchmaking and relay services anyways, so this sort of connection
    // will become obsolete anyways
    // https://stackoverflow.com/questions/3253701/get-public-external-ip-address
    public string FetchIPAddressFromInternet()
    {
        string ip = null;

        string[] checkIPUrl =
        {            
            "https://api.ipify.org",
            "https://ipinfo.io/ip",
            "https://checkip.amazonaws.com/",
            "https://icanhazip.com",
            "https://wtfismyip.com/text"
        };

        for (int i = 0; i < checkIPUrl.Length; i++)
        {
            string ipCandidate = new System.Net.WebClient().DownloadString(checkIPUrl[i]);

            if (!string.IsNullOrEmpty(ipCandidate))
            {
                ip = ipCandidate;
                return ip;
            }
        }

        return "No ip found";
    }

    public void StartLocalHost()
    {
        NetworkManager.Singleton.StartHost();

        NetworkManager.Singleton.SceneManager.LoadScene(TestSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);        
        //NetworkManager.Singleton.SceneManager.SetClientSynchronizationMode(LoadSceneMode.Additive);
    }

    public void StartLocalServer()
    {
        NetworkManager.Singleton.StartServer();
        
        NetworkManager.Singleton.SceneManager.LoadScene(TestSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
        //NetworkManager.Singleton.SceneManager.SetClientSynchronizationMode(LoadSceneMode.Additive);
    }

    public void StartLocalClient()
    {
        NetworkManager.Singleton.StartClient();

        int sceneCount = SceneManager.sceneCount;

        for (int i = 0; i < sceneCount; i++)
        {
            Debug.Log("Scene is loaded " + SceneManager.GetSceneAt(i).name);
        }

        //UnityEngine.SceneManagement.SceneManager.LoadScene(TestSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
        Debug.Log("Client started");
    }

    public void StartHost()
    {
        SetServerData();
        NetworkManager.Singleton.StartHost();

        NetworkManager.Singleton.SceneManager.LoadScene(TestSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);        
        //NetworkManager.Singleton.SceneManager.ActiveSceneSynchronizationEnabled = false;
    }

    public void StartClient()
    {
        SetClientData();
        NetworkManager.Singleton.StartClient();
    }

    public void StartServer()
    {
        SetServerData();
        NetworkManager.Singleton.StartServer();
        NetworkManager.Singleton.SceneManager.LoadScene(TestSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);

    }

    // Stolen from ClientDriven bite size sample
    public void SetServerData()
    {
        var sanitizedIPText = Sanitize(NetworkManagerIP.text);
        //sanitizedIPText = Sanitize("127.0.0.1");
        var sanitizedPortText = Sanitize(ServerHostPortInputField.text);

        ushort.TryParse(sanitizedPortText, out var port);

        var utp = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        utp.SetConnectionData(sanitizedIPText, port);
    }

    
    public void SetClientData()
    {
        SetUtpConnectionData();
    }

    // Stolen from Unity's ClientDriven bite size sample
    void SetUtpConnectionData()
    {
        var sanitizedIPText = Sanitize(ClientIPInputField.text);
        var sanitizedPortText = Sanitize(ClientPortInputField.text);

        ushort.TryParse(sanitizedPortText, out var port);

        var utp = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        utp.SetConnectionData(sanitizedIPText, port);
    }

    // Stolen from Unity's ClientDriven bite size sample
    /// <summary>
    /// Sanitize user port InputField box allowing only alphanumerics and '.'
    /// </summary>
    /// <param name="dirtyString"> string to sanitize. </param>
    /// <returns> Sanitized text string. </returns>
    static string Sanitize(string dirtyString)
    {
        return Regex.Replace(dirtyString, "[^A-Za-z0-9.]", "");
    }
}
