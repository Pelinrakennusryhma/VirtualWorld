using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HymiQuests
{
    public class QuestCategories : MonoBehaviour
    {

        public GameObject QuestCategoryPrefab;

        void Start()
        {


            //Questien kategoriat mitä pelaaja näkee. Myöhemmin tehtävänä että kategoriat näkyvät vain jos pelaajalla on kategoriasta questi.
            List<string> categories = new List<string> { "Main Quest", "Side Quest" };

            foreach (string category in categories)
            {
                //Object prefab = Resources.Load("Prefabs/QuestCategory");
                //GameObject newItem = Instantiate(prefab, gameObject.transform) as GameObject;

                GameObject newItem = Instantiate(QuestCategoryPrefab, gameObject.transform);
                newItem.GetComponentInChildren<QuestCategory>().InitializeCategory(category);
            }
        }
    }
}
