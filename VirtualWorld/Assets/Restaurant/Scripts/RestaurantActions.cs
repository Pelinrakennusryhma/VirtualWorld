using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RestaurantActions : MonoBehaviour
{
    [SerializeField] private GameObject dishLayout;
    [SerializeField] private GameObject mixingLayout;
    public IngredientDatabase ingredientDatabase;
    public GameObject selectedItem;
    public Ingredient selectedIngredient;
    public List<string> mixIngredients = new List<string>();

    public void Prepare(string action)
    {
        if (selectedIngredient != null)
        {
            string nextStep = "";
            bool check = false;
            if(action == "Pour" && selectedIngredient.pour != null)
            {
                nextStep = selectedIngredient.pour;
                check = true;
            }
            else if (action == "Cut" && selectedIngredient.cut != null)
            {
                nextStep = selectedIngredient.cut;
                check = true;
            }
            else if (action == "Fry" && selectedIngredient.fry != null)
            {
                nextStep = selectedIngredient.fry;
                check = true;
            }
            else if (action == "Boil" && selectedIngredient.boil != null)
            {
                nextStep = selectedIngredient.boil;
                check = true;
            }
            else if (action == "Whisk" && selectedIngredient.whisk != null)
            {
                nextStep = selectedIngredient.whisk;
                check = true;
            }
            else if (action == "Defrost" && selectedIngredient.defrost != null)
            {
                nextStep = selectedIngredient.defrost;
                check = true;
            }
            if (check)
            {
                Object prefab = Resources.Load("Prefabs/IngredientItem");
                GameObject newItem = Instantiate(prefab, gameObject.transform) as GameObject;
                newItem.name = nextStep;
                newItem.GetComponent<IngredientItem>().ingredient = ingredientDatabase.GetIngredient(nextStep);
                newItem.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + nextStep);

                //Tama ja teksti poistettava myohemmin kun kuvat ovat valmiina.
                newItem.GetComponentInChildren<TextMeshProUGUI>().text = nextStep;

                Destroy(selectedItem);
                DeselectIngredients();
            }
        }
    }

    public void AddToMix()
    {
        if(selectedIngredient != null)
        {
            selectedItem.transform.SetParent(mixingLayout.transform);
            selectedItem.GetComponent<IngredientItem>().inMixingArea = true;
            Debug.Log(selectedIngredient.type);
            mixIngredients.Add(selectedIngredient.type);
            DeselectIngredients();
        }
    }
    public void Mix()
    {
        foreach(List<string> recipe in ingredientDatabase.mixRecipes)
        {
            bool recipeMatch = true;
            foreach (string recipeIngredient in recipe)
            {
                if(recipeIngredient == recipe[0])
                {
                    continue;
                }
                if (!mixIngredients.Contains(recipeIngredient))
                {
                    recipeMatch = false;
                }
            }
            if(recipeMatch == true)
            {
                string matchingRecipe = recipe[0];
                Debug.Log("Match " + matchingRecipe);

                Object prefab = Resources.Load("Prefabs/IngredientItem");
                GameObject newItem = Instantiate(prefab, gameObject.transform) as GameObject;
                newItem.name = matchingRecipe;
                newItem.GetComponent<IngredientItem>().ingredient = ingredientDatabase.GetIngredient(matchingRecipe);
                newItem.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + matchingRecipe);

                //Tama ja teksti poistettava myohemmin kun kuvat ovat valmiina.
                newItem.GetComponentInChildren<TextMeshProUGUI>().text = matchingRecipe;

                foreach (Transform transform in mixingLayout.transform.GetComponentsInChildren<Transform>())
                {
                    if(transform.gameObject.tag == "Ingredient")
                    {
                        Destroy(transform.gameObject);
                    }

                }
                mixIngredients.Clear();
                DeselectIngredients();
            }
            else
            {
                Debug.Log("Mismatch " + recipe[0]);
            }
        }
    }

    public void Serve()
    {
        if(selectedIngredient != null)
        {
            if (selectedIngredient.serve)
            {
                Object prefab = Resources.Load("Prefabs/Dish");
                GameObject newItem = Instantiate(prefab, dishLayout.transform) as GameObject;
                newItem.name = selectedIngredient.type;
                newItem.GetComponent<DishItem>().dish = selectedIngredient.type;
                newItem.GetComponentInChildren<TextMeshProUGUI>().text = selectedIngredient.type;

                Destroy(selectedItem);
                DeselectIngredients();
            }
        }
    }

    public void DeselectIngredients()
    {
        GameObject[] ingredients;
        ingredients = GameObject.FindGameObjectsWithTag("Ingredient");
        if (ingredients != null)
        {
            foreach (GameObject ingredientGO in ingredients)
            {
                ingredientGO.GetComponent<IngredientItem>().Deselect();
            }
        }
        selectedItem = null;
        selectedIngredient = null;
    }

    public void ClearWorkingArea()
    {
        GameObject[] ingredients;
        ingredients = GameObject.FindGameObjectsWithTag("Ingredient");
        foreach (GameObject ingredientGO in ingredients)
        {
            Destroy(ingredientGO);
        }
        selectedItem = null;
        selectedIngredient = null;
    }
}
