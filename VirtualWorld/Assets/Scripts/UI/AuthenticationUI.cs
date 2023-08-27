using BackendConnection;
using Cysharp.Threading.Tasks;
using System;
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
        [SerializeField] APICalls apiCalls;

        [Header("Shared")]
        [SerializeField] GameObject loginRegisterPanel;
        [SerializeField] Toggle rememberMeToggle;
        [SerializeField] TMP_Text errorMessage;
        [SerializeField] float showErrorDuration = 3f;

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
            if(apiCalls == null)
            {
                apiCalls = FindAnyObjectByType<APICalls>();
            }

            loginButton.onClick.AddListener(async () => await apiCalls.OnBeginLogin(loginNameField.text, loginPasswordField.text, rememberMeToggle.isOn));
            registerButton.onClick.AddListener(async () => await apiCalls.OnBeginRegister(registerNameField.text, registerPasswordField.text, rememberMeToggle.isOn));
            apiCalls.OnAuthSuccess.AddListener(OnEnableLoggedIn);
            apiCalls.OnNoLoggedUser.AddListener(OnEnableRegister);
            apiCalls.OnAuthFailed.AddListener(OnAuthFailed);
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

        void OnEnableLoggedIn(LoggedUserData data)
        {
            loginRegisterPanel.SetActive(false);
            loggedInPanel.SetActive(true);
            usernameText.text = data.username;
        }

        public void OnLogOut()
        {
            apiCalls.LogOut();
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

        void OnAuthFailed(UnityWebRequestException exc)
        {
            if (exc.Message.Contains("invalid"))
            {
                SetError("Invalid username or password.");
            } 
            else if(exc.Message.Contains("unique"))
            {
                SetError("Username already taken.");
            }
            else if (exc.Message.Contains("expired"))
            {
                OnEnableLogin();
                SetError("Token expired. Please login again.");
            }
            else
            {
                SetError(exc.Error);
            }
        }

        void SetError(string message)
        {
            errorMessage.text = message;
            errorMessage.gameObject.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(HideError());
        }

        IEnumerator HideError()
        {
            yield return new WaitForSeconds(showErrorDuration);
            errorMessage.text = "";
            errorMessage.gameObject.SetActive(false);
        }
    }
}

