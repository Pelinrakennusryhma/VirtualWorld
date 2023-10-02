using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddIngredients : MonoBehaviour
{
    [SerializeField] private GameObject layout;
    public IngredientDatabase ingredientDatabase;
    public void AddIngredient(string ingredient)
    {
        Object prefab = Resources.Load("Prefabs/IngredientItem");
        GameObject newItem = Instantiate(prefab, layout.transform) as GameObject;
        newItem.name = ingredient;
        newItem.GetComponent<IngredientItem>().ingredient = ingredientDatabase.GetIngredient(ingredient);
        newItem.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + ingredient);


        //Tama ja teksti poistettava myohemmin kun kuvat ovat valmiina.
        newItem.GetComponentInChildren<TextMeshProUGUI>().text = ingredient;
    }
}
