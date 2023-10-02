using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DiceMinigame
{
    public class QuickSelect : MonoBehaviour, IPointerClickHandler
    {
        public string DefaultText;
        public EventManager eventManager;
        private TMP_Dropdown dropdown;
        private DiceRollSetups diceRollSetups;

        private void Awake()
        {
            dropdown = GetComponent<TMP_Dropdown>();
            eventManager.EventDiceRollSetupsChanged.AddListener(BuildDropdown);
            dropdown.onValueChanged.AddListener(SelectSetup);
        }

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            if (pointerEventData.button == PointerEventData.InputButton.Left)
            {
                SetupDeleteButtons();
            }
        }

        private void SetupDeleteButtons()
        {
            Button[] buttons = GetComponentsInChildren<Button>();

            if (buttons.Length <= 0) return;

            // remove delete button from the last item, from the "Add new" item
            buttons[buttons.Length - 1].gameObject.SetActive(false);

            for (int i = 0; i < buttons.Length - 1; i++)
            {
                int buttonIndex = i;
                buttons[i].onClick.AddListener(delegate { OnDeleteButtonClick(buttonIndex); });
            }
        }

        private void OnDeleteButtonClick(int index)
        {
            eventManager.DeleteDiceRollSetup(index);
        }

        private void SelectSetup(int index)
        {
            dropdown.SetValueWithoutNotify(-1);
            if (index == diceRollSetups.savedRolls.Count)
            {
                eventManager.OpenAddNewDiceRollSetup();
            }
            else
            {
                DiceRollSetup selectedSetup = diceRollSetups.savedRolls[index];
                eventManager.LoadDiceScoreSetup(selectedSetup);
            }

        }

        private void BuildDropdown(DiceRollSetups _diceRollSetups)
        {
            dropdown.Hide();

            diceRollSetups = _diceRollSetups;
            dropdown.ClearOptions();

            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

            foreach (DiceRollSetup diceRollSetup in diceRollSetups.savedRolls)
            {
                TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData(diceRollSetup.name);
                options.Add(optionData);
            }

            TMP_Dropdown.OptionData addNewOption = new TMP_Dropdown.OptionData("            Add new");
            options.Add(addNewOption);

            dropdown.AddOptions(options);
            dropdown.SetValueWithoutNotify(-1);
        }
    }
}