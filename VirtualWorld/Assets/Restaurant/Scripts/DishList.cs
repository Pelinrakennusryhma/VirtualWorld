using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishList : MonoBehaviour
{
    [SerializeField] private Transform orderList;
    public Dictionary<string, double> dishes;
    public GameObject selectedOrder;

    void Awake()
    {
        InitializeDishes();
        NewOrder("random");
        NewOrder("Salmon Soup");
        StartCoroutine(NewOrders());
    }

    public void DeselectDishes()
    {
        GameObject[] orders;
        orders = GameObject.FindGameObjectsWithTag("Order");
        foreach (GameObject orderGO in orders)
        {
            Order order = orderGO.GetComponent<Order>();
            order.selected = false;
            order.selectedBorder.SetActive(false);
        }
    }
    private void NewOrder(string dishName)
    {
        Object prefab = Resources.Load("Prefabs/Order");
        GameObject newItem = Instantiate(prefab, orderList.transform) as GameObject;
        newItem.GetComponent<Order>().SetOrder(dishName);
    }

    private void InitializeDishes()
    {
        dishes = new Dictionary<string, double>()
        {
            { "Glass of Water", 2.0 },
            { "Glass of Soft Drink", 4.0 },
            { "Glass of Beer", 6.0 },
            { "Glass of Wine", 8.0 },
            { "Charcuterie Board", 15.0 },
            { "Sliced Bread", 3.0 },
            { "Chicken Wings", 10.0 },
            { "Salmon Soup", 12.0 },
            { "Spaghetti Carbonara", 18.0 },
            { "Fish Grill", 20.0 },
            { "Steak with Potatoes", 25.0 },
        };
    }
    private IEnumerator NewOrders()
    {
        while (true)
        {
            if (Random.Range(0, 4) == 0 && orderList.childCount < 7)
            {
                NewOrder("random");
            }
            else
            {
                Debug.Log("asd");
            }
            yield return new WaitForSeconds(3f);
        }

    }
}
