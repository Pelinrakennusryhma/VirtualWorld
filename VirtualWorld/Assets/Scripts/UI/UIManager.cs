using BackendConnection;
using Characters;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] GameObject playerUI;
        [SerializeField] GameObject menu;

        StarterAssetsInputs playerInputs;
        public static UIManager Instance;

        public UnityEvent<bool> EventMenuToggled;

        GameObject currentOpenMenuPanel;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }

            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            playerUI.SetActive(true);
            menu.SetActive(false);

            ResetButtonGroups();
            ResetMenuPanels();
        }

        void Update()
        {
            CheckInputs();
        }

        void CheckInputs()
        {
            if (playerInputs == null)
            {
                return;
            }

            if (playerInputs.menu)
            {
                ToggleUIComponents();
            }
        }

        void ToggleUIComponents()
        {
            if (playerUI.activeSelf)
            {
                ResetMenuPanels();
                ResetButtonGroups();
                playerUI.SetActive(false);
                menu.SetActive(true);
                EventMenuToggled.Invoke(true);
            } else
            {

                playerUI.SetActive(true);
                menu.SetActive(false);
                EventMenuToggled.Invoke(false);
            }
        }

        public void SetPlayerCharacter(GameObject playerGO)
        {
            playerInputs = playerGO.GetComponentInChildren<StarterAssetsInputs>();
        }

        public void OnLogOutPressed()
        {
            Debug.Log("Log out pressed");
            SceneManager.LoadScene(0);
            APICalls.Instance.LogOut();
        }

        public void OnQuitPressed()
        {
            Debug.Log("Quit pressed");
            Application.Quit();
        }

        public void OpenMenuPanel(GameObject panel)
        {
            if(currentOpenMenuPanel != null)
            {
                currentOpenMenuPanel.SetActive(false);
            }

            currentOpenMenuPanel = panel;

            if (panel != null)
            {
                panel.SetActive(true);
            }
        }

        void ResetMenuPanels()
        {
            if(currentOpenMenuPanel != null)
            {
                currentOpenMenuPanel.SetActive(false);
                currentOpenMenuPanel = null;
            }
        }

        void ResetButtonGroups()
        {
            ButtonGroup firstBg = menu.GetComponentInChildren<ButtonGroup>(true);
            firstBg.ResetGroup();
        }
    }
}

