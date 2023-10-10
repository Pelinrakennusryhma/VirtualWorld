using BackendConnection;
using Characters;
using Dialog;
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
        [SerializeField] GameObject dialogUI;
        [SerializeField] DialogPanel dialogPanel;

        StarterAssetsInputs playerInputs;
        public static UIManager Instance;

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
            dialogUI.SetActive(false);
        }

        public void SetPlayerCharacter(GameObject playerGO)
        {
            playerInputs = playerGO.GetComponentInChildren<StarterAssetsInputs>();

            PlayerEvents.Instance.EventGameStateChanged.AddListener(OnGameStateChanged);
            PlayerEvents.Instance.EventDialogOpened.AddListener(OnDialogOpen);
        }

        void OnGameStateChanged(GAME_STATE gameState) 
        {
            ToggleUIComponents(gameState);
        }

        // toggle between PlayerUI and menu
        void ToggleUIComponents(GAME_STATE gameState)
        {
            switch (gameState)
            {
                case GAME_STATE.FREE:
                    playerUI.SetActive(true);
                    menu.SetActive(false);
                    dialogUI.SetActive(false);
                    break;
                case GAME_STATE.MENU:
                    ResetMenuPanels();
                    playerUI.SetActive(false);
                    menu.SetActive(true);
                    break;
                case GAME_STATE.TABLET:
                    playerUI.SetActive(false);
                    menu.SetActive(false);
                    dialogUI.SetActive(false);
                    break;
                case GAME_STATE.DIALOG:
                    dialogUI.SetActive(true);
                    playerUI.SetActive(false);
                    menu.SetActive(false);
                    break;
                case GAME_STATE.MINIGAME:
                    playerUI.SetActive(false);
                    menu.SetActive(false);
                    dialogUI.SetActive(false);
                    break;
                default:
                    break;
            }
        }

        public void OnDialogOpen(NPC npc)
        {
            dialogPanel.Setup(npc.Data.mainDialog, npc);
        }

        public void OnDialogClosePressed()
        {
            dialogUI.SetActive(false);
            PlayerEvents.Instance.CallEventDialogClosed();
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

