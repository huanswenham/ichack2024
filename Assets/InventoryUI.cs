using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GridLayoutGroup gridLayout;
    public InventoryManager inventoryManager;

    void Start()
    {
        UpdateInventoryUI();
    }

    void UpdateInventoryUI()
    {
        foreach (var item in inventoryManager.allItems)
        {
            GameObject inventoryItem = Instantiate(item.modelPrefab, gridLayout.transform);  
            inventoryItem.transform.localScale = new Vector3(item.scale, item.scale, item.scale);          
            //inventoryItem.GetComponentInChildren<Text>().text = item.itemName;
            //inventoryItem.GetComponent<Button>().onClick.AddListener(() => OnItemClick(item));
        }
    }

    void OnItemClick(InventoryItem item)
    {
        inventoryPanel.SetActive(false);
        PlaceItemInAR(item);
    }

    void PlaceItemInAR(InventoryItem item)
    {

    }
}
