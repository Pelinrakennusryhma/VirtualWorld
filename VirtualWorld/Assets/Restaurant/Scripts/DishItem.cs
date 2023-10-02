using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DishItem : MonoBehaviour
{
    public string dish;
    private DishList dishList;
    private RestaurantGameSystem gameSystem;
    private Transform dishesTransform;

    private void Start()
    {
        dishList = GameObject.Find("Restaurant/Dishes").GetComponent<DishList>();
        gameSystem = GameObject.Find("Canvas").GetComponent<RestaurantGameSystem>();
    }
    public void DeliverDish()
    {
        // This seems to bug out sometimes for unknown reason.

        if(dishList.selectedOrder != null)
        {
            Debug.Log("Non null selected order");

            Debug.Log("Dishlist selecter order name is " + dishList.selectedOrder.name+ 
                " dishlist gameobject name " + gameObject.name);

            if (dishList.selectedOrder.name == gameObject.name)
            {
                Debug.LogWarning("Should serve");


                dishesTransform = GameObject.Find("Restaurant/Dishes").transform;
                Object prefab = Resources.Load("Prefabs/RestaurantFloatingText");
                GameObject newItem = Instantiate(prefab, dishesTransform) as GameObject;
                newItem.GetComponentInChildren<TextMeshProUGUI>().text = dishList.dishes[dish].ToString("C", gameSystem.culture);

                gameSystem.AddMoneyToPlayer(dishList.dishes[dish]);
                Destroy(dishList.selectedOrder);
                Destroy(gameObject);
            }
        }

        else
        {
            Debug.LogError("Dishlist selected order is null. Can not serve. ");
        }

    }
}
