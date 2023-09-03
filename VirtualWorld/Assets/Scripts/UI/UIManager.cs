using BackendConnection;
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

        public UnityEvent EventMenuPressed;
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
                playerUI.SetActive(false);
                menu.SetActive(true);
            } else
            {
                ButtonGroup firstBg = menu.GetComponentInChildren<ButtonGroup>(true);
                firstBg.ResetGroup();
                playerUI.SetActive(true);
                menu.SetActive(false);

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
    }
}

