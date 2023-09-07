using Scenes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DiceMinigame
{
    public class UIManager : MonoBehaviour
    {
        public EventManager eventManager;
        public GameObject scorePanel;
        public GameObject throwPanel;
        public GameObject rethrowPanel;
        public GameObject setupPanel;
        public Button acceptSetupButton;
        public Button saveSetupButton;
        public GameObject scoreDetailsPanel;
        public GameObject menuPanel;
        public TMP_Text warningText;
        public GameObject setupAcceptButton;
        public GameObject adderContainer;
        public Toggle clearTableToggle;
        public GameObject menuButton;
        public Button clearTableButton;
        private bool somethingToClear = false;
        private List<DiceAdjust> diceAdjusts = new List<DiceAdjust>();

        void Awake()
        {
            eventManager.EventReadyToThrow.AddListener(ReadyToThrowEventHandler);
            eventManager.EventThrowPressed.AddListener(DiceThrownEventHandler);
            eventManager.EventSetupOpened.AddListener(SetupOpenedEventHandler);
            eventManager.EventSetupAccepted.AddListener(SetupAcceptedEventHandler);
            eventManager.EventAllDiceStopped.AddListener(AllDiceStoppedEventHandler);
            eventManager.EventThrowAgainPressed.AddListener(ThrowAgainPressedEventHandler);
            eventManager.EventThrowMorePressed.AddListener(ThrowMorePressedEventHandler);
            eventManager.EventAdjustsSpawned.AddListener(AdjustsSpawnedEventHandler);
            eventManager.EventAdjustsChanged.AddListener(AdjustsChangedEventHandler);
            eventManager.EventAddNewDiceRollSetupOpened.AddListener(AddNewDiceRollSetupHandler);
            eventManager.EventClearTablePressed.AddListener(ClearTableEventHandler);
        }

        void ClearTableEventHandler()
        {
            somethingToClear = false;
            clearTableButton.gameObject.SetActive(false);
        }

        void DiceThrownEventHandler()
        {
            HideUI();
        }

        void ReadyToThrowEventHandler()
        {
            ShowThrow();
        }

        void SetupOpenedEventHandler()
        {
            ShowSetup();
        }

        void SetupAcceptedEventHandler()
        {
            ShowThrow();
        }

        void AllDiceStoppedEventHandler(List<DiceScore> dummy1, List<BonusAdjust> dummy2)
        {
            StartCoroutine(ThrowFinished());
        }

        void ThrowAgainPressedEventHandler()
        {
            somethingToClear = false;
            StartCoroutine(ThrowAgain());
        }

        void ThrowMorePressedEventHandler()
        {
            somethingToClear = true;
            StartCoroutine(ThrowMore());
        }

        void AdjustsSpawnedEventHandler(List<DiceAdjust> _diceAdjusts, List<BonusAdjust> _bonusAdjusts)
        {
            diceAdjusts = _diceAdjusts;
        }

        void AdjustsChangedEventHandler()
        {
            int totalDice = 0;
            foreach (DiceAdjust diceAdjust in diceAdjusts)
            {
                totalDice += diceAdjust.GetAmount();
                if(diceAdjust.dicePrefab.name == "D100" || diceAdjust.dicePrefab.name == "D99")
                {
                    totalDice += diceAdjust.GetAmount();
                }
            }

            if(totalDice <= 0)
            {
                acceptSetupButton.interactable = false;
                saveSetupButton.interactable = false;
                warningText.text = "Add at least one dice.";
                warningText.gameObject.SetActive(true);
            } else if(totalDice > DiceMinigameGlobalSettings.Instance.maxDice)
            {
                acceptSetupButton.interactable = false;
                saveSetupButton.interactable = false;
                warningText.text = $"Maximum amount of dice is {DiceMinigameGlobalSettings.Instance.maxDice}.\n D100 and D99 counts as two.";
                warningText.gameObject.SetActive(true);
            } else
            {
                warningText.gameObject.SetActive(false);
                acceptSetupButton.interactable = true;
                saveSetupButton.interactable = true;
            }
        }

        void AddNewDiceRollSetupHandler()
        {
            ShowAddDiceRollSetup();
        }

        public void HideUI()
        {
            throwPanel.SetActive(false);
            scorePanel.SetActive(false);
            setupPanel.SetActive(false);
            rethrowPanel.SetActive(false);
            scoreDetailsPanel.SetActive(false);
            menuPanel.SetActive(false);
            menuButton.SetActive(false);
        }

        public void OnThrowAgainPressed()
        {
            if (clearTableToggle.isOn)
            {
                eventManager.ThrowAgain();
            } else
            {
                eventManager.ThrowMore();
            }

        }

        public void ShowScore(bool hideOthers=false)
        {
            if (hideOthers)
            {
                HideUI();
            }
            scorePanel.SetActive(true);
        }

        public void ShowRethrow()
        {
            rethrowPanel.SetActive(true);
            menuButton.SetActive(true);
        }

        public void ShowSetup()
        {
            HideUI();
            setupAcceptButton.SetActive(true);
            adderContainer.SetActive(false);
            setupPanel.SetActive(true);
            menuButton.SetActive(true);
        }

        void ShowAddDiceRollSetup()
        {
            HideUI();
            setupAcceptButton.SetActive(false);
            adderContainer.SetActive(true);
            setupPanel.SetActive(true);
        }

        public void ShowThrow()
        {
            HideUI();
            throwPanel.SetActive(true);
            menuButton.SetActive(true);

            clearTableButton.gameObject.SetActive(somethingToClear);
        }

        public void ToggleScoreDetails()
        {
            scoreDetailsPanel.SetActive(!scoreDetailsPanel.activeSelf);
        }

        public void ShowMenu()
        {
            menuPanel.SetActive(true);
        }

        public void HideMenu()
        {
            menuPanel.SetActive(false);
        }

        private IEnumerator ThrowFinished()
        {
            float delay = DiceMinigameGlobalSettings.Instance.zoomInTime;
            yield return new WaitForSeconds(delay);
            ShowScore();
            ShowRethrow();
        }

        private IEnumerator ThrowAgain()
        {
            ShowScore(true);
            float delay = DiceMinigameGlobalSettings.Instance.zoomOutTime;
            yield return new WaitForSeconds(delay);
            ShowThrow();
        }

        private IEnumerator ThrowMore()
        {
            ShowScore(true);
            float delay = DiceMinigameGlobalSettings.Instance.zoomOutTime;
            yield return new WaitForSeconds(delay);
            ShowThrow();
        }

        public void OnClose()
        {
            SceneLoader.Instance.UnloadScene();
            Debug.Log("Just a test log here.");
        }
    }
}
