using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace DiceMinigame
{
    public class DiceManager : MonoBehaviour
    {
        public Transform activeDiceParent;
        public List<DiceAdjust> diceAdjusts;
        public List<BonusAdjust> bonusAdjusts;
        public List<GameObject> activeDice;
        public List<GameObject> storedDice;
        public List<DiceScore> stoppedDices = new List<DiceScore>();
        private EventManager eventManager;
        private List<Vector3> allPositions = new List<Vector3>();
        private List<Vector3> availablePositions = new List<Vector3>();
        private List<List<DiceScore>> bonusDiceGroups = new List<List<DiceScore>>();
        private List<List<DiceScore>> penaltyDiceGroups = new List<List<DiceScore>>();
        private List<DiceScore> diceToFade = new List<DiceScore>();
        // Start is called before the first frame update
        void Awake()
        {
            allPositions = CreatePositions(DiceMinigameGlobalSettings.Instance.maxDice);
            eventManager = GetComponent<EventManager>();
            eventManager.EventAdjustsSpawned.AddListener(AdjustsSpawnedEventHandler);
            eventManager.EventSetupAccepted.AddListener(SetupAcceptedEventHandler);
            eventManager.EventDiceStopped.AddListener(DiceStoppedEventHandler);
            eventManager.EventThrowAgainPressed.AddListener(ThrowAgainPressedEventHandler);
            eventManager.EventThrowMorePressed.AddListener(ThrowMorePressedEventHandler);
            eventManager.EventClearTablePressed.AddListener(ClearTableEventHandler);
        }

        void Start()
        {
            eventManager.Ready();
        }

        public void DiceStoppedEventHandler(DiceScore diceScore)
        {
            stoppedDices.Add(diceScore);

            if (stoppedDices.Count >= activeDice.Count)
            {
                if (bonusDiceGroups.Count > 0)
                {
                    FindLowests();

                }
                if (penaltyDiceGroups.Count > 0)
                {
                    FindHighests();
                }
                StartCoroutine(RemoveLowest());

                eventManager.AllDiceStopped(stoppedDices, bonusAdjusts);
            }
        }

        private void FindLowests()
        {
            foreach (List<DiceScore> group in bonusDiceGroups)
            {
                DiceScore lowestScore = null;
                foreach (DiceScore dice in group)
                {
                    if (!lowestScore)
                    {
                        lowestScore = dice;
                    }
                    else if (dice.GetResult() < lowestScore.GetResult())
                    {
                        lowestScore = dice;
                    }
                }
                diceToFade.Add(lowestScore);
            }
        }

        private void FindHighests()
        {
            foreach (List<DiceScore> group in penaltyDiceGroups)
            {
                DiceScore highestScore = null;
                foreach (DiceScore dice in group)
                {
                    if (!highestScore)
                    {
                        highestScore = dice;
                    }
                    else if (dice.GetResult() > highestScore.GetResult())
                    {
                        highestScore = dice;
                    }
                }
                diceToFade.Add(highestScore);
            }
        }

        IEnumerator RemoveLowest()
        {
            foreach (DiceScore dice in diceToFade)
            {
                if (dice != null)
                {
                    stoppedDices.Remove(dice);
                    activeDice.Remove(dice.gameObject);
                    dice.AddComponent<DiceFader>();
                }
            }

            ClearFades();

            yield return DiceMinigameGlobalSettings.Instance.bonusDiceVanishTime;
        }

        public void DEBUG_AllDiceStopped()
        {
            eventManager.AllDiceStopped(stoppedDices, bonusAdjusts);
        }

        void AdjustsSpawnedEventHandler(List<DiceAdjust> _diceAdjusts, List<BonusAdjust> _bonusAdjusts)
        {
            diceAdjusts = _diceAdjusts;
            bonusAdjusts = _bonusAdjusts;
            SpawnDice(diceAdjusts);
        }

        void SetupAcceptedEventHandler()
        {
            ClearActiveDice();
            SpawnDice(diceAdjusts);
        }

        void ThrowAgainPressedEventHandler()
        {
            StartCoroutine(SpawnNewDice());
        }

        void ThrowMorePressedEventHandler()
        {
            StartCoroutine(SpawnMoreDice());
        }

        void ClearTableEventHandler()
        {
            ClearStoredDice();
        }

        void ClearActiveDice()
        {
            foreach (GameObject dice in activeDice)
            {
                Destroy(dice);
            }
            activeDice.Clear();
            stoppedDices.Clear();
        }

        void ClearStoredDice()
        {
            foreach (GameObject dice in storedDice)
            {
                Destroy(dice);
            }
            storedDice.Clear();
        }

        void ClearFades()
        {
            diceToFade.Clear();
            bonusDiceGroups.Clear();
        }

        void StoreDice()
        {
            foreach (GameObject dice in activeDice)
            {
                storedDice.Add(dice);
            }
            activeDice.Clear();
            stoppedDices.Clear();
        }

        private IEnumerator SpawnNewDice()
        {
            float delay = DiceMinigameGlobalSettings.Instance.zoomOutTime;
            yield return new WaitForSeconds(delay);
            ClearActiveDice();
            ClearStoredDice();
            SpawnDice(diceAdjusts);
        }
        private IEnumerator SpawnMoreDice()
        {
            float delay = DiceMinigameGlobalSettings.Instance.zoomOutTime;
            yield return new WaitForSeconds(delay);
            StoreDice();
            SpawnDice(diceAdjusts);
        }

        public void SpawnDice(List<DiceAdjust> diceAdjusts)
        {
            int totalDice = 0;
            foreach (Adjust diceAdjust in diceAdjusts)
            {
                totalDice += diceAdjust.GetAmount();

                // D100 (and possibly other non-normal dice that use more than one visible dice)
                if (diceAdjust.dicePrefab.name == "D100" || diceAdjust.dicePrefab.name == "D99")
                {
                    totalDice += diceAdjust.GetAmount();
                }
            }

            availablePositions = allPositions.GetRange(0, totalDice);

            foreach (Adjust diceAdjust in diceAdjusts)
            {
                List<DiceScore> diceGroup = new List<DiceScore>();
                bool bonusActive = false;
                bool penaltyActive = false;
                if (diceAdjust.GetType() == typeof(DiceAdjust))
                {
                    DiceAdjust diceAdj = (DiceAdjust)diceAdjust;
                    bonusActive = diceAdj.BonusActive();
                    penaltyActive = diceAdj.PenaltyActive();
                    if (bonusActive)
                    {
                        bonusDiceGroups.Add(diceGroup);
                    }
                    else if (penaltyActive)
                    {
                        penaltyDiceGroups.Add(diceGroup);
                    }
                }

                for (int i = 0; i < diceAdjust.GetAmount(); i++)
                {
                    Vector3 spawnPos = transform.position + GetPosition();
                    GameObject instantiatedDice = Instantiate(diceAdjust.dicePrefab, spawnPos, Quaternion.identity, activeDiceParent);
                    activeDice.Add(instantiatedDice);

                    DiceScore diceScore = instantiatedDice.GetComponent<DiceScore>();
                    diceScore.Init(eventManager);

                    if (bonusActive || penaltyActive)
                    {
                        diceGroup.Add(diceScore);
                    }

                    // D100 (and possibly other non-normal dice that use more than one visible dice)
                    if (diceAdjust.dicePrefab.name == "D100" || diceAdjust.dicePrefab.name == "D99")
                    {
                        D100Score d100Score = instantiatedDice.GetComponent<D100Score>();
                        GameObject childDicePrefab = d100Score.childDicePrefab;

                        spawnPos = transform.position + GetPosition();
                        GameObject instantiatedChildDice = Instantiate(childDicePrefab, spawnPos, Quaternion.identity, activeDiceParent);
                        d100Score.InitChild(instantiatedChildDice);
                    }
                }
            }
        }

        Vector3 GetPosition()
        {
            int index = Random.Range(0, availablePositions.Count);
            Vector3 pos = availablePositions[index];
            availablePositions.RemoveAt(index);
            return pos;
        }

        private List<Vector3> CreatePositions(int amount)
        {
            int maxWidth = 7;
            int maxHeight = 2;

            if (DiceMinigameGlobalSettings.Instance.isPortrait)
            {
                maxWidth = 3;
                maxHeight = 4;
            }

            float gap = 1.5f;

            List<Vector3> positions = new List<Vector3>();

            float x = transform.localPosition.x;
            float y = transform.localPosition.y;
            float z = transform.localPosition.z;
            int placedOnRow = 0;
            int rowsPlaced = 0;
            int columnsPlaced = 0;

            for (int i = 0; i < amount; i++)
            {
                // place every other dice on right and every other on left
                if (i % 2 == 0)
                {
                    x = (-placedOnRow - 1) / 2 * gap / 10;
                }
                else
                {
                    x = (placedOnRow + 1) / 2 * gap / 10;
                }
                y = rowsPlaced * gap / 10;
                z = columnsPlaced * gap / 10;
                placedOnRow++;

                // next row
                if (placedOnRow == maxWidth)
                {
                    placedOnRow = 0;
                    rowsPlaced++;

                    // next z-depth-thingy
                    if (rowsPlaced == maxHeight)
                    {
                        rowsPlaced = 0;
                        columnsPlaced++;
                    }
                }
                //Vector3 pos = new Vector3(x + GetRandomness(), y + GetRandomness(), z + GetRandomness());
                Vector3 pos = Vector3.zero;
                pos += transform.right * x;
                pos += transform.up * y;
                pos += transform.forward * z;
                positions.Add(pos);
            }

            return positions;
        }

        float GetRandomness()
        {
            float randomness = 0.01f;
            return Random.Range(-randomness, randomness);
        }
    }
}
