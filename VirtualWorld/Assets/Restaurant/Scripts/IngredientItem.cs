using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientItem : MonoBehaviour
{
    [SerializeField]public GameObject selectedBorder;
    public bool selected = false;
    public Ingredient ingredient;
    public RestaurantActions actions;
    public bool inMixingArea = false;

    private void Awake()
    {
        actions = GameObject.Find("WorkingArea/IngredientArea").GetComponent<RestaurantActions>();
    }

    public void ToggleSelected()
    {
        if (!inMixingArea)
        {
            if (selected)
            {
                Deselect();
            }
            else
            {
                Select();
            }
        }
        else
        {
            inMixingArea = false;
            actions.mixIngredients.Remove(ingredient.type);
            gameObject.transform.SetParent(actions.gameObject.transform);
        }
    }

    public void Select()
    {
        actions.DeselectIngredients();
        actions.selectedItem = gameObject;
        actions.selectedIngredient = ingredient;
        selected = true;
        selectedBorder.SetActive(true);
    }
    public void Deselect()
    {
        actions.selectedItem = null;
        actions.selectedIngredient = null;
        selected = false;
        selectedBorder.SetActive(false);
    }
}
