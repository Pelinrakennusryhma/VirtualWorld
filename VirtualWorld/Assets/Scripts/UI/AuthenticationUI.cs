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

        [Header("Shared")]
        [SerializeField] GameObject loginRegisterPanel;
        [SerializeField] Toggle rememberMeToggle;

        [Header("Login")]
        [SerializeField] GameObject loginTitle;
        [SerializeField] Button loginButton;
        [SerializeField] Button registerSwitch;
        [SerializeField] TMP_InputField loginNameField;
        [SerializeField] TMP_InputField loginPasswordField;

        [Header("Logged In")]
        [SerializeField] GameObject loggedInPanel;
        [SerializeField] TMP_Text usernameText;

        [Header("Register")]
        [SerializeField] GameObject registerTitle;
        [SerializeField] Button registerButton;
        [SerializeField] Button loginSwitch;
        [SerializeField] TMP_InputField registerNameField;
        [SerializeField] TMP_InputField registerPasswordField;

        void Awake()
        {
            if(backendConnection == null)
            {
                return;
            }
            loginButton.onClick.AddListener(() => 
            backendConnection.OnBeginLogin(loginNameField.text, loginPasswordField.text, rememberMeToggle.isOn));
            registerButton.onClick.AddListener(() =>
            backendConnection.OnBeginRegister(registerNameField.text, registerPasswordField.text, rememberMeToggle.isOn));
            backendConnection.OnAuthSuccess.AddListener(OnEnableLoggedIn);
            backendConnection.OnNoLoggedUser.AddListener(OnEnableRegister);
        }

        public void OnEnableLogin()
        {
            loginNameField.text = "";
            loginPasswordField.text = "";

            loginNameField.transform.parent.gameObject.SetActive(true);
            loginPasswordField.transform.parent.gameObject.SetActive(true);
            registerNameField.transform.parent.gameObject.SetActive(false);
            registerPasswordField.transform.parent.gameObject.SetActive(false);
            loginButton.gameObject.SetActive(true);
            registerButton.gameObject.SetActive(false);
            loginTitle.SetActive(true);
            registerTitle.SetActive(false);
            loginSwitch.gameObject.SetActive(false);
            registerSwitch.gameObject.SetActive(true);
            loginRegisterPanel.SetActive(true);
            loggedInPanel.SetActive(false);
        }

        void OnEnableLoggedIn(string username)
        {
            loginRegisterPanel.SetActive(false);
            loggedInPanel.SetActive(true);
            usernameText.text = username;
        }

        public void OnLogOut()
        {
            backendConnection.LogOut();
            OnEnableLogin();
            loggedInPanel.SetActive(false);
        }

        public void OnEnableRegister()
        {
            registerNameField.text = "";
            registerPasswordField.text = "";

            loginNameField.transform.parent.gameObject.SetActive(false);
            loginPasswordField.transform.parent.gameObject.SetActive(false);
            registerNameField.transform.parent.gameObject.SetActive(true);
            registerPasswordField.transform.parent.gameObject.SetActive(true);
            loginButton.gameObject.SetActive(false);
            registerButton.gameObject.SetActive(true);
            loginTitle.SetActive(false);
            registerTitle.SetActive(true);
            registerSwitch.gameObject.SetActive(false);
            loginSwitch.gameObject.SetActive(true);
            loginRegisterPanel.SetActive(true);
            loggedInPanel.SetActive(false);
        }
    }
}

