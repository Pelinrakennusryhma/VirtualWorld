using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cookbook : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI leftRecipeName;
    [SerializeField] private Image leftRecipeImage;
    [SerializeField] private TextMeshProUGUI leftIngredients;
    [SerializeField] private TextMeshProUGUI leftInstructions;
    [SerializeField] private TextMeshProUGUI rightRecipeName;
    [SerializeField] private Image rightRecipeImage;
    [SerializeField] private TextMeshProUGUI rightIngredients;
    [SerializeField] private TextMeshProUGUI rightInstructions;
    [SerializeField] private GameObject rightPageGO;
    [SerializeField] private GameObject nextPageButton;
    [SerializeField] private GameObject previousPageButton;
    private List<string> recipes;
    private int pageNumber = 0;
    void Start()
    {
        InitializeRecipes();
        DisplayRecipe(recipes[0], recipes[1]);
    }

    public void NextPage()
    {
        if (pageNumber == 0)
        {
            previousPageButton.SetActive(true);
            DisplayRecipe(recipes[2], recipes[3]);
            pageNumber++;
        }
        else if (pageNumber == 1)
        {
            DisplayRecipe(recipes[4], recipes[5]);
            pageNumber++;
        }
        else if (pageNumber == 2)
        {
            nextPageButton.SetActive(false);
            DisplayRecipe(recipes[6], null);
            pageNumber++;
        }
    }

    public void PreviousPage()
    {
        if (pageNumber == 1)
        {
            previousPageButton.SetActive(false);
            DisplayRecipe(recipes[0], recipes[1]);
            pageNumber--;
        }
        else if (pageNumber == 2)
        {
            DisplayRecipe(recipes[2], recipes[3]);
            pageNumber--;
        }
        else if (pageNumber == 3)
        {
            nextPageButton.SetActive(true);
            DisplayRecipe(recipes[4], recipes[5]);
            pageNumber--;
        }
    }

    public void DisplayRecipe(string leftRecipe, string rightRecipe)
    {
        string[] leftRecipeSplit = leftRecipe.Split("|");
        leftRecipeImage.sprite = Resources.Load<Sprite>("Sprites/" + leftRecipeSplit[0]);
        leftRecipeName.text = leftRecipeSplit[0];
        leftIngredients.text = leftRecipeSplit[1];
        leftInstructions.text = leftRecipeSplit[2];
        if(rightRecipe != null)
        {
            string[] rightRecipeSplit = rightRecipe.Split("|");
            rightPageGO.SetActive(true);
            rightRecipeImage.sprite = Resources.Load<Sprite>("Sprites/" + rightRecipeSplit[0]);
            rightRecipeName.text = rightRecipeSplit[0];
            rightIngredients.text = rightRecipeSplit[1];
            rightInstructions.text = rightRecipeSplit[2];
        }
        else
        {
            rightPageGO.SetActive(false);
        }
    }

    private void InitializeRecipes()
    {
        recipes = new List<string>()
        {
            "Sliced Bread|Bread|1. Cut the bread",
            "Charcuterie Board|Meat\nCheese\nPickles|1. Mix all the ingredients",
            "Chicken Wings|Frosted Chicken Wings|1. Unfrost Chicken Wings\n2. Fry Chicken Wings",
            "Salmon Soup|Vegetables\nFish\nDill|1. Boil vegetables\n2. Mix Boiled vegetables, fish and dill",
            "Spaghetti Carbonara|Eggs\nCheese\nMeat\nPasta|1. Mix eggs and cheese\n2.Whisk the mix\n3.Fry meat\n4.Boil Pasta\n5. Mix Everything",
            "Fish Grill|Fish\nPotatoes|1. Cut fish\n2. Cut potatoes\n3. Mix cut fish and cut potatoes\n4. Fry",
            "Steak with Potatoes|Meat\nPotatoes|1. Cut the meat\n2. Cut potatoes\n3. Mix cut meat and cut potatoes\n4. Fry",
        };
    }
    public void OpenCookbook()
    {
        gameObject.SetActive(true);
    }
    public void CloseCookbook()
    {
        gameObject.SetActive(false);
    }
}
