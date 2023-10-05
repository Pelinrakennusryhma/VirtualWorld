using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Animal : MonoBehaviour
{
    [SerializeField] private Image FoodBar;
    [SerializeField] private Image CleanlinessBar;
    [SerializeField] private Image PastureBar;
    [SerializeField] private Image ProductBar;
    [SerializeField] private Image AgeBar;
    public string animalSpecies;
    private float food = 100.0f;
    private float cleanliness = 100.0f;
    private float pasture = 100.0f;
    private float product = 0.0f;
    private float age = 0.0f;
    private float lifespan = 120.0f;

    public void InitializeAnimal()
    {
        StartCoroutine(Bars());
    }

    private IEnumerator Bars()
    {
        while (age < lifespan)
        {
            food -= 1.0f;
            cleanliness -= 0.4f;
            pasture -= 0.2f;
            product += 1.0f;
            age += 0.4f;
            if(food == 0 || cleanliness == 0 || pasture == 0)
            {
                age = lifespan;
            }

            FoodBar.fillAmount = food / 100;
            CleanlinessBar.fillAmount = cleanliness / 100;
            ProductBar.fillAmount = product / 100;
            AgeBar.fillAmount = age / lifespan;
            if (PastureBar != null)
            {
                PastureBar.fillAmount = pasture / 100;
            }

            if (AgeBar.fillAmount >= 1.0f)
            {
                AgeBar.color = Color.black;
            }
            else if (AgeBar.fillAmount >= 0.97f)
            {
                AgeBar.color = Color.red;
            }
            else if (AgeBar.fillAmount >= 0.8f)
            {
                AgeBar.color = Color.yellow;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void FeedAnimal()
    {
        food = 100.0f;
    }

    public void Clean()
    {
        cleanliness = 100.0f;
    }

    public void LetOut()
    {
        pasture = 100.0f;
    }

    public void CollectProduce()
    {
        if (animalSpecies == "Cow")
        {
            if (AgeBar.fillAmount != 1.0f && ProductBar.fillAmount == 1.0f)
            {
                product = 0f;
                ProductInventory productInventory = GameObject.Find("ProductInventory").GetComponent<ProductInventory>();
                productInventory.ownedProducts["Milk"] += 1;
                productInventory.UpdateInventory();
            }
        }
        else if (animalSpecies == "Chicken")
        {
            if (AgeBar.fillAmount != 1.0f && ProductBar.fillAmount >= 0.8f)
            {
                product = 0f;
                ProductInventory productInventory = GameObject.Find("ProductInventory").GetComponent<ProductInventory>();
                productInventory.ownedProducts["Egg"] += 1;
                productInventory.UpdateInventory();
            }
        }
    }

    public void Butcher()
    {
        if (animalSpecies == "Cow")
        {
            if (AgeBar.fillAmount != 1.0f && AgeBar.fillAmount >= 0.8f)
            {
                ProductInventory productInventory = GameObject.Find("ProductInventory").GetComponent<ProductInventory>();
                productInventory.ownedProducts["Beef"] += 1;
                productInventory.UpdateInventory();
            }
            Destroy(gameObject);
        }
        else if (animalSpecies == "Chicken")
        {
            if (AgeBar.fillAmount != 1.0f && AgeBar.fillAmount >= 0.8f)
            {
                ProductInventory productInventory = GameObject.Find("ProductInventory").GetComponent<ProductInventory>();
                productInventory.ownedProducts["Chicken Meat"] += 1;
                productInventory.UpdateInventory();
            }
            Destroy(gameObject);
        }
    }
}
