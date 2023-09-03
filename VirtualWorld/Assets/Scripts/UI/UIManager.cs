using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
            playerUI.SetActive(!playerUI.activeSelf);
            menu.SetActive(!menu.activeSelf);
        }

        public void SetPlayerCharacter(GameObject playerGO)
        {
            playerInputs = playerGO.GetComponentInChildren<StarterAssetsInputs>();
        }
    }
}

