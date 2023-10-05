using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockInventory : MonoBehaviour
{
    [SerializeField] private GameObject layout;
    public StockSystem stockSystem;

    private void Update()
    {
        UpdateInventory();
    }
    public void UpdateInventory()
    {
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("StockInventoryItem"))
        {
            Destroy(go);
        }

        foreach (var stock in stockSystem.playerStocks)
        {
            if (stock.Value > 0)
            {
                Object prefab = Resources.Load("Prefabs/StockInventoryStock");
                GameObject newItem = Instantiate(prefab, layout.transform) as GameObject;
                newItem.GetComponent<StockInventoryItem>().stock = stock.Key;
                newItem.name = stock.Key.symbol.ToString();
                newItem.GetComponent<StockInventoryItem>().SetText(1.0, stock.Value);
            }
        }
    }
    public void CloseInventory()
    {
        gameObject.SetActive(false);
    }
}
