using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Order : MonoBehaviour
{
    [SerializeField] private Image timeLimitCounter;
    [SerializeField] private Image dishImage;
    [SerializeField] public GameObject selectedBorder;
    [SerializeField] private TextMeshProUGUI dishText;
    public DishList dishList;
    public bool selected = false;
    private float orderTimeLimit = 100.0f;
    private List<string> dishes = new List<string>();
    private string dish;
    private double price;
    private IEnumerator TimeLimit()
    {
        while (0 < orderTimeLimit)
        {
            orderTimeLimit -= 0.2f;
            timeLimitCounter.fillAmount = orderTimeLimit / 100;

            if (timeLimitCounter.fillAmount <= 0.25f)
            {
                timeLimitCounter.color = Color.red;
            }
            else if (timeLimitCounter.fillAmount <= 0.5f)
            {
                timeLimitCounter.color = Color.yellow;
            }

            yield return new WaitForSeconds(0.1f);
        }
        Destroy(gameObject);
    }

    public void ToggleSelected()
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

    public void Select()
    {
        dishList.DeselectDishes();
        dishList.selectedOrder = gameObject;
        selected = true;
        selectedBorder.SetActive(true);
    }
    public void Deselect()
    {
        dishList.selectedOrder = null;
        selected = false;
        selectedBorder.SetActive(false);
    }
    public bool DishCheck(string dishListDish)
    {
        if(dishListDish == dish)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void SetOrder(string dishToSet)
    {
        dishList = GameObject.Find("Restaurant/Dishes").GetComponent<DishList>();
        foreach (string d in dishList.dishes.Keys)
        {
            dishes.Add(d);
        }
        if(dishToSet == "random")
        {
            dish = dishes[Random.Range(0, dishes.Count)];
        }
        else
        {
            dish = dishToSet;
        }
        
        gameObject.name = dish;
        price = dishList.dishes[dish];
        dishText.text = dish;
        dishImage.sprite = Resources.Load<Sprite>("Sprites/" + dish);
        StartCoroutine(TimeLimit());
    }
}
