using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace DiceMinigame
{
    public class DiceRollSaverLoader : MonoBehaviour
    {
        public TextAsset diceRollDefaults;
        public TMP_InputField newSetupNameField;
        EventManager eventManager;
        DiceRollSetups diceRollSetups;
        List<DiceAdjust> diceAdjusts;
        List<BonusAdjust> bonusAdjusts;
        private string cachedTitle = "";
        DiceRollSetup cachedSetup = null;
        // Start is called before the first frame update
        void Awake()
        {
            eventManager = GetComponent<EventManager>();
            eventManager.EventAdjustsSpawned.AddListener(AdjustsSpawnedEventListener);
            eventManager.EventTitleChanged.AddListener(UpdateSetupName);
            eventManager.EventAddNewDiceRollSetupOpened.AddListener(CacheDiceRollSetup);
            eventManager.EventDiceRollSetupDeleted.AddListener(DeleteDiceRollSetup);
            LoadDiceRollSetups();
            //Debug.Log(JsonUtility.ToJson(diceRollSetups));
        }
        void Start()
        {
            LoadDiceRollSetups();
        }

        void AdjustsSpawnedEventListener(List<DiceAdjust> _diceAdjusts, List<BonusAdjust> _bonusAdjusts)
        {
            diceAdjusts = _diceAdjusts;
            bonusAdjusts = _bonusAdjusts;
        }

        void LoadDiceRollSetups()
        {
            string stringToLoad = PlayerPrefs.GetString("DiceRollData", diceRollDefaults.text);
            diceRollSetups = JsonUtility.FromJson<DiceRollSetups>(stringToLoad);
            eventManager.UpdateDiceRollSetups(diceRollSetups);
        }

        public void AddNewDiceRollSetup()
        {
            DiceRollSetup diceRollSetup = CreateDiceRollSetup();

            diceRollSetup.name = newSetupNameField.text;
            diceRollSetups.savedRolls.Add(diceRollSetup);

            SaveData();

            eventManager.UpdateDiceRollSetups(diceRollSetups);
        }

        public void DeleteDiceRollSetup(int index)
        {
            Debug.Log("deleting: " + index);
            diceRollSetups.savedRolls.RemoveAt(index);

            SaveData();

            eventManager.UpdateDiceRollSetups(diceRollSetups);
        }

        void SaveData()
        {
            PlayerPrefs.SetString("DiceRollData", JsonUtility.ToJson(diceRollSetups));
        }

        DiceRollSetup CreateDiceRollSetup()
        {
            DiceRollSetup diceRollSetup = new DiceRollSetup();
            diceRollSetup.diceRollData = new List<DiceRollData>();

            foreach (DiceAdjust diceAdjust in diceAdjusts)
            {

                DiceRollData diceRollData = new DiceRollData();
                diceRollData.name = diceAdjust.dicePrefab.name;
                diceRollData.amount = diceAdjust.GetAmount();
                diceRollData.bonus = diceAdjust.BonusActive();
                diceRollData.penalty = diceAdjust.PenaltyActive();
                diceRollSetup.diceRollData.Add(diceRollData);

            }

            diceRollSetup.bonus = bonusAdjusts[0].GetAmount();

            return diceRollSetup;
        }

        void UpdateSetupName(string name)
        {
            int maxLength = 50;
            if (name.Length > maxLength)
            {
                newSetupNameField.text = name[..maxLength];
            }
            else
            {
                newSetupNameField.text = name;
            }

        }

        void CacheDiceRollSetup()
        {
            cachedTitle = newSetupNameField.text;
            cachedSetup = CreateDiceRollSetup();
        }

        public void RecoverCachedDiceRollSetup()
        {
            eventManager.ChangeTitle(cachedTitle);
            eventManager.LoadDiceScoreSetup(cachedSetup);
        }
    }
}
