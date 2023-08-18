using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Authentication
{
    public class AuthenticationUI : MonoBehaviour
    {
        [SerializeField] BackendConnection.BackendConnection backendConnection;

        [SerializeField] GameObject loginPanel;
        [SerializeField] TMP_InputField nameField; 
        [SerializeField] TMP_InputField passwordField;
        [SerializeField] Button loginButton;
        [SerializeField] Toggle rememberMeToggle;

        [SerializeField] GameObject loggedInPanel;
        [SerializeField] TMP_Text usernameText;


        void Awake()
        {
            if(backendConnection == null)
            {
                return;
            }

            loginButton.onClick.AddListener(() => backendConnection.OnBeginLogin(nameField.text, passwordField.text, rememberMeToggle.isOn));
            backendConnection.OnAuthSuccess.AddListener(OnEnableLoggedIn);
        }

        void OnEnableLoggedIn(string username)
        {
            Debug.Log("ENABLE LOGGED IN?!?!");
            loginPanel.SetActive(false);
            loggedInPanel.SetActive(true);
            usernameText.text = username;
        }

        public void OnLogOut()
        {
            backendConnection.LogOut();
            loginPanel.SetActive(true);
            loggedInPanel.SetActive(false);
        }
    }
}

