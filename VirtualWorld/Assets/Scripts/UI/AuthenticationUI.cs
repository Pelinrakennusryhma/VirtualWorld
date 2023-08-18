using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Authentication
{
    public class AuthenticationUI : MonoBehaviour
    {
        [Header("Backend")]
        [SerializeField] BackendConnection.BackendConnection backendConnection;

        [Header("Login")]
        [SerializeField] GameObject loginPanel;
        [SerializeField] TMP_InputField loginNameField; 
        [SerializeField] TMP_InputField loginPasswordField;
        [SerializeField] Button loginButton;
        [SerializeField] Toggle loginRememberMeToggle;

        [Header("Logged In")]
        [SerializeField] GameObject loggedInPanel;
        [SerializeField] TMP_Text usernameText;

        [Header("Register")]
        [SerializeField] GameObject registerPanel;
        [SerializeField] TMP_InputField registerNameField;
        [SerializeField] TMP_InputField registerPasswordField;
        [SerializeField] Button registerButton;
        [SerializeField] Toggle registerRememberMeToggle;

        void Awake()
        {
            if(backendConnection == null)
            {
                return;
            }
            OnEnableRegister();
            loginButton.onClick.AddListener(() => 
            backendConnection.OnBeginLogin(loginNameField.text, loginPasswordField.text, loginRememberMeToggle.isOn));
            registerButton.onClick.AddListener(() =>
            backendConnection.OnBeginRegister(registerNameField.text, registerPasswordField.text, registerRememberMeToggle.isOn));
            backendConnection.OnAuthSuccess.AddListener(OnEnableLoggedIn);
        }

        public void OnEnableLogin()
        {
            loginNameField.text = "";
            loginPasswordField.text = "";

            loginPanel.SetActive(true);
            loggedInPanel.SetActive(false);
            registerPanel.SetActive(false);
        }

        void OnEnableLoggedIn(string username)
        {
            registerPanel.SetActive(false);
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

        public void OnEnableRegister()
        {
            registerNameField.text = "";
            registerPasswordField.text = "";

            loginPanel.SetActive(false);
            loggedInPanel.SetActive(false);
            registerPanel.SetActive(true);
        }
    }
}

