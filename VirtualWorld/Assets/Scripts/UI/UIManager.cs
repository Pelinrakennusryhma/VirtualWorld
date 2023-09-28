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

        [SerializeField] Transform menuPanelParent;
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
        }

        void ToggleUIComponents()
        {
            if (playerUI.activeSelf)
            {
                ResetMenuPanels();
                playerUI.SetActive(false);
                menu.SetActive(true);
                EventMenuToggled.Invoke(true);
#if UNITY_WEBGL
                playerInputs.UnlockCursor();
#endif
            } else
            {
                playerUI.SetActive(true);
                menu.SetActive(false);
                EventMenuToggled.Invoke(false);
#if UNITY_WEBGL
                playerInputs.LockCursor();
#endif
            }
        }

        public void SetPlayerCharacter(GameObject playerGO)
        {
            playerInputs = playerGO.GetComponentInChildren<StarterAssetsInputs>();

            playerInputs.EventMenuPressed.AddListener(OnMenuPressed);
        }

        void OnMenuPressed() 
        {
            ToggleUIComponents();
        }

        public void OnLogOutPressed()
        {
            Debug.Log("Log out pressed");
            SceneManager.LoadScene(0);
            APICalls_Client.Instance.LogOut();
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
            foreach (Transform menuPanel in menuPanelParent)
            {
                menuPanel.gameObject.SetActive(false);
            }
        }
    }
}

