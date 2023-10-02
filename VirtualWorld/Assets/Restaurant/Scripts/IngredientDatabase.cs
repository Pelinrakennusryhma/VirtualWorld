using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientDatabase : MonoBehaviour
{
    public List<List<string>> mixRecipes;
    public List<Ingredient> ingredients;

    void Awake()
    {
        BuildMixRecipeDatabase();
        BuildDatabase();
    }

    public Ingredient GetIngredient(string name)
    {
        return ingredients.Find(ingredient => ingredient.type == name);
    }

    //type, pour, cut, fry, boil, whisk, defrost, serve
    void BuildDatabase()
    {
        ingredients = new List<Ingredient> {
            new Ingredient("Bread", null, "Sliced Bread", null, null, null, null, false),
            new Ingredient("Meat", null, "Cut Meat", "Fried Meat", null, null, null, false),
            new Ingredient("Cheese", null, null, null, null, null, null, false),
            new Ingredient("Pickles", null, null, null, null, null, null, false),
            new Ingredient("Frosted Chicken Wings", null, null, null, null, null, "Unfried Chicken Wings", false),
            new Ingredient("Vegetables", null, null, null, "Boiled Vegetables", null, null, false),
            new Ingredient("Fish", null, "Cut Fish", null, null, null, null, false),
            new Ingredient("Dill", null, null, null, null, null, null, false),
            new Ingredient("Egg", null, null, null, null, null, null, false),
            new Ingredient("Pasta", null, null, null, "Boiled Pasta", null, null, false),
            new Ingredient("Potato", null, "Cut Potato", null, null, null, null, false),
            new Ingredient("Water", "Glass of Water", null, null, null, null, null, false),
            new Ingredient("Soft Drink", "Glass of Soft Drink", null, null, null, null, null, false),
            new Ingredient("Beer", "Glass of Beer", null, null, null, null, null, false),
            new Ingredient("Wine", "Glass of Wine", null, null, null, null, null, false),

            new Ingredient("Cut Meat", null, null, null, null, null, null, false),
            new Ingredient("Fried Meat", null, null, null, null, null, null, false),
            new Ingredient("Boiled Vegetables", null, null, null, null, null, null, false),
            new Ingredient("Cut Fish", null, null, null, null, null, null, false),
            new Ingredient("Boiled Pasta", null, null, null, null, null, null, false),
            new Ingredient("Cut Potato", null, null, null, null, null, null, false),
            new Ingredient("Unfried Chicken Wings", null, null, "Chicken Wings", null, null, null, false),
            new Ingredient("Eggs & Cheese", null, null, null, null, "Whisked Eggs & Cheese", null, false),
            new Ingredient("Whisked Eggs & Cheese", null, null, null, null, null, null, false),
            new Ingredient("Unfried Fish Grill", null, null, "Fish Grill", null, null, null, false),
            new Ingredient("Unfried Steak with Potatoes", null, null, "Steak with Potatoes", null, null, null, false),

            new Ingredient("Sliced Bread", null, null, null, null, null, null, true),
            new Ingredient("Chicken Wings", null, null, null, null, null, null, true),
            new Ingredient("Charcuterie Board", null, null, null, null, null, null, true),
            new Ingredient("Salmon Soup", null, null, null, null, null, null, true),
            new Ingredient("Spaghetti Carbonara", null, null, null, null, null, null, true),
            new Ingredient("Fish Grill", null, null, null, null, null, null, true),
            new Ingredient("Steak with Potatoes", null, null, null, null, null, null, true),

            new Ingredient("Glass of Water", null, null, null, null, null, null, true),
            new Ingredient("Glass of Soft Drink", null, null, null, null, null, null, true),
            new Ingredient("Glass of Beer", null, null, null, null, null, null, true),
            new Ingredient("Glass of Wine", null, null, null, null, null, null, true),

        };
    }

    private void BuildMixRecipeDatabase()
    {
        mixRecipes = new List<List<string>>
        {
            new List<string>() {"Charcuterie Board", "Meat", "Cheese", "Pickles"},
            new List<string>() {"Salmon Soup", "Boiled Vegetables", "Fish", "Dill"},
            new List<string>() {"Eggs & Cheese", "Egg", "Cheese"},
            new List<string>() {"Spaghetti Carbonara", "Whisked Eggs & Cheese", "Fried Meat", "Boiled Pasta"},
            new List<string>() {"Unfried Fish Grill", "Cut Fish", "Cut Potato"},
            new List<string>() {"Unfried Steak with Potatoes", "Cut Meat", "Cut Potato"},

        };
    }
}
