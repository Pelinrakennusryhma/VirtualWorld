using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DiceMinigame
{
    public class ScoreManager : MonoBehaviour
    {
        public EventManager eventManager;
        public RectTransform scoreTextParent;
        public GameObject scoreTextPrefab;
        public TextMeshProUGUI totalScoreText;
        private List<DiceResult> currentScores = new List<DiceResult>();
        private int totalScore = 0;
        private int throwNum = 0;

        void Start()
        {
            eventManager.EventAllDiceStopped.AddListener(AllDiceStoppedEventHandler);
            eventManager.EventThrowAgainPressed.AddListener(ThrowAgainPressedEventHandler);
            eventManager.EventThrowMorePressed.AddListener(ThrowMorePressedEventHandler);
            eventManager.EventClearTablePressed.AddListener(ClearTableEventHandler);
        }

        void AllDiceStoppedEventHandler(List<DiceScore> diceScores, List<BonusAdjust> bonuses)
        {
            CalculateScore(diceScores, bonuses);
            InsertScore();
        }

        void ThrowAgainPressedEventHandler()
        {
            float delay = DiceMinigameGlobalSettings.Instance.zoomOutTime;
            StartCoroutine(ClearScore(delay));
        }

        void ThrowMorePressedEventHandler()
        {
            float delay = DiceMinigameGlobalSettings.Instance.zoomOutTime;
            StartCoroutine(StoreScore(delay));
        }

        void ClearTableEventHandler()
        {
            StartCoroutine(ClearScore(0f));
        }

        IEnumerator ClearScore(float delay)
        {
            yield return new WaitForSeconds(delay);

            totalScore = 0;
            throwNum = 0;
            currentScores.Clear();
            foreach (Transform child in scoreTextParent.transform)
            {
                Destroy(child.gameObject);
            }
        }

        IEnumerator StoreScore(float delay)
        {
            yield return new WaitForSeconds(delay);

            if (throwNum == 1)
            {
                InsertThrowNum();
            }
            currentScores.Clear();
        }

        void CalculateScore(List<DiceScore> diceScores, List<BonusAdjust> bonuses)
        {
            List<DiceResult> results = new List<DiceResult>();
            foreach (DiceScore diceScore in diceScores)
            {
                string diceName = diceScore.gameObject.name.Split("(")[0];
                int score = diceScore.GetResult();
                results.Add(new DiceResult(score, diceName));

            }

            results.Sort(SortByType);

            foreach (DiceResult diceResult in results)
            {
                currentScores.Add(new DiceResult(diceResult.result, diceResult.type));
            }

            foreach (BonusAdjust bonus in bonuses)
            {
                string name = bonus.displayName;
                int amount = bonus.GetAmount();
                if (amount != 0)
                {
                    currentScores.Add(new DiceResult(amount, name, false));
                }
            }
        }

        static int SortByType(DiceResult d1, DiceResult d2)
        {
            return d1.typeN.CompareTo(d2.typeN);
        }

        public void InsertScore()
        {
            throwNum++;
            int indexInParent = 0;
            if (throwNum > 1)
            {
                InsertThrowNum();
                indexInParent++;
            }

            int throwTotal = 0;

            foreach (DiceResult diceResult in currentScores)
            {
                totalScore += diceResult.result;
                throwTotal += diceResult.result;

                GameObject scoreTextObject = Instantiate(scoreTextPrefab, scoreTextParent);
                scoreTextObject.transform.SetSiblingIndex(indexInParent);

                TextMeshProUGUI scoreText = scoreTextObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                scoreText.text = diceResult.result.ToString();

                TextMeshProUGUI typeText = scoreTextObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                typeText.text = $"({diceResult.type})";

                indexInParent++;
            }

            GameObject totalScoreObject = Instantiate(scoreTextPrefab, scoreTextParent);
            totalScoreObject.transform.SetSiblingIndex(indexInParent);

            TextMeshProUGUI throwTotalScore = totalScoreObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            throwTotalScore.text = throwTotal.ToString();

            TextMeshProUGUI throwTotalType = totalScoreObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            throwTotalType.text = "(Total)";

            totalScoreText.text = $"Total: {totalScore}";
        }

        void InsertThrowNum()
        {
            GameObject scoreTextObject = Instantiate(scoreTextPrefab, scoreTextParent);
            scoreTextObject.transform.SetAsFirstSibling();

            TextMeshProUGUI scoreText = scoreTextObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            scoreText.text = $"<u>Throw {throwNum}:</u>";
        }
    }
}
